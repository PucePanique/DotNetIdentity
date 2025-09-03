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

# Build/publish de l'app
RUN dotnet publish ./DotNetIdentity/DotNetIdentity.csproj -c Release -o /app/publish

# Outil EF
RUN dotnet tool install --global dotnet-ef --version 8.*
ENV PATH="/root/.dotnet/tools:${PATH}"

# Variable de connexion utilisée par le factory design-time
ENV EF_DESIGNTIME_CONN="Server=localhost,1433;Database=DesignTime;User Id=sa;Password=Pass@word1;Encrypt=True;TrustServerCertificate=True"

# Debug EF
RUN dotnet ef --version

# Bundle pour le contexte Identity (SqlServer)
RUN dotnet ef migrations bundle \
    --project ./DotNetIdentity/DotNetIdentity.csproj \
    --startup-project ./DotNetIdentity/DotNetIdentity.csproj \
    --configuration Release \
    --context DotNetIdentity.Data.AppDbContextSqlServer \
    --target-runtime linux-x64 \
    --output /app/migrator-identity

# Bundle pour le contexte Application (autre contexte)
RUN dotnet ef migrations bundle \
    --project ./DotNetIdentity/DotNetIdentity.csproj \
    --startup-project ./DotNetIdentity/DotNetIdentity.csproj \
    --configuration Release \
    --context DotNetIdentity.Data.AppDbContext \
    --target-runtime linux-x64 \
    --output /app/migrator-app

# ----------------------
# Étape 2 : Runtime
# ----------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish ./
COPY --from=build /app/migrator-identity /app/migrator-identity
COPY --from=build /app/migrator-app /app/migrator-app

# Entrypoint
COPY entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["/app/entrypoint.sh"]
