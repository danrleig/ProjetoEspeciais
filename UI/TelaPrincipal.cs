using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ProjetoEspeciais.Service;
using ProjetoEspeciais.Data;
using System.Web;

namespace ProjetoEspeciais.UI
{
    using System.Linq;

    public partial class TelaPrincipal : Form
    {

        // Guarda o serviço de autenticação recebido — tem o token válido
        private readonly AtenaAuthService _authService;
        private List<EventoItem> _eventosOriginais = new();
        // Construtor agora RECEBE o authService como parâmetro
        public TelaPrincipal(AtenaAuthService authService)
        {
            InitializeComponent();
            _authService = authService;
        }

        private async void TelaPrincipal_Load(object sender, EventArgs e)
        {
            FormatarGrid();
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
        private void btnPreencherGrid_Click(object sender, EventArgs e)// Quando o usuário clicar no botão para preencher o grid, verifica se um evento, tipo de Super Odd e risco foram selecionados, e se sim, adiciona o evento ao grid a quantidade de vezes definida no numericUpDown
        {
            if (comboBoxEventos.SelectedItem is not EventoItem evento)
            {
                MessageBox.Show("Selecione um evento!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxTipoSuperOdds.SelectedItem == null)
            {
                MessageBox.Show("Selecione o tipo da Super Odd!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxrisco.SelectedItem == null)
            {
                MessageBox.Show("Selecione o risco da Super Odd!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int quantidade = (int)numericUpDown1.Value; // troque "numericUpDown1" pelo nome que você deu

            for (int i = 0; i < quantidade; i++)
            {
                AdicionarEventoNoGrid(evento);
            }
        }

        private async void btnCadastrarSuperOdds_Click(object sender, EventArgs e)
        {
            // Valida se há linhas no grid para cadastrar
            if (dataGridEspeciais.Rows.Count == 0)
            {
                MessageBox.Show("Não há super odds para cadastrar!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Valida se um tipo foi selecionado
            if (comboBoxTipoSuperOdds.SelectedItem == null)
            {
                MessageBox.Show("Selecione o tipo da Super Odd!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Valida se um risco foi selecionado
            if (comboBoxrisco.SelectedItem == null)
            {
                MessageBox.Show("Selecione o risco!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Converte o tipo selecionado para o id do grupo evento
            int idGrupoEvento = comboBoxTipoSuperOdds.SelectedItem.ToString().Contains("Múltiplas Escolhas")
                ? 1273
                : 5891;

            // Converte o risco selecionado para o id do perfil
            int idPerfilRisco = comboBoxrisco.SelectedItem.ToString() switch
            {
                "R$2.000,00" => 22,
                "R$5.000,00" => 18,
                "R$10.000,00" => 20,
                "R$15.000,00" => 21,
                "R$20.000,00" => 23,
                "R$40.000,00" => 25,
                _ => 22 // padrão 2k se não reconhecer
            };

            // Lê os checkboxes para definir o perfil de acesso
            bool apostaExclusivaLink = checkBoxLinkExclusivo.Checked || checkBoxNovosUsuarios.Checked;
            bool apostasExclusivasNovosUsuarios = checkBoxNovosUsuarios.Checked;

            // Desabilita o botão para evitar cliques duplos durante o cadastro
            btnCadastrarSuperOdds.Enabled = false;
            btnCadastrarSuperOdds.Text = "Cadastrando...";

            var service = new AtenaCadastroSuperOddService(_authService);
            int sucesso = 0;
            int falha = 0;
            var links = new System.Text.StringBuilder();
            // Percorre cada linha do grid
            foreach (DataGridViewRow row in dataGridEspeciais.Rows)
            {
                // Ignora a linha vazia no final do grid (linha de nova entrada)
                if (row.IsNewRow) continue;

                // Ignora linhas sem evento preenchido
                string nomeEvento = row.Cells["Evento"].Value?.ToString();
                if (string.IsNullOrEmpty(nomeEvento)) continue;

                try
                {
                    // Lê os valores da linha
                    string nomeEspecial = row.Cells["NomeEspecial"].Value?.ToString();
                    string dataEventoStr = row.Cells["DataEvento"].Value?.ToString();
                    string oddStr = row.Cells["Odd"].Value?.ToString();
                    string oddFinalStr = row.Cells["OddFinal"].Value?.ToString();
                    string valorApostaStr = row.Cells["ValorAposta"].Value?.ToString();

                    // Valida campos obrigatórios da linha
                    if (string.IsNullOrEmpty(nomeEspecial))
                    {
                        MessageBox.Show($"Linha {row.Index + 1}: Nome Especial não preenchido!",
                            "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        falha++;
                        continue;
                    }

                    if (string.IsNullOrEmpty(valorApostaStr) || valorApostaStr == "R$ 0,00")
                    {
                        MessageBox.Show($"Linha {row.Index + 1}: Valor de Aposta não preenchido!",
                            "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        falha++;
                        continue;
                    }

                    // Converte os valores
                    DateTime momentoRealizacao = DateTime.Parse(dataEventoStr);

                    // Remove "R$ " e converte para decimal
                    decimal valorAposta = decimal.Parse(
                        valorApostaStr.Replace("R$", "").Replace(".", "").Trim(),
                        System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));

                    decimal oddOriginal = decimal.Parse(oddStr,
                        System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));

                    decimal oddFinal = decimal.Parse(oddFinalStr,
                        System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));

                    // Pega o EventoItem da linha para obter o id do evento relacionado
                    // O evento foi selecionado no comboBoxEventos e adicionado ao grid
                    // Precisamos do id do evento original (de futebol) que está no EventoItem
                    // Como o grid só armazena texto, precisamos buscá-lo pelo nome no comboBoxEventos
                    int idEventoRelacionado = BuscarIdEventoPorNome(nomeEvento);

                    if (idEventoRelacionado == 0)
                    {
                        MessageBox.Show($"Linha {row.Index + 1}: Evento '{nomeEvento}' não encontrado!",
                            "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        falha++;
                        continue;
                    }

                    // Monta o nome completo da super odd: "Evento - Nome Especial"
                    string nomeCasa = $"{nomeEvento.Replace(" x ", " vs ")} - {nomeEspecial}";
                    MessageBox.Show($"apostaExclusivaLink: {apostaExclusivaLink}\napostasExclusivasNovosUsuarios: {apostasExclusivasNovosUsuarios}");

                    // Chama o serviço que faz os 4 passos do cadastro
                    string link = await service.CadastrarSuperOddAsync(
                         idGrupoEvento,
                         nomeCasa,
                         idEventoRelacionado,
                         momentoRealizacao,
                         oddFinal,
                         oddOriginal,
                         valorAposta,
                         idPerfilRisco,
                         apostaExclusivaLink,
                         apostasExclusivasNovosUsuarios);
                    if (!string.IsNullOrEmpty(link))
                    {
                        string dataHora = momentoRealizacao.ToString("dd/MM/yyyy HH:mm");
                        links.AppendLine($"{nomeEvento} | {dataHora} | {nomeCasa} |{link}");
                    }

                    sucesso++;
                }
                catch (Exception ex)
                {
                    falha++;
                    MessageBox.Show($"Erro na linha {row.Index + 1}: {ex.Message}", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (links.Length > 0)
            {
                textboxLinkEspecial.Visible = true;
                label4.Visible = true;
                textboxLinkEspecial.Text = links.ToString();
            }
            else
                textboxLinkEspecial.Clear();

            // Reabilita o botão
            btnCadastrarSuperOdds.Enabled = true;
            btnCadastrarSuperOdds.Text = "Cadastrar Super Odds";

            MessageBox.Show($"Cadastro concluído!\n✅ Sucesso: {sucesso}\n❌ Falha: {falha}",
                "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);

            dataGridEspeciais.Rows.Clear(); // Limpa o grid após o cadastro
        }

        // Busca o id do evento pelo nome no comboBoxEventos
        // Precisamos disso porque o grid armazena o nome, não o objeto EventoItem
        private int BuscarIdEventoPorNome(string nomeEvento)
        {
            foreach (var item in comboBoxEventos.Items)
            {
                if (item is EventoItem evento && evento.Nome == nomeEvento)
                    return evento.Id;
            }
            return 0;
        }


        public async void FormatarGrid()
        {

            this.WindowState = FormWindowState.Maximized;

            textboxLinkEspecial.Visible = false;
            textboxLinkEspecial.TextAlign = HorizontalAlignment.Left;
            textboxLinkEspecial.ReadOnly = true;
            label4.Visible = false;
            dataGridEspeciais.AllowUserToAddRows = false;
            dataGridEspeciais.ReadOnly = false;// Permite edição, mas vamos controlar quais colunas são editáveis
            dataGridEspeciais.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridEspeciais.ScrollBars = ScrollBars.Vertical; // só scroll vertical


            dataGridEspeciais.Columns["Esporte"].ReadOnly = true;// Esporte não pode ser editado diretamente na grid, só pela seleção
            dataGridEspeciais.Columns["Esporte"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridEspeciais.Columns["Esporte"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridEspeciais.Columns["Esporte"].Width = 100;

            dataGridEspeciais.Columns["Liga"].ReadOnly = true;// Liga não pode ser editada diretamente na grid, só pela seleção
            dataGridEspeciais.Columns["Liga"].Width = 250;
            dataGridEspeciais.Columns["Liga"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;// Centraliza o header da coluna Liga
            dataGridEspeciais.Columns["Liga"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;// Centraliza o conteúdo da coluna Liga


            dataGridEspeciais.Columns["DataEvento"].ReadOnly = true;// Data do evento não pode ser editada diretamente na grid
            dataGridEspeciais.Columns["DataEvento"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;// Centraliza a data do evento
            dataGridEspeciais.Columns["DataEvento"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridEspeciais.Columns["DataEvento"].Width = 100;

            dataGridEspeciais.Columns["Evento"].ReadOnly = true;// Evento não pode ser editado diretamente na grid, só pela seleção
            dataGridEspeciais.Columns["Evento"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;// Centraliza o nome do especial
            dataGridEspeciais.Columns["Evento"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridEspeciais.Columns["Evento"].Width = 230;

            dataGridEspeciais.Columns["NomeEspecial"].ReadOnly = false;// Nome do especial pode ser editado
            dataGridEspeciais.Columns["NomeEspecial"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridEspeciais.Columns["NomeEspecial"].Width = 400;

            dataGridEspeciais.Columns["Odd"].ReadOnly = false;// Odd pode ser editada
            dataGridEspeciais.Columns["Odd"].Width = 60;
            dataGridEspeciais.Columns["Odd"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridEspeciais.Columns["Odd"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            dataGridEspeciais.Columns["ValorAumento"].ReadOnly = false;// Valor de aumento pode ser editado
            dataGridEspeciais.Columns["ValorAumento"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridEspeciais.Columns["ValorAumento"].Width = 95;
            dataGridEspeciais.Columns["ValorAumento"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridEspeciais.Columns["OddFinal"].ReadOnly = false;// Odd final é calculada, não pode ser editada
            dataGridEspeciais.Columns["OddFinal"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridEspeciais.Columns["OddFinal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridEspeciais.Columns["OddFinal"].Width = 85;

            dataGridEspeciais.Columns["ValorAposta"].ReadOnly = false;// Valor da aposta pode ser editado
            dataGridEspeciais.Columns["ValorAposta"].Width = 114;
            dataGridEspeciais.Columns["ValorAposta"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridEspeciais.Columns["ValorAposta"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridEspeciais.Columns["RiscoEspecial"].ReadOnly = true;// Risco não pode ser editado
            dataGridEspeciais.Columns["RiscoEspecial"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridEspeciais.Columns["RiscoEspecial"].Width = 74;
            dataGridEspeciais.Columns["RiscoEspecial"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridEspeciais.Columns["Tipo"].ReadOnly = true;// Tipo não pode ser editado diretamente na grid
            dataGridEspeciais.Columns["Tipo"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridEspeciais.Columns["Tipo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridEspeciais.Columns["Tipo"].Width = 200;

            dataGridEspeciais.Columns["PerfilEspecial"].Width = 93;
            dataGridEspeciais.Columns["PerfilEspecial"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridEspeciais.Columns["PerfilEspecial"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            dataGridEspeciais.Columns["PerfilEspecial"].ReadOnly = true;

            numericUpDown1.Minimum = 1;
            numericUpDown1.Value = 1;

            comboBoxTipoSuperOdds.Items.Clear();
            comboBoxTipoSuperOdds.Width = 300;
            comboBoxTipoSuperOdds.Items.Add("Super Odds - Múltiplas Escolhas");
            comboBoxTipoSuperOdds.Items.Add("Super Odds - Novos Usuários");
            comboBoxTipoSuperOdds.DropDownStyle = ComboBoxStyle.DropDownList;// Impede que o usuário digite um valor, só pode escolher entre as opções


            comboBoxrisco.Items.Clear();
            comboBoxrisco.Items.Add("R$2.000,00");
            comboBoxrisco.Items.Add("R$5.000,00");
            comboBoxrisco.Items.Add("R$10.000,00");
            comboBoxrisco.Items.Add("R$15.000,00");
            comboBoxrisco.Items.Add("R$20.000,00");
            comboBoxrisco.Items.Add("R$40.000,00");
            comboBoxrisco.DropDownStyle = ComboBoxStyle.DropDownList;// Impede que o usuário digite um valor, só pode escolher entre as opções


            comboBoxEventos.DropDownStyle = ComboBoxStyle.DropDown;
            comboBoxEventos.TextUpdate += comboBoxEventos_TextUpdate;

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
            colunaExcluir.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colunaExcluir.Resizable = DataGridViewTriState.False;
            await CarregarEsportesAsync();
        }
        private void comboBoxEventos_TextUpdate(object sender, EventArgs e)
        {
            string texto = comboBoxEventos.Text;

            var filtrados = _eventosOriginais
                .Where(x => x.Nome.Contains(
                    texto,
                    StringComparison.OrdinalIgnoreCase))
                .ToList();

            comboBoxEventos.BeginUpdate();

            comboBoxEventos.Items.Clear();

            foreach (var evento in filtrados)
            {
                comboBoxEventos.Items.Add(evento);
            }

            comboBoxEventos.EndUpdate();

            comboBoxEventos.DroppedDown = true;
            comboBoxEventos.SelectionStart = texto.Length;
            comboBoxEventos.SelectionLength = 0;
        }


        private async Task CarregarEsportesAsync()
        {
            try
            {
                comboBoxEsportes.SelectedIndexChanged -= comboBoxEsportes_SelectedIndexChanged;
                comboBoxEsportes.Enabled = false;
                comboBoxEsportes.Items.Clear();
                comboBoxEsportes.Items.Add("Escolha o Esporte");
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

                _eventosOriginais = eventos;

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

        private void AdicionarEventoNoGrid(EventoItem evento)// Adiciona um evento selecionado no comboBox ao grid, preenchendo as colunas com as informações do evento e os valores padrão
        {
            int index = dataGridEspeciais.Rows.Add();
            var row = dataGridEspeciais.Rows[index];

            // Pega o esporte e liga selecionados nos comboBoxes
            string esporte = comboBoxEsportes.SelectedItem?.ToString() ?? "";
            string liga = comboBoxLigas.SelectedItem?.ToString() ?? "";
            string tipoEspecial = comboBoxTipoSuperOdds.SelectedItem?.ToString() ?? "";
            string risco = comboBoxrisco.SelectedItem?.ToString() ?? "";
            string perfil = "";

            if (checkBoxLinkExclusivo.Checked)
            {
                perfil += "🔗 LE";
            }

            if (checkBoxNovosUsuarios.Checked)
            {
                if (perfil != "")
                    perfil += " ";

                perfil += "🆕👤 NU";
            }

            // Se nenhum checkbox foi marcado
            if (perfil == "")
            {
                perfil = "Geral";
            }

            row.Cells["Esporte"].Value = esporte;
            row.Cells["Liga"].Value = liga;
            row.Cells["Tipo"].Value = tipoEspecial;
            row.Cells["RiscoEspecial"].Value = risco;
            row.Cells["PerfilEspecial"].Value = perfil;

            // Formata a data do evento para exibição
            if (DateTime.TryParse(evento.MomentoRealizacao, out DateTime data))
                row.Cells["DataEvento"].Value = data.ToString("dd/MM/yyyy HH:mm");



            row.Cells["Evento"].Value = evento.Nome;
            row.Cells["Odd"].Value = 0.00m;
            row.Cells["ValorAumento"].Value = 0;
            row.Cells["OddFinal"].Value = 0.00m;
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

        }

        private async void comboBoxLigas_SelectedIndexChanged_1(object sender, EventArgs e)
        {

            if (comboBoxLigas.SelectedItem is not LigaItem liga) return;// Quando o usuário seleciona uma liga, carregamos os eventos correspondentes a ela

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



            // Formata a coluna ValorAposta e RiscoEspecial com R$
            if (dataGridEspeciais.Columns[e.ColumnIndex].Name == "ValorAposta" || dataGridEspeciais.Columns[e.ColumnIndex].Name == "RiscoEspecial" && e.RowIndex >= 0)
            {
                if (e.Value != null && decimal.TryParse(e.Value.ToString(), out decimal valor))
                {
                    e.Value = valor.ToString("C2", new System.Globalization.CultureInfo("pt-BR"));
                    e.FormattingApplied = true;
                }
            }

            if (dataGridEspeciais.Columns[e.ColumnIndex].Name == "Excluir" && e.RowIndex >= 0)// Formata a coluna de exclusão para mostrar o ícone de lixeira e esconde o texto do botão, além de ajustar as cores para manter o ícone visível
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



        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxTipoSuperOdds_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxrisco_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxLinkExclusivo_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxNovosUsuarios_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textboxLinkEspecial_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnLimparLista_Click(object sender, EventArgs e)
        {
            textboxLinkEspecial.Clear();
            textboxLinkEspecial.Visible = false;
            label4.Visible = false;
            btnLimparLista.Visible = false;
        }
    }
}