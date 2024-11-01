# Serviço de Consolidação de Saldo

Este serviço é responsável por consolidar as transações financeiras em um saldo diário, permitindo a consulta do saldo consolidado. Ele faz parte de uma arquitetura de microsserviços e trabalha em conjunto com o serviço de Controle de Lançamentos.

## 📁 Funcionalidades

- **Consultar Saldo Consolidado**: API para consultar o saldo consolidado de um determinado período.
- **Processar Eventos de Transação**: Consome eventos de transação do sistema de mensageria (RabbitMQ) para atualizar o saldo consolidado.

## 🚀 Tecnologias Utilizadas

- **C# e ASP.NET Core**: Framework para construção da API.
- **PostgreSQL**: Banco de dados relacional para persistência dos dados consolidados.
- **RabbitMQ**: Sistema de mensageria para comunicação assíncrona com o serviço de Controle de Lançamentos.
- **Docker e Docker Compose**: Para contêinerização e orquestração dos serviços.

## ⚙️ Arquitetura e Principais Componentes

O serviço de Consolidação de Saldo segue uma estrutura baseada em princípios SOLID e utiliza padrões como Event Sourcing e Repository.

### Componentes

- **Controllers**:
  - `ConsolidationController`: Expõe os endpoints REST para consultar o saldo consolidado.
  
- **Event Handlers**:
  - `TransactionEventHandler`: Processa eventos de transações recebidos do RabbitMQ e atualiza o saldo consolidado.

- **Services**:
  - `ConsolidationService`: Aplica regras de negócio e gerencia a lógica de consolidação dos saldos.

- **Repositories**:
  - `ConsolidationRepository`: Abstrai operações com o banco de dados PostgreSQL para os saldos consolidados.

- **DbContext**:
  - `ConsolidationDbContext`: Gerencia a conexão e o contexto com o banco de dados.

## 🚀 Executando o Serviço

### Pré-requisitos

- Docker e Docker Compose (ou um ambiente configurado com .NET Core SDK e PostgreSQL).

### Instruções para Executar com Docker

1. **Clonar o Repositório**:
   ```bash
   git clone https://github.com/FlavioAndre/lancamentos.git
   cd lancamentos/consolidado
   ```

2. **Construir e Iniciar o Serviço**:
   ```bash
   docker-compose up --build -d
   ```
   Esse comando compilará a imagem do serviço e iniciará os contêineres necessários, incluindo PostgreSQL e RabbitMQ.

3. **Verificar o Status**:
   ```bash
   docker-compose ps
   ```

4. **Acessar o Serviço**:
   - API de saldo consolidado: `http://localhost:8081/api/consolidation`

### Executando Testes

1. **Acessar o Diretório de Testes**:
   ```bash
   cd tests
   ```

2. **Executar Testes**:
   ```bash
   dotnet test
   ```

Esse comando executará os testes unitários e de integração, fornecendo um relatório detalhado dos resultados.

## 📅 API Endpoints

- **GET /api/consolidation**: Retorna o saldo consolidado de um determinado período.
  - **Parâmetros**: `{ "date": "YYYY-MM-DD" }`
  - **Resposta**: Saldo consolidado da data especificada.

## 🛠️ Futuras Melhorias

- **Escalonamento de Processamento**: Implementar a capacidade de processar múltiplos eventos de transação simultaneamente para melhorar a performance.
- **Autenticação e Autorização**: Implementar OAuth2 para proteger os endpoints da API.
- **Melhoria na Resiliência de Mensageria**: Implementar políticas de retry e dead-letter para eventos não processados.
- **Monitoramento e Alertas**: Adicionar monitoramento e geração de alertas para identificar problemas no processamento de eventos e garantir a integridade dos saldos consolidados.
- **Otimização de Consultas**: Implementar caching para melhorar a performance das consultas ao saldo consolidado.

