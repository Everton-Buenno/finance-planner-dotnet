#!/bin/sh
echo "Aguardando 60 segundos para garantir que o banco de dados esteja pronto..."
sleep 60
echo "Iniciando a aplicação..."
dotnet Planner.Api.dll