# 🏦 FintechBank

O **FintechBank** é uma aplicação backend desenvolvida em C# e .NET 10 para gerenciar regras cruciais de contas correntes e análise de concessão de empréstimos, com garantia de qualidade suportada por suítes de testes unitários parametrizados em xUnit.

---

## 📌 Visão Geral da Aplicação

A solução é organizada de forma modular para separar as regras de domínio dos testes automatizados:

- **`FintechBank.App`**: Projeto core da aplicação contendo as regras de negócio para tarifação de contas correntes, validação de limites e cálculo de juros/margem consignável para empréstimos.
- **`FintechBank.Tests`**: Projeto de testes que valida os serviços da aplicação sob múltiplos cenários de uso.

---

## 🛠️ Requisitos Técnicos

- **SDK:** [.NET 10.0 SDK](https://dotnet.microsoft.com/)
- **Linguagem:** C# 13+
- **Framework de Testes:** xUnit
- **Ferramenta de Linha de Comando:** .NET CLI (`dotnet`)

---

## ⚙️ Regras de Negócio Implementadas

### 1. `ContaCorrenteService`
Gerencia a cobrança de manutenção da conta e autorização de operações de saque.

* **`CalcularTaxaManutencao(decimal saldo, bool clienteEspecial)`**
  * **Isenção (R$ 0,00):** Saldo maior ou igual a R$ 5.000,00 **OU** se o cliente for especial (`clienteEspecial == true`).
  * **Taxa Intermediária (R$ 12,50):** Saldo entre R$ 1.000,00 e R$ 4.999,99 para clientes não especiais.
  * **Taxa Básica (R$ 25,00):** Saldo menor que R$ 1.000,00 para clientes não especiais.

* **`PodeRealizarSaque(decimal saldo, decimal limiteChequeEspecial, decimal valorSaque)`**
  * Retorna `true` apenas se o valor do saque for menor ou igual à soma do saldo com o limite do cheque especial (`valorSaque <= saldo + limiteChequeEspecial`).

---

### 2. `EmprestimoService`
Gerencia as taxas de juros com base no score do cliente e valida a margem consignável para aprovação do crédito.

* **`CalcularValorParcela(decimal valorTotal, int parcelas, int scoreCredito)`**
  * **Score $\ge$ 700:** Taxa de juros total de 5% sobre o valor principal (`valorTotal * 1.05m`).
  * **Score de 500 a 699:** Taxa de juros total de 12% sobre o valor principal (`valorTotal * 1.12m`).
  * **Score $<$ 500:** Taxa de juros total de 20% sobre o valor principal (`valorTotal * 1.20m`).
  * Retorna o valor de cada parcela dividindo o total com juros pela quantidade de parcelas.

* **`AprovarEmprestimo(decimal rendaMensal, decimal valorParcela)`**
  * O empréstimo é aprovado (`true`) somente se o valor da parcela não ultrapassar **30% da renda mensal** do cliente (`valorParcela <= rendaMensal * 0.30m`).

---

## 🚀 Como Clonar e Compilar a Solução

Siga os passos no terminal/prompt de comando para obter e compilar o projeto:

1. **Clonar o repositório:**
   ```bash
   git clone https://github.com/karolynekschaves/fintech-bank-xunit.git
   cd fintech-bank-xunit
   ```

2. **Compilar a solução:**
   ```bash
   dotnet build
   ```

*(Opcional)* Caso queira recriar a estrutura da solução do zero via .NET CLI:
```bash
dotnet new sln -n FintechBank
dotnet new console -n FintechBank.App -f net10.0
dotnet new xunit -n FintechBank.Tests -f net10.0
dotnet sln add FintechBank.App/FintechBank.App.csproj
dotnet sln add FintechBank.Tests/FintechBank.Tests.csproj
dotnet add FintechBank.Tests/FintechBank.Tests.csproj reference FintechBank.App/FintechBank.App.csproj
```

---

## 🧪 Estrutura de Testes Parametrizados (`[Theory]` / `[InlineData]`)

Em vez de criar métodos repetitivos para cada cenário, utilizou-se o atributo `[Theory]` do xUnit. Ele permite declarar a lógica de validação uma única vez e alimentá-la com múltiplos valores de entrada e retornos esperados usando o atributo `[InlineData]`.

### Cenários Cobertos

#### `ContaCorrenteServiceTests.cs`
- **Taxa de Manutenção:**
  - `[InlineData(6000.00, false, 0.00)]` — Saldo alto (Isento)
  - `[InlineData(500.00, true, 0.00)]` — Cliente Especial (Isento)
  - `[InlineData(2500.00, false, 12.50)]` — Saldo intermediário (R$ 12,50)
  - `[InlineData(300.00, false, 25.00)]` — Saldo baixo (R$ 25,00)
- **Validação de Saque:**
  - `[InlineData(1000.00, 500.00, 1200.00, true)]` — Dentro do limite (Saldo + Cheque Especial)
  - `[InlineData(1000.00, 500.00, 1600.00, false)]` — Excede o limite total

#### `EmprestimoServiceTests.cs`
- **Cálculo de Parcela com Juros por Score:**
  - `[InlineData(10000.00, 10, 800, 1050.00)]` — Score alto (5% juros $\rightarrow$ R$ 10.500 / 10 = R$ 1.050)
  - `[InlineData(10000.00, 10, 600, 1120.00)]` — Score médio (12% juros $\rightarrow$ R$ 11.200 / 10 = R$ 1.120)
  - `[InlineData(10000.00, 10, 400, 1200.00)]` — Score baixo (20% juros $\rightarrow$ R$ 12.000 / 10 = R$ 1.200)
- **Aprovação de Empréstimo (Margem Consignável):**
  - `[InlineData(5000.00, 1500.00, true)]` — Parcela igual a 30% da renda (Aprovado)
  - `[InlineData(5000.00, 1600.00, false)]` — Parcela acima de 30% da renda (Recusado)

---

## ⚡ Execução dos Testes Unitários

Para executar toda a suíte de testes e validar o comportamento das regras de negócio, utilize o comando abaixo no terminal a partir da raiz da solução:

```bash
dotnet test
```

Para ver detalhes de cada teste executado individualmente no terminal:

```bash
dotnet test --logger "console;verbosity=detailed"
```

---
Anna Vitória Rocha Dias - 325118421  - Dev Backend / Core  
Maycon Gomes - 325125878 - QA / Testes Unitários  
Karolyne Silva - 32517941 - Documentação e DevOps / Versionamento