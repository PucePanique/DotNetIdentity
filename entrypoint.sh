#!/usr/bin/env bash
set -euo pipefail

# Attendre la DB si nécessaire (retry simple)
echo "Waiting for database to be reachable..."
RETRIES=30
SLEEP=2
for i in $(seq 1 $RETRIES); do
  /app/migrator --connection "${ConnectionStrings__Default:-}" --apply --verbose && break || true
  echo "Migration try $i/$RETRIES failed; retrying in ${SLEEP}s..."
  sleep $SLEEP
  if [ "$i" -eq "$RETRIES" ]; then
    echo "Failed to apply migrations after $RETRIES attempts"
    exit 1
  fi
done

echo "Starting application..."
exec dotnet DotNetIdentity.dll
