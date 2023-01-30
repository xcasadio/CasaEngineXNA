
/*

 Based in the project Neoforce Controls (http://neoforce.codeplex.com/)
 GNU Library General Public License (LGPL)

-----------------------------------------------------------------------------------------------------------------------------------------------
Modified by: Schneider, Jos� Ignacio (jis@cs.uns.edu.ar)
-----------------------------------------------------------------------------------------------------------------------------------------------

*/

using Microsoft.Xna.Framework.Graphics;


namespace XNAFinalEngine.UserInterface
{

    public class Window : ModalContainer
    {


        private const string skinWindow = "Window";
        private const string layerWindow = "Control";
        private const string layerCaption = "Caption";
        private const string layerFrameTop = "FrameTop";
        private const string layerFrameLeft = "FrameLeft";
        private const string layerFrameRight = "FrameRight";
        private const string layerFrameBottom = "FrameBottom";
        private const string layerIcon = "Icon";
        private const string skinButton = "Window.CloseButton";
        private const string layerButton = "Control";
        private const string skinShadow = "Window.Shadow";
        private const string layerShadow = "Control";



        private readonly Button buttonClose;

        private bool closeButtonVisible = true;

        private bool iconVisible = true;

        private bool shadow = true;

        private bool captionVisible = true;

        private bool borderVisible = true;

        private byte dragAlpha = 200;
        private byte oldAlpha = 255;



        public Texture2D Icon { get; set; }

        public virtual bool Shadow
        {
            get => shadow;
            set => shadow = value;
        } // Shadow

        public virtual bool CloseButtonVisible
        {
            get => closeButtonVisible;
            set
            {
                closeButtonVisible = value;
                if (buttonClose != null)
                    buttonClose.Visible = value;
            }
        } // CloseButtonVisible

        public virtual bool IconVisible
        {
            get => iconVisible;
            set => iconVisible = value;
        } // IconVisible

        public virtual bool CaptionVisible
        {
            get => captionVisible;
            set
            {
                captionVisible = value;
                AdjustMargins();
            }
        } // CaptionVisible

        public virtual bool BorderVisible
        {
            get => borderVisible;
            set
            {
                borderVisible = value;
                AdjustMargins();
            }
        } // BorderVisible

        public virtual byte DragAlpha
        {
            get => dragAlpha;
            set => dragAlpha = value;
        } // DragAlpha



        public Window(UserInterfaceManager userInterfaceManager_)
            : base(userInterfaceManager_)
        {
            SetDefaultSize(640, 480);
            SetMinimumSize(100, 75);

            buttonClose = new Button(UserInterfaceManager)
            {
                SkinInformation = new SkinControlInformation(UserInterfaceManager.Skin.Controls[skinButton]),
                Detached = true,
                CanFocus = false,
                Text = null,
            };
            buttonClose.Click += ButtonClose_Click;
            buttonClose.SkinChanged += ButtonClose_SkinChanged;

            AdjustMargins();

            AutoScroll = true;
            Movable = true;
            Resizable = true;
            CenterWindow();

            Add(buttonClose, false);

            oldAlpha = Alpha;
        } // Window



        protected internal override void Init()
        {
            base.Init();
            SkinLayer skinLayer = buttonClose.SkinInformation.Layers[layerButton];
            buttonClose.Width = skinLayer.Width - buttonClose.SkinInformation.OriginMargins.Horizontal;
            buttonClose.Height = skinLayer.Height - buttonClose.SkinInformation.OriginMargins.Vertical;
            buttonClose.Left = ControlAndMarginsWidth - SkinInformation.OriginMargins.Right - buttonClose.Width + skinLayer.OffsetX;
            buttonClose.Top = SkinInformation.OriginMargins.Top + skinLayer.OffsetY;
            buttonClose.Anchor = Anchors.Top | Anchors.Right;
        } // Init

        protected internal override void InitSkin()
        {
            base.InitSkin();
            SkinInformation = new SkinControlInformation(UserInterfaceManager.Skin.Controls[skinWindow]);
            AdjustMargins();

            CheckLayer(SkinInformation, layerWindow);
            CheckLayer(SkinInformation, layerCaption);
            CheckLayer(SkinInformation, layerFrameTop);
            CheckLayer(SkinInformation, layerFrameLeft);
            CheckLayer(SkinInformation, layerFrameRight);
            CheckLayer(SkinInformation, layerFrameBottom);
            CheckLayer(UserInterfaceManager.Skin.Controls[skinButton], layerButton);
            CheckLayer(UserInterfaceManager.Skin.Controls[skinShadow], layerShadow);
        } // InitSkin



        private void ButtonClose_SkinChanged(object sender, EventArgs e)
        {
            buttonClose.SkinInformation = new SkinControlInformation(UserInterfaceManager.Skin.Controls[skinButton]);
        } // ButtonClose_SkinChanged

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close(ModalResult = ModalResult.Cancel);
        } // ButtonClose_Click



