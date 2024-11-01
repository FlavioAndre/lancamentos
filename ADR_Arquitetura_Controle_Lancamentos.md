
# 📄 Arquivo de Registros de Decisões Arquiteturais (ADR)

Este documento contém registros das principais decisões arquiteturais tomadas para o sistema de Controle de Lançamentos e Consolidação de Saldo. Cada decisão inclui o contexto, justificativa e possíveis implicações.

---

## ADR 1: Escolha de Arquitetura de Microsserviços

**📅 Data**: 2024-10-31

- **📌 Contexto**: O sistema exige uma arquitetura que permita escalabilidade e independência entre os módulos de controle de lançamentos e consolidação de saldo.
- **🏁 Decisão**: Implementar a arquitetura baseada em microsserviços.
- **💡 Justificativa**: A arquitetura de microsserviços oferece escalabilidade independente para cada módulo, permitindo que cada serviço seja dimensionado conforme necessário e facilitando a manutenção e o isolamento de falhas.
- **⚠️ Implicações**: Requer gerenciamento de comunicação entre serviços e um sistema de mensageria para sincronização.

---

## ADR 2: Uso do RabbitMQ para Comunicação Assíncrona

**📅 Data**: 2024-10-31

- **📌 Contexto**: O sistema precisa de comunicação eficiente e resiliente entre os serviços de controle de lançamentos e consolidação, sem depender de chamadas diretas.
- **🏁 Decisão**: Utilizar RabbitMQ como sistema de mensageria para comunicação assíncrona.
- **💡 Justificativa**: RabbitMQ oferece alta confiabilidade e suporta padrões de mensageria necessários para manter os serviços desacoplados, garantindo que o serviço de consolidado receba e processe eventos de transação de forma independente.
- **⚠️ Implicações**: Aumenta a resiliência do sistema, mas requer configuração e monitoramento do RabbitMQ para garantir disponibilidade e evitar perda de mensagens.

---

## ADR 3: Persistência com PostgreSQL para Cada Serviço

**📅 Data**: 2024-10-31

- **📌 Contexto**: Cada serviço precisa armazenar dados transacionais e de consolidação de forma consistente e durável.
- **🏁 Decisão**: Usar PostgreSQL como banco de dados relacional para cada serviço.
- **💡 Justificativa**: PostgreSQL é robusto, escalável e permite a criação de esquemas de banco de dados específicos para cada serviço, facilitando o gerenciamento de dados e mantendo a integridade.
- **⚠️ Implicações**: Requer estratégias de backup e replicação para garantir que os dados sejam duráveis e que estejam disponíveis em caso de falhas.

---

## ADR 4: Implementação de CQRS para Separação de Comandos e Consultas

**📅 Data**: 2024-10-31

- **📌 Contexto**: O serviço de controle de lançamentos precisa lidar com operações de criação de transações, enquanto o serviço de consolidação consulta saldos.
- **🏁 Decisão**: Aplicar CQRS para separar comandos (operações de gravação) de consultas (operações de leitura).
- **💡 Justificativa**: Essa abordagem melhora a performance e simplifica a lógica ao isolar operações de gravação e leitura, facilitando a escalabilidade de cada uma.
- **⚠️ Implicações**: Requer desenvolvimento e manutenção de serviços distintos para leitura e gravação, além de uma camada de mensageria que sincronize os dados entre as operações de comando e consulta.

---

## ADR 5: Uso de Docker e Docker Compose para Orquestração

**📅 Data**: 2024-10-31

- **📌 Contexto**: A implantação do sistema precisa ser fácil, portátil e isolada entre ambientes de desenvolvimento, teste e produção.
- **🏁 Decisão**: Usar Docker para contêineres e Docker Compose para orquestração.
- **💡 Justificativa**: Docker permite que cada serviço seja isolado em seu próprio contêiner, e Docker Compose simplifica a orquestração de múltiplos serviços em um único ambiente.
- **⚠️ Implicações**: Facilita a portabilidade e consistência entre ambientes, mas exige que os desenvolvedores tenham familiaridade com Docker.

---

### Conclusão
Este documento registra as principais decisões arquiteturais tomadas para o sistema, proporcionando uma visão clara do raciocínio e das implicações de cada escolha. Esses registros podem ser expandidos ou atualizados conforme o sistema evolui.
