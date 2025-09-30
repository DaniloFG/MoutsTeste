
Build stage
FROM https://www.google.com/search?q=mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY ./.sln .
COPY src/Api/.csproj src/Api/
COPY src/Application/.csproj src/Application/
COPY src/Domain/.csproj src/Domain/
COPY src/Infrastructure/*.csproj src/Infrastructure/
RUN dotnet restore "DeveloperStoreTeam.sln"
COPY . .
WORKDIR "/source/src/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

Publish stage
FROM build AS publish
WORKDIR "/source/src/Api"
RUN dotnet publish "Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