        internal override void DrawControlOntoMainTexture()
        {


            if (Visible && Shadow)
            {
                SkinControlInformation skinControlShadow = UserInterfaceManager.Skin.Controls[skinShadow];
                SkinLayer skinLayerShadow = skinControlShadow.Layers[layerShadow];

                Color shadowColor = Color.FromNonPremultiplied(skinLayerShadow.States.Enabled.Color.R,
                                                               skinLayerShadow.States.Enabled.Color.G,
                                                               skinLayerShadow.States.Enabled.Color.B, Alpha);

                UserInterfaceManager.Renderer.Begin();
                UserInterfaceManager.Renderer.DrawLayer(skinLayerShadow,
                                       new Rectangle(Left - skinControlShadow.OriginMargins.Left,
                                                     Top - skinControlShadow.OriginMargins.Top,
                                                     Width + skinControlShadow.OriginMargins.Horizontal,
                                                     Height + skinControlShadow.OriginMargins.Vertical),
                                       shadowColor, 0);
                UserInterfaceManager.Renderer.End();
            }


            base.DrawControlOntoMainTexture();
        } // Render

        private Rectangle GetIconRectangle()
        {
            SkinLayer skinLayerCaption = SkinInformation.Layers[layerCaption];
            SkinLayer skinLayerIcon = SkinInformation.Layers[layerIcon];

            int iconHeight = skinLayerCaption.Height - skinLayerCaption.ContentMargins.Vertical;
            return new Rectangle(DrawingRectangle.Left + skinLayerCaption.ContentMargins.Left + skinLayerIcon.OffsetX,
                                 DrawingRectangle.Top + skinLayerCaption.ContentMargins.Top + skinLayerIcon.OffsetY,
                                 iconHeight, iconHeight);

        } // GetIconRectangle

        protected override void DrawControl(Rectangle rect)
        {
            SkinLayer skinLayerFrameTop = captionVisible ? SkinInformation.Layers[layerCaption] : SkinInformation.Layers[layerFrameTop];
            SkinLayer skinLayerFrameLeft = SkinInformation.Layers[layerFrameLeft];
            SkinLayer skinLayerFrameRight = SkinInformation.Layers[layerFrameRight];
            SkinLayer skinLayerFrameBottom = SkinInformation.Layers[layerFrameBottom];
            SkinLayer skinLayerIcon = SkinInformation.Layers[layerIcon];
            LayerStates layerStateFrameTop, layerStateFrameLeft, layerStateFrameRight, layerStateFrameButtom;
            Font font = skinLayerFrameTop.Text.Font.Font;
            Color color;

            if ((Focused || (UserInterfaceManager.FocusedControl != null && UserInterfaceManager.FocusedControl.Root == Root)) && ControlState != ControlState.Disabled)
            {
                layerStateFrameTop = skinLayerFrameTop.States.Focused;
                layerStateFrameLeft = skinLayerFrameLeft.States.Focused;
                layerStateFrameRight = skinLayerFrameRight.States.Focused;
                layerStateFrameButtom = skinLayerFrameBottom.States.Focused;
                color = skinLayerFrameTop.Text.Colors.Focused;
            }
            else if (ControlState == ControlState.Disabled)
            {
                layerStateFrameTop = skinLayerFrameTop.States.Disabled;
                layerStateFrameLeft = skinLayerFrameLeft.States.Disabled;
                layerStateFrameRight = skinLayerFrameRight.States.Disabled;
                layerStateFrameButtom = skinLayerFrameBottom.States.Disabled;
                color = skinLayerFrameTop.Text.Colors.Disabled;
            }
            else
            {
                layerStateFrameTop = skinLayerFrameTop.States.Enabled;
                layerStateFrameLeft = skinLayerFrameLeft.States.Enabled;
                layerStateFrameRight = skinLayerFrameRight.States.Enabled;
                layerStateFrameButtom = skinLayerFrameBottom.States.Enabled;
                color = skinLayerFrameTop.Text.Colors.Enabled;
            }
            // Render Background plane
            UserInterfaceManager.Renderer.DrawLayer(SkinInformation.Layers[layerWindow], rect, SkinInformation.Layers[layerWindow].States.Enabled.Color, SkinInformation.Layers[layerWindow].States.Enabled.Index);
            // Render border
            if (borderVisible)
            {
                UserInterfaceManager.Renderer.DrawLayer(skinLayerFrameTop, new Rectangle(rect.Left, rect.Top, rect.Width, skinLayerFrameTop.Height), layerStateFrameTop.Color, layerStateFrameTop.Index);
                UserInterfaceManager.Renderer.DrawLayer(skinLayerFrameLeft, new Rectangle(rect.Left, rect.Top + skinLayerFrameTop.Height, skinLayerFrameLeft.Width, rect.Height - skinLayerFrameTop.Height - skinLayerFrameBottom.Height), layerStateFrameLeft.Color, layerStateFrameLeft.Index);
                UserInterfaceManager.Renderer.DrawLayer(skinLayerFrameRight, new Rectangle(rect.Right - skinLayerFrameRight.Width, rect.Top + skinLayerFrameTop.Height, skinLayerFrameRight.Width, rect.Height - skinLayerFrameTop.Height - skinLayerFrameBottom.Height), layerStateFrameRight.Color, layerStateFrameRight.Index);
                UserInterfaceManager.Renderer.DrawLayer(skinLayerFrameBottom, new Rectangle(rect.Left, rect.Bottom - skinLayerFrameBottom.Height, rect.Width, skinLayerFrameBottom.Height), layerStateFrameButtom.Color, layerStateFrameButtom.Index);

                if (iconVisible && (Icon != null || skinLayerIcon != null) && captionVisible)
                {
                    Texture2D i = Icon ?? skinLayerIcon.Image.Texture.Resource;
                    UserInterfaceManager.Renderer.Draw(i, GetIconRectangle(), Color.White);
                }

                int icosize = 0;
                if (skinLayerIcon != null && iconVisible && captionVisible)
                {
                    icosize = skinLayerFrameTop.Height - skinLayerFrameTop.ContentMargins.Vertical + 4 + skinLayerIcon.OffsetX;
                }
                int closesize = 0;
                if (buttonClose.Visible)
                {
                    closesize = buttonClose.Width - (buttonClose.SkinInformation.Layers[layerButton].OffsetX);
                }

                Rectangle r = new Rectangle(rect.Left + skinLayerFrameTop.ContentMargins.Left + icosize,
                                            rect.Top + skinLayerFrameTop.ContentMargins.Top,
                                            rect.Width - skinLayerFrameTop.ContentMargins.Horizontal - closesize - icosize,
                                            skinLayerFrameTop.Height - skinLayerFrameTop.ContentMargins.Top - skinLayerFrameTop.ContentMargins.Bottom);
                int ox = skinLayerFrameTop.Text.OffsetX;
                int oy = skinLayerFrameTop.Text.OffsetY;
                UserInterfaceManager.Renderer.DrawString(font, Text, r, color, skinLayerFrameTop.Text.Alignment, ox, oy, true);
            }
        } // DrawControl



