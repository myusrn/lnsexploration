# lift and shift exploration work

## azure functions http triggered use of c++ native code functions

go serverless with azure c# functions talk https://www.youtube.com/watch?v=2ZGYLblGZQA by cecil philip cloud developer advocate https://www.linkedin.com/in/cecil-phillip/
associated demo site http://dnc-todos.azurewebsites.net/ and repo https://github.com/anthonychu/ToDoFunctions
at t=28m30s mark [ to t=30m50s mark ] the walkthrough discusses azfn proxy settings use of blob storage to host static and spa web app content as part of service  
at t=39m0s mark covers how you use telementryClient, e.g. telementryClient.TrackEvent/TrackException, in lieu of your own logger api
recommended using postman, not fiddler, for http endpoint test and debugging services
additional resources https://azure.com/serverless and https://functions.azure.com/try
dos and dont's of azure functions https://www.youtube.com/watch?v=kvTostlJp7M by jeff hollan senior pm https://www.linkedin.com/in/jeffhollan/
azure functions include dll -> https://social.msdn.microsoft.com/Forums/windowsapps/en-US/1b6e2eda-51a7-437d-b859-8707cda78ac9/adding-custom-dll-in-azure-functions -> 
https://code.msdn.microsoft.com/custom-dll-reference-c2c9b57e
add the ability to load native c++ libraries (dll) -> https://github.com/Azure/Azure-Functions/issues/68 | see ask you added to thread
manual work around deployment of c++ native code dll with c# [DllImport] referenced functions -> azSxp | <azfn storage account> | file shares | 
<azfn app deployment>/site/wwwroot/bin | upload | <c++ native code dll>
twitter post for help = @cecilphillip watched your talk on azure .net core functions . Have a scenario where I need to use platform invoke [DllImport] to enable calling some legacy c++ native code dll exported math functions. Works locally, any pointers on how to get that dll included in deployment.
. . . note that I can get things to work if I use azure storage explorer and upload the c++ native code dll with [DllImport] referenced functions to <azfn storage account> | file shares | <azfn app deployment>/site/wwwroot/bin | upload | <c++ native code dll>

  
## c++ native code api exports and managed code access

tests of managed code access to c++ native code dll wrapped legacy static library (.lib) or dynamic link library (.dll) apis using
1. platform invoke c# [DllImport] / c++ dll extern "C" __declspec( dllexport )  
2. c++ dll /clr generated managed code build output that is directly referencable  
  
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

c# c++ /clr [ it just works / ijw ] -> 
https://msdn.microsoft.com/en-us/library/k8d11d4s.aspx | latest version of this topic can be found at -> 
https://docs.microsoft.com/en-us/cpp/build/reference/clr-common-language-runtime-compilation?view=vs-2017
  
https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/interop/interoperability-overview which says use 
c++ interop, aka it just works (ijw), compiler /clr switch option vs platform invoke or com options  
&lt;dll project&gt; | properties | configuration properties | c/c++ |
1. configuration = All Configurations and platforms = All Platforms 
general | common language runtime support = <unset> -> common language runtime support (/clr)  
code generation | enable c++ extensions = Yes (/EHsc) -> Yes with SEH Exceptions (/EHa)    
2. configuration = Debug and platforms = All Platforms
general | debug information format = program database for edit and continue (/ZI) -> program database (/Zi)  
code generation | basice runtime checks = Both (/RTC1, equiv. to /RTCsu) (/RTC1) -> Default  
  
Managed Debugging Assistant 'LoaderLock' : 'DLL 'D:\src\repos\wrapTest\CnslApp.Dnf.Tests\bin\Debug\Dll2.dll' is attempting managed execution inside OS Loader lock. Do not attempt to run managed code inside a DllMain or image initialization function since doing so can cause the application to hang.'  
/clr attempting managed execution inside OS Loader lock -> 
https://stackoverflow.com/questions/23689521/os-loader-lock-when-doing-managed-to-native-interop  
debug | windows | exception settings | managed debugging assistants | LoaderLock = checked -> unchecked

Unhandled Exception: System.IO.FileLoadException: Could not load file or assembly 'Dll2.dll' or one of its dependencies. A dynamic link library (DLL) initialization routine failed. (Exception from HRESULT: 0x8007045A)
/clr Could not load file or assembly 0x8007045A ->   
    
filever CnslApp.Dnf.Tests.exe -> W32i always with AnyCPU  
filever Dll2.dll -> W32i or W32x64  
dumpbin /dependents Dll2.dll -> VCRUNTIME140D.dll ucrtbased.dll KERNEL32.dll mscoree.dll  
Environment.Is64BitProcess -> dnc dotnet.exe process is always 64bit even though AnyCPU output is W32i not W32x64 format  
Environment.Is64BitProcess -> dnf process is x86 when launched from vs17, given its x86, and x64 elsewhere ???  
test | test settings | default processor architecture | x86 only affects .net framework test runs not .net core ones  
   
