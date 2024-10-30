# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar os arquivos de solução e de projeto
COPY ./*.sln ./
COPY ./src/Application/Application.csproj ./src/Application/
COPY ./src/Domain/Domain.csproj ./src/Domain/
COPY ./src/Infrastructure/Infrastructure.csproj ./src/Infrastructure/
COPY ./src/WebAPI/WebAPI.csproj ./src/WebAPI/

# Restaurar dependências
RUN dotnet restore 

# Copiar o restante dos arquivos
COPY ./src/Application/ ./src/Application/
COPY ./src/Domain/ ./src/Domain/
COPY ./src/Infrastructure/ ./src/Infrastructure/
COPY ./src/WebAPI/ ./src/WebAPI/

# Publicar a aplicação
WORKDIR /app/src/WebAPI
RUN dotnet publish -c Release -o /publish

# Etapa final: execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]

# Expor a porta onde a API estará disponível
EXPOSE 8080
