#language:pt
#coding:utf-8

Funcionalidade: Operações CRUD no CartController
  Dado um usuário da API de Carrinho
  Eu quero ser capaz de criar, ler, atualizar e deletar itens no meu carrinho
  Para que eu possa gerenciar minha experiência de compra de forma eficaz.

  Cenário: Criar um novo carrinho para um cliente
    Dado que eu tenho um ID de cliente único
    Quando eu crio um novo carrinho para o cliente
    Então o status da resposta deve ser 201 Created
    E o carrinho deve existir no sistema

  Cenário: Recuperar um carrinho existente
    Dado que um carrinho existe para um cliente
    Quando eu recupero o carrinho para o cliente
    Então o status da resposta deve ser 200 OK
    E os Dados do carrinho devem corresponder ao formato esperado

  Cenário: Adicionar um item ao carrinho
    Dado que um carrinho existe para um cliente
    Quando eu adiciono um item ao carrinho com detalhes específicos
    Então o status da resposta deve ser 200 OK
    E o item deve aparecer no carrinho com os detalhes corretos

  Cenário: Atualizar a quantidade de um item no carrinho
    Dado que um carrinho com itens existe para um cliente
    Quando eu atualizo a quantidade de um item no carrinho
    Então o status da resposta deve ser 200 OK
    E a quantidade do item deve ser atualizada no carrinho

  Cenário: Remover um item do carrinho
    Dado que um carrinho com itens existe para um cliente
    Quando eu removo um item do carrinho
    Então o status da resposta deve ser 200 OK
    E o item não deve mais existir no carrinho

  Cenário: Obter o valor total do carrinho
    Dado que um carrinho com itens existe para um cliente
    Quando eu obtenho o valor total do carrinho
    Então o status da resposta deve ser 200 OK
    E o valor total deve estar correto