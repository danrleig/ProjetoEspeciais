using ProjetoEspeciais.UI;
using ProjetoEspeciais.Service;

namespace ProjetoEspeciais
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var authService = new AtenaAuthService();

            // Tenta carregar o token salvo — se ainda for válido, pula o login
            if (authService.CarregarTokenSalvo())
            {
                // Token válido — abre direto a TelaPrincipal
                Application.Run(new TelaPrincipal(authService));
            }
            else
            {
                // Token inválido ou inexistente — abre a tela de login
                using (var telaLogin = new TelaLogin())
                {
                    if (telaLogin.ShowDialog() == DialogResult.OK)
                    {
                        Application.Run(new TelaPrincipal(telaLogin.AuthService));
                    }
                }
            }
        }
    }
}