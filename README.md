# 💥 Rinha de Backend 2025 

Este repositório contém uma implementação da **Rinha de Backend 2025** 


## 🧠 O que é a Rinha de Backend?

A **Rinha de Backend** é uma competição anual que desafia desenvolvedores a criarem APIs de alta performance, respeitando **regras rigorosas de recursos**, como:

- 1 CPU
- 42MB de RAM
- Banco de dados compartilhado
- Alta concorrência (10k+ requisições)

Site oficial: [https://github.com/zanfranceschi/rinha-de-backend-2025](https://github.com/zanfranceschi/rinha-de-backend-2025)

Docker Image: https://hub.docker.com/r/boscobecker/dotnetrinha

---

## ⚙️ Tecnologias Usadas

```markdown
- Linguagem: C#, .NET 8
- Framework: ASP.NET
- Banco de Dados: PostgreSQL
- Docker e Docker Compose
- Redis
```

## 🚀 Como Rodar o Projeto

```bash
# Suba os containers

git clone https://github.com/zanfranceschi/rinha-de-backend-2025
cd /payment-processor docker compose up --build

git clone https://github.com/BoscoBecker/dotnetRinha
cd src docker compose up --build

```
A API estará disponível em: http://localhost:9999
