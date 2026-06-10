using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoEspeciais.Data
{
    public class SuperOddItem
    {
        public int IdGrupoEvento { get; set; }        // id da liga selecionada
        public string NomeCasa { get; set; }           // nome da super odd
        public int IdEventoRelacionado { get; set; }   // id do evento do grid
        public System.DateTime MomentoRealizacao { get; set; }
        public decimal OddFinal { get; set; }
        public decimal OddOriginal { get; set; }
        public decimal ValorAposta { get; set; }
        public int IdPerfilRisco { get; set; }
        public bool ApostaExclusivaLink { get; set; }
        public bool ApostasExclusivasNovosUsuarios { get; set; }

    }
}
