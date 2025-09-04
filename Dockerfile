# ----------------------
# Étape 1 : Build
# ----------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1 DOTNET_NOLOGO=true DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1

WORKDIR /src

# Restore (cache-friendly)
COPY DotNetIdentity/*.csproj ./DotNetIdentity/
RUN dotnet restore ./DotNetIdentity/DotNetIdentity.csproj

# Code
COPY . .

# Build/publish de l’app
RUN dotnet publish ./DotNetIdentity/DotNetIdentity.csproj -c Release -o /app/publish

# Outil EF
RUN dotnet tool install --global dotnet-ef --version 8.*
ENV PATH="/root/.dotnet/tools:${PATH}"

# Variable de connexion utilisée par le factory design-time
ENV EF_DESIGNTIME_CONN="Server=localhost,1433;Database=DesignTime;User Id=sa;Password=Pass@word1;Encrypt=True;TrustServerCertificate=True"

# Forcer SQL Server pour les migrations
ENV AppSettings__DataBaseType=SqlServer

# Bundle des migrations sans tenter de booter ton host applicatif
RUN dotnet ef migrations bundle \
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

# Créer le répertoire certs
RUN mkdir -p /app/certs

# Copier les certificats (optionnel pour le build)
COPY certs.pem /app/certs/ 2>/dev/null || echo "Pas de certificat trouvé"
COPY certs.key /app/certs/ 2>/dev/null || echo "Pas de clé trouvée"

COPY --from=build /app/publish ./
COPY --from=build /app/migrator /app/migrator

COPY entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

ENV AppSettings__DataBaseType=SqlServer

EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_URLS=http://+:443;https://+:80
ENTRYPOINT ["/app/entrypoint.sh"]
