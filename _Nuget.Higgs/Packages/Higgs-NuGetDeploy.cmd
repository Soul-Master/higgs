cd "Higgs RIA Framework\_Nuget.Higgs\Packages"
..\..\.nuget\nuget.exe setApiKey e213594f-f207-44f0-b80c-37daf5b5735e
..\..\.nuget\nuget.exe push *.nupkg
..\..\.nuget\nuget.exe push *.nupkg -Source http://nuget.gw.symbolsource.org/Public/NuGet