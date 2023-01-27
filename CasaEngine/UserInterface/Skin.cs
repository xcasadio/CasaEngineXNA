
#region License
/*

 Based in the project Neoforce Controls (http://neoforce.codeplex.com/)
 GNU Library General Public License (LGPL)

-----------------------------------------------------------------------------------------------------------------------------------------------
Modified by: Schneider, Jos� Ignacio (jis@cs.uns.edu.ar)
-----------------------------------------------------------------------------------------------------------------------------------------------

*/
#endregion

#region Using directives
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using CasaEngine.Asset;
using XNAFinalEngine.Assets;
using System.IO;
using CasaEngine.Game;
using CasaEngine.Asset.Cursors;
using Microsoft.Xna.Framework.Graphics;
using CasaEngine.Asset.Fonts;
#endregion

namespace XNAFinalEngine.UserInterface
{

    #region Structs

    /// <summary>
    /// Skin element states.
    /// </summary>
    public struct SkinStates<T>
    {
        public T Enabled;
        public T Hovered;
        public T Pressed;
        public T Focused;
        public T Disabled;

        public SkinStates(T enabled, T hovered, T pressed, T focused, T disabled)
        {
            Enabled = enabled;
            Hovered = hovered;
            Pressed = pressed;
            Focused = focused;
            Disabled = disabled;
        } // SkinStates

    } // SkinStates

    /// <summary>
    /// Layers states
    /// </summary>
    public struct LayerStates
    {
        public int Index;
        public Color Color;
        public bool Overlay;
    } // LayerStates

    /// <summary>
    /// Layer Overlays
    /// </summary>
    public struct LayerOverlays
    {
        public int Index;
        public Color Color;
    } // LayerOverlays

    #endregion

    #region SkinList

    public class SkinList<T> : List<T>
    {

        #region Indexers

        public T this[string index]
        {
            get
            {
                for (int i = 0; i < Count; i++)
                {
                    SkinBase s = (SkinBase)(object)this[i];
                    //if (s.Name.ToLower() == index.ToLower()) // Not need to produce so much garbage unnecessary.
                    if (s.Name == index)
                    {
                        return this[i];
                    }
                }
                return default(T);
            }
            set
            {
                for (int i = 0; i < Count; i++)
                {
                    SkinBase s = (SkinBase)(object)this[i];
                    //if (s.Name.ToLower() == index.ToLower())
                    if (s.Name == index)
                    {
                        this[i] = value;
                    }
                }
            }
        } // this

        #endregion

        #region Constructors

        public SkinList() { }

        public SkinList(SkinList<T> source)
        {
            foreach (T t1 in source)
            {
                Type[] t = new Type[1];
                t[0] = typeof(T);

                object[] p = new object[1];
                p[0] = t1;

                Add((T)t[0].GetConstructor(t).Invoke(p));
            }
        } // SkinList

        #endregion

    } // SkinList

    #endregion

    #region SkinBase

    public class SkinBase
    {

        #region Variables

        /// <summary>
        /// Name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public SkinBase() { }

        public SkinBase(SkinBase source)
        {
            if (source != null)
            {
                Name = source.Name;
            }
        } // SkinBase

        #endregion

    } // SkinBase

    #endregion

    #region SkinLayer

    public class SkinLayer : SkinBase
    {

        #region Variables

        public SkinImage Image
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public int OffsetX
        {
            get;
            set;
        }

        public int OffsetY
        {
            get;
            set;
        }

        public Alignment Alignment
        {
            get;
            set;
        }

        public Margins SizingMargins
        {
            get;
            set;
        }

        public Margins ContentMargins
        {
            get;
            set;
        }

        public SkinStates<LayerStates> States
        {
            get;
            set;
        }

        public SkinStates<LayerOverlays> Overlays
        {
            get;
            set;
        }

        public SkinText Text
        {
            get;
            set;
        }

        public SkinList<SkinAttribute> Attributes
        {
            get;
            set;
        }


        #endregion

        #region Constructors

