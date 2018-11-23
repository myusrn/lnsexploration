# lift and shift exploration work

## azure functions http triggered use of c++ native code libraries
aad developers guide [ https://docs.microsoft.com/en-us/azure/active-directory/develop/ ] | v2.0 | 
  quickstarts | web apis | asp.net core | 11/10/18 https://azure.microsoft.com/en-us/resources/samples/active-directory-dotnet-native-aspnetcore-v2/
  how to guides | authentication | configure role claims | 10/04/18 https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-enterprise-app-role-management
aad application user role -> 09/18/18 https://azure.microsoft.com/en-us/resources/samples/active-directory-dotnet-webapp-roleclaims/ 
aad role permissions https://docs.microsoft.com/en-us/azure/active-directory/users-groups-roles/directory-assign-admin-roles  
graph api scope permissions -> https://msdn.microsoft.com/library/azure/ad/graph/howto/azure-ad-graph-api-permission-scopes  
  
azure web app [ aad security / ] authentication ->  &lt; see aad application user role hit about &gt; and
  https://docs.microsoft.com/en-us/azure/active-directory/develop/tutorial-v2-asp-webapp  
azure functions [ aad security / ] authentication -> 02/19/18 https://blogs.msdn.microsoft.com/stuartleeks/2018/02/19/azure-functions-and-app-service-authentication/ 
  04/26/16 https://contos.io/working-with-identity-in-an-azure-function-1a981e10b900  
azure functions app settings -> https://docs.microsoft.com/en-us/azure/azure-functions/functions-how-to-use-azure-function-app-settings	
  plaform features tab | advanced tools (kudu) | https://&lt;myfunctionapp&gt;.scm.azurewebsites.net/, e.g. https://azfndn.scm.azurewebsites.net/ | debug console | cmd  
  
aad developer glossary -> https://docs.microsoft.com/en-us/azure/active-directory/develop/developer-glossary 
  where it highlights fact that application id is aka client id which is different than a user id claim which is typically present in isssued tokens
  roles / rbac and scopes / sbac are both solutions to enabling application permissions, aka authorization
  roles are respource defined strings, resource.role, can be "user" role which allow for users/groups as members and "application" role which allows for client applications as members
  scopes are resource definded strings,resource.operation.constraint, like roles / role based access control [rbac] they enable scopes / scope based access control [sbac] 
  e.g. mail.read or directory.readwrite.all, vs rbac typically covers just resource.operation and this is hard aspect of authZ that crm/erp platforms cover
aad client resource server role application service principal object -> https://docs.microsoft.com/en-us/azure/active-directory/develop/app-objects-and-service-principals 
  client role and resource server role | application object and service principal object and user principal object
oauth 2.0 authorization code grant flow -> https://docs.microsoft.com/en-us/azure/active-directory/develop/v1-protocols-oauth-code 
  implicit grant flow | auth code grant | on-behalf-of flow | client credentials grant  
openid connect vs oauth -> https://stackoverflow.com/questions/1087031/whats-the-difference-between-openid-and-oauth
  openid connect is replacement/extended story based on oauth 2.0 that adds/includes authN in addition to authZ
http://cakebaker.42dh.com/2008/04/01/openid-versus-oauth-from-the-users-perspective/  
openid connect vs openid -> https://security.stackexchange.com/questions/44797/when-do-you-use-openid-vs-openid-connect 
  openid connect is replacement of deprecated openid 2.0 and both are based on oauth
    
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
   
