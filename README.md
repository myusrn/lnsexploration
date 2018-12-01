## lift and shift exploration work using bash shell .sh scripts
// or set path=%path%;%programfiles(x86)%\Microsoft Visual Studio\Shared\Anaconda3_64;%programfiles(x86)%\Microsoft Visual Studio\Shared\Anaconda3_64\Scripts;%appdata%\Python\Python36\Scripts
gbash.exe // or windows subsystem for linux [wsl] store app distribution install terminal session
echo $PATH // or printenv PATH
//https://stackoverflow.com/questions/53563543/bash-shell-access-to-programfilesx86-environment-variable
//access %programfiles(x86)% environment variable, e.g. echo $ProgramFiles(x86), echo $ProgramFiles\(x86\) and echo $"ProgramFiles(x86)" ???
//replace every : with nothing and backslash with forward to create path friendly version of environment variables, e.g. echo "${PROGRAMFILES//:\\//} (x86)/Some App InstallDir With Spaces"
//enabling use in PATH environment variable updates, e.g. PATH=$PATH:"/${PROGRAMFILES//:\\//} (x86)/Some Path With Spaces/"
// or PATH=$PATH:"/c/Program Files (x86)/Microsoft Visual Studio/Shared/Anaconda3_64":"/c/Program Files (x86)/Microsoft Visual Studio/Shared/Anaconda3_64/Scripts":/c/Users/ob1/AppData/Roaming/Python/Python36/Scripts
PATH=$PATH:"/${PROGRAMFILES//:\\//} (x86)/Microsoft Visual Studio/Shared/Anaconda3_64":"/${PROGRAMFILES//:\\//} (x86)/Microsoft Visual Studio/Shared/Anaconda3_64/Scripts":/${APPDATA//:\\//}/Python/Python36/Scripts
// or PFX86="${PROGRAMFILES//:\\//} (x86)" & PATH=$PATH:"/$PFX86/Microsoft Visual Studio/Shared/Anaconda3_64":"/$PFX86/Microsoft Visual Studio/Shared/Anaconda3_64/Scripts":/${APPDATA//:\\//}/Python/Python36/Scripts
echo $PATH // or printenv PATH
// usage: .\deploy.ps1 -i <subscriptionId> -g <resourceGroupName> -n <deploymentName> -l <resourceGroupLocation>  
./deploy.sh -i 1336717a-463c-4c74-b90f-a357edd79989 -g exnmRgn -n exnmDpn -l centralus  
  
## lift and shift exploration work using powershell .ps1 scripts  
win+s | windows powershell // or powershell.exe -noexit -executionpolicy unrestricted 
//get-executionpolicy -list // or get-executionpolicy -scope currentuser
//set-executionpolicy -scope currentuser -executionpolicy unrestricted | y[es]
open parameters.json and provide values for each of the entries noting that they have to be unique across all azure subscriptions
e.g. <provide example values here and for more info see https://aka.ms/arm-deploy and https://aka.ms/arm-template/#resources references>
// usage: .\deploy.ps1 <subscriptionId> <resourceGroupName> <deploymentName> <resourceGroupLocation>
// noting that all names must be unique within azure subscription and things like function app, web app and storage account names must be unique across all azure subscriptions
.\deploy.ps1 1336717a-463c-4c74-b90f-a357edd79989 exnmRgn exnmDpn centralus | r[un once] | <enter azure subscription credentials>
  
## azure functions and web apps openid/oauth security and rbac 
owin authorization header and session cookie authentication -> https://github.com/MicrosoftDocs/azure-docs/issues/19717 
  and https://stackoverflow.com/questions/53544037/owin-authorization-header-and-session-cookie-authentication  
aad developers guide [ https://docs.microsoft.com/en-us/azure/active-directory/develop/ ] | v2.0 |  
  quickstarts | mobile and desktop apps | windows desktop | 11/14/18 https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-v2-windows-desktop  
  quickstarts | web apps | asp.net core | 11/10/18 https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-v2-aspnet-core-webapp
  quickstarts | web apis | asp.net core | 11/10/18 https://azure.microsoft.com/en-us/resources/samples/active-directory-dotnet-native-aspnetcore-v2/  
  tutorials | mobile and desktop apps | windows desktop | 09/18/18 https://docs.microsoft.com/en-us/azure/active-directory/develop/tutorial-v2-windows-desktop  
  tutorials | web apps | asp.net | 09/10/18 https://docs.microsoft.com/en-us/azure/active-directory/develop/tutorial-v2-asp-webapp  
  how to guides | application configuration | add app roles | 09/23/18 https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-add-app-roles-in-azure-ad-apps  
  how to guides | authentication | configure role claims | 10/04/18 https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-enterprise-app-role-management  
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
  works in v1 and in v2 in soon to be released 1.0.24+ of Microsoft.NET.Sdk.Functions nupkg according to this sample 
  https://github.com/Azure/azure-functions-host/blob/dev/sample/CSharp/HttpTrigger-Identities/run.csx  
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
must be accessed using .net core compatible platform invoke DllImport attribute and not .net framework only compatible c++/cli managed assembly build output reference

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
   
