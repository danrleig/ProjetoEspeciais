using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoEspeciais.Data
{
    public class EsporteItem
    {
        public int Id { get; set; }
        public string Nome { get; set; }




        public override string ToString()
        {
            return Nome; // Isso faz com que o ComboBox mostre o nome do esporte
        }
    }
}
