using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ProjetoEspeciais.Service;
using ProjetoEspeciais.Data;

namespace ProjetoEspeciais.UI
{
    public partial class TelaPrincipal : Form
    {
        // Guarda o serviço de autenticação recebido — tem o token válido
        private readonly AtenaAuthService _authService;

        // Construtor agora RECEBE o authService como parâmetro
        public TelaPrincipal(AtenaAuthService authService)
        {
            InitializeComponent();
            _authService = authService;
        }

        private async void TelaPrincipal_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            //dataGridEspeciais.BackgroundColor = dataGridEspeciais.ColumnHeadersDefaultCellStyle.BackColor;
           // dataGridEspeciais.BackgroundColor = SystemColors.Control;
            dataGridEspeciais.ReadOnly = false;// Permite edição, mas vamos controlar quais colunas são editáveis
            dataGridEspeciais.Columns["Esporte"].ReadOnly = true;// Esporte não pode ser editado diretamente na grid, só pela seleção
            dataGridEspeciais.Columns["Esporte"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridEspeciais.Columns["Liga"].ReadOnly = true;// Liga não pode ser editada diretamente na grid, só pela seleção
            dataGridEspeciais.Columns["Liga"].Width = 200;
            dataGridEspeciais.Columns["Liga"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridEspeciais.Columns["DataEvento"].ReadOnly = true;// Data do evento não pode ser editada diretamente na grid
            dataGridEspeciais.Columns["Evento"].ReadOnly = true;// Evento não pode ser editado diretamente na grid, só pela seleção
            dataGridEspeciais.Columns["Evento"].Width = 200;
            dataGridEspeciais.Columns["Evento"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridEspeciais.Columns["NomeEspecial"].ReadOnly = false;// Nome do especial pode ser editado
            dataGridEspeciais.Columns["NomeEspecial"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridEspeciais.Columns["Odd"].ReadOnly = false;// Odd pode ser editada
            dataGridEspeciais.Columns["Odd"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridEspeciais.Columns["ValorAumento"].ReadOnly = false;// Valor de aumento pode ser editado
            dataGridEspeciais.Columns["ValorAumento"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridEspeciais.Columns["OddFinal"].ReadOnly = false;// Odd final é calculada, não pode ser editada
            dataGridEspeciais.Columns["OddFinal"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridEspeciais.Columns["ValorAposta"].ReadOnly = false;// Valor da aposta pode ser editado
            dataGridEspeciais.Columns["ValorAposta"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;




            // Adiciona coluna de exclusão com ícone de lixeira
            var colunaExcluir = new DataGridViewButtonColumn();
            colunaExcluir.Name = "Excluir";
            colunaExcluir.HeaderText = "";
            colunaExcluir.Text = "🗑";
            colunaExcluir.UseColumnTextForButtonValue = true;
            colunaExcluir.Width = 40;
            colunaExcluir.FlatStyle = FlatStyle.Flat;
            dataGridEspeciais.Columns.Add(colunaExcluir);

            colunaExcluir.DefaultCellStyle.BackColor = Color.White;
            colunaExcluir.DefaultCellStyle.ForeColor = Color.Black;
            colunaExcluir.DefaultCellStyle.SelectionBackColor = Color.White;
            colunaExcluir.DefaultCellStyle.SelectionForeColor = Color.Black;

            await CarregarEsportesAsync();
        }

        private async Task CarregarEsportesAsync()
        {
            try
            {
                comboBoxEsportes.SelectedIndexChanged -= comboBoxEsportes_SelectedIndexChanged;
                comboBoxEsportes.Enabled = false;
                comboBoxEsportes.Items.Clear();
                comboBoxEsportes.Items.Add("Carregando esportes...");
                comboBoxEsportes.SelectedIndex = 0;

                var esporteService = new AtenaEsporteService(_authService);
                var esportes = await esporteService.BuscarEsportesAsync();

                comboBoxEsportes.Items.Clear();
                foreach (var esporte in esportes)
                    comboBoxEsportes.Items.Add(esporte);

                comboBoxEsportes.Enabled = true;
                comboBoxEsportes.SelectedIndexChanged += comboBoxEsportes_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                comboBoxEsportes.SelectedIndexChanged += comboBoxEsportes_SelectedIndexChanged;
                comboBoxEsportes.Items.Clear();
                comboBoxEsportes.Enabled = true;

                // Se for erro de autenticação, tenta renovar o login e recarrega
                if (ex.Message.Contains("Unauthorized"))
                {
                    bool renovado = await RenovarLoginAsync();
                    if (renovado) await CarregarEsportesAsync(); // Tenta de novo com o novo token
                    return;
                }

                MessageBox.Show($"Erro ao carregar esportes: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void dataGridEspeciais_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)// Valida a entrada do usuário para garantir que apenas números sejam digitados nas colunas de Odd, ValorAumento, OddFinal e ValorAposta
        {
            var colunas = new List<string> { "Odd", "ValorAumento", "OddFinal", "ValorAposta" };

            string nomeColuna = dataGridEspeciais.Columns[e.ColumnIndex].Name;

            if (colunas.Contains(nomeColuna))
            {
                string valor = e.FormattedValue.ToString();

                if (!string.IsNullOrEmpty(valor) && !decimal.TryParse(valor, out _))
                {
                    MessageBox.Show("Digite apenas números!");
                    e.Cancel = true;
                }
            }
        }

        private void dataGridEspeciais_CellEndEdit(object sender, DataGridViewCellEventArgs e)// Sempre que o usuário terminar de editar uma célula, recalcula a OddFinal se a coluna editada for Odd ou ValorAumento
        {
            var row = dataGridEspeciais.Rows[e.RowIndex];

            string nomeColuna = dataGridEspeciais.Columns[e.ColumnIndex].Name;

            if (nomeColuna == "Odd" || nomeColuna == "ValorAumento")
            {
                decimal odd = 0;
                decimal aumento = 0;

                decimal.TryParse(row.Cells["Odd"].Value?.ToString(), out odd);
                decimal.TryParse(row.Cells["ValorAumento"].Value?.ToString(), out aumento);

                decimal oddFinal = odd * (1 + (aumento / 100));

                row.Cells["OddFinal"].Value = Math.Round(oddFinal, 2);
            }
        }

        private async Task CarregarLigasAsync(int idEsporte)
        {
            try
            {
                comboBoxLigas.SelectedIndexChanged -= comboBoxLigas_SelectedIndexChanged;
                comboBoxEventos.SelectedIndexChanged -= comboBoxEventos_SelectedIndexChanged;

                comboBoxLigas.Enabled = false;
                comboBoxEventos.Enabled = false;
                comboBoxLigas.Items.Clear();
                comboBoxEventos.Items.Clear();

                var service = new AtenaEventoService(_authService);
                var ligas = await service.BuscarLigasAsync(idEsporte);

                foreach (var liga in ligas)
                    comboBoxLigas.Items.Add(liga);

                comboBoxLigas.Enabled = true;
                comboBoxLigas.SelectedIndexChanged += comboBoxLigas_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                comboBoxLigas.SelectedIndexChanged += comboBoxLigas_SelectedIndexChanged;
                comboBoxEventos.SelectedIndexChanged += comboBoxEventos_SelectedIndexChanged;
                comboBoxLigas.Enabled = true;

                if (ex.Message.Contains("Unauthorized"))
                {
                    bool renovado = await RenovarLoginAsync();
                    if (renovado) await CarregarEsportesAsync();
                    return;
                }

                MessageBox.Show($"Erro ao carregar ligas: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CarregarEventosAsync(int idLiga)
        {
            try
            {
                comboBoxEventos.SelectedIndexChanged -= comboBoxEventos_SelectedIndexChanged;

                comboBoxEventos.Enabled = false;
                comboBoxEventos.Items.Clear();

                var service = new AtenaEventoService(_authService);
                var eventos = await service.BuscarEventosAsync(idLiga);

                foreach (var evento in eventos)
                    comboBoxEventos.Items.Add(evento);

                comboBoxEventos.Enabled = true;
                comboBoxEventos.SelectedIndexChanged += comboBoxEventos_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                comboBoxEventos.SelectedIndexChanged += comboBoxEventos_SelectedIndexChanged;
                comboBoxEventos.Enabled = true;

                if (ex.Message.Contains("Unauthorized"))
                {
                    bool renovado = await RenovarLoginAsync();
                    if (renovado) await CarregarEsportesAsync();
                    return;
                }

                MessageBox.Show($"Erro ao carregar eventos: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AdicionarEventoNoGrid(EventoItem evento)
        {
            int index = dataGridEspeciais.Rows.Add();
            var row = dataGridEspeciais.Rows[index];

            // Pega o esporte e liga selecionados nos comboBoxes
            string esporte = comboBoxEsportes.SelectedItem?.ToString() ?? "";
            string liga = comboBoxLigas.SelectedItem?.ToString() ?? "";

            row.Cells["Esporte"].Value = esporte;
            row.Cells["Liga"].Value = liga;

            // Formata a data do evento para exibição
            if (DateTime.TryParse(evento.MomentoRealizacao, out DateTime data))
                row.Cells["DataEvento"].Value = data.ToString("dd/MM/yyyy HH:mm");

            row.Cells["Evento"].Value = evento.Nome;
            row.Cells["Odd"].Value = 0.00m;
            row.Cells["ValorAumento"].Value = 0;
            row.Cells["OddFinal"].Value = 0.00m;
        }

        // Chamado sempre que receber erro 401 (token expirado ou sessão inválida)
        private async Task<bool> RenovarLoginAsync()
        {
            using (var telaLogin = new TelaLogin(forcarLogin: true))


                MessageBox.Show("Sessão expirada ou inválida. Faça login novamente.", "Sessão Inválida",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            // Apaga o token salvo em disco para forçar login real
            _authService.LimparTokenSalvo();

            using (var telaLogin = new TelaLogin())
            {
                if (telaLogin.ShowDialog() == DialogResult.OK)
                {
                    _authService.DefinirToken(telaLogin.AuthService.TokenValido);
                    _authService.SalvarToken();
                    return true;
                }
            }

            return false;
        }

        private void dataGridEspeciais_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var colunas = new List<string> { "Odd", "ValorAumento", "OddFinal", "ValorAposta" };
            string nomeColuna = dataGridEspeciais.Columns[dataGridEspeciais.CurrentCell.ColumnIndex].Name;

            if (colunas.Contains(nomeColuna))
            {
                e.Control.KeyPress -= ApenasNumeros; // evita registrar o evento duas vezes
                e.Control.KeyPress += ApenasNumeros;
            }
            else
            {
                e.Control.KeyPress -= ApenasNumeros; // remove se trocar de coluna
            }
        }

        private void ApenasNumeros(object sender, KeyPressEventArgs e)
        {
            // Permite: números, vírgula, ponto e backspace
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '.' && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // bloqueia o caractere — não aparece na célula
            }
        }

        private void lblEsportes_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxLigas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxEventos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxEventos.SelectedItem is not EventoItem evento) return;

            AdicionarEventoNoGrid(evento);
        }

        private async void comboBoxLigas_SelectedIndexChanged_1(object sender, EventArgs e)
        {

            if (comboBoxLigas.SelectedItem is not LigaItem liga) return;

            await CarregarEventosAsync(liga.Id);

        }

        private async void comboBoxEsportes_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBoxEsportes.SelectedItem is not EsporteItem esporte) return;

            await CarregarLigasAsync(esporte.Id);// Quando o usuário seleciona um esporte, carregamos as ligas correspondentes a ele
        }

        private void dataGridEspeciais_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica se clicou na coluna Excluir e em uma linha válida
            if (e.ColumnIndex == dataGridEspeciais.Columns["Excluir"].Index && e.RowIndex >= 0)
            {
                var row = dataGridEspeciais.Rows[e.RowIndex];
                bool linhaVazia = string.IsNullOrEmpty(row.Cells["Evento"].Value?.ToString());
                if (linhaVazia) return; // Não faz nada se a linha estiver vazia (sem evento)
                dataGridEspeciais.Rows.RemoveAt(e.RowIndex);
            }
        }

        
        
        private void dataGridEspeciais_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
        }

        private void dataGridEspeciais_CellFormatting_1(object sender, DataGridViewCellFormattingEventArgs e)
        {



            // Formata a coluna ValorAposta com R$
            if (dataGridEspeciais.Columns[e.ColumnIndex].Name == "ValorAposta" && e.RowIndex >= 0)
            {
                if (e.Value != null && decimal.TryParse(e.Value.ToString(), out decimal valor))
                {
                    e.Value = valor.ToString("C2", new System.Globalization.CultureInfo("pt-BR"));
                    e.FormattingApplied = true;
                }
            }

            if (dataGridEspeciais.Columns[e.ColumnIndex].Name == "Excluir" && e.RowIndex >= 0)
            {
                var row = dataGridEspeciais.Rows[e.RowIndex];
                bool linhaVazia = string.IsNullOrEmpty(row.Cells["Evento"].Value?.ToString());

                if (linhaVazia)
                {
                    e.Value = "";
                    e.FormattingApplied = true;


                    var cell = row.Cells[e.ColumnIndex];
                    cell.Style.BackColor = dataGridEspeciais.BackgroundColor;
                    cell.Style.ForeColor = dataGridEspeciais.BackgroundColor;
                }
                else
                {
                    var cell = row.Cells[e.ColumnIndex];
                    cell.Style.BackColor = Color.White;// Mantém o fundo branco para linhas com evento
                    cell.Style.ForeColor = Color.Black;// Mantém o texto visível para linhas com evento
                    cell.Style.SelectionBackColor = Color.White;// Mantém o fundo branco mesmo quando a célula estiver selecionada
                    cell.Style.SelectionForeColor = Color.Black;// Mantém o texto visível mesmo quando a célula estiver selecionada
                }
            }


        } 
    }
}