        public SkinLayer()
        {
            SkinStates<LayerStates> states = new SkinStates<LayerStates>();

            states.Enabled.Color = Color.White;
            states.Pressed.Color = Color.White;
            states.Focused.Color = Color.White;
            states.Hovered.Color = Color.White;
            states.Disabled.Color = Color.White;
            States = states;

            SkinStates<LayerOverlays> overlays = new SkinStates<LayerOverlays>();
            overlays.Enabled.Color = Color.White;
            overlays.Pressed.Color = Color.White;
            overlays.Focused.Color = Color.White;
            overlays.Hovered.Color = Color.White;
            overlays.Disabled.Color = Color.White;
            Overlays = overlays;

            Text = new SkinText();
            Attributes = new SkinList<SkinAttribute>();
            Image = new SkinImage();
        } // SkinLayer

        public SkinLayer(SkinLayer source) : base(source)
        {
            if (source != null)
            {
                Image = new SkinImage(source.Image);
                Width = source.Width;
                Height = source.Height;
                OffsetX = source.OffsetX;
                OffsetY = source.OffsetY;
                Alignment = source.Alignment;
                SizingMargins = source.SizingMargins;
                ContentMargins = source.ContentMargins;
                States = source.States;
                Overlays = source.Overlays;
                Text = new SkinText(source.Text);
                Attributes = new SkinList<SkinAttribute>(source.Attributes);
            }
            else
            {
                throw new Exception("Parameter for SkinLayer copy constructor cannot be null.");
            }
        } // SkinLayer

        #endregion

    } // SkinLayer

    #endregion

    #region SkinText

    public class SkinText : SkinBase
    {

        #region Variables

        /// <summary>
        /// Associated font.
        /// </summary>
        public SkinFont Font
        {
            get;
            set;
        }

        /// <summary>
        /// Offset from the left.
        /// </summary>
        public int OffsetX
        {
            get;
            set;
        }


        /// <summary>
        /// Offset from the bottom.
        /// </summary>
        public int OffsetY
        {
            get;
            set;
        }


        /// <summary>
        /// Text alignment.
        /// </summary>
        public Alignment Alignment
        {
            get;
            set;
        }


        /// <summary>
        /// Colors when enabled, hovered, pressed, focused, and disabled.
        /// </summary>
        public SkinStates<Color> Colors
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public SkinText()
        {
            SkinStates<Color>  colors = new SkinStates<Color>();
            colors.Enabled = Color.White;
            colors.Pressed = Color.White;
            colors.Focused = Color.White;
            colors.Hovered = Color.White;
            colors.Disabled = Color.White;
            Colors = colors;
        } // SkinText

        public SkinText(SkinText source) : base(source)
        {
            if (source != null)
            {
                Font = new SkinFont(source.Font);
                OffsetX = source.OffsetX;
                OffsetY = source.OffsetY;
                Alignment = source.Alignment;
                Colors = source.Colors;
            }
        } // SkinText

        #endregion

    } // SkinText

    #endregion

    #region SkinFont

    public class SkinFont : SkinBase
    {

        #region Variables

        /// <summary>
        /// Asset.
        /// </summary>
        public Font Font;

        /// <summary>
        /// Asset filename.
        /// </summary>
        public string Filename
        {
            get;
            set;
        }

        #endregion

        #region Properties
        
        public int Height
        {
            get
            {
                if (Font != null)
                {
                    return (int)Font.MeasureString("AaYy").Y;
                }
                return 0;
            }
        } // Height

        #endregion

        #region Constructors

        public SkinFont() { }

        public SkinFont(SkinFont source) : base(source)
        {
            if (source != null)
            {
                Font = source.Font;
                Filename = source.Filename;
            }
        } // SkinFont

        #endregion

    } // SkinFont

    #endregion

    #region SkinImage

    public class SkinImage : SkinBase
    {

        #region Variables

        /// <summary>
        /// Asset.
        /// </summary>
        public CasaEngine.Asset.Texture Texture;

        /// <summary>
        /// Asset filename.
        /// </summary>
        public string Filename
        {
            get;
            set;
        }

        #endregion

        #region Constructors
        
        public SkinImage() { }

        public SkinImage(SkinImage source) : base(source)
        {
            Texture = source.Texture;
            Filename = source.Filename;
        } // SkinImage

        #endregion

    } // SkinImage

    #endregion

    #region SkinCursor

    #if (WINDOWS)