        public virtual void CenterWindow()
        {
            Left = (UserInterfaceManager.Screen.Width / 2) - (Width / 2);
            Top = (UserInterfaceManager.Screen.Height - Height) / 2;
        } // Center



        protected override void OnResize(ResizeEventArgs e)
        {
            SetMovableArea();
            base.OnResize(e);
        } // OnResize

        protected override void OnMoveBegin(EventArgs e)
        {
            base.OnMoveBegin(e);
            oldAlpha = Alpha;
            Alpha = dragAlpha;
        } // OnMoveBegin

        protected override void OnMoveEnd(EventArgs e)
        {
            base.OnMoveEnd(e);
            Alpha = oldAlpha;
        } // OnMoveEnd

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);

            MouseEventArgs ex = (e is MouseEventArgs) ? (MouseEventArgs)e : new MouseEventArgs();

            if (IconVisible && ex.Button == MouseButton.Left)
            {
                Rectangle r = GetIconRectangle();
                r.Offset(ControlLeftAbsoluteCoordinate, ControlTopAbsoluteCoordinate);
                if (r.Contains(ex.Position))
                {
                    Close();
                }
            }
        } // OnDoubleClick



        protected override void AdjustMargins()
        {
            if (captionVisible && borderVisible)
            {
                ClientMargins = new Margins(SkinInformation.ClientMargins.Left, SkinInformation.Layers[layerCaption].Height,
                                            SkinInformation.ClientMargins.Right, SkinInformation.ClientMargins.Bottom);
            }
            else if (!captionVisible && borderVisible)
            {
                ClientMargins = new Margins(SkinInformation.ClientMargins.Left, SkinInformation.ClientMargins.Top,
                                            SkinInformation.ClientMargins.Right, SkinInformation.ClientMargins.Bottom);
            }
            else if (!borderVisible)
            {
                ClientMargins = new Margins(0, 0, 0, 0);
            }

            if (buttonClose != null)
            {
                buttonClose.Visible = closeButtonVisible && captionVisible && borderVisible;
            }

            SetMovableArea();

            base.AdjustMargins();
        } // AdjustMargins



        private void SetMovableArea()
        {
            if (captionVisible && borderVisible)
            {
                MovableArea = new Rectangle(SkinInformation.OriginMargins.Left, SkinInformation.OriginMargins.Top, Width, SkinInformation.Layers[layerCaption].Height - SkinInformation.OriginMargins.Top);
            }
            else if (!captionVisible)
            {
                MovableArea = new Rectangle(0, 0, Width, Height);
            }
        } // SetMovableArea


    } // Window
} // XNAFinalEngine.UserInterface