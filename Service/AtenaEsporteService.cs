using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProjetoEspeciais.Data;

namespace ProjetoEspeciais.Service
{
    public class AtenaEsporteService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        private const string ORIGIN = "https://api.certified.apispt.net/atn/api/common/list/esporte_empresa?columns=id_tipo_esporte+AS+id&filter=id_empresa+%3D+2&related=tipo_esporte:nome+as+nome&order=nome";
        private const string URL_ESPORTES = "https://api.certified.apispt.net/atn/api/common/list/esporte_empresa?columns=id_tipo_esporte+AS+id&filter=id_empresa+%3D+2&related=tipo_esporte:nome+as+nome&order=nome";
        private readonly AtenaAuthService _authService;// Recebe o serviço de autenticação para usar o token

        public AtenaEsporteService(AtenaAuthService authService)// Construtor agora RECEBE o authService como parâmetro
        {
            _authService = authService;
        }

        public async Task<List<EsporteItem>> BuscarEsportesAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, URL_ESPORTES);

            // Adiciona o token no cabeçalho Authorization — é assim que a API sabe que você está autenticado
            request.Headers.Add("Authorization", $"Bearer {_authService.TokenValido.AccessToken.Trim()}");
            request.Headers.Add("Origin", ORIGIN);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            string responseBody = await response.Content.ReadAsStringAsync();

            //MessageBox.Show(responseBody);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao buscar esportes: {response.StatusCode}");

            // A resposta é um array JSON — cada item tem "id" e "nome"
            JObject jsonResponse = JObject.Parse(responseBody);
            JArray jsonArray = (JArray)jsonResponse["data"];

            var esportes = new List<EsporteItem>();

            foreach (JObject item in jsonArray)
            {
                esportes.Add(new EsporteItem
                {
                    Id = item["id"]?.Value<int>() ?? 0,
                    Nome = item["nome"]?.ToString()
                });
            }

            return esportes;
        }
    }
}

