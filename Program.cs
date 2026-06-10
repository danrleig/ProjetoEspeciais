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
            bool tokenValido = false;

            if (authService.CarregarTokenSalvo())
            {
                tokenValido = authService.TestarTokenAsync()
                                         .GetAwaiter()
                                         .GetResult();
            }

            if (tokenValido)
            {
                Application.Run(new TelaPrincipal(authService));
            }
            else
            {
                authService.LimparTokenSalvo();

                using (var telaLogin = new TelaLogin(forcarLogin: true))
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