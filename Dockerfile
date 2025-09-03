# ----------------------
# Étape 1 : Build
# ----------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Variables utiles
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    DOTNET_NOLOGO=true \
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1

WORKDIR /src

# Copier le .csproj (cache restore)
COPY DotNetIdentity/*.csproj ./DotNetIdentity/
RUN dotnet restore ./DotNetIdentity/*.csproj

# Copier le reste du code
COPY . .

# Publier l'app
RUN dotnet publish ./DotNetIdentity/*.csproj -c Release -o /app/publish

# Installer l’outil EF et exposer le PATH
RUN dotnet tool install --global dotnet-ef --version 8.*
ENV PATH="/root/.dotnet/tools:${PATH}"

# (optionnel) vérifier la version
RUN dotnet ef --version

# Générer le bundle EF (avec logs détaillés)
RUN dotnet ef --verbose migrations bundle \
    --project ./DotNetIdentity/DotNetIdentity.csproj \
    --startup-project ./DotNetIdentity/DotNetIdentity.csproj \
    --configuration Release \
    --context DotNetIdentity.Data.AppDbContextSqlServer \
    --target-runtime linux-x64 \
    --output /app/migrator

# ----------------------
# Étape 2 : Runtime
# ----------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish ./
COPY --from=build /app/migrator /app/migrator

# Script d'entrypoint (adapte le chemin si besoin)
COPY entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["/app/entrypoint.sh"]
