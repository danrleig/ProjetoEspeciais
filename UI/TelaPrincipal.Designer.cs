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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            lblEsportes = new Label();
            dataGridEspeciais = new DataGridView();
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
            ((System.ComponentModel.ISupportInitialize)dataGridEspeciais).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // lblEsportes
            // 
            lblEsportes.AutoSize = true;
            lblEsportes.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEsportes.Location = new Point(12, 30);
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
            dataGridEspeciais.Columns.AddRange(new DataGridViewColumn[] { Esporte, Liga, DataEvento, Evento, NomeEspecial, Odd, ValorAumento, OddFinal, ValorAposta, RiscoEspecial, Tipo });
            dataGridEspeciais.Location = new Point(12, 114);
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
            // comboBoxEsportes
            // 
            comboBoxEsportes.FormattingEnabled = true;
            comboBoxEsportes.Location = new Point(79, 30);
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
            labelEventos.Location = new Point(683, 31);
            labelEventos.Name = "labelEventos";
            labelEventos.Size = new Size(70, 20);
            labelEventos.TabIndex = 9;
            labelEventos.Text = "Eventos: ";
            // 
            // labelLigas
            // 
            labelLigas.AutoSize = true;
            labelLigas.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelLigas.Location = new Point(360, 31);
            labelLigas.Name = "labelLigas";
            labelLigas.Size = new Size(51, 20);
            labelLigas.TabIndex = 10;
            labelLigas.Text = "Ligas: ";
            // 
            // comboBoxEventos
            // 
            comboBoxEventos.FormattingEnabled = true;
            comboBoxEventos.Location = new Point(750, 30);
            comboBoxEventos.Name = "comboBoxEventos";
            comboBoxEventos.Size = new Size(339, 23);
            comboBoxEventos.TabIndex = 12;
            comboBoxEventos.SelectedIndexChanged += comboBoxEventos_SelectedIndexChanged;
            // 
            // comboBoxLigas
            // 
            comboBoxLigas.FormattingEnabled = true;
            comboBoxLigas.Location = new Point(404, 32);
            comboBoxLigas.Name = "comboBoxLigas";
            comboBoxLigas.Size = new Size(262, 23);
            comboBoxLigas.TabIndex = 13;
            comboBoxLigas.SelectedIndexChanged += comboBoxLigas_SelectedIndexChanged_1;
            // 
            // btnPreencherGrid
            // 
            btnPreencherGrid.Location = new Point(1540, 28);
            btnPreencherGrid.Name = "btnPreencherGrid";
            btnPreencherGrid.Size = new Size(75, 29);
            btnPreencherGrid.TabIndex = 14;
            btnPreencherGrid.Text = "OK";
            btnPreencherGrid.UseVisualStyleBackColor = true;
            btnPreencherGrid.Click += btnPreencherGrid_Click;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(1471, 30);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(49, 23);
            numericUpDown1.TabIndex = 15;
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // comboBoxTipoSuperOdds
            // 
            comboBoxTipoSuperOdds.FormattingEnabled = true;
            comboBoxTipoSuperOdds.Location = new Point(1165, 30);
            comboBoxTipoSuperOdds.Name = "comboBoxTipoSuperOdds";
            comboBoxTipoSuperOdds.Size = new Size(300, 23);
            comboBoxTipoSuperOdds.TabIndex = 16;
            comboBoxTipoSuperOdds.SelectedIndexChanged += comboBoxTipoSuperOdds_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(1112, 30);
            label1.Name = "label1";
            label1.Size = new Size(47, 20);
            label1.TabIndex = 17;
            label1.Text = "Tipo: ";
            label1.Click += label1_Click;
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
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle1.NullValue = null;
            Odd.DefaultCellStyle = dataGridViewCellStyle1;
            Odd.HeaderText = "Odd";
            Odd.Name = "Odd";
            Odd.ReadOnly = true;
            // 
            // ValorAumento
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle2.NullValue = null;
            ValorAumento.DefaultCellStyle = dataGridViewCellStyle2;
            ValorAumento.HeaderText = "% de aumento";
            ValorAumento.Name = "ValorAumento";
            ValorAumento.ReadOnly = true;
            // 
            // OddFinal
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopCenter;
            OddFinal.DefaultCellStyle = dataGridViewCellStyle3;
            OddFinal.HeaderText = "Odd Final";
            OddFinal.Name = "OddFinal";
            OddFinal.ReadOnly = true;
            // 
            // ValorAposta
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle4.Format = "C2";
            dataGridViewCellStyle4.NullValue = null;
            ValorAposta.DefaultCellStyle = dataGridViewCellStyle4;
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
            // TelaPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1875, 890);
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
    }
}