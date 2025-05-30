FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all .csproj files into their correct paths
COPY ["NewManagementSystem/NewsManagementSystem.WebMVC.csproj", "NewManagementSystem/"]
COPY ["NewsManagementSystem.BLL/NewsManagementSystem.BusinessObject.csproj", "NewsManagementSystem.BLL/"]
COPY ["NewsManagementSystem.DAL/NewsManagementSystem.DataAccess.csproj", "NewsManagementSystem.DAL/"]
COPY ["NewsManagementSystem.Services/NewsManagementSystem.Services.csproj", "NewsManagementSystem.Services/"]

# Restore
RUN dotnet restore "NewManagementSystem/NewsManagementSystem.WebMVC.csproj"

# Copy everything for the build
COPY . .

WORKDIR "/src/NewManagementSystem"
RUN dotnet build "NewsManagementSystem.WebMVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NewsManagementSystem.WebMVC.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NewsManagementSystem.WebMVC.dll"]
