using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoEspeciais.Data
{
    public class EventoItem
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string MomentoRealizacao { get; set; }
        public int IdGrupoEvento { get; set; }
        public string NomeGrupoEvento { get; set; }

        public override string ToString()
        { 
            if (DateTime.TryParse(MomentoRealizacao, out DateTime momento))
            {
                return $"{Nome} - {momento:dd/MM/yyyy HH:mm}";
            }
            return Nome;
        }

    }
}
