# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar os arquivos de solução e de projeto
COPY ./*.sln ./
COPY ./src/Application/Application.csproj ./src/Application/
COPY ./src/Domain/Domain.csproj ./src/Domain/
COPY ./src/Infrastructure/Infrastructure.csproj ./src/Infrastructure/
COPY ./src/WebAPI/WebAPI.csproj ./src/WebAPI/
COPY ./tests/Consolidado.Tests/Consolidado.Tests.csproj ./tests/Consolidado.Tests/

# Restaurar dependências
RUN dotnet restore

# Copiar o restante dos arquivos e compilar o projeto
COPY ./ .
WORKDIR /app/src/WebAPI
RUN dotnet publish -c Release -o /publish

# Etapa final: execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /publish .

# Expor a porta onde a API estará disponível
EXPOSE 8080

# Instala o netcat (versão openbsd)
RUN apt-get update && apt-get install -y netcat-openbsd

# Copia o script de espera
COPY wait-for-it.sh /app/wait-for-it.sh
RUN chmod +x /app/wait-for-it.sh

# Definir o comando de entrada
ENTRYPOINT ["/app/wait-for-it.sh", "dotnet", "WebAPI.dll"]
