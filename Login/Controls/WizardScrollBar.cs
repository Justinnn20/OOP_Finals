using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Login
{
    /// <summary>
    /// A wizardly-styled antique scroll bar with transparent background,
    /// a slender antique channel track, and a polished antique gold pill thumb.
    /// Replaces Windows native scrollbars with zero white background artifacts.
    /// </summary>
    public class WizardScrollBar : Control
    {
        private int _minimum = 0;
        private int _maximum = 100;
        private int _value = 0;
        private int _visibleAmount = 20;
        private int _smallChange = 28;
        private int _largeChange = 60;

        private bool _isHovered = false;
        private bool _isDragging = false;
        private int _dragStartY = 0;
        private int _dragStartValue = 0;

        // Antique Wizardly Color Palette
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color TrackColor { get; set; } = Color.FromArgb(228, 210, 170);
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color TrackBorderColor { get; set; } = Color.FromArgb(204, 182, 138);
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color ThumbColorTop { get; set; } = Color.FromArgb(218, 174, 68);
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color ThumbColorBottom { get; set; } = Color.FromArgb(178, 132, 38);
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color ThumbBorderColor { get; set; } = Color.FromArgb(136, 96, 22);
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color ThumbHoverTop { get; set; } = Color.FromArgb(244, 196, 84);
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color ThumbHoverBottom { get; set; } = Color.FromArgb(204, 154, 46);
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color ThumbDragTop { get; set; } = Color.FromArgb(180, 134, 40);
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color ThumbDragBottom { get; set; } = Color.FromArgb(144, 98, 24);

        public event EventHandler? ValueChanged;
        public event ScrollEventHandler? Scroll;

        public WizardScrollBar()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw, true);

            this.BackColor = Color.Transparent;
            this.Width = 10;
            this.Cursor = Cursors.Default;
        }

        [Category("Behavior")]
        [DefaultValue(0)]
        public int Minimum
        {
            get => _minimum;
            set
            {
                _minimum = value;
                if (_maximum < _minimum) _maximum = _minimum;
                SetValueInternal(_value);
                Invalidate();
            }
        }

        [Category("Behavior")]
        [DefaultValue(100)]
        public int Maximum
        {
            get => _maximum;
            set
            {
                _maximum = value;
                if (_minimum > _maximum) _minimum = _maximum;
                SetValueInternal(_value);
                Invalidate();
            }
        }

        [Category("Behavior")]
        [DefaultValue(0)]
        public int Value
        {
            get => _value;
            set => SetValueInternal(value);
        }

        [Category("Behavior")]
        [DefaultValue(20)]
        public int VisibleAmount
        {
            get => _visibleAmount;
            set
            {
                _visibleAmount = Math.Max(1, value);
                Invalidate();
            }
        }

        [Category("Behavior")]
        [DefaultValue(28)]
        public int SmallChange
        {
            get => _smallChange;
            set => _smallChange = Math.Max(1, value);
        }

        [Category("Behavior")]
        [DefaultValue(60)]
        public int LargeChange
        {
            get => _largeChange;
            set => _largeChange = Math.Max(1, value);
        }

        public void SetTheme(bool isDark)
        {
            if (isDark)
            {
                TrackColor = Color.FromArgb(50, 18, 25);
                TrackBorderColor = Color.FromArgb(80, 30, 40);
                ThumbColorTop = Color.FromArgb(212, 160, 23);
                ThumbColorBottom = Color.FromArgb(160, 115, 15);
                ThumbBorderColor = Color.FromArgb(120, 80, 10);
                ThumbHoverTop = Color.FromArgb(240, 190, 45);
                ThumbHoverBottom = Color.FromArgb(185, 135, 20);
                ThumbDragTop = Color.FromArgb(170, 120, 15);
                ThumbDragBottom = Color.FromArgb(130, 90, 8);
            }
            else
            {
                TrackColor = Color.FromArgb(228, 210, 170);
                TrackBorderColor = Color.FromArgb(204, 182, 138);
                ThumbColorTop = Color.FromArgb(218, 174, 68);
                ThumbColorBottom = Color.FromArgb(178, 132, 38);
                ThumbBorderColor = Color.FromArgb(136, 96, 22);
                ThumbHoverTop = Color.FromArgb(244, 196, 84);
                ThumbHoverBottom = Color.FromArgb(204, 154, 46);
                ThumbDragTop = Color.FromArgb(180, 134, 40);
                ThumbDragBottom = Color.FromArgb(144, 98, 24);
            }
            Invalidate();
        }

        private void SetValueInternal(int newVal)
        {
            int maxVal = Math.Max(_minimum, _maximum);
            int clamped = Math.Max(_minimum, Math.Min(maxVal, newVal));
            if (_value != clamped)
            {
                _value = clamped;
                Invalidate();
                ValueChanged?.Invoke(this, EventArgs.Empty);
                Scroll?.Invoke(this, new ScrollEventArgs(ScrollEventType.ThumbPosition, _value));
            }
        }

        private Rectangle GetThumbRectangle()
        {
            int trackH = this.Height;
            if (trackH <= 0) return Rectangle.Empty;

            int range = _maximum - _minimum;
            if (range <= 0)
            {
                return new Rectangle(1, 0, Math.Max(2, this.Width - 2), trackH);
            }

            int totalSpan = range + _visibleAmount;
            float ratio = (float)_visibleAmount / totalSpan;
            int thumbH = (int)(trackH * ratio);
            thumbH = Math.Max(24, Math.Min(trackH, thumbH));

            int availableTravel = trackH - thumbH;
            float valRatio = (float)(_value - _minimum) / range;
            int thumbY = (int)(availableTravel * valRatio);

            int thumbW = Math.Max(4, this.Width - 2);
            int thumbX = (this.Width - thumbW) / 2;

            return new Rectangle(thumbX, thumbY, thumbW, thumbH);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isHovered = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                Rectangle thumb = GetThumbRectangle();
                if (thumb.Contains(e.Location))
                {
                    _isDragging = true;
                    _dragStartY = e.Y;
                    _dragStartValue = _value;
                    this.Capture = true;
                    Invalidate();
                }
                else
                {
                    if (e.Y < thumb.Y)
                    {
                        Value -= _largeChange;
                    }
                    else if (e.Y > thumb.Bottom)
                    {
                        Value += _largeChange;
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_isDragging)
            {
                Rectangle thumb = GetThumbRectangle();
                int trackH = this.Height;
                int availableTravel = trackH - thumb.Height;
                if (availableTravel > 0)
                {
                    int deltaY = e.Y - _dragStartY;
                    int range = _maximum - _minimum;
                    int deltaValue = (int)Math.Round((float)deltaY * range / availableTravel);
                    Value = _dragStartValue + deltaValue;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (_isDragging)
            {
                _isDragging = false;
                this.Capture = false;
                Invalidate();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            int step = (e.Delta > 0) ? -_smallChange : _smallChange;
            Value += step;
            if (e is HandledMouseEventArgs he)
            {
                he.Handled = true;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int w = this.Width;
            int h = this.Height;
            if (w <= 0 || h <= 0) return;

            // Slender track canal (centered, 4px wide)
            int canalW = Math.Max(3, w / 2 - 1);
            int canalX = (w - canalW) / 2;
            Rectangle canalRect = new Rectangle(canalX, 2, canalW, h - 4);

            using (GraphicsPath canalPath = CreatePillPath(canalRect))
            using (SolidBrush canalBrush = new SolidBrush(TrackColor))
            using (Pen canalPen = new Pen(TrackBorderColor, 1f))
            {
                g.FillPath(canalBrush, canalPath);
                g.DrawPath(canalPen, canalPath);
            }

            // Draw thumb pill
            if (_maximum > _minimum)
            {
                Rectangle thumbRect = GetThumbRectangle();
                if (thumbRect.Height > 0 && thumbRect.Width > 0)
                {
                    Color topClr = _isDragging ? ThumbDragTop : (_isHovered ? ThumbHoverTop : ThumbColorTop);
                    Color btmClr = _isDragging ? ThumbDragBottom : (_isHovered ? ThumbHoverBottom : ThumbColorBottom);
                    Color borderClr = _isDragging ? Color.FromArgb(100, 68, 14) : ThumbBorderColor;

                    using (GraphicsPath thumbPath = CreatePillPath(thumbRect))
                    using (LinearGradientBrush thumbBrush = new LinearGradientBrush(thumbRect, topClr, btmClr, 90f))
                    using (Pen thumbPen = new Pen(borderClr, 1.2f))
                    {
                        g.FillPath(thumbBrush, thumbPath);
                        g.DrawPath(thumbPen, thumbPath);
                    }

                    // 3 Embossed Antique Grip Dots in the middle of the thumb
                    int midY = thumbRect.Y + thumbRect.Height / 2;
                    int midX = thumbRect.X + thumbRect.Width / 2;
                    using (SolidBrush dotShadow = new SolidBrush(Color.FromArgb(90, 60, 15)))
                    using (SolidBrush dotLight = new SolidBrush(Color.FromArgb(255, 235, 140)))
                    {
                        for (int offset = -4; offset <= 4; offset += 4)
                        {
                            g.FillRectangle(dotShadow, midX - 1, midY + offset, 2, 2);
                            g.FillRectangle(dotLight, midX, midY + offset + 1, 1, 1);
                        }
                    }
                }
            }
        }

        private static GraphicsPath CreatePillPath(Rectangle r)
        {
            GraphicsPath path = new GraphicsPath();
            int rad = Math.Min(r.Width, r.Height) / 2;
            if (rad <= 0) rad = 1;
            int diam = rad * 2;

            path.AddArc(r.X, r.Y, diam, diam, 180, 90);
            path.AddArc(r.Right - diam, r.Y, diam, diam, 270, 90);
            path.AddArc(r.Right - diam, r.Bottom - diam, diam, diam, 0, 90);
            path.AddArc(r.X, r.Bottom - diam, diam, diam, 90, 90);
            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Binds this scrollbar to synchronize an outer viewport panel with an inner content control.
        /// </summary>
        public void BindTo(Panel viewport, Control content)
        {
            void UpdateScroll()
            {
                int overflow = Math.Max(0, content.Height - viewport.Height);
                this.Visible = overflow > 0;
                this.Minimum = 0;
                this.Maximum = overflow;
                this.VisibleAmount = Math.Max(1, viewport.Height);
                this.Value = Math.Min(this.Value, overflow);
                content.Top = -this.Value;
            }

            this.ValueChanged += (s, e) =>
            {
                content.Top = -this.Value;
            };

            viewport.Resize += (s, e) => UpdateScroll();
            content.SizeChanged += (s, e) => UpdateScroll();

            AttachMouseWheel(viewport);
            AttachMouseWheel(content);

            UpdateScroll();
        }

        /// <summary>
        /// Attaches mouse wheel listener to a control and all its current and future children.
        /// </summary>
        public void AttachMouseWheel(Control root)
        {
            void OnWheel(object? sender, MouseEventArgs e)
            {
                if (this.Visible)
                {
                    int step = (e.Delta > 0) ? -this.SmallChange : this.SmallChange;
                    this.Value += step;
                    if (e is HandledMouseEventArgs he) he.Handled = true;
                }
            }

            void Hook(Control ctrl)
            {
                ctrl.MouseWheel -= OnWheel;
                ctrl.MouseWheel += OnWheel;
                ctrl.ControlAdded += (s, e) => { if (e.Control != null) Hook(e.Control); };
                foreach (Control child in ctrl.Controls)
                {
                    Hook(child);
                }
            }

            Hook(root);
        }
    }
}
