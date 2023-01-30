﻿using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CasaEngineCommon.Design;

namespace CasaEngine.FrontEnd.Screen.Gadget
{
    public
#if EDITOR
    partial
#endif
    class ScreenGadgetLabel
        : ScreenGadget
    {





        public ScreenGadgetLabel(XmlElement el_, SaveOption opt_)
            : base(el_, opt_)
        {

        }



#if EDITOR
        public
#else
        protected
#endif
        override void DrawGadget(float elapsedTime_)
        {
            Renderer2DComponent.AddSprite2D(
                WhiteTexture,
                Location,
                0.0f,
                Scale,
                BackgroundColor,
                0.0001f,
                SpriteEffects.None);

            Renderer2DComponent.AddText2D(
                Font,
                Text,
                Location + Vector2.One * 5,
                0.0f,
                Scale,
                FontColor,
                0.0f);
        }

    }
}
