namespace SIMBA
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tbActions = new System.Windows.Forms.ToolBar();
            this.btnNew = new System.Windows.Forms.ToolBarButton();
            this.btnOpen = new System.Windows.Forms.ToolBarButton();
            this.btnRun = new System.Windows.Forms.ToolBarButton();
            this.btnSave = new System.Windows.Forms.ToolBarButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // tbActions
            // 
            this.tbActions.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.btnNew,
            this.btnOpen,
            this.btnRun,
            this.btnSave});
            this.tbActions.DropDownArrows = true;
            this.tbActions.ImageList = this.imageList1;
            this.tbActions.Location = new System.Drawing.Point(0, 0);
            this.tbActions.Name = "tbActions";
            this.tbActions.ShowToolTips = true;
            this.tbActions.Size = new System.Drawing.Size(800, 42);
            this.tbActions.TabIndex = 0;
            this.tbActions.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbActions_ButtonClick);
            // 
            // btnNew
            // 
            this.btnNew.ImageIndex = 0;
            this.btnNew.Name = "btnNew";
            this.btnNew.Text = "New Alignment";
            this.btnNew.ToolTipText = "Clear the fields to input a new alignment";
            // 
            // btnOpen
            // 
            this.btnOpen.ImageIndex = 1;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Text = "Open File";
            this.btnOpen.ToolTipText = "Open a fasta file";
            // 
            // btnRun
            // 
            this.btnRun.ImageIndex = 2;
            this.btnRun.Name = "btnRun";
            this.btnRun.Text = "Run Alignment";
            this.btnRun.ToolTipText = "Execute the alignment";
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.ImageIndex = 3;
            this.btnSave.Name = "btnSave";
            this.btnSave.Text = "Save Alignment";
            this.btnSave.ToolTipText = "Save the alignment";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "documento.png");
            this.imageList1.Images.SetKeyName(1, "arquivo.png");
            this.imageList1.Images.SetKeyName(2, "botao-play (1).png");
            this.imageList1.Images.SetKeyName(3, "salve-.png");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Input Sequences";
            // 
            // txtInput
            // 
            this.txtInput.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInput.Location = new System.Drawing.Point(15, 61);
            this.txtInput.Multiline = true;
            this.txtInput.Name = "txtInput";
            this.txtInput.ReadOnly = true;
            this.txtInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInput.Size = new System.Drawing.Size(773, 187);
            this.txtInput.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 251);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Output Sequences";
            // 
            // txtOutput
            // 
            this.txtOutput.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutput.Location = new System.Drawing.Point(12, 267);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutput.Size = new System.Drawing.Size(773, 187);
            this.txtOutput.TabIndex = 4;
            this.txtOutput.WordWrap = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 464);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbActions);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ToolBarButton btnOpen;
        public System.Windows.Forms.ToolBarButton btnRun;
        public System.Windows.Forms.ToolBarButton btnSave;
        public System.Windows.Forms.ToolBar tbActions;
        public System.Windows.Forms.ToolBarButton btnNew;
        public System.Windows.Forms.TextBox txtInput;
        public System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