        public class SkinCursor : SkinBase
        {

            /// <summary>
            /// Asset.
            /// </summary>
            public Cursor Cursor;

            /// <summary>
            /// Asset filename.
            /// </summary>
            public string Filename
            {
                get;
                set;
            }
        } // SkinCursor

    #endif

    #endregion

    #region SkinControl

    public class SkinControlInformation : SkinBase
    {

        #region Variables

        public Size DefaultSize
        {
            get;
            set;
        }

        public int ResizerSize
        {
            get;
            set;
        }

        public Size MinimumSize
        {
            get;
            set;
        }

        public Margins OriginMargins
        {
            get;
            set;
        }

        public Margins ClientMargins
        {
            get;
            set;
        }

        public SkinList<SkinLayer> Layers
        {
            get;
            private set;
        }

        public SkinList<SkinAttribute> Attributes
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        public SkinControlInformation()
        {
            Layers = new SkinList<SkinLayer>();
            Attributes = new SkinList<SkinAttribute>();
        }

        public SkinControlInformation(SkinControlInformation source) : base(source)
        {
            DefaultSize = source.DefaultSize;
            MinimumSize = source.MinimumSize;
            OriginMargins = source.OriginMargins;
            ClientMargins = source.ClientMargins;
            ResizerSize = source.ResizerSize;
            Layers = new SkinList<SkinLayer>(source.Layers);
            Attributes = new SkinList<SkinAttribute>(source.Attributes);
        } // SkinControl

        #endregion

    } // SkinControl

    #endregion

    #region SkinAttribute

    public class SkinAttribute : SkinBase
    {

        #region Variables
        
        /// <summary>
        /// Value.
        /// </summary>
        public string Value
        {
            get;
            set;
        }
        
        #endregion

        #region Contructors

        public SkinAttribute() { }

        public SkinAttribute(SkinAttribute source) : base(source)
        {
            Value = source.Value;
        } // SkinAttribute

        #endregion

    } // SkinAttribute

    #endregion

    #region Skin

    /// <summary>
    /// Manage the skin content (mouse cursors, elements' images, fonts, and skin's parameters)
    /// The main task is to load the skin information and skin resources.
    /// </summary>
    public class Skin
    {

        #region Variables

        /// <summary>
        /// Skin XML document.
        /// </summary>
        private Document skinDescription;

        /// <summary>
        /// Skin content manager.
        /// </summary>
        //private AssetContentManager skinContentManager;

        /// <summary>
        /// Skin content manager.
        /// </summary>
        private static readonly string skinContentManagerCategory = "UserInterfaceSkin";
        
        #endregion

        #region Properties

        /// <summary>
        /// Current skin name.
        /// </summary>
        public string CurrentSkinName { get; private set; }
       
        /// <summary>
        /// Skin information for controls.
        /// </summary>
        public SkinList<SkinControlInformation> Controls { get; private set; }

        /// <summary>
        /// Skin information for fonts.
        /// </summary>
        public SkinList<SkinFont> Fonts { get; private set; }

        #if (WINDOWS)
            /// <summary>
            /// Skin information for cursors.
            /// </summary>
            public SkinList<SkinCursor> Cursors { get; private set; }
        #endif

        /// <summary>
        /// Skin information for images.
        /// </summary>
        public SkinList<SkinImage> Images { get; private set; }

        #endregion

        #region Load Skin

