#!/usr/bin/env bash
set -euo pipefail

# Debug : afficher les variables d'environnement (sans mot de passe)
echo "=== DEBUG ENV ==="
echo "ConnectionStrings__Default length: ${#ConnectionStrings__Default:-0}"
echo "First 20 chars: ${ConnectionStrings__Default:0:20}..."
echo "=================="

# La connexion arrive de l'env "ConnectionStrings__Default" (style ASP.NET Core)
CONN_STR="${ConnectionStrings__Default:-}"
if [[ -z "$CONN_STR" ]]; then
  echo "ERREUR: variable d'environnement ConnectionStrings__Default absente."
  echo "Définis-la dans docker-compose (.env) avant de démarrer."
  exit 1
fi

# Vérifier si la chaîne contient des variables non résolues
if [[ "$CONN_STR" == *'${'* ]]; then
  echo "ERREUR: La chaîne de connexion contient des variables non résolues:"
  echo "$CONN_STR"
  exit 1
fi

echo "Applying EF migrations..."
attempts=30
for i in $(seq 1 $attempts); do
  if /app/migrator --connection "$CONN_STR"; then
    echo "Migrations appliquées."
    break
  fi
  if [[ "$i" -eq "$attempts" ]]; then
    echo "Echec migrations après ${attempts} tentatives."
    exit 1
  fi
  echo "Try ${i}/${attempts} - retry in 2s..."
  sleep 2
done

# Démarrer l'application
exec dotnet DotNetIdentity.dll
