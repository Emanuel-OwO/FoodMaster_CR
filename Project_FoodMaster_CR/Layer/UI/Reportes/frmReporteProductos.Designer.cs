namespace appFoodMaster_CR.Layer.UI.Reportes
{
    partial class frmReporteProductos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReporteProductos));
            this.btnSalir = new System.Windows.Forms.Button();
            this.btnReporte = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboTipoProducto = new System.Windows.Forms.ComboBox();
            this.cboProducto = new System.Windows.Forms.ComboBox();
            this.rdbPorProducto = new System.Windows.Forms.RadioButton();
            this.rdbMostrarTodo = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSalir
            // 
            this.btnSalir.Location = new System.Drawing.Point(363, 282);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(75, 23);
            this.btnSalir.TabIndex = 15;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // btnReporte
            // 
            this.btnReporte.Location = new System.Drawing.Point(226, 282);
            this.btnReporte.Name = "btnReporte";
            this.btnReporte.Size = new System.Drawing.Size(75, 23);
            this.btnReporte.TabIndex = 14;
            this.btnReporte.Text = "Reporte";
            this.btnReporte.UseVisualStyleBackColor = true;
            this.btnReporte.Click += new System.EventHandler(this.btnReporte_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(193, 181);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Tipo de Producto";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(179, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Filtrar ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(193, 144);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Producto";
            // 
            // cboTipoProducto
            // 
            this.cboTipoProducto.FormattingEnabled = true;
            this.cboTipoProducto.Location = new System.Drawing.Point(317, 173);
            this.cboTipoProducto.Name = "cboTipoProducto";
            this.cboTipoProducto.Size = new System.Drawing.Size(121, 21);
            this.cboTipoProducto.TabIndex = 10;
            // 
            // cboProducto
            // 
            this.cboProducto.FormattingEnabled = true;
            this.cboProducto.Location = new System.Drawing.Point(317, 136);
            this.cboProducto.Name = "cboProducto";
            this.cboProducto.Size = new System.Drawing.Size(121, 21);
            this.cboProducto.TabIndex = 8;
            // 
            // rdbPorProducto
            // 
            this.rdbPorProducto.AutoSize = true;
            this.rdbPorProducto.Location = new System.Drawing.Point(276, 91);
            this.rdbPorProducto.Name = "rdbPorProducto";
            this.rdbPorProducto.Size = new System.Drawing.Size(87, 17);
            this.rdbPorProducto.TabIndex = 16;
            this.rdbPorProducto.TabStop = true;
            this.rdbPorProducto.Text = "Por Producto";
            this.rdbPorProducto.UseVisualStyleBackColor = true;
            this.rdbPorProducto.CheckedChanged += new System.EventHandler(this.rdbPorProducto_CheckedChanged);
            // 
            // rdbMostrarTodo
            // 
            this.rdbMostrarTodo.AutoSize = true;
            this.rdbMostrarTodo.Location = new System.Drawing.Point(399, 91);
            this.rdbMostrarTodo.Name = "rdbMostrarTodo";
            this.rdbMostrarTodo.Size = new System.Drawing.Size(151, 17);
            this.rdbMostrarTodo.TabIndex = 17;
            this.rdbMostrarTodo.TabStop = true;
            this.rdbMostrarTodo.Text = "Mostrar todo los Productos";
            this.rdbMostrarTodo.UseVisualStyleBackColor = true;
            this.rdbMostrarTodo.CheckedChanged += new System.EventHandler(this.rdbPorProducto_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(193, 218);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Descripcion";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(317, 215);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(121, 20);
            this.txtDescripcion.TabIndex = 19;
            // 
            // frmReporteProductos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.rdbMostrarTodo);
            this.Controls.Add(this.rdbPorProducto);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnReporte);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboTipoProducto);
            this.Controls.Add(this.cboProducto);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmReporteProductos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ReporteProductos";
            this.Load += new System.EventHandler(this.frmReporteProductos_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Button btnReporte;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboTipoProducto;
        private System.Windows.Forms.ComboBox cboProducto;
        private System.Windows.Forms.RadioButton rdbPorProducto;
        private System.Windows.Forms.RadioButton rdbMostrarTodo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDescripcion;
    }
}