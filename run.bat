@echo off

set DEV=false
set CUR_DIR=%cd%

if "%OS%"=="Windows_NT" (
    set RUN_IN_TERMINAL=start cmd /k
)

if "%DEV%"=="true" (
    set DOTNET_RUN_CMD=dotnet watch
) else (
    set DOTNET_RUN_CMD=dotnet run
)

:run
echo Starting all services...
@REM %RUN_IN_TERMINAL% :api 
@REM %RUN_IN_TERMINAL% goto :web 
start cmd /c :api
start cmd /c :web
echo end...
goto :eof

:console
echo Running Console app...
%RUN_IN_TERMINAL%  "cd Heatington.Console && %DOTNET_RUN_CMD%"
echo Console app terminated
goto :eof

:web
echo Running Web app...
%RUN_IN_TERMINAL% "cd Heatington.Web && %DOTNET_RUN_CMD%"
echo Web app is terminated
goto :eof

:api
echo Running APIs...
%RUN_IN_TERMINAL%  "cd %CUR_DIR%/Heatington.Microservice.OPT && %DOTNET_RUN_CMD%"
%RUN_IN_TERMINAL%  "cd %CUR_DIR%/Heatington.Microservice.AM && %DOTNET_RUN_CMD%"
%RUN_IN_TERMINAL%  "cd %CUR_DIR%/Heatington.Microservice.SDM && %DOTNET_RUN_CMD%"
%RUN_IN_TERMINAL%  "cd %CUR_DIR%/Heatington.Microservice.RDM && %DOTNET_RUN_CMD%"
echo APIs terminated!
goto :eof
