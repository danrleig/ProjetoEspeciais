using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoEspeciais.Data
{
    // Essa classe representa os dados que o Atena nos devolve após o login.
    // Cada propriedade aqui corresponde a um campo do JSON de resposta.

    public class AtenaToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Csrf { get; set; }// O token CSRF é usado para proteger contra ataques de falsificação de solicitação entre sites. Ele é gerado pelo servidor e deve ser incluído em cada solicitação subsequente para garantir que a solicitação seja legítima.
        public long AccessExpiresAt { get; set; }

    }
}
