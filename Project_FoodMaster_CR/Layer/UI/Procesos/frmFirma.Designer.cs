namespace appFoodMaster_CR.Layer.UI.Procesos
{
    partial class frmFirma
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFirma));
            this.btnLimpieza = new System.Windows.Forms.Button();
            this.pnlFirma = new System.Windows.Forms.PictureBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnAceptar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pnlFirma)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLimpieza
            // 
            this.btnLimpieza.Location = new System.Drawing.Point(324, 37);
            this.btnLimpieza.Name = "btnLimpieza";
            this.btnLimpieza.Size = new System.Drawing.Size(75, 23);
            this.btnLimpieza.TabIndex = 9;
            this.btnLimpieza.Text = "Limpieza";
            this.btnLimpieza.UseVisualStyleBackColor = true;
            this.btnLimpieza.Click += new System.EventHandler(this.btnLimpieza_Click);
            // 
            // pnlFirma
            // 
            this.pnlFirma.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFirma.Location = new System.Drawing.Point(51, 87);
            this.pnlFirma.Margin = new System.Windows.Forms.Padding(2);
            this.pnlFirma.Name = "pnlFirma";
            this.pnlFirma.Size = new System.Drawing.Size(414, 273);
            this.pnlFirma.TabIndex = 8;
            this.pnlFirma.TabStop = false;
            this.pnlFirma.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlFirma_MouseDown);
            this.pnlFirma.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlFirma_MouseMove);
            this.pnlFirma.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlFirma_MouseUp);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(176, 37);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 7;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(51, 37);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 6;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // frmFirma
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 409);
            this.Controls.Add(this.btnLimpieza);
            this.Controls.Add(this.pnlFirma);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAceptar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmFirma";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Firma";
            this.Load += new System.EventHandler(this.frmFirma_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pnlFirma)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLimpieza;
        private System.Windows.Forms.PictureBox pnlFirma;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnAceptar;
    }
}