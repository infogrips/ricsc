# Checkservice Commander (RICSC .NET)

1. Documentation

   The documentation can be found under /doc and includes:
   - RICS Commander usage
   - API Documentation for the Checkservice

2. Base usage

```batch
   ! CMD (Windows)
   cd %ricsc_dir%
   ricsc -service <service-name> -user <user-name> -command
      <command> -file <file-name-or-pattern> 

   ! Terminal (Linux and macOS)
   cd %ricsc_dir%
   ./ricsc -service <service-name> -user <user-name> -command
      <command> -file <file-name-or-pattern> 
```  

3. Binaries

   Binaries are built for .NET Core 3.1. The .NET Core must be installed on the client machine.

   If you prefer to get the standalone version, you can do this by downloading the package from our [Download Portal](https://www.infogrips.ch/download.html) .
   (Windows, Linux and macOS)