using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appFoodMaster_CR.Layer.UI.Procesos
{
    public partial class frmFirma : Form
    {
        private Bitmap _lienzo;
        private bool _pintando = false;
        private Point _prev;
        private bool _hayTrazo = false;
        public byte[] FirmaBytes { get; private set; }

        public frmFirma()
        {
            InitializeComponent();
           
        }

        private void frmFirma_Load(object sender, EventArgs e)
        {
            _lienzo = new Bitmap(pnlFirma.Width, pnlFirma.Height);
            using (var g = Graphics.FromImage(_lienzo))
                g.Clear(Color.White);   // fondo blanco

            pnlFirma.BackgroundImage = _lienzo;
            pnlFirma.BackgroundImageLayout = ImageLayout.None;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            using (var ms = new MemoryStream())
            {
                _lienzo.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                FirmaBytes = ms.ToArray();
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnLimpieza_Click(object sender, EventArgs e)
        {
            using (var g = Graphics.FromImage(_lienzo))
            {
                g.Clear(Color.White);
            }
            pnlFirma.Invalidate();
            FirmaBytes = null;
        }

        private void pnlFirma_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            _pintando = true;
            _prev = e.Location;
        }

        private void pnlFirma_MouseUp(object sender, MouseEventArgs e)
        {
            _pintando = false;
        }

        private void pnlFirma_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_pintando) return;
            using (var g = Graphics.FromImage(_lienzo))
            using (var pen = new Pen(Color.Black, 2f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawLine(pen, _prev, e.Location);
            }
            _prev = e.Location;
            pnlFirma.Invalidate();
        }
    }
}
