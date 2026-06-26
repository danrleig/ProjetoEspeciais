using System;
using System.Collections.Generic;
using System.Text;
using ProjetoEspeciais.Data;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjetoEspeciais.Data;
using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography.X509Certificates;
using System.IO;
namespace ProjetoEspeciais.Service

{

    // HttpClient é a classe do C# responsável por fazer requisições HTTP
    // É uma boa prática deixá-lo como estático para reutilizar a conexão
    public class AtenaAuthService
    {
        private static readonly HttpClient _httpClient = new HttpClient(); //Um objeto que será salvo dentro de uma variavel estática, ou seja, compartilhada por todas as instâncias da classe AtenaAuthService. Ele é usado para fazer requisições HTTP.
      
        private const string LOGIN_URL = "https://api.certified.apispt.net/atn/login";
        private const string ORIGIN = "https://bo-cert.sptservices.io";
        public AtenaToken TokenValido { get; set; }


        // "async Task<bool>" significa que esse método:
        // - Roda de forma assíncrona (não trava a tela enquanto espera a resposta)
        // - Retorna true se o login funcionou, false se falhou
        public async Task<bool> FazerLoginAsync(string email, string senha, string totp)
        {
            try
            {
                // Monta o objeto que será enviado como JSON no corpo da requisição
                var payload = new
                {
                    email = email,
                    senha = senha,
                    totp = totp
                };

                string jsonPayload = JsonConvert.SerializeObject(payload); // Converte o objeto payload para uma string JSON
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");  // Cria o conteúdo HTTP com o JSON e define o tipo como "application/json"

                // Monta a requisição manualmente para adicionar os headers necessários
                var request = new HttpRequestMessage(HttpMethod.Post, LOGIN_URL);
                request.Content = content;
                request.Headers.Add("Origin", ORIGIN);

                HttpResponseMessage response = await _httpClient.SendAsync(request); // Envia a requisição e espera a resposta

                string responseBody = await response.Content.ReadAsStringAsync(); // Lê o corpo da resposta como string

                if (response.IsSuccessStatusCode)
                {
                    JObject jsonResponse = JObject.Parse(responseBody); // Converte a string JSON de resposta para um objeto JObject

                    TokenValido = new AtenaToken
                    {
                        AccessToken = jsonResponse["accessToken"]?.ToString(),
                        RefreshToken = jsonResponse["refreshToken"]?.ToString(),
                        Csrf = jsonResponse["csrf"]?.ToString(),
                        AccessExpiresAt = jsonResponse["accessExpiresAt"]?.Value<long>() ?? 0
                    };

                    return true;// Se a resposta for um código de sucesso (200-299), extrai os tokens e retorna true



                }
                else
                {
                    JObject json = JObject.Parse(responseBody); // Se a resposta for um código de erro, tenta extrair a mensagem de erro do JSON
                    string mensagem = json["message"]?.ToString() ?? "Erro desconhecido"; // Se não conseguir extrair a mensagem, usa um texto genérico
                    throw new Exception("Erro de login: {mensagem}"); // Lança uma exceção com a mensagem de erro
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha ao conectar ao Atena: {ex.Message}");
            }
        }
            public bool TokenEstaValido() 
            { 
                if (TokenValido == null) return false; // Se não tiver token, não é válido
                return DateTimeOffset.UtcNow.ToUnixTimeSeconds() < TokenValido.AccessExpiresAt; // Compara o tempo atual com o tempo de expiração do token
            }

        // Ele permite definir o token externamente (usado pela TelaLogin após capturar do WebView)
        public void DefinirToken(Data.AtenaToken token)
        {
            TokenValido = token;
        }

       

    // Caminho onde o token será salvo no computador
    private static readonly string TOKEN_PATH = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ProjetoEspeciais", "token.json"
    );

        // Salva o token em disco após o login
        public void SalvarToken()
        {
            // Cria a pasta se não existir
            Directory.CreateDirectory(Path.GetDirectoryName(TOKEN_PATH));

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(TokenValido);
            File.WriteAllText(TOKEN_PATH, json);
        }

        // Tenta carregar o token salvo do disco
        // Retorna true se encontrou um token ainda válido
        public bool CarregarTokenSalvo()
        {
            if (!File.Exists(TOKEN_PATH)) return false;

            try
            {
                string json = File.ReadAllText(TOKEN_PATH);
                TokenValido = Newtonsoft.Json.JsonConvert.DeserializeObject<Data.AtenaToken>(json);
                return TokenEstaValido();
            }
            catch
            {
                return false;
            }
        }

        public void LimparTokenSalvo()
        {
            TokenValido = null;

            if (File.Exists(TOKEN_PATH))
                File.Delete(TOKEN_PATH);
        }


        public async Task<bool> TestarTokenAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get,
                    "https://api.certified.apispt.net/atn/api/common/list/esporte_empresa?columns=id_tipo_esporte+AS+id&filter=id_empresa+%3D+2&related=tipo_esporte:nome+as+nome&order=nome&limit=1");

                request.Headers.Add("Authorization", $"Bearer {TokenValido.AccessToken.Trim()}");
                request.Headers.Add("Origin", "https://bo-cert.sptservices.io");

                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
