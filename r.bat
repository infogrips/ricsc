set dotnet_run_cmd=dotnet run

rem Run rics-command

rem Command send
rem -service <service-name> -user <user-name> -command send -file <path-to-file>

rem Command check_level
rem -service <service-name> -user <user-name> -command check_level -file <path-to-file>

rem Command get_log
rem -service <service-name> -user <user-name> -command get_log -file <path-to-file>

%dotnet_run_cmd% %*