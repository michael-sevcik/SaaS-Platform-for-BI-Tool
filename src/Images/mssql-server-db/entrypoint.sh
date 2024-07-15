# Run Microsoft SQl Server and initialization script (at the same time)
echo "Starting SQL Server with custom initialization script..."

/usr/src/app/run-initialization.sh & /opt/mssql/bin/sqlservr

