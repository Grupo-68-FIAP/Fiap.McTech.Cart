Feature: Operações CRUD no CartController
  Given um usuário da API de Carrinho
  Eu quero ser capaz de criar, ler, atualizar e deletar itens no meu carrinho
  Para que eu possa gerenciar minha experiência de compra de forma eficaz.

  Scenario: Criar um novo carrinho para um cliente
    Given que eu tenho um ID de cliente único
    When eu crio um novo carrinho para o cliente
    Then o status da resposta deve ser 201 Created
    And o carrinho deve existir no sistema

  Scenario: Recuperar um carrinho existente
    Given que um carrinho existe para um cliente
    When eu recupero o carrinho para o cliente
    Then o status da resposta deve ser 200 OK
    And os Givens do carrinho devem corresponder ao formato esperado

  Scenario: Adicionar um item ao carrinho
    Given que um carrinho existe para um cliente
    When eu adiciono um item ao carrinho com detalhes específicos
    Then o status da resposta deve ser 200 OK
    And o item deve aparecer no carrinho com os detalhes corretos

  Scenario: Atualizar a quantidade de um item no carrinho
    Given que um carrinho com itens existe para um cliente
    When eu atualizo a quantidade de um item no carrinho
    Then o status da resposta deve ser 200 OK
    And a quantidade do item deve ser atualizada no carrinho

  Scenario: Remover um item do carrinho
    Given que um carrinho com itens existe para um cliente
    When eu removo um item do carrinho
    Then o status da resposta deve ser 200 OK
    And o item não deve mais existir no carrinho

  Scenario: Obter o valor total do carrinho
    Given que um carrinho com itens existe para um cliente
    When eu obtenho o valor total do carrinho
    Then o status da resposta deve ser 200 OK
    And o valor total deve estar correto