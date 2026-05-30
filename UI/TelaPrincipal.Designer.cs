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
            Esporte = new DataGridViewTextBoxColumn();
            Liga = new DataGridViewTextBoxColumn();
            DataEvento = new DataGridViewTextBoxColumn();
            Evento = new DataGridViewTextBoxColumn();
            NomeEspecial = new DataGridViewTextBoxColumn();
            Odd = new DataGridViewTextBoxColumn();
            ValorAumento = new DataGridViewTextBoxColumn();
            OddFinal = new DataGridViewTextBoxColumn();
            ValorAposta = new DataGridViewTextBoxColumn();
            comboBoxEsportes = new ComboBox();
            comboBox2 = new ComboBox();
            labelEventos = new Label();
            labelLigas = new Label();
            comboBoxEventos = new ComboBox();
            comboBoxLigas = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dataGridEspeciais).BeginInit();
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
            dataGridEspeciais.BackgroundColor = SystemColors.GradientInactiveCaption;
            dataGridEspeciais.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridEspeciais.Columns.AddRange(new DataGridViewColumn[] { Esporte, Liga, DataEvento, Evento, NomeEspecial, Odd, ValorAumento, OddFinal, ValorAposta });
            dataGridEspeciais.Location = new Point(12, 114);
            dataGridEspeciais.Name = "dataGridEspeciais";
            dataGridEspeciais.ReadOnly = true;
            dataGridEspeciais.Size = new Size(1505, 244);
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
            DataEvento.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
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
            NomeEspecial.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
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
            // TelaPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1861, 890);
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
        private DataGridViewTextBoxColumn Esporte;
        private DataGridViewTextBoxColumn Liga;
        private DataGridViewTextBoxColumn DataEvento;
        private DataGridViewTextBoxColumn Evento;
        private DataGridViewTextBoxColumn NomeEspecial;
        private DataGridViewTextBoxColumn Odd;
        private DataGridViewTextBoxColumn ValorAumento;
        private DataGridViewTextBoxColumn OddFinal;
        private DataGridViewTextBoxColumn ValorAposta;
    }
}