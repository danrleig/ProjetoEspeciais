using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Newtonsoft.Json.Linq;
using ProjetoEspeciais.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ProjetoEspeciais.UI
{
    public partial class TelaLogin : Form
    {
        private WebView2 _webView;// Campo privado para armazenar a instância do WebView2
        public AtenaAuthService AuthService { get; private set; }// Propriedade pública para acessar o serviço de autenticação do Atena
        private const string URL_ATENA = "https://bo-cert.sptservices.io";// URL do login do Atena

        private readonly bool _forcarLogin;
        public TelaLogin() : this(false) { }

        public TelaLogin(bool forcarLogin)
        {
            InitializeComponent();
            _forcarLogin = forcarLogin;
        }

        private async void TelaLogin_Load(object sender, EventArgs e)
        {
            this.Text = "Login - Atena";
            this.Width = 1200;
            this.Height = 800;
            this.StartPosition = FormStartPosition.CenterScreen;

            _webView = new WebView2();
            _webView.Dock = DockStyle.Fill;
            this.Controls.Add(_webView);

            await _webView.EnsureCoreWebView2Async(null);

            _webView.CoreWebView2.WebResourceResponseReceived += OnResponseReceived;

            if (_forcarLogin)
            {
                // Limpa o localStorage na primeira navegação para forçar login real
                bool jaLimpou = false;

                _webView.CoreWebView2.NavigationCompleted += async (s, ev) =>
                {
                    if (jaLimpou) return;
                    jaLimpou = true;

                    await _webView.ExecuteScriptAsync("localStorage.clear()");
                };
            }
            else
            {
                // Login normal — tenta capturar token do localStorage
                _webView.CoreWebView2.NavigationCompleted += OnNavigationCompleted;
            }

            _webView.CoreWebView2.Navigate(URL_ATENA);
        }

        // Chamado automaticamente toda vez que uma página termina de carregar
        private async void OnNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            await TentarCapturarTokenDoLocalStorage();
        }

        // Lê o localStorage do WebView2 via JavaScript e tenta capturar o token
        private async Task TentarCapturarTokenDoLocalStorage()
        {
            try
            {
                // ExecuteScriptAsync executa JavaScript dentro do WebView2 e retorna o resultado
                // Aqui estamos lendo as chaves que precisamos do localStorage
                string script = @"
                JSON.stringify({
                accessToken: localStorage.getItem('accessToken'),
                refreshToken: localStorage.getItem('refreshToken'),
                accessExpiresAt: localStorage.getItem('accessExpiresAt'),
                csrf: localStorage.getItem('csrf')
                })
                    ";

                string resultado = await _webView.ExecuteScriptAsync(script);

                // O resultado vem entre aspas por ser uma string JSON — removemos elas
                // e desfazemos os escapes de caracteres especiais
                resultado = resultado.Trim('"').Replace("\\\"", "\"").Replace("\\\\", "\\");

                var json = Newtonsoft.Json.Linq.JObject.Parse(resultado);

                string accessToken = json["accessToken"]?.ToString();

                // Se não tem token ou é nulo, o usuário ainda não está logado
                if (string.IsNullOrEmpty(accessToken) || accessToken == "null") return;

                // Verifica se o token ainda é válido pelo tempo de expiração
                long expiresAt = 0;
                long.TryParse(json["accessExpiresAt"]?.ToString(), out expiresAt);

                if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() >= expiresAt) return;

                // Token válido encontrado! Captura e fecha a tela
                this.Invoke((Action)(() =>
                {
                    AuthService = new AtenaAuthService();
                    AuthService.DefinirToken(new Data.AtenaToken
                    {
                        AccessToken = accessToken,
                        RefreshToken = json["refreshToken"]?.ToString(),
                        Csrf = json["csrf"]?.ToString(),
                        AccessExpiresAt = expiresAt
                    });

                    AuthService.SalvarToken();

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }));
            }
            catch
            {
                // Ignora erros — pode ser que a página ainda não carregou o localStorage
            }
        }

        private async void OnResponseReceived(object sender, CoreWebView2WebResourceResponseReceivedEventArgs e)
        {
            try
            {
                // Só nos interessa a resposta da URL de login
                if (!e.Request.Uri.Contains("/atn/login")) return;

                // Lê o corpo da resposta
                var stream = await e.Response.GetContentAsync();
                var reader = new System.IO.StreamReader(stream);
                string body = await reader.ReadToEndAsync();

                // Se não tem accessToken na resposta, não é o login
                if (!body.Contains("accessToken")) return;

                // Desserializa o JSON para pegar o token
                var json = Newtonsoft.Json.Linq.JObject.Parse(body);
                string accessToken = json["accessToken"]?.ToString();
                string refreshToken = json["refreshToken"]?.ToString();
                string csrf = json["csrf"]?.ToString();
                long expiresAt = json["accessExpiresAt"]?.Value<long>() ?? 0;

                if (string.IsNullOrEmpty(accessToken)) return;

                // Precisamos voltar para a thread principal para mexer na interface
                // Regra do Windows Forms: só a thread principal pode mexer nos controles
                this.Invoke((Action)(() =>
                {
                    // Cria e preenche o AuthService com o token capturado
                    AuthService = new AtenaAuthService();
                    AuthService.DefinirToken(new Data.AtenaToken
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        Csrf = csrf,
                        AccessExpiresAt = expiresAt
                    });

                    AuthService.DefinirToken(new Data.AtenaToken
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        Csrf = csrf,
                        AccessExpiresAt = expiresAt
                    });

                    //Adicione essa linha para salvar o token em disco
                    AuthService.SalvarToken();

                    // Sinaliza sucesso e fecha a tela de login
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }));
            }
            catch
            {
                // Se der qualquer erro lendo a resposta, ignora e continua
            }
        }
    }


}

