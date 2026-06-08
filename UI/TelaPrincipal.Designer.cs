namespace ProjetoEspeciais.UI
{
    partial class TelaPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            lblEsportes = new Label();
            dataGridEspeciais = new DataGridView();
            Esporte = new DataGridViewTextBoxColumn();
            Liga = new DataGridViewTextBoxColumn();
            DataEvento = new DataGridViewTextBoxColumn();
            Evento = new DataGridViewTextBoxColumn();
            NomeEspecial = new DataGridViewTextBoxColumn();
            Odd = new DataGridViewTextBoxColumn();
            ValorAumento = new DataGridViewTextBoxColumn();
            OddFinal = new DataGridViewTextBoxColumn();
            ValorAposta = new DataGridViewTextBoxColumn();
            RiscoEspecial = new DataGridViewTextBoxColumn();
            Tipo = new DataGridViewTextBoxColumn();
            PerfilEspecial = new DataGridViewTextBoxColumn();
            comboBoxEsportes = new ComboBox();
            comboBox2 = new ComboBox();
            labelEventos = new Label();
            labelLigas = new Label();
            comboBoxEventos = new ComboBox();
            comboBoxLigas = new ComboBox();
            btnPreencherGrid = new Button();
            numericUpDown1 = new NumericUpDown();
            comboBoxTipoSuperOdds = new ComboBox();
            label1 = new Label();
            comboBoxrisco = new ComboBox();
            label2 = new Label();
            checkBoxLinkExclusivo = new CheckBox();
            checkBoxNovosUsuarios = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)dataGridEspeciais).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // lblEsportes
            // 
            lblEsportes.AutoSize = true;
            lblEsportes.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEsportes.Location = new Point(12, 35);
            lblEsportes.Name = "lblEsportes";
            lblEsportes.Size = new Size(74, 20);
            lblEsportes.TabIndex = 0;
            lblEsportes.Text = "Esportes: ";
            lblEsportes.Click += lblEsportes_Click;
            // 
            // dataGridEspeciais
            // 
            dataGridEspeciais.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridEspeciais.BackgroundColor = SystemColors.GradientInactiveCaption;
            dataGridEspeciais.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridEspeciais.Columns.AddRange(new DataGridViewColumn[] { Esporte, Liga, DataEvento, Evento, NomeEspecial, Odd, ValorAumento, OddFinal, ValorAposta, RiscoEspecial, Tipo, PerfilEspecial });
            dataGridEspeciais.Location = new Point(12, 169);
            dataGridEspeciais.Name = "dataGridEspeciais";
            dataGridEspeciais.ReadOnly = true;
            dataGridEspeciais.Size = new Size(1851, 418);
            dataGridEspeciais.TabIndex = 2;
            dataGridEspeciais.CellContentClick += dataGridEspeciais_CellContentClick;
            dataGridEspeciais.CellEndEdit += dataGridEspeciais_CellEndEdit;
            dataGridEspeciais.CellFormatting += dataGridEspeciais_CellFormatting_1;
            dataGridEspeciais.CellValidating += dataGridEspeciais_CellValidating;
            dataGridEspeciais.EditingControlShowing += dataGridEspeciais_EditingControlShowing;
            // 
            // Esporte
            // 
            Esporte.HeaderText = "Esporte";
            Esporte.Name = "Esporte";
            Esporte.ReadOnly = true;
            // 
            // Liga
            // 
            Liga.HeaderText = "Liga";
            Liga.Name = "Liga";
            Liga.ReadOnly = true;
            // 
            // DataEvento
            // 
            DataEvento.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataEvento.HeaderText = "Data do evento";
            DataEvento.MaxInputLength = 10;
            DataEvento.MinimumWidth = 115;
            DataEvento.Name = "DataEvento";
            DataEvento.ReadOnly = true;
            DataEvento.Resizable = DataGridViewTriState.False;
            DataEvento.Width = 115;
            // 
            // Evento
            // 
            Evento.HeaderText = "Evento";
            Evento.MinimumWidth = 100;
            Evento.Name = "Evento";
            Evento.ReadOnly = true;
            Evento.Resizable = DataGridViewTriState.False;
            // 
            // NomeEspecial
            // 
            NomeEspecial.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            NomeEspecial.HeaderText = "Nome Especial";
            NomeEspecial.MaxInputLength = 100;
            NomeEspecial.MinimumWidth = 400;
            NomeEspecial.Name = "NomeEspecial";
            NomeEspecial.ReadOnly = true;
            NomeEspecial.Resizable = DataGridViewTriState.False;
            NomeEspecial.Width = 400;
            // 
            // Odd
            // 
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle9.NullValue = null;
            Odd.DefaultCellStyle = dataGridViewCellStyle9;
            Odd.HeaderText = "Odd";
            Odd.Name = "Odd";
            Odd.ReadOnly = true;
            // 
            // ValorAumento
            // 
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle10.NullValue = null;
            ValorAumento.DefaultCellStyle = dataGridViewCellStyle10;
            ValorAumento.HeaderText = "% de aumento";
            ValorAumento.Name = "ValorAumento";
            ValorAumento.ReadOnly = true;
            // 
            // OddFinal
            // 
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.TopCenter;
            OddFinal.DefaultCellStyle = dataGridViewCellStyle11;
            OddFinal.HeaderText = "Odd Final";
            OddFinal.Name = "OddFinal";
            OddFinal.ReadOnly = true;
            // 
            // ValorAposta
            // 
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle12.Format = "C2";
            dataGridViewCellStyle12.NullValue = null;
            ValorAposta.DefaultCellStyle = dataGridViewCellStyle12;
            ValorAposta.HeaderText = "Valor de Aposta";
            ValorAposta.Name = "ValorAposta";
            ValorAposta.ReadOnly = true;
            // 
            // RiscoEspecial
            // 
            RiscoEspecial.HeaderText = "Risco";
            RiscoEspecial.Name = "RiscoEspecial";
            RiscoEspecial.ReadOnly = true;
            // 
            // Tipo
            // 
            Tipo.HeaderText = "Tipo";
            Tipo.Name = "Tipo";
            Tipo.ReadOnly = true;
            // 
            // PerfilEspecial
            // 
            PerfilEspecial.HeaderText = "Perfil";
            PerfilEspecial.Name = "PerfilEspecial";
            PerfilEspecial.ReadOnly = true;
            // 
            // comboBoxEsportes
            // 
            comboBoxEsportes.FormattingEnabled = true;
            comboBoxEsportes.Location = new Point(81, 34);
            comboBoxEsportes.Name = "comboBoxEsportes";
            comboBoxEsportes.Size = new Size(262, 23);
            comboBoxEsportes.TabIndex = 3;
            comboBoxEsportes.SelectedIndexChanged += comboBoxEsportes_SelectedIndexChanged;
            // 
            // comboBox2
            // 
            comboBox2.Location = new Point(0, 0);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(121, 23);
            comboBox2.TabIndex = 0;
            // 
            // labelEventos
            // 
            labelEventos.AutoSize = true;
            labelEventos.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelEventos.Location = new Point(689, 35);
            labelEventos.Name = "labelEventos";
            labelEventos.Size = new Size(70, 20);
            labelEventos.TabIndex = 9;
            labelEventos.Text = "Eventos: ";
            // 
            // labelLigas
            // 
            labelLigas.AutoSize = true;
            labelLigas.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelLigas.Location = new Point(349, 35);
            labelLigas.Name = "labelLigas";
            labelLigas.Size = new Size(51, 20);
            labelLigas.TabIndex = 10;
            labelLigas.Text = "Ligas: ";
            // 
            // comboBoxEventos
            // 
            comboBoxEventos.FormattingEnabled = true;
            comboBoxEventos.Location = new Point(754, 34);
            comboBoxEventos.Name = "comboBoxEventos";
            comboBoxEventos.Size = new Size(339, 23);
            comboBoxEventos.TabIndex = 12;
            comboBoxEventos.SelectedIndexChanged += comboBoxEventos_SelectedIndexChanged;
            // 
            // comboBoxLigas
            // 
            comboBoxLigas.FormattingEnabled = true;
            comboBoxLigas.Location = new Point(397, 34);
            comboBoxLigas.Name = "comboBoxLigas";
            comboBoxLigas.Size = new Size(286, 23);
            comboBoxLigas.TabIndex = 13;
            comboBoxLigas.SelectedIndexChanged += comboBoxLigas_SelectedIndexChanged_1;
            // 
            // btnPreencherGrid
            // 
            btnPreencherGrid.Location = new Point(1822, 49);
            btnPreencherGrid.Name = "btnPreencherGrid";
            btnPreencherGrid.Size = new Size(40, 29);
            btnPreencherGrid.TabIndex = 14;
            btnPreencherGrid.Text = "OK";
            btnPreencherGrid.UseVisualStyleBackColor = true;
            btnPreencherGrid.Click += btnPreencherGrid_Click;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(1662, 37);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(49, 23);
            numericUpDown1.TabIndex = 15;
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // comboBoxTipoSuperOdds
            // 
            comboBoxTipoSuperOdds.FormattingEnabled = true;
            comboBoxTipoSuperOdds.Location = new Point(1152, 36);
            comboBoxTipoSuperOdds.Name = "comboBoxTipoSuperOdds";
            comboBoxTipoSuperOdds.Size = new Size(300, 23);
            comboBoxTipoSuperOdds.TabIndex = 16;
            comboBoxTipoSuperOdds.SelectedIndexChanged += comboBoxTipoSuperOdds_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(1099, 34);
            label1.Name = "label1";
            label1.Size = new Size(47, 20);
            label1.TabIndex = 17;
            label1.Text = "Tipo: ";
            label1.Click += label1_Click;
            // 
            // comboBoxrisco
            // 
            comboBoxrisco.FormattingEnabled = true;
            comboBoxrisco.Location = new Point(1516, 36);
            comboBoxrisco.Name = "comboBoxrisco";
            comboBoxrisco.Size = new Size(140, 23);
            comboBoxrisco.TabIndex = 18;
            comboBoxrisco.SelectedIndexChanged += comboBoxrisco_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(1458, 37);
            label2.Name = "label2";
            label2.Size = new Size(52, 20);
            label2.TabIndex = 19;
            label2.Text = "Risco: ";
            // 
            // checkBoxLinkExclusivo
            // 
            checkBoxLinkExclusivo.AutoSize = true;
            checkBoxLinkExclusivo.Location = new Point(1717, 34);
            checkBoxLinkExclusivo.Name = "checkBoxLinkExclusivo";
            checkBoxLinkExclusivo.Size = new Size(99, 19);
            checkBoxLinkExclusivo.TabIndex = 20;
            checkBoxLinkExclusivo.Text = "Link exclusivo";
            checkBoxLinkExclusivo.UseVisualStyleBackColor = true;
            checkBoxLinkExclusivo.CheckedChanged += checkBoxLinkExclusivo_CheckedChanged;
            // 
            // checkBoxNovosUsuarios
            // 
            checkBoxNovosUsuarios.AutoSize = true;
            checkBoxNovosUsuarios.Location = new Point(1717, 59);
            checkBoxNovosUsuarios.Name = "checkBoxNovosUsuarios";
            checkBoxNovosUsuarios.Size = new Size(90, 19);
            checkBoxNovosUsuarios.TabIndex = 21;
            checkBoxNovosUsuarios.Text = "Novos users";
            checkBoxNovosUsuarios.UseVisualStyleBackColor = true;
            checkBoxNovosUsuarios.CheckedChanged += checkBoxNovosUsuarios_CheckedChanged;
            // 
            // TelaPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1875, 890);
            Controls.Add(checkBoxNovosUsuarios);
            Controls.Add(checkBoxLinkExclusivo);
            Controls.Add(label2);
            Controls.Add(comboBoxrisco);
            Controls.Add(label1);
            Controls.Add(comboBoxTipoSuperOdds);
            Controls.Add(numericUpDown1);
            Controls.Add(btnPreencherGrid);
            Controls.Add(comboBoxLigas);
            Controls.Add(comboBoxEventos);
            Controls.Add(labelLigas);
            Controls.Add(labelEventos);
            Controls.Add(comboBoxEsportes);
            Controls.Add(dataGridEspeciais);
            Controls.Add(lblEsportes);
            Name = "TelaPrincipal";
            Text = "TelaPrincipal";
            Load += TelaPrincipal_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridEspeciais).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblEsportes;
        private DataGridView dataGridEspeciais;
        private ComboBox comboBoxEsportes;
        private Label labelLigas;
        private ComboBox comboBox2;
        private Label labelEventos;
        private ComboBox comboBox1;
        private ComboBox comboBoxEventos;
        private ComboBox comboBoxLigas;
        private Button btnPreencherGrid;
        private NumericUpDown numericUpDown1;
        private ComboBox comboBoxTipoSuperOdds;
        private Label label1;
        private DataGridViewTextBoxColumn Esporte;
        private DataGridViewTextBoxColumn Liga;
        private DataGridViewTextBoxColumn DataEvento;
        private DataGridViewTextBoxColumn Evento;
        private DataGridViewTextBoxColumn NomeEspecial;
        private DataGridViewTextBoxColumn Odd;
        private DataGridViewTextBoxColumn ValorAumento;
        private DataGridViewTextBoxColumn OddFinal;
        private DataGridViewTextBoxColumn ValorAposta;
        private DataGridViewTextBoxColumn RiscoEspecial;
        private DataGridViewTextBoxColumn Tipo;
        private ComboBox comboBoxrisco;
        private Label label2;
        private CheckBox checkBoxLinkExclusivo;
        private CheckBox checkBoxNovosUsuarios;
        private DataGridViewTextBoxColumn PerfilEspecial;
    }
}