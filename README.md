## miscellaneous finishing touches
azure windows nano server -> https://www.thomasmaurer.ch/2016/11/how-to-deploy-nano-server-in-azure/
vm enable iis and web deploy -> https://github.com/aspnet/Tooling/blob/AspNetVMs/docs/create-asp-net-vm-with-webdeploy.md    
visual studio azure web app publish credential -> portal | download publish profile | see userName and userPWD where encrypted version of latter gets stored in .pubxml.user under <Project...><PropertyGroup><EncryptedPassword>...</EncryptedPassword></PropertyGroup></Project>  
web api frombody attribute [ and multiple input parameters ] -> https://stackoverflow.com/questions/24625303/why-do-we-have-to-specify-frombody-and-fromuri    
lookup azure image skus https://docs.microsoft.com/en-us/azure/virtual-machines/windows/cli-ps-findimage 
   Connect-AzureRMAccount
   $locName = 'West US 2'; Get-AzureRMVMImagePublisher -Location $locName | Select PublisherName [ > d:\temp\publishers.log ]
     "publisher": "MicrosoftWindowsDesktop", "offer": "Windows-10", "sku": "rs5-pro",
     "publisher": "MicrosoftWindowsServer","offer": "WindowsServer", "sku": "2019-Datacenter-Core-smalldisk",
   $pubName = 'MicrosoftWindowsDesktop'; Get-AzureRMVMImageOffer -Location $locName -Publisher $pubName | Select Offer
     $offerName = 'Windows-10'; Get-AzureRMVMImageSku -Location $locName -Publisher $pubName -Offer $offerName | Select Skus
     "rs5-pro" which is w10 1809 option you have been using
   $pubName = 'MicrosoftWindowsServer'; Get-AzureRMVMImageOffer -Location $locName -Publisher $pubName | Select Offer
     $offerName = 'WindowsServer'; Get-AzureRMVMImageSku -Location $locName -Publisher $pubName -Offer $offerName | Select Skus
     "2019-Datacenter-Core-smalldisk" and "2019-Datacenter-smalldisk" which is w19s 1809 options you have been using
https://github.com/Azure/Azure-Functions/issues/1029 Azure function v2 dot net core 2.1 consumption plan performance is much slower than aws.  
https://github.com/Azure/Azure-Functions/issues/1080 arm template deployed function app has no host keys and throws errors  
  https://github.com/Azure/Azure-Functions/issues/879 Function App creation/deploy from RM template with Runtime 'beta' -- Always /admin/host/keys Error  
  https://github.com/Azure/Azure-Functions/issues/516 [Question] Functions 'Host Keys' & ARM templates  
  https://github.com/Azure/Azure-Functions/issues/226 Getting error: "We are not able to retrieve the keys for function "  
https://github.com/Azure/azure-functions-host/issues/3810 vs17 attach debugger for [ functions ] v2  
https://github.com/Azure/azure-functions-host/issues/3898 `ClaimsPrincipal.IsInRole` doesn't work with AAD application roles  
https://github.com/Azure/azure-functions-host/issues/3857 `ClaimsPrincipal` doesn't include identity when `authType` is `anonymous`  
https://github.com/Azure/azure-functions-host/issues/33 Support EasyAuth  
azure easyauth user claimsprincipal github -> https://stackoverflow.com/questions/41501612/trouble-getting-claimsprincipal-populated-when-using-easyauth-to-authenticate-ag/41501796?noredirect=1#comment94218519_41501796  
    
## azure active directory command line application registration
https://stackoverflow.com/questions/31684821/how-to-add-application-to-azure-ad-programmatically  
where you could just issue command to provision app registration container then use preconfigured manifest file upload to do all the steps in one shot  
msonline 07/09/17 https://docs.microsoft.com/en-us/powershell/azure/active-directory/install-msonlinev1 | install-module msonline  
is being deprecated and replaced by azuread https://docs.microsoft.com/en-us/powershell/module/Azuread/  
https://docs.microsoft.com/en-us/powershell/azure/active-directory/install-adv2 -> install-module azuread [ | azureadpreview ]  & get-module -listavailable -name azuread
https://www.powershellgallery.com/packages/MsOnline/ [ 1.1.183.17 ] -> https://www.powershellgallery.com/packages/AzureAD/ [ 2.0.2.4 ] |
https://www.powershellgallery.com/packages/AzureADPreview/ [ 2.0.2.5 ]  

## azure resource manager [arm] template and parameters json resource group | settings | automation script | generating template ... download required fixes
  