        /// <summary>
        /// Manage the skin content (mouse cursors, elements' images, fonts, and skin's parameters)
        /// </summary>
        public void LoadSkin(GraphicsDevice graphicsDevice_, string skinName)
        {
            CurrentSkinName = skinName;

            //AssetContentManager userContentManager = AssetContentManager.CurrentContentManager;

            #region Unload previous skin
            
            Controls = new SkinList<SkinControlInformation>();
            Fonts = new SkinList<SkinFont>();
            Images = new SkinList<SkinImage>();
            #if (WINDOWS)
                Cursors = new SkinList<SkinCursor>();
            #endif

            /*if (skinContentManager == null)
                skinContentManager = new AssetContentManager { Name = "Skin Content Manager", Hidden = true };
            else
                skinContentManager.Unload();*/
            /*if (skinContentManager != null)
            {
                skinContentManager.Unload(skinContentManagerCategory);
            }*/
            Engine.Instance.AssetContentManager.Unload(skinContentManagerCategory);

            #endregion

            #region Load Description File

            string fullPath = "Skin" + Path.DirectorySeparatorChar + skinName;
            skinDescription = new Document(fullPath  + Path.DirectorySeparatorChar + "Description");
            
            // Read XML data.
            if (skinDescription.Resource.Element("Skin") != null)
            {
                try
                {
                    LoadImagesDescription();
                    LoadFontsDescription();
                    #if (WINDOWS)
                        LoadCursorsDescription();
                    #endif
                    LoadControlsDescription();
                }
                catch (Exception e)
                {
                    throw new Exception("Failed to load skin: " + skinName + ".\n\n" + e.Message);
                }
            }
            else
            {
                throw new Exception("Failed to load skin: " + skinName + ". Skin tag doesn't exist.");
            }

            #endregion

            #region Load Resources

            try
            {
                foreach (SkinFont skinFont in Fonts)
                {
                    skinFont.Font = (CasaEngine.Asset.Fonts.Font)Engine.Instance.ObjectManager.GetObjectByPath(skinFont.Filename);
                    skinFont.Font.LoadTexture("", graphicsDevice_);
                    //skinFont.Font = new CasaEngine.Asset.Fonts.Font(graphicsDevice_, fullPath + Path.DirectorySeparatorChar + "Fonts" + Path.DirectorySeparatorChar + skinFont.Filename);
                }
                #if (WINDOWS)
                    foreach (SkinCursor skinCursor in Cursors)
                    {
                        skinCursor.Cursor = new Cursor(graphicsDevice_,
                            Engine.Instance.AssetContentManager.RootDirectory + Path.DirectorySeparatorChar +
                            fullPath + Path.DirectorySeparatorChar + "Cursors" + Path.DirectorySeparatorChar + skinCursor.Filename + ".cur");
                    }
                #endif
                foreach (SkinImage skinImage in Images)
                {
                    skinImage.Texture = new CasaEngine.Asset.Texture(graphicsDevice_, 
                        fullPath + Path.DirectorySeparatorChar + "Textures" + Path.DirectorySeparatorChar + skinImage.Filename + ".png");
                }
                foreach (SkinControlInformation skinControl in Controls)
                {
                    foreach (SkinLayer skinLayer in skinControl.Layers)
                    {
                        if (skinLayer.Image.Name != null)
                        {
                            skinLayer.Image = Images[skinLayer.Image.Name];
                        }
                        else
                        {
                            skinLayer.Image = Images[0];
                        }
                        skinLayer.Text.Font = skinLayer.Text.Name != null ? Fonts[skinLayer.Text.Name] : Fonts[0];
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to load skin: " + skinName + ".", e);
            }

            #endregion

        } // LoadSkin

        #endregion

        #region Load Controls

        /// <summary>
        /// Load the skin information of every control.
        /// </summary>
        private void LoadControlsDescription()
        {
            if (skinDescription.Resource.Element("Skin").Element("Controls") == null)
                return;

            foreach (XElement control in skinDescription.Resource.Descendants("Control"))
            {
                SkinControlInformation skinControl;
                // Create skin control
                string parent = ReadAttribute(control, "Inherits", null, false);
                bool inherit = false;
                if (parent != null) // If there is a parent then it loads the information from it.
                {
                    skinControl = new SkinControlInformation(Controls[parent]);
                    inherit = true;
                }
                else
                    skinControl = new SkinControlInformation();

                // Load general information
                string name = "";
                ReadAttribute(ref name, inherit, control, "Name", null, true);
                skinControl.Name = name;

                Size size = new Size();
                ReadAttribute(ref size.Width, inherit, control.Element("DefaultSize"), "Width", 0, false);
                ReadAttribute(ref size.Height, inherit, control.Element("DefaultSize"), "Height", 0, false);
                skinControl.DefaultSize = size;

                ReadAttribute(ref size.Width, inherit, control.Element("MinimumSize"), "Width", 0, false);
                ReadAttribute(ref size.Height, inherit, control.Element("MinimumSize"), "Height", 0, false);
                skinControl.MinimumSize = size;

                Margins margin = new Margins();
                ReadAttribute(ref margin.Left, inherit, control.Element("OriginMargins"), "Left", 0, false);
                ReadAttribute(ref margin.Top, inherit, control.Element("OriginMargins"), "Top", 0, false);
                ReadAttribute(ref margin.Right, inherit, control.Element("OriginMargins"), "Right", 0, false);
                ReadAttribute(ref margin.Bottom, inherit, control.Element("OriginMargins"), "Bottom", 0, false);
                skinControl.OriginMargins = margin;

                ReadAttribute(ref margin.Left, inherit, control.Element("ClientMargins"), "Left", 0, false);
                ReadAttribute(ref margin.Top, inherit, control.Element("ClientMargins"), "Top", 0, false);
                ReadAttribute(ref margin.Right, inherit, control.Element("ClientMargins"), "Right", 0, false);
                ReadAttribute(ref margin.Bottom, inherit, control.Element("ClientMargins"), "Bottom", 0, false);
                skinControl.ClientMargins = margin;

                int resizerSize = 0;
                ReadAttribute(ref resizerSize, inherit, control.Element("ResizerSize"), "Value", 0, false);
                skinControl.ResizerSize = resizerSize;

                // Load control's layers
                if (control.Element("Layers") != null)
                {
                    foreach (var layer in control.Element("Layers").Elements())
                    {
                        if (layer.Name == "Layer")
                        {
                            LoadLayer(skinControl, layer);
                        }
                    }
                }
                Controls.Add(skinControl);
            }
        } // LoadControls

        #endregion

        #region Load Layers

        /// <summary>
        /// Load layers information.
        /// </summary>
        private void LoadLayer(SkinControlInformation skinControl, XElement layerNode)
        {
            string name = ReadAttribute(layerNode, "Name", null, true);
            bool over = ReadAttribute(layerNode, "Override", false, false);
            SkinLayer skinLayer = skinControl.Layers[name];
                
            bool inherent = true;
            if (skinLayer == null)
            {
                skinLayer = new SkinLayer();
                inherent = false;
            }

            if (inherent && over)
            {
                skinLayer = new SkinLayer();
                skinControl.Layers[name] = skinLayer;
            }

            Color color = new Color();
            int integer = 0;
            bool boolean = true;
            Margins margin = new Margins();

            ReadAttribute(ref name, inherent, layerNode, "Name", null, true);
            skinLayer.Name = name;
            ReadAttribute(ref name, inherent, layerNode, "Image", "Control", false);
            skinLayer.Image.Name = name;
            ReadAttribute(ref integer, inherent, layerNode, "Width",  0, false);
            skinLayer.Width = integer;
            ReadAttribute(ref integer, inherent, layerNode, "Height", 0, false);
            skinLayer.Height = integer;

            string layerAlignment = skinLayer.Alignment.ToString();
            ReadAttribute(ref layerAlignment, inherent, layerNode, "Alignment", "MiddleCenter", false);
            skinLayer.Alignment = (Alignment)Enum.Parse(typeof(Alignment), layerAlignment, true);

            ReadAttribute(ref integer, inherent, layerNode, "OffsetX", 0, false);
            skinLayer.OffsetX = integer;
            ReadAttribute(ref integer, inherent, layerNode, "OffsetY", 0, false);
            skinLayer.OffsetY = integer;
            
            ReadAttribute(ref margin.Left, inherent, layerNode.Element("SizingMargins"), "Left", 0, false);
            ReadAttribute(ref margin.Top, inherent, layerNode.Element("SizingMargins"), "Top", 0, false);
            ReadAttribute(ref margin.Right, inherent, layerNode.Element("SizingMargins"), "Right", 0, false);
            ReadAttribute(ref margin.Bottom, inherent, layerNode.Element("SizingMargins"), "Bottom", 0, false);
            skinLayer.SizingMargins = margin;

            ReadAttribute(ref margin.Left, inherent, layerNode.Element("ContentMargins"), "Left", 0, false);
            ReadAttribute(ref margin.Top, inherent, layerNode.Element("ContentMargins"), "Top", 0, false);
            ReadAttribute(ref margin.Right, inherent, layerNode.Element("ContentMargins"), "Right", 0, false);
            ReadAttribute(ref margin.Bottom, inherent, layerNode.Element("ContentMargins"), "Bottom", 0, false);
            skinLayer.ContentMargins = margin;

            #region States

            if (layerNode.Element("States") != null)
            {
                SkinStates<LayerStates> states = new SkinStates<LayerStates>();

                ReadAttribute(ref integer, inherent, layerNode.Element("States").Element("Enabled"), "Index", 0, false);
                states.Enabled.Index = integer;
                int di = skinLayer.States.Enabled.Index;
                ReadAttribute(ref states.Hovered.Index, inherent, layerNode.Element("States").Element("Hovered"), "Index", di, false);
                states.Hovered.Index = integer;
                ReadAttribute(ref states.Pressed.Index, inherent, layerNode.Element("States").Element("Pressed"), "Index", di, false);
                states.Pressed.Index = integer;
                ReadAttribute(ref states.Focused.Index, inherent, layerNode.Element("States").Element("Focused"), "Index", di, false);
                states.Focused.Index = integer;
                ReadAttribute(ref states.Disabled.Index, inherent, layerNode.Element("States").Element("Disabled"), "Index", di, false);
                states.Disabled.Index = integer;
                
                ReadAttribute(ref color, inherent, layerNode.Element("States").Element("Enabled"), "Color", Color.White, false);
                states.Enabled.Color = color;
                Color dc = skinLayer.States.Enabled.Color;
                ReadAttribute(ref color, inherent, layerNode.Element("States").Element("Hovered"), "Color", dc, false);
                states.Hovered.Color = color;
                ReadAttribute(ref color, inherent, layerNode.Element("States").Element("Pressed"), "Color", dc, false);
                states.Pressed.Color = color;
                ReadAttribute(ref color, inherent, layerNode.Element("States").Element("Focused"), "Color", dc, false);
                states.Focused.Color = color;
                ReadAttribute(ref color, inherent, layerNode.Element("States").Element("Disabled"), "Color", dc, false);
                states.Disabled.Color = color;
                
                ReadAttribute(ref boolean, inherent, layerNode.Element("States").Element("Enabled"), "Overlay", false, false);
                states.Enabled.Overlay = boolean;
                bool dv = skinLayer.States.Enabled.Overlay;
                ReadAttribute(ref boolean, inherent, layerNode.Element("States").Element("Hovered"), "Overlay", dv, false);
                states.Hovered.Overlay = boolean;
                ReadAttribute(ref boolean, inherent, layerNode.Element("States").Element("Pressed"), "Overlay", dv, false);
                states.Pressed.Overlay = boolean;
                ReadAttribute(ref boolean, inherent, layerNode.Element("States").Element("Focused"), "Overlay", dv, false);
                states.Focused.Overlay = boolean;
                ReadAttribute(ref boolean, inherent, layerNode.Element("States").Element("Disabled"), "Overlay", dv, false);
                states.Disabled.Overlay = boolean;

                skinLayer.States = states;
            }

            #endregion

            #region Overlays

            if (layerNode.Element("Overlays") != null)
            {
                SkinStates<LayerOverlays> overlay = new SkinStates<LayerOverlays>();

                ReadAttribute(ref integer, inherent, layerNode.Element("Overlays").Element("Enabled"), "Index", 0, false);
                overlay.Enabled.Index = integer;
                int di = skinLayer.Overlays.Enabled.Index;
                ReadAttribute(ref overlay.Hovered.Index, inherent, layerNode.Element("Overlays").Element("Hovered"), "Index", di, false);
                overlay.Hovered.Index = integer;
                ReadAttribute(ref overlay.Pressed.Index, inherent, layerNode.Element("Overlays").Element("Pressed"), "Index", di, false);
                overlay.Pressed.Index = integer;
                ReadAttribute(ref overlay.Focused.Index, inherent, layerNode.Element("Overlays").Element("Focused"), "Index", di, false);
                overlay.Focused.Index = integer;
                ReadAttribute(ref overlay.Disabled.Index, inherent, layerNode.Element("Overlays").Element("Disabled"), "Index", di, false);
                overlay.Disabled.Index = integer;

                ReadAttribute(ref overlay.Enabled.Color, inherent, layerNode.Element("Overlays").Element("Enabled"), "Color", Color.White, false);
                overlay.Enabled.Color = color;
                Color dc = skinLayer.Overlays.Enabled.Color;
                ReadAttribute(ref overlay.Hovered.Color, inherent, layerNode.Element("Overlays").Element("Hovered"), "Color", dc, false);
                overlay.Hovered.Color = color;
                ReadAttribute(ref overlay.Pressed.Color, inherent, layerNode.Element("Overlays").Element("Pressed"), "Color", dc, false);
                overlay.Pressed.Color = color;
                ReadAttribute(ref overlay.Focused.Color, inherent, layerNode.Element("Overlays").Element("Focused"), "Color", dc, false);
                overlay.Focused.Color = color;
                ReadAttribute(ref overlay.Disabled.Color, inherent, layerNode.Element("Overlays").Element("Disabled"), "Color", dc, false);
                overlay.Disabled.Color = color;

                skinLayer.Overlays = overlay;
            }

            #endregion

            #region Text

            if (layerNode.Element("Text") != null)
            {
                SkinText skinText = new SkinText();

                ReadAttribute(ref name, inherent, layerNode.Element("Text"), "Font", null, true);
                skinText.Name = name;
                ReadAttribute(ref integer, inherent, layerNode.Element("Text"), "OffsetX", 0, false);
                skinText.OffsetX = integer;
                ReadAttribute(ref integer, inherent, layerNode.Element("Text"), "OffsetY", 0, false);
                skinText.OffsetY = integer;

                layerAlignment = skinLayer.Text.Alignment.ToString();
                ReadAttribute(ref layerAlignment, inherent, layerNode.Element("Text"), "Alignment", "MiddleCenter", false);
                skinLayer.Text.Alignment = (Alignment)Enum.Parse(typeof(Alignment), layerAlignment, true);

                SkinStates<Color> colors = new SkinStates<Color>();
                LoadColors(inherent, layerNode.Element("Text"), ref colors);
                skinLayer.Text.Colors = colors;

                skinLayer.Text = skinText;
            }

            #endregion

            #region Attributes

            if (layerNode.Element("Attributes") != null)
            {
                foreach (var attribute in layerNode.Element("Attributes").Elements())
                {
                    if (attribute.Name == "Attribute")
                    {
                        LoadLayerAttribute(skinLayer, attribute);
                    }
                }
            }

            #endregion

            if (!inherent)
                skinControl.Layers.Add(skinLayer);
        } // LoadLayer

        #region Load Colors

        private void LoadColors(bool inherited, XElement e, ref SkinStates<Color> colors)
        {
            if (e != null)
            {
                ReadAttribute(ref colors.Enabled,  inherited, e.Element("Colors").Element("Enabled"),  "Color", Color.White,    false);
                ReadAttribute(ref colors.Hovered,  inherited, e.Element("Colors").Element("Hovered"),  "Color", colors.Enabled, false);
                ReadAttribute(ref colors.Pressed,  inherited, e.Element("Colors").Element("Pressed"),  "Color", colors.Enabled, false);
                ReadAttribute(ref colors.Focused,  inherited, e.Element("Colors").Element("Focused"),  "Color", colors.Enabled, false);
                ReadAttribute(ref colors.Disabled, inherited, e.Element("Colors").Element("Disabled"), "Color", colors.Enabled, false);
            }
        } // LoadColors

        #endregion

        #region Load Layer Attributes

        /// <summary>
        /// Load Layer Attributes
        /// </summary>
        private void LoadLayerAttribute(SkinLayer skinLayer, XElement e)
        {
            string name = ReadAttribute(e, "Name", null, true);
            SkinAttribute skinAttribute = skinLayer.Attributes[name];
            bool inherent = true;

            if (skinAttribute == null)
            {
                skinAttribute = new SkinAttribute();
                inherent = false;
            }

            skinAttribute.Name = name;
            ReadAttribute(ref name, inherent, e, "Value", null, true);
            skinAttribute.Value = name;

            if (!inherent) 
                skinLayer.Attributes.Add(skinAttribute);

        } // LoadLayerAttribute

        #endregion

        #endregion

        #region Load Fonts

        /// <summary>
        /// Load fonts information.
        /// </summary>
        private void LoadFontsDescription()
        {
            if (skinDescription.Resource.Element("Skin").Element("Fonts") == null)
                return;

            foreach (var font in skinDescription.Resource.Element("Skin").Element("Fonts").Elements())
            {
                SkinFont skinFont = new SkinFont
                {
                    Name = ReadAttribute(font, "Name", null, true),
                    Filename = ReadAttribute(font, "Asset", null, true)
                };
                Fonts.Add(skinFont);
            }
        } // LoadFonts

        #endregion

        #region Load Cursors

        #if (WINDOWS)
            /// <summary>
            /// Load cursors information.
            /// </summary>
            private void LoadCursorsDescription()
            {
                if (skinDescription.Resource.Element("Skin").Element("Cursors") == null)
                    return;

                foreach (var cursor in skinDescription.Resource.Element("Skin").Element("Cursors").Elements())
                {
                    SkinCursor skinCursor = new SkinCursor
                    {
                        Name = ReadAttribute(cursor, "Name", null, true),
                        Filename = ReadAttribute(cursor, "Asset", null, true)
                    };
                    Cursors.Add(skinCursor);
                }
            } // LoadCursors
        #endif

        #endregion

        #region Load Images

        /// <summary>
        /// Load images information.
        /// </summary>
        private void LoadImagesDescription()
        {
            if (skinDescription.Resource.Element("Skin").Element("Images") == null)
                return;

            foreach (var image in skinDescription.Resource.Element("Skin").Element("Images").Elements())
            {
                SkinImage skinImage = new SkinImage
                {
                    Name = ReadAttribute(image, "Name", null, true),
                    Filename = ReadAttribute(image, "Asset", null, true)
                };
                Images.Add(skinImage);
            }
        } // LoadImages

        #endregion

        #region Read Attribute

        private string ReadAttribute(XElement element, string attributeName, string defval, bool needed)
        {
            if (element != null && element.Attribute(attributeName) != null)
            {
                return element.Attribute(attributeName).Value;
            }
            if (needed)
            {
                throw new Exception("Missing required attribute \"" + attributeName + "\" in the skin file.");
            }
            return defval;
        } // ReadAttribute

        private void ReadAttribute(ref string retval, bool inherited, XElement element, string attributeName, string defaultValue, bool needed)
        {
            if (element != null && element.Attribute(attributeName) != null)
            {
                retval = element.Attribute(attributeName).Value;
            }
            else if (inherited)
            {
                // Do nothing, the parent has the attribute.
            }
            else if (needed)
            {
                throw new Exception("Missing required attribute \"" + attributeName + "\" in the skin file.");
            }
            else
            {
                retval = defaultValue;
            }
        } // ReadAttribute

        private void ReadAttribute(ref int retval, bool inherited, XElement element, string attrib, int defval, bool needed)
        {
            string tmp = retval.ToString();
            ReadAttribute(ref tmp, inherited, element, attrib, defval.ToString(), needed);
            retval = int.Parse(tmp);
        } // ReadAttributeInt

        private bool ReadAttribute(XElement element, string attrib, bool defval, bool needed)
        {
            return bool.Parse(ReadAttribute(element, attrib, defval.ToString(), needed));
        } // ReadAttributeBool

        private void ReadAttribute(ref bool retval, bool inherited, XElement element, string attrib, bool defval, bool needed)
        {
            string tmp = retval.ToString();
            ReadAttribute(ref tmp, inherited, element, attrib, defval.ToString(), needed);
            retval = bool.Parse(tmp);
        } // ReadAttributeBool

        private string ColorToString(Color c)
        {
            return string.Format("{0};{1};{2};{3}", c.R, c.G, c.B, c.A);
        } // ColorToString

        private void ReadAttribute(ref Color retval, bool inherited, XElement element, string attrib, Color defval, bool needed)
        {
            string tmp = ColorToString(retval);
            ReadAttribute(ref tmp, inherited, element, attrib, ColorToString(defval), needed);
            retval = Utilities.ParseColor(tmp);
        } // ReadAttributeColor

        #endregion

    } // Skin

    #endregion

} // XNAFinalEngine.UserInterface