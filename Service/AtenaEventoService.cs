using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProjetoEspeciais.Data;

namespace ProjetoEspeciais.Service
{
    public class AtenaEventoService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string ORIGIN = "https://bo-cert.sptservices.io";
        private const string BASE_URL = "https://api.certified.apispt.net/atn/api/common/list";

        private readonly AtenaAuthService _authService;

        public AtenaEventoService(AtenaAuthService authService)
        {
            _authService = authService;
        }

        // Monta o header de autenticação — reutilizado em todas as requisições
        private HttpRequestMessage CriarRequest(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", $"Bearer {_authService.TokenValido.AccessToken.Trim()}");
            request.Headers.Add("Origin", ORIGIN);
            return request;
        }

        // Busca ligas filtradas pelo id do esporte
        public async Task<List<LigaItem>> BuscarLigasAsync(int idEsporte)
        {
            // Montamos a URL substituindo o id do esporte dinamicamente
            string filter = Uri.EscapeDataString(
                $"public.tipo_esporte.id={idEsporte} AND 1 = any(select (json_array_elements(templates_odd)::text)::integer) AND cache.evento.momento_realizacao::date >= 'now'::text::date AND cache.evento.momento_realizacao::date <= ('now'::text::date + 30)"
            );

            string url = $"{BASE_URL}/grupo_evento?columns=id,+nome_secundario+as+nome,+id_pais&filter={filter}&related=gerencia_grupo_evento:id+as+id_gerencia_grupo_evento&groupBy=cache.grupo_evento.id,cache.grupo_evento.id_pais,public.tipo_esporte.id,cache.grupo_evento.nome_secundario,data_source.esporte.id,+gerencia_grupo_evento.id&order=nome_secundario";

            var response = await _httpClient.SendAsync(CriarRequest(url));
            string body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao buscar ligas: {response.StatusCode}");

            var json = JObject.Parse(body);
            var array = (JArray)json["data"];

            var ligas = new List<LigaItem>();
            foreach (JObject item in array)
            {
                ligas.Add(new LigaItem
                {
                    Id = item["id"]?.Value<int>() ?? 0,
                    Nome = item["nome"]?.ToString(),
                    IdPais = item["id_pais"]?.Value<int?>()
                });
            }

            return ligas;
        }

        // Busca eventos filtrados pelo id da liga selecionada
        public async Task<List<EventoItem>> BuscarEventosAsync(int idLiga)
        {
            string hoje = DateTime.Now.ToString("yyyy-MM-dd");

            string filter = Uri.EscapeDataString(
             $"momento_realizacao >= '{hoje}' and id_tipo=1 and cache.evento.bloqueado IS NOT TRUE AND evento.outright IS NOT TRUE AND cache.evento.id_grupo_evento={idLiga}"

            );

            string url = $"{BASE_URL}/evento?columns=*&filter={filter}&related=grupo_evento:nome_secundario,id+as+id_grupo_evento&order=momento_realizacao";

            var response = await _httpClient.SendAsync(CriarRequest(url));
            string body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao buscar eventos: {response.StatusCode}");

            var json = JObject.Parse(body);
            var array = (JArray)json["data"];

            var eventos = new List<EventoItem>();
            foreach (JObject item in array)
            {
                eventos.Add(new EventoItem
                {
                    Id = item["id"]?.Value<int>() ?? 0,
                    Nome = item["nome"]?.ToString(),
                    MomentoRealizacao = item["momento_realizacao"]?.ToString(),
                    IdGrupoEvento = item["id_grupo_evento"]?.Value<int>() ?? 0,
                    NomeGrupoEvento = item["grupo_evento__nome_secundario"]?.ToString()
                });
            }

            return eventos;
        }
    }
}