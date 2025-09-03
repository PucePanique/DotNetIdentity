#!/usr/bin/env bash
set -euo pipefail

echo "=== SQL Server Migration & Startup ==="

# La connexion
CONN_STR="${ConnectionStrings__Default:-}"
if [[ -z "$CONN_STR" ]]; then
  echo " ERREUR: ConnectionStrings__Default manquante"
  exit 1
fi

# Vérifier que c'est bien SQL Server
echo "Database Type: ${AppSettings__DataBaseType:-UNDEFINED}"

# Appliquer les migrations avec retry
echo "Applying SQL Server migrations..."
attempts=30
for i in $(seq 1 $attempts); do
  if /app/migrator --connection "$CONN_STR"; then
    echo "Migrations SQL Server appliquées."
    break
  fi
  if [[ "$i" -eq "$attempts" ]]; then
    echo "Echec migrations après ${attempts} tentatives."
    exit 1
  fi
  echo "Try ${i}/${attempts} - retry in 2s..."
  sleep 2
done

echo "Starting .NET application..."
exec dotnet DotNetIdentity.dll
