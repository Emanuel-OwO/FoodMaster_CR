namespace appFoodMaster_CR.Layer.UI.Reportes
{
    partial class frmReporteClientes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReporteClientes));
            this.btnSalir = new System.Windows.Forms.Button();
            this.btnReporte = new System.Windows.Forms.Button();
            this.txtFiltro = new System.Windows.Forms.TextBox();
            this.lblFiltro = new System.Windows.Forms.Label();
            this.grbOrdenamiento = new System.Windows.Forms.GroupBox();
            this.rdbOrdenadoCedula = new System.Windows.Forms.RadioButton();
            this.rdbOrdenadoNombre = new System.Windows.Forms.RadioButton();
            this.rdbMostrarTodos = new System.Windows.Forms.RadioButton();
            this.grbOrdenamiento.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSalir
            // 
            this.btnSalir.Location = new System.Drawing.Point(621, 78);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(75, 23);
            this.btnSalir.TabIndex = 3;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // btnReporte
            // 
            this.btnReporte.Location = new System.Drawing.Point(540, 78);
            this.btnReporte.Name = "btnReporte";
            this.btnReporte.Size = new System.Drawing.Size(75, 23);
            this.btnReporte.TabIndex = 2;
            this.btnReporte.Text = "Reporte";
            this.btnReporte.UseVisualStyleBackColor = true;
            this.btnReporte.Click += new System.EventHandler(this.btnReporte_Click);
            // 
            // txtFiltro
            // 
            this.txtFiltro.Location = new System.Drawing.Point(178, 40);
            this.txtFiltro.Margin = new System.Windows.Forms.Padding(2);
            this.txtFiltro.Name = "txtFiltro";
            this.txtFiltro.Size = new System.Drawing.Size(362, 20);
            this.txtFiltro.TabIndex = 8;
            // 
            // lblFiltro
            // 
            this.lblFiltro.AutoSize = true;
            this.lblFiltro.Location = new System.Drawing.Point(41, 40);
            this.lblFiltro.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFiltro.Name = "lblFiltro";
            this.lblFiltro.Size = new System.Drawing.Size(82, 13);
            this.lblFiltro.TabIndex = 7;
            this.lblFiltro.Text = "Filtro por Cliente";
            // 
            // grbOrdenamiento
            // 
            this.grbOrdenamiento.Controls.Add(this.rdbMostrarTodos);
            this.grbOrdenamiento.Controls.Add(this.rdbOrdenadoCedula);
            this.grbOrdenamiento.Controls.Add(this.rdbOrdenadoNombre);
            this.grbOrdenamiento.Location = new System.Drawing.Point(44, 78);
            this.grbOrdenamiento.Margin = new System.Windows.Forms.Padding(2);
            this.grbOrdenamiento.Name = "grbOrdenamiento";
            this.grbOrdenamiento.Padding = new System.Windows.Forms.Padding(2);
            this.grbOrdenamiento.Size = new System.Drawing.Size(166, 100);
            this.grbOrdenamiento.TabIndex = 10;
            this.grbOrdenamiento.TabStop = false;
            this.grbOrdenamiento.Text = "Ordenamiento";
            // 
            // rdbOrdenadoCedula
            // 
            this.rdbOrdenadoCedula.AutoSize = true;
            this.rdbOrdenadoCedula.Checked = true;
            this.rdbOrdenadoCedula.Location = new System.Drawing.Point(14, 28);
            this.rdbOrdenadoCedula.Margin = new System.Windows.Forms.Padding(2);
            this.rdbOrdenadoCedula.Name = "rdbOrdenadoCedula";
            this.rdbOrdenadoCedula.Size = new System.Drawing.Size(126, 17);
            this.rdbOrdenadoCedula.TabIndex = 7;
            this.rdbOrdenadoCedula.TabStop = true;
            this.rdbOrdenadoCedula.Text = "Ordenado por Cédula";
            this.rdbOrdenadoCedula.UseVisualStyleBackColor = true;
            this.rdbOrdenadoCedula.CheckedChanged += new System.EventHandler(this.rdbOrdenadoCedula_CheckedChanged);
            // 
            // rdbOrdenadoNombre
            // 
            this.rdbOrdenadoNombre.AutoSize = true;
            this.rdbOrdenadoNombre.Location = new System.Drawing.Point(14, 49);
            this.rdbOrdenadoNombre.Margin = new System.Windows.Forms.Padding(2);
            this.rdbOrdenadoNombre.Name = "rdbOrdenadoNombre";
            this.rdbOrdenadoNombre.Size = new System.Drawing.Size(130, 17);
            this.rdbOrdenadoNombre.TabIndex = 8;
            this.rdbOrdenadoNombre.TabStop = true;
            this.rdbOrdenadoNombre.Text = "Ordenado por Nombre";
            this.rdbOrdenadoNombre.UseVisualStyleBackColor = true;
            this.rdbOrdenadoNombre.CheckedChanged += new System.EventHandler(this.rdbOrdenadoCedula_CheckedChanged);
            // 
            // rdbMostrarTodos
            // 
            this.rdbMostrarTodos.AutoSize = true;
            this.rdbMostrarTodos.Location = new System.Drawing.Point(14, 70);
            this.rdbMostrarTodos.Margin = new System.Windows.Forms.Padding(2);
            this.rdbMostrarTodos.Name = "rdbMostrarTodos";
            this.rdbMostrarTodos.Size = new System.Drawing.Size(149, 17);
            this.rdbMostrarTodos.TabIndex = 9;
            this.rdbMostrarTodos.TabStop = true;
            this.rdbMostrarTodos.Text = "Mostrar Todos los Clientes";
            this.rdbMostrarTodos.UseVisualStyleBackColor = true;
            this.rdbMostrarTodos.CheckedChanged += new System.EventHandler(this.rdbOrdenadoCedula_CheckedChanged);
            // 
            // frmReporteClientes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 244);
            this.Controls.Add(this.grbOrdenamiento);
            this.Controls.Add(this.txtFiltro);
            this.Controls.Add(this.lblFiltro);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnReporte);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmReporteClientes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ReporteClientes";
            this.Load += new System.EventHandler(this.frmReporteClientes_Load);
            this.grbOrdenamiento.ResumeLayout(false);
            this.grbOrdenamiento.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Button btnReporte;
        private System.Windows.Forms.TextBox txtFiltro;
        private System.Windows.Forms.Label lblFiltro;
        private System.Windows.Forms.GroupBox grbOrdenamiento;
        private System.Windows.Forms.RadioButton rdbOrdenadoCedula;
        private System.Windows.Forms.RadioButton rdbOrdenadoNombre;
        private System.Windows.Forms.RadioButton rdbMostrarTodos;
    }
}