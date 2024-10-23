@echo off
:: Stop the running PostgreSQL container
docker stop PredictionGameDB

:: Remove the container
docker rm PredictionGameDB

:: Recreate the PostgreSQL container
docker run -d -p 5432:5432 --name PredictionGameDB -e POSTGRES_PASSWORD=parool postgres

:: Notify the user
echo PostgreSQL container has been reset successfully.
pause