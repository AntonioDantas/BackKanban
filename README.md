
# Introduction 
Desafio Técnico - Backend da Escola de Programação Let's Code.
Autor: Antonio Dantas

# Getting Started
1. Será necessário copiar o arquivo appsettings.json para appsettings.Development.json e incluir as informações de usuário/senha e chave secreta key para o Jwt.
2. Para rodar usando o VS basta apertar em play que o launchSettings.json já está configurado para http://localhost:5000, já para o docker a porta está aleatória e talvez necessite de ajuste.

# Test
É possível utilizar a API diretamente pela interface gráfica em http://localhost:5000/swagger/index.html, para isso:
1. Abra a seção de login e clique em "Try it out"
2. Insira as informações de usuário e senha, e então clique em execute
3. Em caso de sucesso na solicitação o "Response body" apresentará o token gerado
4. Clique em Authorize no início da página e copie o token, em seguida clique em Authorize
5. As funções do cards estarão disponíveis para utilização em cada seção respectiva.

# Observações
Foi adicionado documentação para melhor compreensão das funcionalidades de cada chamada da API
Foi desenvolvido um projeto muito simples de teste das funcionalidades dos cards
Foi utilizado arquitetura de injeção por dependência com responsabilidades próprias (seguindo princípios SOLID)
Foi incorporado o StyleCop.Analyzers, porém nem todas as sugestões foram ajustadas ainda

Obrigado pela oportunidade, tenha um ótimo dia.
Antonio Dantas