for a good starting point overview on using arm templates for deployments see -> https://blogs.msdn.microsoft.com/benjaminperkins/2018/05/16/how-to-usecreate-arm-templates-for-deployments/  
os provisioning for vm 'emuamvmiisapp1' did not finish in the allotted time -> https://github.com/Azure/azure-sdk-for-node/issues/1491
  
1. deployment/template.json updates to vm osProfile resource configuration to include adminPassword property  
see https://blogs.msdn.microsoft.com/benjaminperkins/2018/05/16/required-parameter-adminpassword-is-missing-null/  
"parameters": {  
"virtualMachines_all_adminUsername": {  
    "defaultValue": "vmLogon",  
    "type": "String"  
},  
"virtualMachines_all_adminPassword": {  
    "defaultValue": "P@ssw0rd1234",  
    "type": "String"  
},  
  
"osProfile": {  
    "computerName": "[parameters('virtualMachines_&lt;vmName&gt;_name')]",  
    "adminUsername": "[parameters('virtualMachines_all_adminUsername')]",  
	"adminPassword": "[parameters('virtualMachines_all_adminPassword')]",

template.json/parameter.json  
"virtualMachines_all_adminUsername": {  
    "value": "vmLogon"  
},  
"virtualMachines_all_adminPassword": {  
    "value": "P@ssw0rd1234"  
},  
  
2. deployment/template.json updates to vm schedules_shutdown resource configuration to remove disallowed uniqueIdentifier property, note that you cannot use/leave comments in template or parameters.json files  
"schedules_shutdown_computevm_emuamvmiisapp_uniqueidentifier": {
    "value": "6a6dc1285f1944dfb830ea3ecb4a7ba3"
},
"taskType": "ComputeVmShutdownTask",
"uniqueIdentifier": "[parameters('schedules_shutdown_computevm_emuamvmiisapp_uniqueidentifier')]"

3. deployment/template.json updates to vm osDisk resource configuration to remove disallowed managedDisk.id property, note that you cannot use/leave comments in template or parameters.json files  
which is good as it means you can remove virtualMachines_&lt;vmName&gt;_id setting from template.json and parameters.json which has subscriptionid and resourcegroupname details  
see https://blogs.msdn.microsoft.com/benjaminperkins/2018/05/16/osdisk-manageddisk-id-is-not-allowed/
"osDisk": {  
    "osType": "Windows",  
    "name": "[concat(parameters('virtualMachines_&lt;vmName&gt;_name'),'_OsDisk_1_addfa1192f7e4f109d72734ec305cee7')]",  
    "createOption": "FromImage",  
    "caching": "ReadWrite",  
    // "managedDisk": {  
    //     "id": "[parameters('virtualMachines_&lt;vmName&gt;_id')]"  
    // }  
}  
  
4. deployment/template.json updates to vm publicIPAddresses resource dnsSettings to enable use of parameters, note that you cannot use/leave comments in template or parameters.json files  
"dnsSettings": {  
    //"domainNameLabel": "emuamvmiisapp",  
    "domainNameLabel": "[parameters('virtualMachines_emuamvmiisapp_name')]",  
    //"fqdn": "emuamvmiisapp.westus2.cloudapp.azure.com"  
    "fqdn": "[concat(parameters('virtualMachines_emuamvmiisapp_name'),'westus2.cloudapp.azure.com')]"  
}  

## lift and shift exploration work using bash .sh scripts
// or set path=%path%;%programfiles(x86)%\Microsoft Visual Studio\Shared\Anaconda3_64;%programfiles(x86)%\Microsoft Visual Studio\Shared\Anaconda3_64\Scripts;%appdata%\Python\Python36\Scripts  
git-bash // or windows subsystem for linux [wsl] store app distribution install terminal session and note that ctrl+u/w/c clears current line not esc like you are used to  
echo $PATH // or printenv PATH  
//https://stackoverflow.com/questions/53563543/bash-shell-access-to-programfilesx86-environment-variable  
//access %programfiles(x86)% environment variable, e.g. echo $ProgramFiles(x86), echo $ProgramFiles\(x86\) and echo $"ProgramFiles(x86)" ???  
//replace every : with nothing and backslash with forward to create path friendly version of environment variables, e.g. echo "${PROGRAMFILES//:\\//} (x86)/Some App InstallDir With Spaces"  
//enabling use in PATH environment variable updates, e.g. PATH=$PATH:"/${PROGRAMFILES//:\\//} (x86)/Some Path With Spaces/"  
PFX86="$(env | sed -n 's/^ProgramFiles(x86)=//p')"; PFX86="${PFX86/:\\//}"; echo $PFX86;  
APPDATAX="${APPDATA//\\//}"; APPDATAX="${APPDATAX/:/}"; echo $APPDATAX    
\# or APPDATAX="$(cygpath $APPDATA)"; echo $APPDATAX # this includes leading forwardslash don't include in path  
PATH=$PATH:"/$PFX86/Microsoft Visual Studio/Shared/Anaconda3_64":"/$PFX86/Microsoft Visual Studio/Shared/Anaconda3_64/Scripts":/$APPDATAX/Python/Python36/Scripts; echo $PATH # or printenv PATH  
// usage: .\deploy.ps1 -i <subscriptionId> -g <resourceGroupName> -l <resourceGroupLocation> -n <deploymentName>  
./deploy.sh -i 1336717a-463c-4c74-b90f-a357edd79989 -g EmUamRgn -l centralus -n EmUamDpn  
**/\*\*\* it appears that bash ./deploy.sh provides better failure case details than powershell \.deploy.ps1 \*\*\*/**

## lift and shift exploration work using powershell .ps1 scripts  
win+s | windows powershell // or powershell.exe -noexit -executionpolicy unrestricted  
//get-executionpolicy -list // or get-executionpolicy -scope currentuser  
//set-executionpolicy -scope currentuser -executionpolicy unrestricted | y[es]  
open parameters.json and provide values for each of the entries noting that they have to be unique across all azure subscriptions  
e.g. <provide the deploy menu option with provide example values and for more info see https://aka.ms/arm-deploy and https://aka.ms/arm-template/#resources references>  
see https://stackoverflow.com/questions/53565723/meaning-and-relevant-values-for-azure-functions-resource-manager-template-parame?noredirect=1#comment94001130_53565723  
// usage: .\deploy.ps1 <subscriptionId> <resourceGroupName> <resourceGroupLocation> <deploymentName>  
// noting that all names must be unique within azure subscription and things like function app, web app and storage account names must be unique across all azure subscriptions  
.\deploy.ps1 1336717a-463c-4c74-b90f-a357edd79989 myRgn centralus myDpn | r[un once] | <enter azure subscription credentials>  
  
## azure functions and web apps openid/oauth security and rbac notes and urls
https://azfndn1.azurewebsites.net/.auth/me to acquire signed in user details like id_token and access_token and after visit
https://azfndn1.azurewebsites.net/.auth/login/aad POST with { "id_token": "&lt;from /.auth/me&gt;", "access_token": "&lt;from /.auth/me&gt;" } to acquire session token 
  for use in X-ZUMO-AUTH header alternative to msal acquired authorization header bearer token which didn't seem to work but using a GET did return
  https://azfndn1.azurewebsites.net/.auth/login/done#token=&lt;json containing authentication_token / X-ZUMO-AUTH value&gt; query string encoded result
https://graph.windows.net/v1.0/me [ azuread graph ] -> https://graph.microsoft.com/v1.0/me [ microsoft graph ]
microsoft graph vs azure active directory [ | azure ad ] graph -> https://blogs.msdn.microsoft.com/aadgraphteam/2016/07/08/microsoft-graph-or-azure-ad-graph/
and https://docs.microsoft.com/en-us/azure/active-directory/identity-protection/graph-get-started  
  
using postman to acquire azuread tokens to attach to web api requests https://www.bruttin.com/2017/11/21/azure-api-postman.html 
and https://learning.getpostman.com/docs/postman/sending_api_requests/authorization/

** the application registration fully qualified scope doesn't have to be followed in msal/postman requests only the suffix and prefix is whatever you want, specifically application(client)id **
** for easyauth and openid connect / oauth services authentication the audience "aud" claim must be the guid of the app you are trying to talk to not the scope prefix **  
** for native app oauth secured calls the native app registration controls common, consumers, organizations, &lt;tenantid&gt; user signins allowed which suggests that 
for browser app openid connect secured calls the web app registration and appsettings.json tenantid setting controls user signins allowed **  
  
azure api app vs web app -> https://stackoverflow.com/questions/31387073/what-is-the-difference-between-an-api-app-and-a-web-app  
Currently all of Web, Mobile and Api Apps are collectively called App Services, see https://azure.microsoft.com/en-us/services/app-service/api | /mobile | /web 
There was a point in time when there were differences between the different app service types, but that is no longer true. The documentation now states: 
The only difference between the three app types (API, web, mobile) is the name and icon used for them in the Azure portal.  
  
asp.net core ilogger injection -> https://stackoverflow.com/questions/30194919/how-to-register-ilogger-for-injection-in-asp-net-mvc-6
  https://stackoverflow.com/questions/51345161/should-i-take-ilogger-iloggert-iloggerfactory-or-iloggerprovider-for-a-libra
  https://simpleinjector.org/blog/2016/06/whats-wrong-with-the-asp-net-core-di-abstraction/  
  https://vuscode.wordpress.com/2009/10/16/inversion-of-control-single-responsibility-principle-and-nikola-s-laws-of-dependency-injection/  

easyauth web apps localhost development -> https://weblogs.asp.net/pglavich/easy-auth-app-service-authentication-using-multiple-providers 
  and https://blogs.msdn.microsoft.com/kaushal/2016/04/01/azure-web-apps-how-to-retrieve-user-email-in-the-claim-when-using-microsoft-account-as-a-provider-in-easy-auth/  
  is application running in visual studio debugger -> https://stackoverflow.com/questions/101806/check-if-application-was-started-from-within-visual-studio
  ConfigureServices access to IHostingEnvironment -> https://stackoverflow.com/questions/37660043/accessing-the-ihostingenvironment-in-configureservices-method
  to enable env and log access from ConfigureServices delegates can use the following but no solution for top level ConfigureServices which executes before Configure
  private IHostingEnvironment env; private ILogger<Startup> log; public void Configure(. . . , ILogger<Startup> log)) { this.env = env; this.log = log; . . . }
  and https://stackoverflow.com/questions/32548948/how-to-get-the-development-staging-production-hosting-environment-in-configurese
  public static class SystemDiagnosticsProcessExtensions {
    public static bool IsRunningFromVisualStudio(this System.Diagnostics.Process currentProcess) {
      //return System.Diagnostics.Debugger.IsAttached;
      //System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
      var moduleName = currentProcess.MainModule.ModuleName; var isRunningFromVisualStudio = moduleName.Contains(".vshost") || moduleName.Contains("qtagent");
      return isRunningFromVisualStudio;
    }
  }
  and for functions solution see https://stackoverflow.com/questions/53688014/function-app-equivalent-for-ihostingenvironment-isdevelopment
  which notes that IHostingEnvironment.IsDevelopment() is just a check for ASPNETCORE_ENVIRONMENT variable, i.e. Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") 
  
asp.net core web api easyauth x-ms-client-principal-id claimsprincipal user -> https://stackoverflow.com/questions/41501612/trouble-getting-claimsprincipal-populated-when-using-easyauth-to-authenticate-ag  
azure .net core app easyauth claims -> https://stackoverflow.com/questions/42260708/azure-apps-easyauth-claims-with-net-core ->  
  similar issue https://stackoverflow.com/questions/41501612/trouble-getting-claimsprincipal-populated-when-using-easyauth-to-authenticate-ag/42260375#42260375
  alternative middleware nuget solution https://stackoverflow.com/questions/41501612/trouble-getting-claimsprincipal-populated-when-using-easyauth-to-authenticate-ag/42402163#42402163 ->
  https://github.com/lpunderscore/azureappservice-authentication-middleware -> https://www.nuget.org/packages/AzureAppserviceAuthenticationMiddleware/
  or better still one with recent commits https://github.com/kirkone/KK.AspNetCore.EasyAuthAuthentication -> https://www.nuget.org/packages/KK.AspNetCore.EasyAuthAuthentication/
  https://github.com/kirkone/KK.AspNetCore.EasyAuthAuthentication/issues/7

webapi supporting both browser/user agent openid connect and desktop/mobile/spa oauth authentication -> https://github.com/MicrosoftDocs/azure-docs/issues/19717 
  which recommended reviewing old vittorio 04/28/18 post on the subject http://www.cloudidentity.com/blog/2014/04/28/use-owin-azure-ad-to-secure-both-mvc-ux-and-web-api-in-the-same-project/ 
  and https://stackoverflow.com/questions/53544037/owin-authorization-header-and-session-cookie-authentication you also created  
  
aad for developers v2 overview https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-overview | getting started | web apps and web apis
  add sign-in to asp.net core web app -> 12/06/18 https://azure.microsoft.com/en-us/resources/samples/active-directory-aspnetcore-webapp-openidconnect-v2/
  protect and asp.net core web api -> 12/06/18 https://azure.microsoft.com/en-us/resources/samples/active-directory-dotnet-native-aspnetcore-v2/
aad application user role -> 09/18/18 https://azure.microsoft.com/en-us/resources/samples/active-directory-dotnet-webapp-roleclaims/ and 
  11/22/17 https://azure.microsoft.com/en-us/resources/samples/active-directory-dotnet-webapp-multitenant-openidconnect/  
aad role permissions https://docs.microsoft.com/en-us/azure/active-directory/users-groups-roles/directory-assign-admin-roles  
graph api scope permissions -> https://msdn.microsoft.com/library/azure/ad/graph/howto/azure-ad-graph-api-permission-scopes  

msal vs adal -> https://stackoverflow.com/questions/40046598/msal-or-adal-library-for-use-with-azure-ad-b2c-and-xamarin  
msal = microsoft authentication library nupkg [ Microsoft.Identity.Client ] and adal = active directory authentication library nupkg [ Microsoft.IdentityModel.Clients.ActiveDirectory ] 
adal is designed to only work against aad classic, not b2c, and adfs 3.0 onward. there are important protocol and feature differences that make the adal object model and protocol 
capabilities incompatible with b2c. msal represents the new generation of microsoft's authentication libraries, designed to work with aad v2 endpoints, msa and b2c  
    
azure web app [ aad security / ] authentication ->  &lt; see aad application user role hit about &gt; and 
  https://docs.microsoft.com/en-us/azure/active-directory/develop/tutorial-v2-asp-webapp   
azure function v1 vs v2 -> https://stackoverflow.com/questions/50114965/azure-functions-v2-support-and-v1-lifetime 
  https://blogs.msdn.microsoft.com/appserviceteam/2017/12/05/announcing-azure-functions-runtime-preview-2/  
azure function httptrigger authorizationlevel.user easyauth -> 02/25/16 to- 11/20/18 https://github.com/Azure/azure-functions-host/issues/33  
  works in v1 based on .net framework story and in just released v2 runtime 2.0.12210.0 based on .net core story  
  as shown in https://github.com/Azure/azure-functions-host/blob/dev/sample/CSharp/HttpTrigger-Identities/run.csx     
  also see related bug https://github.com/Azure/azure-functions-host/issues/3857  
azure functions [ aad security / ] authentication -> 02/19/18 https://blogs.msdn.microsoft.com/stuartleeks/2018/02/19/azure-functions-and-app-service-authentication/  
  and live demo site https://easyauthweb.azurewebsites.net/ and arm deployment script
  ??/??/16 https://docs.microsoft.com/en-us/azure/functions/tutorial-static-website-serverless-api-with-database?tutorial-step=6  
  04/26/16 https://contos.io/working-with-identity-in-an-azure-function-1a981e10b900   
azure functions app settings -> https://docs.microsoft.com/en-us/azure/azure-functions/functions-how-to-use-azure-function-app-settings	 
  plaform features tab | advanced tools (kudu) | https://&lt;myfunctionapp&gt;.scm.azurewebsites.net/, e.g. https://azfndn.scm.azurewebsites.net/ | debug console | cmd  
 
aad developer glossary -> https://docs.microsoft.com/en-us/azure/active-directory/develop/developer-glossary   
  the redirect uri [ optional ] is also referred to as sign-on url by some  
  every application [ / client ] id also has an app id uri, e.g. api://&lt;application id%gt;  
  directory [ / tenant ] id | application [ / client ] id | user [ / ??? ] id are all distinctly different things all of which are typically included in issued jwt token  
  scope names use resource defined strings, resource.operation[.constraint], to enable on-behalf-of user api permissions, aka delegated authorization, with user consent UI  
  role names for "allowedMemberTypes": [ "Application" ] use resource defined strings, resource.operation[.constraint], to enable on-behalf-of processing permissions, aka delegated authorization, with admin consent  
  role names for "allowedMemberTypes": [ "User" ] use persona defined strings, Admin/Approver/Observer/Writer/etc, to enable more managable user permissions, aka authorization, than groups  
  role with "allowedMemberTypes": [ "Application" ] vs [ "User" ] appear to be an admin consent only scope permission  
aad application scope and role permission -> 03/12/17 https://joonasw.net/view/defining-permissions-and-roles-in-aad  
  scope is delegated permission for service principal object downstream api calls and role is permission for user object or service principal object against local resources  
aad application objects and service principal objects -> https://docs.microsoft.com/en-us/azure/active-directory/develop/app-objects-and-service-principals  
  application object 1:1 relationship with the software application and 1:many relationships with its corresponding service principal object(s)  
  application object serves as the template from which common and default properties are derived for use in creating corresponding service principal objects  
  application object is global representation of application across all tenants and service principal is local representation for use in a specific tenant  
  there exist client role and resource server role | application object and service principal object and user principal object  
oauth 2.0 authorization code grant flow -> https://docs.microsoft.com/en-us/azure/active-directory/develop/v1-protocols-oauth-code   
  implicit grant flow | auth code grant | on-behalf-of flow | client credentials grant  
openid connect vs oauth -> https://stackoverflow.com/questions/1087031/whats-the-difference-between-openid-and-oauth  
  openid connect is replacement/extended story based on oauth 2.0 that adds/includes authN in addition to authZ  
openid connect vs openid -> https://security.stackexchange.com/questions/44797/when-do-you-use-openid-vs-openid-connect  
  openid connect is replacement of deprecated openid 2.0 and both are based on oauth  

azure functions remote debug -> https://stackoverflow.com/questions/39343291/remote-debugging-azure-functions-symbols-not-found   
  recommends disabling debug | options | "Enable Just My Code" and "Require source files to exactly match the original version"  
  use publishing profile that doesn't have run from package file (recommended) [ / application settings WEBSITE_RUN_FROM_PACKAGE Remote = 1 ]  
  use publishing profile that uses configuration Debug/AnyCPU output and file publish options | remove additional files at destination  
  interim work around is log.LogInformation("some debugging message"); and portal platform features | monitoring | log streaming | application logs output  
  issue you opened against issue which doesn't repo for v1/dnf but does for v2/dnc https://github.com/Azure/azure-functions-host/issues/3810  
azure functions publishing fails -> https://github.com/Azure/Azure-Functions/issues/506 go to portal and stop function app first then publish  
azure functions publish app settings -> https://docs.microsoft.com/en-us/azure/azure-functions/functions-app-settings  
    
azure functions proxies.json reference -> https://docs.microsoft.com/en-us/azure/azure-functions/functions-proxies#advanced-configuration  
azure functions host.json reference -> https://docs.microsoft.com/en-us/azure/azure-functions/functions-host-json  
azure functions local.settings.json reference -> https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local#local-settings-file  

azure functions linux containers -> https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-function-linux-custom-image  
azure functions iot edge module -> https://docs.microsoft.com/en-us/azure/iot-edge/tutorial-deploy-function?toc=%2fazure%2fazure-functions%2ftoc.json  
azure functions iot ring door bell integration -> https://medium.com/@jeffhollan/serverless-doorbell-azure-functions-and-ring-com-f24b44e01645  
     
azure functions including dll in deployment -> https://blogs.msdn.microsoft.com/benjaminperkins/2017/04/13/how-to-add-assembly-references-to-an-azure-function-app/
where you added question about this on 20nov, then deleted and added updated one on 22nov to see if you could get some leads on this matter 
manual work around deployment of c++ native code dll with c# [DllImport] referenced functions -> azSxp | <azfn storage account> | file shares | 
<azfn app deployment>/site/wwwroot/bin | upload | <c++ native code dll>
twitter post that you deleted = @cecilphillip watched your talk on azure .net core functions . Have a scenario where I need to use platform invoke 
[DllImport] to enable calling some legacy c++ native code dll exported math functions. Works locally, any pointers on how to get that dll included in 
deployment . . . note that I can get things to work if I use azure storage explorer and upload the c++ native code dll with [DllImport] referenced 
functions to azSxp | <azfn storage account> | file shares | <azfn app deployment>/site/wwwroot/bin | upload | <c++ native code dll>  
  
requests for ability to use c++/cli managed assemblies in functions [ / .net core ] -> https://github.com/Azure/Azure-Functions/issues/68 -? https://github.com/Azure/azure-functions-host/issues/1470  
  
go serverless with azure c# functions talk https://www.youtube.com/watch?v=2ZGYLblGZQA by cecil philip cloud developer advocate https://www.linkedin.com/in/cecil-phillip/ 
associated demo site http://dnc-todos.azurewebsites.net/ and repo https://github.com/anthonychu/ToDoFunctions
at t=28m30s mark [ to t=30m50s mark ] the walkthrough discusses azfn proxy settings use of blob storage to host static and spa web app content as part of service  
at t=39m0s mark covers how you use telementryClient, e.g. telementryClient.TrackEvent/TrackException, in lieu of your own logger api
recommended using postman, not fiddler, for http endpoint test and debugging services
additional resources https://azure.com/serverless and https://functions.azure.com/try
dos and dont's of azure functions https://www.youtube.com/watch?v=kvTostlJp7M by jeff hollan senior pm https://www.linkedin.com/in/jeffhollan/
functions getting started https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-your-first-function-visual-studio  
 
## azure functions inclusion of c++ native code libraries 
must be accessed using functions v2 .net core compatible platform invoke DllImport attribute and not functions v1 .net framework only compatible c++/cli managed assembly build output reference  
https://stackoverflow.com/questions/tagged/azure-functions -> https://social.msdn.microsoft.com/forums/azure/en-us/home?forum=azurefunctions -> https://github.com/azure/azure-functions/issues  
https://stackoverflow.com/questions/53643543/include-c-c-unmanaged-code-dll-consumed-using-dllimport-in-azure-functions-p/53643896   
https://social.msdn.microsoft.com/Forums/en-US/8e8340ee-3963-4b10-b8f5-1e139fd106ee/include-cc-unmanaged-code-dll-consumed-using-dllimport-in-azure-functions-publish-process?forum=AzureFunctions  
https://github.com/Azure/Azure-Functions/issues/1061  
function project | pack puts files in $(ProjectDir)\bin\$(Configuration)\netcoreapp2.1\publish\bin 
function project | publish output can be viewed in $(ProjectDir)\obj\$(Configuration)\netcoreapp2.1\PubTmp\Out\bin

current workaround is use azSxp to upload Dll1.dll and dbd Dll1.dll output %windir%\System32\vcruntime140d.dll + ucrtbased.dll [ if Debug build vs 
vcruntime140d.dll & ucrtbase.dll that should already be a part of host process environment path if Release build ]  
  
## c# managed code access to c++ native code library wrapped legacy dynamic link library (.dll) and/or static library (.lib)
https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/interop/interoperability-overview and https://docs.microsoft.com/en-us/cpp/build/reference/clr-common-language-runtime-compilation  
1. platform invoke c# [DllImport] of c++ | windows desktop | dll "class __declspec(dllexport) className" or "extern "C" declexport class returnType functionName" entries  
2. c++/cli /clr managed assembly "it just works" [ijw] build output that is directly referencable like any managed code assembly  
  
https://www.codeproject.com/articles/19354/quick-c-cli-learn-c-cli-in-less-than-minutes  
c++/cli [ common language infrastructure ] usage details specifically use or ref keywoard to declare a managed class and use of ^ punctuator to allocate managed objects  
  
https://stackoverflow.com/questions/53419367/pinvoke-marshalling-of-2d-multidimensional-array-of-type-double-as-input-and-out  
pinvoke marshalling of 2d multidimensional array of type double as input and output between c# and c++  
  
&lt;default visual c++ | windows desktop | dynamic-link library (dll) project template&gt; | properties | configuration properties | 
1. general | configuration = All Configurations and platforms = All Platforms 
common language runtime support = No Common Runtime Language Support -> Common Language Runtime Support (/clr)
2. c/c++ | configuration = All Configurations and platforms = All Platforms 
//general | common language runtime support = <unset> -> Common Language Runtime Support (/clr) // this should have been set by previous step don't do it here as it breaks
//things leading you change other settings, e.g. projectname.vcproj CompileAsManaged, ExceptionHandling, DebugInformationFormat, BasicRuntimeChecks, to get things to build
//that are unnecessary when step 1 change is used
precompiled headers | precompiled header = Use (/Yu) -> Not Using Precompiled Headers // == &lt;PrecompiledHeader&gt;Use -&gt; NotUsing&lt;/PrecompiledHeader&gt; and not required
all options | additional options | /Zc:twoPhase- %(AdditionalOptions) [ or should it be $(AdditionalOptions) or is that suffix even required since things build and run w/o it ]
3. &lt;default visual c++ | windows desktop | dynamic-link library (dll) project template&gt; |
Source Files | dllmain.cpp [ , stdafx.cpp, &lt;dll project name&gt;.cpp ] | delete 
// Header Files | stdafx.h, targetver.h | delete   
4. add | new item | c++ class | NewClass | other options Managed = checked | 
NewClass.h | ref class NewClass -> public ref class NewClass [ sealed ] + optionally namespace NewNamespace { . . . } wrapper
NewClass.cpp | optionally using namespace NewNamespace; if used in header file 
using namespace System; // to enable use of managed types in variable and method parameter and return type declarations, e.g. String^ mystring = gcnew String("some string value");  
suffix reference[/instance] type declarations with ^ punctuator to have them allocated on the cli[/managed] heap but not value[/integral] types as doing so will cause them to be passed as System.ValueType<T> instead
#include <vector> // to enable use of c++ native code arrays, e.g. jagged array std::vector<std::vector<double>> vs managed form array<double, 2>^  

with Source Files | dllmain.cpp left in place you get build warning C4747: Calling managed 'DllMain': Managed code may not be run under loader lock, including the DLL entrypoint and calls reached from the DLL entrypoint  
and you get following runtime alert and process crash result "Managed Debugging Assistant 'LoaderLock' : 'DLL 'fqdn path to dll' is attempting managed execution inside OS Loader lock. Do not attempt to run managed code 
inside a DllMain or image initialization function since doing so can cause the application to hang.'" with a call to this class in place which can be made to go away using using debug | exception settings | 
managed debugging assistants | loaderloack = checked -> unchecked to suppress message is not a fix as you then get "The program '[20804] testhost.x86.exe' has exited with code -532462766 (0xe0434352)".  the fix is to 
remove <default visual c++ | windows desktop | dynamic-link library (dll) project template> | dllmain.cpp and "class __declspec(dllexport) className" or "extern "C" declexport class returnType functionName" entries  

without c/c++ | all options | additional options | /Zc:twoPhase- %(AdditionalOptions) [ and have Source Files | stdafx.cpp, &lt;dll project name&gt;.cpp left in place you will see build warning C4199: two-phase name 
lookup is not supported for C++/CLI, C++/CX, or OpenMP; use /Zc:twoPhase-  

.net core c/c++ native code library access currently only works with platform invoke c# [DllImport] option and not c++/cli /clr managed assembly "it just works" [ijw] build output  
.net core the c++ module failed to load [ EntryPointNotFoundException: A library name must be specified in a DllImport attribute applied to non-IJW methods ] -> 
https://stackoverflow.com/questions/51958187/managed-c-with-net-core-2-1 -> https://github.com/dotnet/coreclr/issues/659  
  
c++ project template rationalization of "Output Directory" | $(OutDir) | $(OutputPath) setting  
Win32 = $(SolutionDir)$(Configuration)\$(MSBuildProjectName)\  
ARM, ARM64, x64 = $(SolutionDir)$(Platform)\$(Configuration)\$(MSBuildProjectName)\  
windows universal unit test app |  
  build | output path setting = bin\<ARM, ARM64, x64, x86>\<Debug, Release>, i.e. hardcoded bin\$(Platform)\$(Configuration)\  
  debug | layout folder path setting = $(ProjectDir)\<ARM, ARM64, x64, x86>\<Debug, Release>\AppX, i.e. hardcoded $(ProjectDir)bin\$(Platform)\$(Configuration)\AppX  
AnyCPU managed code project templates hardcoded related setting = bin\$(Configuration)\[netcoreapp2.1]  
setting rooted in project folder that doesn't treat Win32 like AnyCPU = bin\$(Platform)\$(Configuration)\   

c++ project template rationalization of "Intermediate Directory" | $(IntDir) | $(IntermediateOutputPath) setting
Win32 = $(Configuration)\
ARM, ARM64, x64 = $(Platform)\$(Configuration)\
windows universal unit test app | ??? = obj\<ARM, ARM64, x64, x86>\<Debug, Release>, i.e. hardcoded obj\$(Platform)\$(Configuration)\  
AnyCPU managed code project templates hardcoded related setting = obj\$(Configuration)\[netcoreapp2.1]  
setting rooted in project folder that doesn't treat Win32 like AnyCPU = obj\$(Platform)\$(Configuration)\  

to make $(OutDir) and $(IntDir) setting changes use &lt;project&gt; | properties | configuration = All Configurations and 
platforms = All Platforms at which point you can replace "&lt;different options&gt;" value you'll see in $(OutDir) and 
$(IntDir) boxes with the recommended ones above  
   
filever CnslApp.Dnf.Tests.exe -> W32i always with AnyCPU  
filever Dll2.dll -> W32i or W32x64  
dumpbin /dependents Dll2.dll -> VCRUNTIME140D.dll ucrtbased.dll KERNEL32.dll mscoree.dll  
Environment.Is64BitProcess -> dnc dotnet.exe process is always 64bit even though AnyCPU output is W32i not W32x64 format  
Environment.Is64BitProcess -> dnf process is x86 when launched from vs17, given its x86, and x64 elsewhere ???  
test | test settings | default processor architecture | x86 only affects .net framework test runs not .net core ones  
   
