@echo off
start cmd /c dotnet run --project ./Heatington.Microservices.AM/Heatington.Microservices.AM.csproj
start cmd /c dotnet run --project ./Heatington.Microservices.SDM/Heatington.Microservices.SDM.csproj
start cmd /c dotnet run --project ./Heatington.Microservice.OPT/Heatington.Microservice.OPT.csproj
start cmd /c dotnet run --project ./Heatington.Microservices.RDM/Heatington.Microservices.RDM.csproj
start cmd /c dotnet run --project ./Heatington.Web/Heatington.Web.csproj
