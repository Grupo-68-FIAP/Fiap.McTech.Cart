# Fiap.McTech.Cart

Este projeto é uma API desenvolvida em C# usando .NET 6, com o Redis como banco de dados não relacional. Ele é responsável por gerenciar o carrinho de compras de uma aplicação de Toten de alimentação do projeto McTech.

## Descrição do Projeto

O **Fiap.McTech.Cart** fornece uma interface de programação de aplicativos (API) que permite aos usuários criar, ler, atualizar e excluir itens no carrinho de compras. Esta funcionalidade é fundamental para a experiência de compra em quiosques de alimentação, permitindo que os usuários gerenciem suas seleções de maneira eficaz.

## Abordagem Arquitetural

Para esta aplicação, decidimos seguir uma arquitetura mais simplificada, sem a complexidade de camadas como Arquitetura Limpa, Onion, Vertical Slice, Ports and Adapters, etc. Por ser uma aplicação simples, a ideia é que a arquitetura seja a mais direta possível para facilitar a manutenção. Abaixo, você pode visualizar um diagrama que ilustra essa abordagem.

![image](https://github.com/user-attachments/assets/bcad681d-a178-41f8-af38-718e1b054e98)

## Funcionalidades

- Criar um novo carrinho de compras para um cliente.
- Recuperar um carrinho existente.
- Adicionar itens ao carrinho.
- Atualizar a quantidade de itens no carrinho.
- Remover itens do carrinho.
- Obter o valor total do carrinho.

## Tecnologias Utilizadas

- **C#**: Linguagem de programação utilizada para desenvolver a API.
- **.NET 6**: Plataforma de desenvolvimento que fornece suporte para a construção da API.
- **Redis**: Banco de dados não relacional utilizado para armazenar os dados do carrinho de forma eficiente.

## Requisitos

- .NET 6 SDK
- Redis instalado e em execução

## Como Executar o Projeto

1. Clone o repositório:
   ```bash
   git clone https://github.com/seuusuario/Fiap.McTech.Cart.git](https://github.com/Grupo-68-FIAP/Fiap.McTech.Cart.git
   cd Fiap.McTech.Cart
