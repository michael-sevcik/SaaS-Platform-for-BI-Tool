#!/bin/bash
#
# Script: run-initialization.sh
# Description: Initializes the MSSQL server database for the SaaS platform
# Author: Michael Ševčík
#

if [ -f /usr/src/app/initialized ] && [ "$(cat /usr/src/app/initialized)" = "Initialized" ]; then
    echo "Already initialized"
    exit 0
fi

sleep 5s
# Wait until the test passes
while ! /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -Q "SELECT 1" -b -o /dev/null; do
    echo "Waiting for SQL Server to be available..."
    sleep 5s
done

# Run the setup script to create the DB and the schema in the DB
# Note: make sure that your password matches what is in the Dockerfile
if ! /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -d master -i SaasPlatformDbCreateScript.sql; then
    echo "Error creating database"
    exit 1
fi

echo "Initialized" > /usr/src/app/initialized
echo "Databasse initialized" 