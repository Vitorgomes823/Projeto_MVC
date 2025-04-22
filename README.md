# Projeto MVC - Simulador de Cálculo de IRPF

Este projeto é uma aplicação que simula o cálculo do **IRPF (Imposto de Renda Pessoa Física)** utilizando o padrão de arquitetura **MVC (Model-View-Controller)**. Ele foi desenvolvido para demonstrar como funciona a estrutura de um sistema organizado em camadas e, ao mesmo tempo, auxiliar no entendimento do cálculo do imposto de renda.

## 📋 Estrutura do Projeto

### 1. **Model**
A camada de Model é responsável por representar os dados e a lógica de negócios do cálculo de IRPF. Aqui são implementadas as regras de cálculo, como:
- Faixas de tributação.
- Deduções permitidas.
- Cálculo do valor devido ou a ser restituído.

### 2. **View**
A View é responsável pela interface com o usuário. Nela, o usuário pode:
- Inserir os dados necessários para o cálculo (rendimentos, dependentes, despesas dedutíveis, etc.).
- Visualizar os resultados do cálculo, como o valor do imposto devido ou a restituição.

### 3. **Controller**
A camada de Controller conecta a View e o Model. Ela processa as entradas do usuário, envia os dados para o Model realizar os cálculos e retorna os resultados para a View.

## 🚀 Como Executar o Projeto

### Pré-requisitos
Certifique-se de ter os seguintes itens instalados:
- **.NET Framework ou .NET Core** (para executar projetos em C#).
- Um navegador web para acessar a aplicação.

### Passos
1. Clone este repositório:
   ```bash
   git clone https://github.com/Vitorgomes823/Projeto_MVC.git
   ```
2. Abra o projeto no **Visual Studio** ou no seu editor favorito.
3. Restaure os pacotes necessários (se aplicável).
4. Compile e execute o projeto.
5. Acesse o sistema no navegador, geralmente disponível em `http://localhost:5000`.

## 📦 Funcionalidades

- Entrada de dados como rendimentos, despesas dedutíveis e dependentes.
- Simulação do cálculo do IRPF com base nas regras vigentes.
- Exibição do valor total do imposto devido ou da restituição.

## 🛠️ Tecnologias Utilizadas

- **C#**: Para a lógica de negócios e controladores.
- **HTML/CSS**: Para a interface do usuário.
- **JavaScript**: Para funcionalidades dinâmicas (se necessário).

## 🤝 Contribuição

Contribuições são bem-vindas! Siga os passos abaixo para colaborar:
1. Faça um fork do repositório.
2. Crie uma branch com sua funcionalidade/ajuste:
   ```bash
   git checkout -b minha-feature
   ```
3. Realize as alterações e faça o commit:
   ```bash
   git commit -m "Minha nova feature"
   ```
4. Envie as alterações para o repositório:
   ```bash
   git push origin minha-feature
   ```
5. Abra um Pull Request.

