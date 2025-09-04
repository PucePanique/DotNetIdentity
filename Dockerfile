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

RUN mkdir /app/keys
ENV DOTNET_DATA_PROTECTION__KEYS__PATH=/app/keys

COPY --from=build /app/publish ./
COPY --from=build /app/migrator /app/migrator

# Entrypoint: applique migrations puis démarre l’appli
COPY entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

# Forcer SQL Server au runtime aussi
ENV AppSettings__DataBaseType=SqlServer

EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["/app/entrypoint.sh"]
