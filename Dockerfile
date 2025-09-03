# ----------------------
# Étape 1 : Build
# ----------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copier les fichiers projet pour bénéficier du cache
COPY DotNetIdentity/*.csproj ./DotNetIdentity/
# Si tu as un .sln et plusieurs projets, copie aussi le .sln :
# COPY *.sln ./
RUN dotnet restore ./DotNetIdentity/DotNetIdentity.csproj

# Copier le reste du code
COPY . .

# Publier l'app
RUN dotnet publish ./DotNetIdentity/DotNetIdentity.csproj -c Release -o /app/publish

# Installer l'outil EF et générer le migrations bundle (EF Core 8+)
RUN dotnet tool install --global dotnet-ef --version 8.*
ENV PATH="$PATH:/root/.dotnet/tools"
# Le bundle produit un exécutable autonome qui applique les migrations
RUN dotnet ef migrations bundle \
    --project ./DotNetIdentity/DotNetIdentity.csproj \
    --configuration Release \
    --context DotNetIdentity.Data.AppDbContextSqlServer

# ----------------------
# Étape 2 : Runtime
# ----------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Copier l'appli et le binaire de migration
COPY --from=build /app/publish ./
COPY --from=build /app/migrator /app/migrator

# Script d'entrypoint
COPY entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

# Ports exposés (HTTP/HTTPS)
EXPOSE 80
EXPOSE 443

# Variables d'env .NET (facultatif mais utile en prod)
ENV ASPNETCORE_URLS=http://+:80
# La chaîne de connexion sera injectée par docker-compose via ConnectionStrings__Default
# Exemple: "Server=db,1433;Database=CesiZen;User Id=sa;Password=********;TrustServerCertificate=true;"

ENTRYPOINT ["/app/entrypoint.sh"]
