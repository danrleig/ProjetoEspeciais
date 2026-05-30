using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoEspeciais.Data
{
    public class LigaItem
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int? IdPais { get; set; }

        public override string ToString() => Nome;
    }
}
