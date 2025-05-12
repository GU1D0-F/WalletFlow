# WalletFlow

## Descrição

WalletFlow é um projeto em .NET 8 que fornece uma API para gerenciamento de carteiras digitais com JWT Authentication. Com ele, é possível:

* Registrar novos usuários
* Autenticar via JWT
* Consultar saldo da carteira
* Adicionar saldo (depósitos)
* Transferir fundos entre carteiras
* Listar transferências realizadas

O projeto utiliza PostgreSQL como banco de dados.

## Pré-requisitos

Antes de rodar o projeto localmente, certifique-se de ter instalado:

* [.NET 8 SDK](https://dotnet.microsoft.com/download)
* [Docker](https://www.docker.com/) (necessário para executar o PostgreSQL e para rodar os testes locais)

## Configuração do ambiente

1. Clone este repositório:

   ```bash
   git clone https://github.com/seu-usuario/WalletFlow.git
   cd WalletFlow
   ```

## Banco de dados

### Executando o PostgreSQL com Docker

Para subir um container PostgreSQL:

```bash
docker run --name walletflow-postgres \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=123 \
  -e POSTGRES_DB=walletflow \
  -p 5432:5432 \
  -d postgres:15
```

### Aplicando migrations

Após ter o PostgreSQL rodando, execute:

```bash
dotnet ef database update \
  --project src/WalletFlow.Infrastructure \
  --startup-project src/WalletFlow.Api
```

## Executando a API localmente

No diretório raiz do projeto, rode:

```bash
dotnet run --project src/WalletFlow.Api
```

## Endpoints principais

| Método | Rota                       | Descrição                         |
| ------ | -------------------------- | --------------------------------- |
| POST   | `/api/v1/auth/register`    | Registrar novo usuário            |
| POST   | `/api/v1/auth/login`       | Obter token JWT                   |
| GET    | `/api/v1/wallet/balance`   | Consultar saldo da carteira       |
| POST   | `/api/v1/wallet/add-funds` | Adicionar saldo (depósito)        |
| POST   | `/api/v1/wallet/transfer`  | Transferir fundos entre carteiras |
| GET    | `/api/v1/wallet/transfers` | Listar transferências realizadas  |

## Executando testes

Para rodar a suíte de testes locais, o Docker deve estar em execução na máquina (para subir dependências, como o container do PostgreSQL). Então execute:

```bash
dotnet test
```

## Observações

* Se já tiver um container PostgreSQL rodando, apenas ajuste a string de conexão na configuração de ambiente (p.ex. `ConnectionStrings__Default`).
* Para limpar o banco, pare e remova o container:

  ```bash
  docker stop WalletFlow && docker rm WalletFlow
  ```

---

Bom desenvolvimento! 🚀
