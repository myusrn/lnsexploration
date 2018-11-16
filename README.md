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

