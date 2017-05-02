SET msbuild="C:\Program Files (x86)\MSBuild\15.0\Bin\msbuild.exe"

%msbuild% ..\src\ChannelAdam.Serilog.Sinks.MicrosoftExtensionsLoggingLogger\ChannelAdam.Serilog.Sinks.MicrosoftExtensionsLoggingLogger.csproj /t:Rebuild /p:Configuration=Release;OutDir=bin\Release

..\tools\nuget\nuget.exe pack ..\src\ChannelAdam.Serilog.Sinks.MicrosoftExtensionsLoggingLogger\ChannelAdam.Serilog.Sinks.MicrosoftExtensionsLoggingLogger.csproj -Symbols

pause
