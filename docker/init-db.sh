#!/bin/bash
set -e

echo "Waiting for SQL Server to be ready..."
sleep 20

echo "Running SQL script..."
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'Your_password123' -i /docker-entrypoint-initdb.d/entrypoint-init.sql
