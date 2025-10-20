# HealthConnect API

API backend para o sistema de agendamento de consultas médicas HealthConnect, construído com .NET e C#, seguindo os princípios da Clean Architecture. O objetivo é conectar pacientes a médicos especialistas certificados (foco em RQE) e facilitar o agendamento de consultas.

✨ **Features Principais:**

* **Autenticação e Autorização:** Sistema seguro baseado em JWT (JSON Web Tokens).
* **Gerenciamento de Usuários:** Cadastro e atualização de dados de usuários.
* **Perfis de Médico:** Cadastro detalhado com CRM, RQE, Biografia e Especialidade (atualmente 1:N, planejado para N:N).
* **Perfis de Cliente (Paciente):** Cadastro de pacientes.
* **Gerenciamento de Disponibilidade:** Médicos podem cadastrar seus horários vagos.
* **Agendamento de Consultas:** Clientes podem agendar horários disponíveis com médicos.
* **Gerenciamento de Especialidades:** CRUD para as especialidades médicas.
* **Validação:** Validação robusta de entrada usando FluentValidation.
* **Seeding de Banco de Dados:** Inicialização com dados padrão (usuários admin/teste) via comando CLI.

---

## 🚀 Tecnologias Utilizadas

* **.NET 9**
* **C#**
* **ASP.NET Core:** Para a construção da API RESTful.
* **Entity Framework Core:** ORM para interação com o banco de dados.
* **PostgreSQL:** Banco de dados relacional (gerenciado via Docker para desenvolvimento).
* **AutoMapper:** Para mapeamento entre entidades e DTOs.
* **FluentValidation:** Para validação de DTOs e modelos.
* **JWT Bearer Authentication:** Para segurança da API.
* **Swagger/OpenAPI:** Para documentação interativa da API.
* **xUnit:** Para testes unitários e de integração.
* **Docker:** Para containerização do banco de dados em desenvolvimento.

---

## 🏗️ Estrutura do Projeto (Clean Architecture)

O projeto segue os princípios da Clean Architecture para garantir separação de responsabilidades, testabilidade e manutenibilidade:

* **`HealthConnect.Domain`**: Contém as entidades de negócio principais, enums, interfaces de domínio e lógica de domínio pura. Não depende de nenhuma outra camada.
* **`HealthConnect.Application`**: Contém a lógica da aplicação (casos de uso), DTOs (Data Transfer Objects), interfaces de serviços e repositórios, validações (FluentValidation) e configurações de mapeamento (AutoMapper). Depende apenas do Domain.
* **`HealthConnect.Infrastructure`**: Implementa as interfaces definidas na Application para interagir com tecnologias externas, como o banco de dados (EF Core, Repositórios), serviços de autenticação, etc. Depende da Application.
* **`HealthConnect.Api`**: A camada de apresentação (API REST). Contém os Controllers, configuração do ASP.NET Core, middlewares e o ponto de entrada da aplicação (`Program.cs`). Depende da Application e Infrastructure.
* **`*.Tests`**: Projetos separados para testes unitários e de integração de cada camada.

---

## ⚙️ Configuração e Execução Local

Siga estes passos para configurar e rodar o projeto na sua máquina.

### Pré-requisitos

* [.NET SDK](https://dotnet.microsoft.com/download) (Versão 9.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop/) (para rodar o banco de dados PostgreSQL)
* [Git](https://git-scm.com/)
* (Opcional) Ferramenta de linha de comando `dotnet-ef` instalada globalmente: `dotnet tool install --global dotnet-ef` (ou `update` se já tiver).

### Passos

1.  **Clonar o Repositório:**
    ```bash
    git clone https://github.com/gustavosacoman/HealthConnect.Api.git
    cd HealthConnect.Api # Navegue para a pasta raiz da solução
    ```

2.  **Configurar o Banco de Dados (Docker):**
    * Certifique-se de que o Docker Desktop está rodando.
    * Execute o contêiner do PostgreSQL definido no arquivo `docker-compose.yml`:
        ```bash
        docker-compose up --build
        ```

3.  **Configurar Segredos (User Secrets):**
    A aplicação utiliza User Secrets para armazenar informações sensíveis em desenvolvimento (string de conexão, chave JWT).
    * Navegue até a pasta do projeto da API: `cd HealthConnect.Api`
    * Inicialize os segredos (se ainda não o fez): `dotnet user-secrets init`
    * Configure os segredos necessários:
        ```bash
        # String de Conexão para o banco no Docker (ajuste usuário/senha se necessário)
        dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=5432;Database=healthconnect_db;User Id=postgres;Password=<sua-senha-do-docker-compose>;"

        # Configurações do JWT (use uma chave longa e segura!)
        dotnet user-secrets set "Jwt:Key" "<sua-chave-secreta-jwt-super-longa-e-segura>"
        dotnet user-secrets set "Jwt:Issuer" "HealthConnectDev"
        dotnet user-secrets set "Jwt:Audience" "HealthConnectApp"
        ```
    * Volte para a pasta raiz da solução: `cd ..`

4.  **Aplicar Migrations do Banco de Dados:**
    Este comando cria o banco de dados (se não existir) e aplica todas as migrations para criar o schema.
    ```bash
    dotnet ef database update --project HealthConnect.Infrastructure --startup-project HealthConnect.Api
    ```

5.  **Rodar a Aplicação:**
    Finalmente, inicie a API.
    ```bash
    dotnet run --project HealthConnect.Api
    ```
    A API estará rodando nas URLs indicadas no terminal (geralmente `http://localhost:5251`).

### Executando Testes

Para rodar todos os testes unitários e de integração da solução:
```bash
dotnet test
