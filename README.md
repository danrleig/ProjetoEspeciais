Projeto Especiais

Aplicação desktop desenvolvida em **C# e .NET com Windows Forms**, criada para automatizar e centralizar processos operacionais relacionados ao gerenciamento de eventos e mercados esportivos.

O projeto integra uma aplicação desktop com uma **API REST externa**, permitindo autenticação, consulta de dados, processamento de informações e execução de operações diretamente pela interface da aplicação.

Objetivo do Projeto:

O projeto surgiu da necessidade de otimizar processos que anteriormente exigiam diversas etapas manuais.

A aplicação foi desenvolvida para centralizar essas operações em uma interface desktop, permitindo:

* autenticação e gerenciamento de sessão;
* consumo de dados através de API REST;
* consulta dinâmica de esportes, países, competições e eventos;
* criação e gerenciamento de configurações especiais;
* aplicação de regras de negócio e validações;
* cálculo automático de valores e odds;
* importação e processamento de arquivos CSV;
* redução de tarefas repetitivas e erros operacionais.

Tecnologias Utilizadas:

* C#
* .NET
* Windows Forms
* HttpClient
* APIs REST
* JSON
* Programação Assíncrona com async/await
* LINQ
* CsvHelper
* Git
* GitHub

Estrutura do Projeto:

O projeto foi organizado buscando separar as responsabilidades da aplicação.

### Models

Responsáveis pela representação das entidades e estruturas de dados utilizadas pela aplicação.

Exemplos:

* eventos;
* esportes;
* competições;
* mercados;
* usuário autenticado;
* configurações das operações.

Services

Camada responsável pela comunicação com serviços externos e processamento das regras da aplicação.

Entre suas responsabilidades estão:

* autenticação;
* gerenciamento de sessão;
* comunicação com APIs;
* carregamento de eventos;
* processamento de dados;
* leitura de arquivos CSV;
* envio de operações para serviços externos.

UI

Camada responsável pela interface gráfica da aplicação, interação com o usuário, validação dos campos e apresentação das informações.

Autenticação e Sessão

A aplicação possui um fluxo de autenticação integrado ao sistema externo.

Após a autenticação, a aplicação realiza o gerenciamento da sessão e utiliza as informações do usuário autenticado nas operações realizadas pelo sistema.

O fluxo inclui:

1. autenticação do usuário;
2. obtenção e armazenamento da sessão;
3. validação da sessão existente;
4. carregamento dos dados do usuário;
5. utilização da autenticação nas requisições à API.

Informações sensíveis, credenciais e tokens não são armazenados no repositório público.

Integração com API REST

A comunicação com os serviços externos é realizada utilizando `HttpClient`.

A aplicação executa operações como:

* requisições GET e POST;
* envio e recebimento de JSON;
* desserialização de respostas;
* tratamento de códigos HTTP;
* autenticação de requisições;
* tratamento de erros;
* carregamento assíncrono de informações.

As chamadas são realizadas utilizando `async/await`, evitando o bloqueio da interface durante operações de rede.

Funcionalidades

Consulta dinâmica de dados

A aplicação permite carregar informações provenientes da API, como:

* esportes;
* países;
* competições;
* eventos;
* mercados e configurações relacionadas.

Os dados são apresentados em componentes da interface e podem ser filtrados dinamicamente.

### Cadastro de configurações especiais

A aplicação permite configurar operações utilizando dados dos eventos disponíveis.

Antes do envio, são realizadas validações como:

* preenchimento dos campos obrigatórios;
* validação de valores numéricos;
* validação das regras da operação;
* cálculo automático de valores;
* confirmação da operação pelo usuário.

Cálculo automático

O sistema possui regras para cálculo automático de valores utilizados nas operações.

Exemplo de regra:

`Valor Final = Valor Base × (1 + Percentual de Ajuste / 100)`

Os cálculos são executados automaticamente pela aplicação, reduzindo erros de preenchimento manual.

Importação de arquivos CSV

O projeto possui suporte à leitura e processamento de arquivos CSV.

O fluxo de importação permite:

1. seleção do arquivo através da interface do Windows;
2. leitura do arquivo;
3. conversão dos registros para objetos da aplicação;
4. validação dos dados;
5. utilização das informações nas regras do sistema.

Conceitos Aplicados

Durante o desenvolvimento foram utilizados conceitos como:

* Programação Orientada a Objetos;
* separação de responsabilidades;
* arquitetura em camadas;
* integração entre sistemas;
* consumo de APIs REST;
* programação assíncrona;
* tratamento de exceções;
* validação de dados;
* manipulação de JSON;
* processamento de arquivos CSV;
* aplicação de regras de negócio;
* desenvolvimento de interfaces desktop.

Demonstração

> Screenshots e GIFs demonstrando as principais funcionalidades da aplicação serão adicionados nesta seção.

Melhorias Futuras

Entre as melhorias planejadas para o projeto estão:

* ampliação da cobertura de logs;
* melhorias no tratamento centralizado de exceções;
* criação de testes automatizados;
* aprimoramento da experiência do usuário;
* implementação de novos módulos de análise e automação;
* expansão das funcionalidades de importação de dados.

Autor

**Danrlei Gomes**

Bacharel em Sistemas de Informação, com experiência profissional em infraestrutura de TI, suporte técnico, redes, servidores e análise de risco, além de desenvolvimento de soluções utilizando C#, .NET, SQL, APIs REST e automação de processos.
