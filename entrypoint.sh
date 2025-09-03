#!/usr/bin/env bash
set -euo pipefail

# Debug : afficher les variables d'environnement (sans mot de passe)
echo "=== DEBUG ENV ==="
CONN_STR_VALUE="${ConnectionStrings__Default:-}"
echo "ConnectionStrings__Default length: ${#CONN_STR_VALUE}"
if [[ -n "$CONN_STR_VALUE" ]]; then
  echo "First 30 chars: ${CONN_STR_VALUE:0:30}..."
else
  echo "ConnectionStrings__Default is EMPTY or UNSET"
fi
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

# Fonction pour appliquer les migrations avec retry
apply_migrations() {
  local migrator_path=$1
  local context_name=$2
  
  echo " Applying $context_name migrations..."
  attempts=30
  for i in $(seq 1 $attempts); do
    if "$migrator_path" --connection "$CONN_STR"; then
      echo " $context_name migrations appliquées."
      return 0
    fi
    if [[ "$i" -eq "$attempts" ]]; then
      echo " Echec $context_name migrations après ${attempts} tentatives."
      exit 1
    fi
    echo "Try ${i}/${attempts} for $context_name - retry in 2s..."
    sleep 2
  done
}

# Appliquer les migrations des 2 contextes
apply_migrations "/app/migrator-identity" "Identity"
apply_migrations "/app/migrator-app" "Application"

echo "Toutes les migrations sont appliquées avec succès!"

# Démarrer l'application
echo " Starting application..."
exec dotnet DotNetIdentity.dll
