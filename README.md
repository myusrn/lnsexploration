# c++ native code api exports and managed code access

tests of managed code access to c++ native code dll wrapped legacy static library (.lib) or dynamic link library (.dll) apis using
1. platform invoke c# [DllImport] / c++ dll extern "C" __declspec( dllexport )  
2. dll /clr build output which is supposed to create a directly consumable managed code solution

c++ project template rationalization of "Output Directory" | $(OutDir) | $(OutputPath) setting
Win32 = $(SolutionDir)$(Configuration)\$(MSBuildProjectName)\
ARM, ARM64, x64 = $(SolutionDir)$(Platform)\$(Configuration)\$(MSBuildProjectName)\
AnyCPU managed code project templates hardcoded related setting = bin\$(Configuration)\[netcoreapp2.1]
setting rooted in project folder that doesn't treat Win32 like AnyCPU = bin\$(Configuration)\$(Platform)\

c++ project template rationalization of "Intermediate Directory" | $(IntDir) | $(IntermediateOutputPath) setting
Win32 = $(Configuration)\
ARM, ARM64, x64 = $(Platform)\$(Configuration)\
AnyCPU managed code project templates hardcoded related setting = obj\$(Configuration)\[netcoreapp2.1]
setting rooted in project folder that doesn't treat Win32 like AnyCPU = obj\$(Configuration)\$(Platform)\

to make $(OutDir) and $(IntDir) setting changes use &lt;project&gt; | properties | configuration = All Configurations and 
platforms = All Platforms at which point you can replace "&lt;different options&gt;" value you'll see in $(OutDir) and 
$(IntDir) boxes with the recommended ones above

https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/interop/interoperability-overview which says use 
c++ interop, aka it just works (ijw), compiler /clr switch option vs platform invoke or com options  
&lt;dll project&gt; | properties | configuration properties | c/c++ |
1. configuration = All Configurations and platforms = All Platforms 
general | common language runtime support = <unset> -> common language runtime support (/clr)  
code generation | enable c++ extensions = Yes (/EHsc) -> Yes with SEH Exceptions (/EHa)    
2. configuration = Debug and platforms = All Platforms
general | debug information format = program database for edit and continue (/ZI) -> program database (/Zi)  
code generation | basice runtime checks = Both (/RTC1, equiv. to /RTCsu) (/RTC1) -> Default  
   
