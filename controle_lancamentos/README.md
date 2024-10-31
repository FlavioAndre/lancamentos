# Controle de Lançamentos

Este projeto é uma API para o sistema de controle de lançamentos financeiros. Ele utiliza ASP.NET Core, Entity Framework Core, RabbitMQ e MediatR para fornecer uma solução robusta e escalável.

## Estrutura do Projeto

- **Application**: Contém os comandos, handlers e interfaces da aplicação.
- **Infrastructure**: Contém a implementação dos repositórios, serviços de mensageria e configurações de banco de dados.
- **WebAPI**: Contém a configuração da API e os controladores.
- **Tests**: Contém os testes unitários e de integração.

## Requisitos

- .NET 6.0 SDK
- PostgreSQL
- RabbitMQ

## Configuração

### Variáveis de Ambiente

Crie um arquivo `.env` na raiz do projeto com as seguintes variáveis de ambiente:

```env
# Banco de Dados
DB_HOST=localhost
DB_PORT=5432
DB_NAME=controle_lancamentos
DB_USER=postgres
DB_PASSWORD=postgres

# RabbitMQ
RABBITMQ_URI=amqp://guest:guest@localhost:5672

# JWT
JWT_SECRET=your_jwt_secret
JWT_ISSUER=your_jwt_issuer
JWT_AUDIENCE=your_jwt_audience

Claro! Você pode incluir diagramas em XML no 

README.md

 usando a sintaxe de blocos de código. Aqui está um exemplo de como você pode fazer isso:

```markdown
# Controle de Lançamentos

Este projeto é uma API para o sistema de controle de lançamentos financeiros. Ele utiliza ASP.NET Core, Entity Framework Core, RabbitMQ e MediatR para fornecer uma solução robusta e escalável.

## Estrutura do Projeto

- **Application**: Contém os comandos, handlers e interfaces da aplicação.
- **Infrastructure**: Contém a implementação dos repositórios, serviços de mensageria e configurações de banco de dados.
- **WebAPI**: Contém a configuração da API e os controladores.
- **Tests**: Contém os testes unitários e de integração.

## Requisitos

- .NET 6.0 SDK
- PostgreSQL
- RabbitMQ

## Configuração

### Variáveis de Ambiente

Crie um arquivo `.env` na raiz do projeto com as seguintes variáveis de ambiente:

```env
# Banco de Dados
DB_HOST=localhost
DB_PORT=5432
DB_NAME=controle_lancamentos
DB_USER=postgres
DB_PASSWORD=postgres

# RabbitMQ
RABBITMQ_URI=amqp://guest:guest@localhost:5672

# JWT
JWT_SECRET=your_jwt_secret
JWT_ISSUER=your_jwt_issuer
JWT_AUDIENCE=your_jwt_audience
```

### Banco de Dados

Certifique-se de que o PostgreSQL esteja em execução e crie o banco de dados especificado no arquivo `.env`.

### RabbitMQ

Certifique-se de que o RabbitMQ esteja em execução e acessível através da URI especificada no arquivo `.env`.

## Execução

### Restaurar Dependências

```bash
dotnet restore
```

### Aplicar Migrações

```bash
dotnet ef database update --project WebAPI
```

### Executar a Aplicação

```bash
dotnet run --project WebAPI
```

A aplicação estará disponível em `http://localhost:5000`.

## Testes

### Executar Testes

```bash
dotnet test
```

## Estrutura de Código

### Program.cs

O arquivo `Program.cs` configura os serviços e o pipeline HTTP da aplicação. Ele carrega as variáveis de ambiente, configura o banco de dados, RabbitMQ, JWT, Swagger e MediatR.

### CreateTransactionCommandHandler.cs

O `CreateTransactionCommandHandler` é responsável por lidar com o comando `CreateTransactionCommand`. Ele utiliza o serviço de transações para adicionar uma nova transação e registra logs de informações e erros.

### CustomLogger.cs

O `CustomLogger` é uma implementação personalizada de logger que encapsula o `ILogger` do ASP.NET Core. Ele fornece métodos para registrar informações e erros.

### Testes

Os testes estão localizados no diretório `Tests` e utilizam o framework xUnit e Moq para criar mocks e verificar comportamentos.


## Contribuição

1. Fork o repositório.
2. Crie uma branch para sua feature (`git checkout -b feature/nova-feature`).
3. Commit suas mudanças (`git commit -am 'Adiciona nova feature'`).
4. Push para a branch (`git push origin feature/nova-feature`).
5. Abra um Pull Request.

## Licença

Este projeto está licenciado sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

Seguindo os princípios de Clean Code, este projeto foi estruturado para ser claro, simples e fácil de manter. Cada componente tem uma responsabilidade bem definida e o código é organizado de forma a facilitar a compreensão e a colaboração.
```

### Explicação

Este 

README.md

 fornece todas as informações necessárias para configurar, executar e contribuir com o projeto, além de incluir um diagrama XML para melhor visualização da estrutura do projeto.