#!/usr/bin/env bash
set -euo pipefail

CONN="${ConnectionStrings__Default:-${ConnectionStrings__SqlServer:-}}"
if [ -z "${CONN}" ]; then
  echo "Connection string manquante (ConnectionStrings__Default)."; exit 1
fi

echo "Applying EF migrations..."
RETRIES=30; SLEEP=2
for i in $(seq 1 $RETRIES); do
  /app/migrator --connection "$CONN" --apply && break || true
  echo "Try $i/$RETRIES - retry in ${SLEEP}s..."
  sleep $SLEEP
  if [ "$i" -eq "$RETRIES" ]; then
    echo "Echec migrations."; exit 1
  fi
done

echo "Starting app..."
exec dotnet DotNetIdentity.dll
