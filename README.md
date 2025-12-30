#  Planner - Sistema de Gestão Financeira Pessoal

![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=flat&logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat&logo=docker&logoColor=white)
![License](https://img.shields.io/badge/license-MIT-green)

Um sistema completo de planejamento e controle financeiro pessoal construído com .NET 8, seguindo os princípios de Clean Architecture e Domain-Driven Design (DDD).

##  Índice

- [Sobre o Projeto](#-sobre-o-projeto)
- [Funcionalidades](#-funcionalidades)
- [Arquitetura](#-arquitetura)
- [Tecnologias](#-tecnologias)
- [Pré-requisitos](#-pré-requisitos)
- [Instalação](#-instalação)
- [Configuração](#-configuração)
- [Uso](#-uso)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [API Endpoints](#-api-endpoints)
- [Contribuindo](#-contribuindo)
- [Licença](#-licença)

##  Sobre o Projeto

O **Planner** é uma solução robusta para gestão financeira pessoal que permite aos usuários controlar suas finanças de forma completa e organizada. O sistema oferece suporte para múltiplas contas bancárias, categorização de transações, gestão de cartões de crédito e análise financeira detalhada.

### Diferenciais

- ✅ **270 Instituições Financeiras** disponíveis para cadastro
- ✅ Arquitetura limpa e escalável
- ✅ Validações em múltiplas camadas
- ✅ Sistema de autenticação seguro com JWT
- ✅ Suporte a transações recorrentes
- ✅ Gestão avançada de cartões de crédito
- ✅ Dashboard com métricas e análises

##  Funcionalidades

### Gestão de Contas Bancárias
- Cadastro de múltiplas contas (Corrente, Poupança, Investimentos, VR/VA, Dinheiro)
- Integração com 270 bancos brasileiros
- Cálculo automático de saldo atual e projetado
- Controle de transferências entre contas

### Transações Financeiras
- Registro de receitas e despesas
- Categorização personalizada com ícones e cores
- Suporte a transações recorrentes (diária, semanal, mensal, anual)
- Marcação de transações pagas/pendentes
- Opção de ignorar transações no cálculo de saldo
- Filtros avançados por período, categoria, conta e status

### Cartões de Crédito
- Cadastro de múltiplos cartões por conta
- Controle de limite e limite disponível
- Gestão de fechamento e vencimento de faturas
- Cálculo automático do valor da fatura
- Pagamento de faturas
- Histórico de transações por cartão

### Categorias
- Categorias pré-definidas para receitas e despesas
- Criação de categorias personalizadas
- Associação de ícones e cores
- Análise de gastos por categoria

### Dashboard & Relatórios
- Visão consolidada do patrimônio
- Análise de receitas vs despesas
- Projeção de saldo futuro
- Métricas por conta bancária
- Indicadores financeiros

### Segurança
- Autenticação JWT
- Criptografia de senhas com BCrypt
- Validação de CPF e e-mail
- Soft delete para histórico completo
- Auditoria automática de registros

## Arquitetura

O projeto segue os princípios de **Clean Architecture**, garantindo separação de responsabilidades, testabilidade e manutenibilidade:

```
┌─────────────────────────────────────────┐
│         Planner.Api (Presentation)      │
│  Controllers, Middlewares, Swagger      │
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│      Planner.Application (Use Cases)    │
│  Services, DTOs, Validators, Interfaces │
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│       Planner.Domain (Business Logic)   │
│  Entities, Value Objects, Enums,        │
│  Domain Exceptions, Interfaces          │
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│    Planner.Infrastructure (Data Access) │
│  Repositories, DbContext, Migrations,   │
│  Mappings, External Services            │
└─────────────────────────────────────────┘
```

### Padrões Implementados

- **Domain-Driven Design (DDD)**: Entidades ricas, agregados e value objects
- **Repository Pattern**: Abstração da camada de dados
- **Dependency Injection**: Injeção de dependências nativa do .NET
- **CQRS (simplificado)**: Separação de comandos e consultas
- **Unit of Work**: Implementado através do DbContext
- **Specification Pattern**: Filtros e queries reutilizáveis

##  Tecnologias

### Backend
- **.NET 8** - Framework principal
- **C# 12** - Linguagem de programação
- **Entity Framework Core 8** - ORM
- **PostgreSQL** - Banco de dados relacional
- **FluentValidation** - Validação de dados
- **BCrypt.NET** - Criptografia de senhas
- **JWT Bearer** - Autenticação

### DevOps & Ferramentas
- **Docker** - Containerização
- **Swagger/OpenAPI** - Documentação da API
- **Git** - Controle de versão

##  Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL 15+](https://www.postgresql.org/download/) ou [Docker](https://www.docker.com/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)

##  Instalação

### Usando Docker (Recomendado)

1. Clone o repositório:
```bash
git clone https://github.com/Everton-Buenno/finance-planner-dotnet.git
cd finance-planner-dotnet
```

2. Configure as variáveis de ambiente (veja [Configuração](#-configuração))

3. Execute com Docker Compose:
```bash
docker-compose up -d
```

4. Acesse a API em `http://localhost:8080` e o Swagger em `http://localhost:8080/swagger`

### Instalação Local

1. Clone o repositório:
```bash
git clone https://github.com/Everton-Buenno/finance-planner-dotnet.git
cd finance-planner-dotnet
```

2. Restaure as dependências:
```bash
dotnet restore
```

3. Configure o banco de dados PostgreSQL e atualize a connection string em `appsettings.json`

4. Execute as migrations:
```bash
cd Planner.Api
dotnet ef database update --project ../Planner.Infrastructure
```

5. Execute a aplicação:
```bash
dotnet run --project Planner.Api
```

##  Configuração

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=planner;Username=postgres;Password=sua_senha"
  },
  "Jwt": {
    "Key": "sua_chave_secreta_aqui_minimo_32_caracteres",
    "Issuer": "PlannerAPI",
    "Audience": "PlannerClients",
    "ExpireMinutes": 10080
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Variáveis de Ambiente (Docker)

Crie um arquivo `.env` na raiz do projeto:

```env
# Database
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres123
POSTGRES_DB=planner
DB_HOST=db
DB_PORT=5432

# JWT
JWT_KEY=sua_chave_secreta_super_segura_com_pelo_menos_32_caracteres
JWT_ISSUER=PlannerAPI
JWT_AUDIENCE=PlannerClients
JWT_EXPIRE_MINUTES=10080

# Application
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://+:8080
```

##  Uso

### Registro de Usuário

```http
POST /api/auth/register
Content-Type: application/json

{
  "name": "João Silva",
  "email": "joao@example.com",
  "password": "SenhaSegura123!"
}
```

### Login

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "joao@example.com",
  "password": "SenhaSegura123!"
}
```

### Criar Conta Bancária

```http
POST /api/bankaccount/create
Authorization: Bearer {seu_token}
Content-Type: application/json

{
  "bankId": 214,
  "description": "Minha Conta Corrente",
  "userId": "guid-do-usuario",
  "color": "#FF5733",
  "type": 0,
  "initialBalance": 1000.00
}
```

### Registrar Transação

```http
POST /api/transaction
Authorization: Bearer {seu_token}
Content-Type: application/json

{
  "dateTransaction": "2025-01-15T10:00:00Z",
  "value": 150.00,
  "description": "Supermercado",
  "type": 1,
  "accountId": "guid-da-conta",
  "categoryId": "guid-da-categoria",
  "isPaid": true,
  "ignored": false,
  "repeatType": 0,
  "repeatCount": 1
}
```

##  Estrutura do Projeto

```
planner/
├── Planner.Api/                    # Camada de apresentação
│   ├── Controllers/                # Endpoints da API
│   ├── Middlewares/                # Middlewares customizados
│   ├── wwwroot/                    # Arquivos estáticos (bancos.json)
│   └── Program.cs                  # Configuração da aplicação
│
├── Planner.Application/            # Camada de aplicação
│   ├── DTOs/                       # Data Transfer Objects
│   ├── Interfaces/                 # Contratos de serviços
│   ├── Services/                   # Implementação dos casos de uso
│   ├── Validators/                 # Validações com FluentValidation
│   └── ApplicationModule.cs        # Registro de dependências
│
├── Planner.Domain/                 # Camada de domínio
│   ├── Entities/                   # Entidades de negócio
│   │   ├── User.cs
│   │   ├── BankAccount.cs
│   │   ├── Category.cs
│   │   ├── Transaction.cs
│   │   └── CreditCard.cs
│   ├── ValueObjects/               # Value Objects
│   │   ├── Email.cs
│   │   ├── Cpf.cs
│   │   └── PhoneNumber.cs
│   ├── Enums/                      # Enumerações
│   ├── Exceptions/                 # Exceções de domínio
│   └── Interfaces/                 # Contratos de repositórios
│
├── Planner.Infrastructure/         # Camada de infraestrutura
│   ├── Data/                       # Contexto do banco de dados
│   ├── Mappings/                   # Configurações do EF Core
│   ├── Migrations/                 # Migrations do banco
│   ├── Repositories/               # Implementação dos repositórios
│   └── InfrastructureModule.cs     # Registro de dependências
│
├── docker-compose.yml              # Configuração Docker
├── Dockerfile                      # Imagem da aplicação
└── README.md                       # Este arquivo
```

##  API Endpoints

### Autenticação
- `POST /api/auth/register` - Registrar novo usuário
- `POST /api/auth/login` - Autenticar usuário

### Contas Bancárias
- `GET /api/bankaccount/banks` - Listar todos os bancos disponíveis
- `GET /api/bankaccount/banks/base` - Listar bancos principais
- `GET /api/bankaccount/user/{userId}` - Listar contas do usuário
- `POST /api/bankaccount/create` - Criar nova conta
- `PUT /api/bankaccount/update` - Atualizar conta
- `DELETE /api/bankaccount/delete/{id}` - Excluir conta

### Categorias
- `GET /api/category/get-all/{userId}` - Listar categorias do usuário
- `GET /api/category/get-by-id/{id}` - Buscar categoria por ID
- `POST /api/category/create` - Criar categoria
- `PUT /api/category/update/{id}` - Atualizar categoria
- `DELETE /api/category/delete/{id}` - Excluir categoria

### Transações
- `GET /api/transaction` - Listar transações com filtros
- `POST /api/transaction` - Criar transação
- `PUT /api/transaction/{id}` - Atualizar transação
- `DELETE /api/transaction/{id}` - Excluir transação

### Cartões de Crédito
- `GET /api/creditcard/user/{userId}` - Listar cartões do usuário
- `GET /api/creditcard/{id}` - Buscar cartão por ID
- `GET /api/creditcard/{id}/invoice` - Obter fatura do cartão
- `POST /api/creditcard/create` - Criar cartão
- `PUT /api/creditcard/update/{id}` - Atualizar cartão
- `DELETE /api/creditcard/delete/{id}` - Excluir cartão
- `POST /api/creditcard/{id}/pay-invoice` - Pagar fatura

### Balanço & Dashboard
- `GET /api/balance/monthly` - Obter balanço mensal
- `GET /api/dashboard/{userId}` - Obter dados do dashboard

>  **Documentação Completa**: Acesse `/swagger` após iniciar a aplicação para ver todos os endpoints e testar a API interativamente.

##  Contribuindo

Contribuições são sempre bem-vindas! Para contribuir:

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

### Diretrizes

- Siga os padrões de código do projeto
- Escreva testes para novas funcionalidades
- Atualize a documentação quando necessário
- Mantenha commits limpos e descritivos

##  Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE.txt) para mais detalhes.

##  Autor

**Everton Bueno**

- GitHub: [@Everton-Buenno](https://github.com/Everton-Buenno)
- LinkedIn: [Everton Bueno](https://www.linkedin.com/in/everton-bueno/)

##  Agradecimentos

- Inspirado nas melhores práticas de Clean Architecture
- Comunidade .NET pelo suporte e recursos
- Todos os contribuidores do projeto

---

⭐ Se este projeto foi útil para você, considere dar uma estrela no repositório!

**Desenvolvido usando .NET 8**
