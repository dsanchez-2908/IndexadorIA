using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace IndexadorIA.Pantallas.FinalizacionPlanos
{
    /// <summary>Visor de imagen con zoom (rueda / botones) y desplazamiento arrastrando con el mouse.</summary>
    internal class VisorImagenZoom : Control
    {
        private const float ZoomMin = 0.02f;
        private const float ZoomMax = 16f;
        private const float FactorZoom = 1.25f;

        private Image? _imagen;
        private float _zoom = 1f;
        private PointF _origen = PointF.Empty;
        private bool _ajustar = true;
        private bool _arrastrando;
        private Point _inicioArrastre;
        private PointF _origenInicio;

        public event EventHandler? ZoomCambiado;

        public VisorImagenZoom()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            Cursor = Cursors.Hand;
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image? Imagen
        {
            get => _imagen;
            set
            {
                _imagen = value;
                AjustarAVentana();
            }
        }

        public int ZoomPorcentaje => (int)Math.Round(_zoom * 100);

        public void AjustarAVentana()
        {
            _ajustar = true;
            if (_imagen != null && Width > 0 && Height > 0)
            {
                _zoom = Math.Clamp(Math.Min((float)Width / _imagen.Width, (float)Height / _imagen.Height), ZoomMin, ZoomMax);
                _origen = new PointF((Width - _imagen.Width * _zoom) / 2f, (Height - _imagen.Height * _zoom) / 2f);
            }
            Invalidate();
            ZoomCambiado?.Invoke(this, EventArgs.Empty);
        }

        public void Zoom100() => AplicarZoom(1f, Centro());

        public void Acercar() => AplicarZoom(_zoom * FactorZoom, Centro());

        public void Alejar() => AplicarZoom(_zoom / FactorZoom, Centro());

        private Point Centro() => new(Width / 2, Height / 2);

        private void AplicarZoom(float nuevoZoom, Point ancla)
        {
            if (_imagen == null) return;
            nuevoZoom = Math.Clamp(nuevoZoom, ZoomMin, ZoomMax);
            float ix = (ancla.X - _origen.X) / _zoom;
            float iy = (ancla.Y - _origen.Y) / _zoom;
            _zoom = nuevoZoom;
            _origen = new PointF(ancla.X - ix * _zoom, ancla.Y - iy * _zoom);
            _ajustar = false;
            Invalidate();
            ZoomCambiado?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            Focus();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            AplicarZoom(e.Delta > 0 ? _zoom * FactorZoom : _zoom / FactorZoom, e.Location);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left || _imagen == null) return;
            _arrastrando = true;
            _inicioArrastre = e.Location;
            _origenInicio = _origen;
            Cursor = Cursors.SizeAll;
            Capture = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!_arrastrando) return;
            _origen = new PointF(_origenInicio.X + e.X - _inicioArrastre.X, _origenInicio.Y + e.Y - _inicioArrastre.Y);
            _ajustar = false;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (!_arrastrando) return;
            _arrastrando = false;
            Capture = false;
            Cursor = Cursors.Hand;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_ajustar) AjustarAVentana();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_imagen == null) return;
            var g = e.Graphics;
            g.InterpolationMode = _zoom < 1f ? InterpolationMode.HighQualityBicubic : InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            g.DrawImage(_imagen, new RectangleF(_origen.X, _origen.Y, _imagen.Width * _zoom, _imagen.Height * _zoom));
        }
    }
}
