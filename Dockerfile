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

# Installer l’outil EF (AVANT le bundle) et exposer le PATH
RUN dotnet tool install --global dotnet-ef --version 8.*
ENV PATH="/root/.dotnet/tools:${PATH}"

# (Optionnel) Valider que le projet voit bien le DbContext
# RUN dotnet ef dbcontext list --project ./DotNetIdentity/*.csproj --startup-project ./DotNetIdentity/*.csproj

# Générer le bundle EF
RUN dotnet ef migrations bundle \
    --project ./DotNetIdentity/*.csproj \
    --startup-project ./DotNetIdentity/*.csproj \
    --configuration Release \
    
    --target-runtime linux-x64 \
    --output /app/migrator

# --context DotNetIdentity.Data.AppDbContextSqlServer \

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
