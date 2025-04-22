 # Projeto - Cadastro de Empresas com Consulta via CNPJ
## Pre-requisitos
MySQL Workbench 8.0
## Build da aplicação
Ao abrir o programa é nescessario a configuração da conecção do MySql a partir do do arquivo de configuração appsettings.json e alterar o Uid(UserId) e Pwd(Password) para aquele configurado no MySQL.
 
Abrir o terminal PowerShell do desenvolvedor, verificar se está dentro da pasta do projeto e usar os comandos ```dotnet ef migrations add (NomedaMigração)``` em seguida ```dotnet ef database update```, para criar as tabelas no banco de dados.

## Objetivo

Desenvolver uma aplicação que permita o cadastro e autenticação de usuários, bem como o cadastro de empresas a partir do número de CNPJ, integrando-se à API pública [ReceitaWS](https://developers.receitaws.com.br/#/operations/queryCNPJFree) para obtenção automática de dados. 
## Aplicação
Este programa usa o Swagger para acessar os Endpoints:
1. Registro de Usuario (`api/Usuario/cadastro`): Faz o registro do usuario no banco de dados
2. Login de Usuario (`api/Usuario/login`) : Gera o Token do JWT do usuario cadastrado
3. Lista das Empresas(`api/Empresa/listar_empresas`): Lista todas as empresas cadastradas pelo usuario
4. Cadastro das Empresas(`api/Empresa/cadastrar_cnpj`): Pelo Cnpj busca as informações na ReceitaWs e cadastra as informações
### Fluxograma
![Fluxograma](https://github.com/gmitisuo/Localize/blob/master/Fluxograma.jpeg)

![Diagrama de Banco de dados](https://github.com/gmitisuo/Localize/blob/master/Diagrama_de_Banco_de_dados.jpeg)


## Melhorias
1. O SecretKey da configuração do Token do JWT deve ser armazenado em um ambiente seguro e não deve ser commitado no GitHub. Para o projeto ele está exposto apenas para fins de teste.
2. Atualização das informações da empresa, por exemplo se uma empresa cadastrada mudar de endereço, deveria ter um fluxo onde as informações cadastradas devem ser atualizadas.