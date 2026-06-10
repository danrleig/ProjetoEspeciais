using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjetoEspeciais.Data;
using System;
using System.Buffers.Text;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoEspeciais.Service
{
    internal class AtenaCadastroSuperOddService
    {
        // Essa classe é responsável por todo o processo de cadastro de uma Super Odd no Atena
        // Ela replica exatamente os 4 passos que você faz manualmente no sistema



        // HttpClient é o objeto que faz as requisições HTTP (GET, POST, PUT)
        // É estático para ser compartilhado — criar um novo HttpClient a cada requisição pode causar problemas de desempenho e esgotamento de recursos, então é recomendado usar um único HttpClient para toda a aplicação.
        private static readonly HttpClient _httpClient = new HttpClient();


        private const string BASE_URL    = "https://api.certified.apispt.net/atn/api"; // URL base da API do Atena — todas as requisições começam com essa URL


        // O Origin é exigido pela API do Atena como um cabeçalho de segurança
        // Sem ele, a API rejeita a requisição como se viesse de um lugar não autorizado
        private const string ORIGIN = "https://bo-cert.sptservices.io";



        // Método auxiliar privado que monta uma requisição HTTP com os cabeçalhos obrigatórios
        // É privado porque só é usado internamente por essa classe
        // Recebe: o método HTTP (GET/POST/PUT), a URL e opcionalmente um objeto para o body
        // Retorna: um HttpRequestMessage pronto para ser enviado

        private readonly AtenaAuthService _authService; // => _authService que recebe o objeto AtenaAuthService que faz o login e fornece o token de acesso necessário para autenticar as requisições


        // Construtor — recebe o AuthService injetado de fora
        // Isso garante que sempre usamos o token válido da sessão atual
        public AtenaCadastroSuperOddService(AtenaAuthService authService)
        {
            _authService = authService;
        }

        // Método auxiliar privado que monta uma requisição HTTP com os cabeçalhos obrigatórios
        // É privado porque só é usado internamente por essa classe
        // Recebe: o método HTTP (GET/POST/PUT), a URL e opcionalmente um objeto para o body
        // Retorna: um HttpRequestMessage pronto para ser enviado


        private HttpRequestMessage CriarRequest(HttpMethod method, string url, object body = null)
        {
            var request = new HttpRequestMessage(method, url);

            // Authorization com Bearer token — é como a API identifica quem está fazendo a requisição
            // Trim() remove espaços ou quebras de linha que possam ter vindo do localStorage
            request.Headers.Add("Authorization", $"Bearer {_authService.TokenValido.AccessToken.Trim()}");

            // Origin é obrigatório — a API verifica se a requisição vem do domínio correto
            request.Headers.Add("Origin", ORIGIN);

            // Se um body foi passado, serializa o objeto para JSON e define o tipo do conteúdo
            // StringContent empacota o JSON em um formato que o HttpClient consegue enviar
            // Encoding.UTF8 garante que caracteres especiais (acentos) sejam enviados corretamente
            // "application/json" avisa à API que estamos enviando JSON
            if (body != null)
            {
                string json = JsonConvert.SerializeObject(body, new JsonSerializerSettings
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                });
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return request;


        }

        // ═══════════════════════════════════════════════════════
        // PÁGINA 1 — Cria o evento especial no Atena
        // Equivale ao POST /business/evento/cadastrar
        // ═══════════════════════════════════════════════════════
        // Recebe todos os dados necessários para criar o evento
        // Retorna o id do evento criado (capturado via GET após o POST)

        public async Task<int> CriarEventoEspecialAsync(int idGrupoEvento, string nomeCasa, int idEventoRelacionado, DateTime momentoRealizacao)// id da liga (Múltiplas Escolhas ou Novos Usuários)
                                                                                                                                                // nome da super odd digitado pelo usuário
                                                                                                                                                // id do evento de futebol vinculado
                                                                                                                                                // data e hora do evento
        {
            // Monta o objeto exatamente como o Atena espera receber
            // "new { }" cria um objeto anônimo — usado quando não precisamos de uma classe específica

            var payload = new
            {
                IdGrupoEvento = idGrupoEvento,
                NomeCasa = nomeCasa,
                nomeFora = "", // O nome do time fora é obrigatório, mas como a Super Odd não tem time fora, deixamos vazio
                disputaIndireta = true, // A Super Odd é sempre uma disputa indireta, então definimos como true
                especial = true, // A Super Odd é um tipo especial de evento, então definimos como true
                idsEventosRelacionados = new int[] { idEventoRelacionado },// A API do Atena espera um array de ids relacionados, mesmo que seja apenas um evento, então colocamos o id dentro de um array
                MomentoRealizacao = momentoRealizacao.AddHours(3).ToString("yyyyMMddHHmmss")// O Atena exige a data nesse formato específico: yyyyMMddHHmmss
            };

            //MessageBox.Show(JsonConvert.SerializeObject(payload, Formatting.Indented));// Exibe o payload em formato JSON para verificar se está correto antes de enviar a requisição

            var response = await _httpClient.SendAsync(CriarRequest(HttpMethod.Post,
                $"{BASE_URL}/business/evento/cadastrar", payload));

            string body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao criar evento especial: {response.StatusCode} - {body}");

            // O POST não retorna o id do evento criado diretamente
            // Por isso fazemos um GET logo em seguida para buscar o evento pelo nome
            return await BuscarIdEventoCriadoAsync(nomeCasa);

        }

        // Método auxiliar que busca o evento recém criado pelo nome
        // É necessário porque o POST da página 1 não retorna o id do evento
        // É privado porque só é chamado internamente pelo CriarEventoEspecialAsync
        private async Task<int> BuscarIdEventoCriadoAsync(string nomeCasa)
        {
            // Busca eventos criados nos últimos 5 minutos para encontrar o que acabamos de criar
            // AddMinutes(-5) garante que não perdemos o evento mesmo se houver pequena diferença de horário
            string agora = DateTime.Now.AddMinutes(-5).ToString("yyyy-MM-dd HH:mm:ss");

            // Uri.EscapeDataString codifica caracteres especiais para uso na URL
            // Por exemplo: espaços viram %20, = vira %3D etc.
            string filter = Uri.EscapeDataString(
                $"momento_realizacao >= '{agora}' and id_tipo=30 AND evento.outright IS NOT TRUE AND evento.nome_casa = '{nomeCasa}'"
            );

            string url = $"{BASE_URL}/common/list/evento?columns=*&filter={filter}&related=grupo_evento:id+as+id_grupo_evento&order=momento_realizacao&limit=-1";

            var response = await _httpClient.SendAsync(CriarRequest(HttpMethod.Get, url));
            string body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao buscar evento criado: {response.StatusCode}");

            var json = JObject.Parse(body);

            // json["data"] retorna o array de eventos
            // (JArray) faz o cast para JArray para podermos iterar
            var array = (JArray)json["data"];

            // Percorre os eventos retornados procurando o que tem o mesmo nome que criamos
            foreach (JObject item in array)
            {
                string nome = item["nome_casa"]?.ToString();

                // Comparação exata do nome para garantir que pegamos o evento certo
                if (nome == nomeCasa)
                {
                    // Value<int>() converte o valor JSON para int
                    // ?? 0 define 0 como valor padrão caso o campo seja nulo
                    return item["id"]?.Value<int>() ?? 0;
                }
            }

            throw new Exception($"Evento '{nomeCasa}' não encontrado após cadastro.");
        }

        // Busca os detalhes completos do evento por id
        // Usamos para capturar o idEventoFornecedorInt que é necessário nas páginas 3 e 4
        // Retorna uma tupla com o id e o idEventoFornecedorInt
        // Tupla é um tipo que permite retornar múltiplos valores de uma só vez
        public async Task<(int id, int idEventoFornecedorInt)> BuscarDetalhesEventoAsync(int idEvento)
        {
            string filter = Uri.EscapeDataString($"id_tipo=30 AND evento.id = {idEvento}");
            string url = $"{BASE_URL}/common/list/evento?columns=*&filter={filter}&limit=-1";

            var response = await _httpClient.SendAsync(CriarRequest(HttpMethod.Get, url));
            string body = await response.Content.ReadAsStringAsync();

            var json = JObject.Parse(body);
            var array = (JArray)json["data"];

            if (array.Count == 0)
                throw new Exception($"Evento id {idEvento} não encontrado.");

            var item = (JObject)array[0];

            int id = item["id"]?.Value<int>() ?? 0;

            // id_evento_fornecedor_int é o número após "ow_" no código do evento
            // Ex: "ow_11130" → idEventoFornecedorInt = 11130
            // Esse número é usado para montar o id_evento_fornecedor da página 4: "ow_" + número
            int idFornecedorInt = item["id_evento_fornecedor_int"]?.Value<int>() ?? 0;

            return (id, idFornecedorInt);
        }

        // ═══════════════════════════════════════════════════════
        // PÁGINA 2 — Cadastra as odds da super odd
        // Equivale ao POST /business/evento/modalidade
        // ═══════════════════════════════════════════════════════
        public async Task CadastrarOddsAsync(int idEvento, decimal oddFinal, decimal oddOriginal)
        {
            var payload = new
            {
                idEvento = idEvento,
                idTipoSubEvento = 1053,              // sempre 1053 para Super Odds
                nomeTipoSubevento = "Super Odds",    // sempre esse nome

                // Array com as duas opções de resultado da super odd
                odds = new[]
                {
                    new
                    {
                        resultado = "Sim",
                        idTipoDoResultado = 1,
                        odd = (double)oddFinal,      // OddFinal calculada no grid
                        oldOdd = (double)oddOriginal  // Odd original antes do aumento
                        // (double) converte decimal para double pois a API espera esse tipo
                    },
                    new
                    {
                        resultado = "Não",
                        idTipoDoResultado = 1,
                        odd = 0.0,    // sempre 0 para o resultado "Não"
                        oldOdd = 0.0  // sempre 0 para o resultado "Não"
                    }
                }
            };

            var response = await _httpClient.SendAsync(CriarRequest(HttpMethod.Post,
                $"{BASE_URL}/business/evento/modalidade", payload));

            if (!response.IsSuccessStatusCode)
            {
                string body = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao cadastrar odds: {response.StatusCode} - {body}");
            }
        }

        // ═══════════════════════════════════════════════════════
        // PÁGINA 3 — Libera o evento (desbloqueia)
        // Equivale ao PUT /entity/evento/releaseOrBlockEvent/{id}
        // ═══════════════════════════════════════════════════════
        public async Task LiberarEventoAsync(int idEvento, int idEventoFornecedorInt)
        {
            var payload = new
            {
                bloqueado = false,           // false = liberar, true = bloquear
                forceManual = false,          // sempre false
                idEventoFornecedorInt = idEventoFornecedorInt, // número do evento no fornecedor
                idFornecedorOrigem = 8,       // sempre 8 — id do fornecedor de origem
                idGestor = 150,               // seu id de usuário no sistema
                nomeGestor = "Danrlei Nascimento", // seu nome no sistema
                observacoes = "ok"            // texto padrão exigido pelo sistema
            };

            // O id do evento vai na própria URL, não no body
            string url = $"{BASE_URL}/entity/evento/releaseOrBlockEvent/{idEvento}";

            var response = await _httpClient.SendAsync(CriarRequest(HttpMethod.Put, url, payload));

            if (!response.IsSuccessStatusCode)
            {
                string body = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao liberar evento: {response.StatusCode} - {body}");
            }
        }

        // ═══════════════════════════════════════════════════════
        // PÁGINA 4 — Configura risco e limite de aposta
        // Equivale ao POST /entity/configuracao_evento
        // ═══════════════════════════════════════════════════════
        public async Task ConfigurarRiscoAsync(
            int idEvento,
            int idEventoFornecedorInt,
            decimal valorAposta,
            int idPerfilRisco,
            bool apostaExclusivaLink,
            bool apostasExclusivasNovosUsuarios)
        {
            // Monta o prefixo "ow_" + número — ex: "ow_11130"
            string idEventoFornecedor = $"ow_{idEventoFornecedorInt}";

            // configRisco é um objeto complexo que será serializado para string JSON
            // O Atena espera esse campo como uma string JSON dentro do JSON principal
            // Por isso serializamos separadamente com JsonConvert.SerializeObject
            var configRisco = new
            {
                limitePrejuizoVendaSimples = new { },   // objeto vazio — exigido pela API
                limitePrejuizoVendaCasada = new { },    // objeto vazio — exigido pela API
                fatorRiscoPorTipo = 0,                  // sempre 0
                fatorLimitePorEvento = 0,               // sempre 0
                ignorarBloqueioSequencial = false,      // sempre false
                idEventoGerencia = idEvento,            // id do evento especial criado na página 1
                apostasExclusivasNovosUsuarios = apostasExclusivasNovosUsuarios, // vem do checkbox
                apostaExclusivaLink = apostaExclusivaLink, // vem do checkbox

                // Define o limite de aposta para dois tipos de usuário
                // tipoUsuario 1 = usuário comum, tipoUsuario 3 = usuário VIP
                // limiteSimples = limite por aposta simples
                // limiteCasado = limite por aposta combinada
                // Ambos recebem o mesmo valor digitado pelo usuário no grid
                limitePorEvento = new[]
                {
                    new
                    {
                        tipoUsuario = 1,
                        limites = new
                        {
                            limiteSimples = (int)valorAposta,
                            limiteCasado = (int)valorAposta
                        }
                    },
                    new
                    {
                        tipoUsuario = 3,
                        limites = new
                        {
                            limiteSimples = (int)valorAposta,
                            limiteCasado = (int)valorAposta
                        }
                    }
                }
            };

            var payload = new
            {
                id_fornecedor = 9,                   // sempre 9 — Owner Provider
                id_evento_fornecedor = idEventoFornecedor, // "ow_" + idEventoFornecedorInt
                id_empresa = 2,                      // sempre 2 — Betsul
                id_localidade = 1,                   // sempre 1 — fixo
                id_perfil_risco_por_tipo = idPerfilRisco, // 22=2k, 18=5k, 20=10k
                id_usuario_cadastro = 150,           // seu id de usuário
                bloqueado = false,                   // sempre false ao criar
                // Serializa o objeto configRisco para string JSON
                // O Atena espera esse campo como string, não como objeto
                configuracao_risco = JsonConvert.SerializeObject(configRisco),
                // metadados fixo — exigido pela API mas sem uso prático aqui
                metadados = "{\"pgtoAntecipado\":false,\"percentualCotacaoPgtoAntecipado\":null}",
                // Todos os limites de prêmio zerados — não usados em super odds
                limite_premio_casa = 0,
                limite_premio_casado_empate = 0,
                limite_premio_casado_favorito = 0,
                limite_premio_casado_indistinto = 0,
                limite_premio_casado_zebra = 0,
                limite_premio_empate = 0,
                limite_premio_fora = 0,
                limite_premio_indistinto = 0,
                limite_premio_simples_empate = 0,
                limite_premio_simples_favorito = 0,
                limite_premio_simples_indistinto = 0,
                limite_premio_simples_zebra = 0
            };

            var response = await _httpClient.SendAsync(CriarRequest(HttpMethod.Post,
                $"{BASE_URL}/entity/configuracao_evento", payload));

            if (!response.IsSuccessStatusCode)
            {
                string body = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao configurar risco: {response.StatusCode} - {body}");
            }

            MessageBox.Show(JsonConvert.SerializeObject(configRisco, Formatting.Indented));
        }

        // ═══════════════════════════════════════════════════════
        // MÉTODO PRINCIPAL — Orquestra os 4 passos em sequência
        // É esse método que a TelaPrincipal vai chamar para cada linha do grid
        // ═══════════════════════════════════════════════════════
        public async Task<bool> CadastrarSuperOddAsync(
            int idGrupoEvento,
            string nomeCasa,
            int idEventoRelacionado,
            DateTime momentoRealizacao,
            decimal oddFinal,
            decimal oddOriginal,
            decimal valorAposta,
            int idPerfilRisco,
            bool apostaExclusivaLink,
            bool apostasExclusivasNovosUsuarios)
        {
            // Passo 1 — Cria o evento e já busca o id via GET
            int idEvento = await CriarEventoEspecialAsync(
                idGrupoEvento, nomeCasa, idEventoRelacionado, momentoRealizacao);

            // Passo 1.5 — Busca o idEventoFornecedorInt necessário para os passos 3 e 4
            // Desestruturação de tupla: _ ignora o primeiro valor (id), só usamos o segundo
            var (_, idEventoFornecedorInt) = await BuscarDetalhesEventoAsync(idEvento);

            // Passo 2 — Cadastra as odds
            await CadastrarOddsAsync(idEvento, oddFinal, oddOriginal);

            // Passo 3 — Libera o evento
            await LiberarEventoAsync(idEvento, idEventoFornecedorInt);

            // Passo 4 — Configura risco e limite de aposta
            await ConfigurarRiscoAsync(
                idEvento, idEventoFornecedorInt, valorAposta,
                idPerfilRisco, apostaExclusivaLink, apostasExclusivasNovosUsuarios);

            // Retorna true indicando que tudo correu bem
            return true;
        }

    }
}
  