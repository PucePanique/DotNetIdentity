# ----------------------
# Étape 1 : Build
# ----------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copier les fichiers csproj et restaurer les dépendances (cache)
COPY DotNetIdentity/*.csproj ./
RUN dotnet restore

# Copier le reste du code et build
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# ----------------------
# Étape 2 : Runtime
# ----------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Ports exposés
EXPOSE 80
EXPOSE 443

# Remplace "DotNetIdentity.dll" par le nom de ton assembly principal
ENTRYPOINT ["dotnet", "DotNetIdentity.dll"]
