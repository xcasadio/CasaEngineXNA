
/*

 Based in the project Neoforce Controls (http://neoforce.codeplex.com/)
 GNU Library General Public License (LGPL)

-----------------------------------------------------------------------------------------------------------------------------------------------
Modified by: Schneider, Jos� Ignacio (jis@cs.uns.edu.ar)
-----------------------------------------------------------------------------------------------------------------------------------------------

*/


namespace XNAFinalEngine.UserInterface
{


    public enum ProgressBarMode
    {
        Default,
        Infinite
    } // ProgressBarMode


    public class ProgressBar : Control
    {


        private int range = 100;

        private int value;

        private ProgressBarMode mode = ProgressBarMode.Default;

        private double time;

        private int sign = 1;



        public int Value
        {
            get => value;
            set
            {
                if (mode == ProgressBarMode.Default)
                {
                    if (this.value != value)
                    {
                        this.value = value;
                        if (this.value > range) this.value = range;
                        if (this.value < 0) this.value = 0;
                        Invalidate();

                        if (!Suspended) OnValueChanged(new EventArgs());
                    }
                }
            }
        } // Value

        public ProgressBarMode Mode
        {
            get => mode;
            set
            {
                if (mode != value)
                {
                    mode = value;
                    if (mode == ProgressBarMode.Infinite)
                    {
                        range = 100;
                        this.value = 0;
                        time = 0;
                        sign = 1;
                    }
                    else
                    {
                        this.value = 0;
                        range = 100;
                    }
                    Invalidate();

                    if (!Suspended) OnModeChanged(new EventArgs());
                }
            }
        } // Mode

        public int Range
        {
            get => range;
            set
            {
                if (range != value)
                {
                    if (mode == ProgressBarMode.Default)
                    {
                        range = value;
                        if (range < 0) range = 0;
                        if (range < this.value) this.value = range;
                        Invalidate();

                        if (!Suspended) OnRangeChanged(new EventArgs());
                    }
                }
            }
        } // Range



        public event EventHandler ValueChanged;
        public event EventHandler RangeChanged;
        public event EventHandler ModeChanged;



        public ProgressBar(UserInterfaceManager userInterfaceManager_)
            : base(userInterfaceManager_)
        {
            Width = 128;
            Height = 16;
            MinimumHeight = 8;
            MinimumWidth = 32;
            Passive = true;
            CanFocus = false;
        } // ProgressBar



        protected override void DisposeManagedResources()
        {
            // A disposed object could be still generating events, because it is alive for a time, in a disposed state, but alive nevertheless.
            ValueChanged = null;
            RangeChanged = null;
            ModeChanged = null;
            base.DisposeManagedResources();
        } // DisposeManagedResources



        protected override void DrawControl(Rectangle rect)
        {
            CheckLayer(SkinInformation, "Control");
            CheckLayer(SkinInformation, "Scale");

            base.DrawControl(rect);

            if (Value > 0 || mode == ProgressBarMode.Infinite)
            {
                SkinLayer p = SkinInformation.Layers["Control"];
                SkinLayer l = SkinInformation.Layers["Scale"];
                Rectangle r = new Rectangle(rect.Left + p.ContentMargins.Left,
                                            rect.Top + p.ContentMargins.Top,
                                            rect.Width - p.ContentMargins.Vertical,
                                            rect.Height - p.ContentMargins.Horizontal);

                float perc = ((float)value / range) * 100;
                int w = (int)((perc / 100) * r.Width);
                Rectangle rx;
                if (mode == ProgressBarMode.Default)
                {
                    if (w < l.SizingMargins.Vertical) w = l.SizingMargins.Vertical;
                    rx = new Rectangle(r.Left, r.Top, w, r.Height);
                }
                else
                {
                    int s = r.Left + w;
                    if (s > r.Left + p.ContentMargins.Left + r.Width - (r.Width / 4)) s = r.Left + p.ContentMargins.Left + r.Width - (r.Width / 4);
                    rx = new Rectangle(s, r.Top, (r.Width / 4), r.Height);
                }

                UserInterfaceManager.Renderer.DrawLayer(this, l, rx);
            }
        } // DrawControl



        protected internal override void Update(float elapsedTime_)
        {
            base.Update(elapsedTime_);

            if (mode == ProgressBarMode.Infinite && Enabled && Visible)
            {
                time += elapsedTime_; // From seconds to milliseconds.
                if (time >= 33f)
                {
                    value += sign * (int)Math.Ceiling(time / 20f);
                    if (value >= Range - (Range / 4))
                    {
                        value = Range - (Range / 4);
                        sign = -1;
                    }
                    else if (value <= 0)
                    {
                        value = 0;
                        sign = 1;
                    }
                    time = 0;
                    Invalidate();
                }
            }
        } // Update



        protected virtual void OnValueChanged(EventArgs e)
        {
            if (ValueChanged != null) ValueChanged.Invoke(this, e);
        } // OnValueChanged

        protected virtual void OnRangeChanged(EventArgs e)
        {
            if (RangeChanged != null) RangeChanged.Invoke(this, e);
        } // OnRangeChanged

        protected virtual void OnModeChanged(EventArgs e)
        {
            if (ModeChanged != null) ModeChanged.Invoke(this, e);
        } // OnModeChanged


    } // ProgressBar
} // XNAFinalEngine.UserInterface