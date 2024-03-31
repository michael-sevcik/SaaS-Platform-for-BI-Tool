# We are using for the mcr.microsoft.com/dotnet/sdk:8.0 to build the app

# First we copy the project files to the container and restore the dependencies then we run the build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ManagamentApp/ManagamentApp.csproj", "ManagamentApp/"]
COPY ["Common/Application.Contracts/Application.Contracts.csproj", "Common/Application.Contracts/"]
COPY ["Common/Shared/Shared.csproj", "Common/Shared/"]
COPY ["Common/Application/Application.csproj", "Common/Application/"]
COPY ["Common/Domain/Domain.csproj", "Common/Domain/"]
COPY ["Modules/DataIntegration/MapperPages/MapperPages.csproj", "Modules/DataIntegration/MapperPages/"]
RUN dotnet restore "ManagamentApp/ManagamentApp.csproj"
COPY . .
WORKDIR "/src/ManagamentApp"
RUN dotnet build "./ManagamentApp.csproj" -c Release -o /app/build

# Here we publish the app and set the entrypoint
FROM build AS publish
RUN dotnet publish "./ManagamentApp.csproj" -c Release -o /app/publish /p:UseAppHost=false
EXPOSE 8080
EXPOSE 8081
ENTRYPOINT ["dotnet", "/app/publish/ManagamentApp.dll"]
