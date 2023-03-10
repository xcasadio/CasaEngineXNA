//-----------------------------------------------------------------------------
// DebugManager.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------


using Microsoft.Xna.Framework.Graphics;

#if EDITOR
//using CasaEngine.Editor.GameComponent;
#endif


namespace CasaEngine.Framework.Debugger
{
    public class DebugManager
        : Microsoft.Xna.Framework.DrawableGameComponent
    {

        public bool DebugPickBufferTexture
        {
            get;
            set;
        }

        public bool MakeScreenShot
        {
            get;
            set;
        }

        public Texture2D WhiteTexture { get; private set; }



        public DebugManager(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            // Added as a Service.
            //Game.Services.AddService(typeof(DebugManager), this);
            //Game.Components.Add(this);

            // This component doesn't need be call neither update nor draw.
            Enabled = false;
            Visible = false;

            /*UpdateOrder = (int)ComponentUpdateOrder.DebugManager;
			DrawOrder = (int)ComponentDrawOrder.DebugManager;*/

            VisibleChanged += DebugManager_VisibleChanged;
            EnabledChanged += DebugManager_EnabledChanged;
        }

        private void DebugManager_EnabledChanged(object sender, EventArgs e)
        {
            Enabled = false;
        }

        private void DebugManager_VisibleChanged(object sender, EventArgs e)
        {
            Visible = false;
        }

        protected override void LoadContent()
        {
            // Load debug content.
            //SpriteBatch = new SpriteBatch(GraphicsDevice);

            //DebugFont = Game.Content.Load<SpriteFont>(debugFont);

            // Create white texture.
            WhiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            var whitePixels = new Color[] { Color.White };
            WhiteTexture.SetData(whitePixels);

            base.LoadContent();
        }

    }
}