set dotnet_publish_cmd=dotnet publish --configuration Release --framework netcoreapp3.1 --self-contained false --verbosity quiet

rem Clean binaries-directory
del /S /Q binaries\*
rmdir /S /Q binaries\*

rem Release Win
%dotnet_publish_cmd% --runtime win-x86 --output binaries/win-x86
%dotnet_publish_cmd% --runtime win-x64 --output binaries/win-x64

rem Release Linux
%dotnet_publish_cmd% --runtime linux-x64 --output binaries/linux-x64

rem Release macOS
%dotnet_publish_cmd% --runtime osx-x64 --output binaries/osx-x64