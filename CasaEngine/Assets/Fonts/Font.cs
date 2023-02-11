﻿using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using System.Xml;
using CasaEngineCommon.Design;
using CasaEngineCommon.Extension;
using CasaEngine.Gameplay.Actor.Object;


#if EDITOR
using System.ComponentModel;
using CasaEngine.Editor.Assets;
#endif

namespace CasaEngine.Asset.Fonts
{
    public
#if EDITOR
    partial
#endif  
    class Font
        : BaseObject
#if EDITOR
        , INotifyPropertyChanged, IAssetable
#endif
    {
        readonly Dictionary<char, FontChar> _charsDic;
        readonly List<string> _texturesFileNames;


        public GraphicsDevice GraphicsDevice { get; private set; }

        public Texture[] Textures
        {
            get;
            internal set;
        }

        public bool UseKerning
        {
            get;
            set;
        }

        public FontInfo Info
        {
            get;
            set;
        }

        public FontCommon Common
        {
            get;
            set;
        }

        public List<FontPage> Pages
        {
            get;
            set;
        }

        public List<FontChar> Chars
        {
            get;
            set;
        }

        public List<FontKerning> Kernings
        {
            get;
            set;
        }

        //public int LineSpacing { get; set; }

        //public float Spacing { get; set; }

        public int LineSpacing
        {
            get => Common.LineHeight;
            set => Common.LineHeight = value;
        } // LineSpacing

        public int Spacing
        {
            get => Info.Spacing.X;
            set => Info.Spacing.X = value;
        } // Spacing

        public char? DefaultCharacter => '¤'; //Resource.DefaultCharacter; }
                                              //set { Resource.DefaultCharacter = value; }
                                              // DefaultCharacter



        private Font()
        {
            _charsDic = new Dictionary<char, FontChar>();
            _texturesFileNames = new List<string>();
            Pages = new List<FontPage>();
            Chars = new List<FontChar>();
            Kernings = new List<FontKerning>();
        }

        public Font(XmlElement node, SaveOption option)
            : this()
        {
            Load(node, option);
        }



        public Vector2 MeasureString(StringBuilder str)
        {
            return MeasureString(str.ToString());
        }

        public Vector2 MeasureString(string text)
        {
            float width = 0.0f;

            foreach (char c in text.ToCharArray())
            {
                if (_charsDic.ContainsKey(c) == true)
                {
                    width += _charsDic[c].Width;
                }
                else
                {
                    width += Info.Spacing.X;
                }
            }

            return new Vector2(width, Common.LineHeight);
        }

        public void LoadTexture(string path, GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;

            if (Textures == null)
            {
                Textures = new Texture[_texturesFileNames.Count];
            }

            int i = 0;

            foreach (string texFileName in _texturesFileNames)
            {
                //string fileName = path_ + Path.DirectorySeparatorChar + ProjectManager.AssetDirPath + Path.DirectorySeparatorChar + texFileName;
                //fileName = fileName.Replace(Engine.Instance.AssetContentManager.RootDirectory + Path.DirectorySeparatorChar, "");
                Textures[i] = new Texture(graphicsDevice, texFileName); //fileName);
                i++;
            }

            /*
            string assetFile;

#if EDITOR
            assetFile = Engine.Instance.ProjectManager.ProjectPath + System.IO.Path.DirectorySeparatorChar +
                ProjectManager.AssetDirPath + System.IO.Path.DirectorySeparatorChar + _AssetFileName;
#else
            assetFile = Engine.Instance.Game.Content.RootDirectory + System.IO.Path.DirectorySeparatorChar + _AssetFileName;
#endif

            if (_Texture2D != null
                && _Texture2D.IsDisposed == false
                && _Texture2D.GraphicsDevice.IsDisposed == false)
            {
                return;
            }

            _Texture2D = Texture2D.FromStream(device_, File.OpenRead(assetFile));
            */
        }



        public override void Load(XmlElement el, SaveOption opt)
        {
            base.Load(el, opt);

            Common = new FontCommon(el.SelectSingleNode("Font/Common"));

            foreach (XmlNode n in el.SelectNodes("Font/Pages/Page"))
            {
                _texturesFileNames.Add(n.Attributes["file"].Value);
            }

            foreach (XmlNode n in el.SelectNodes("Font/Chars/Char"))
            {
                FontChar f = new FontChar(n);
                Chars.Add(f);
                _charsDic.Add((char)f.Id, f);
            }

            foreach (XmlNode n in el.SelectNodes("Font/Kernings/Kerning"))
            {
                Kernings.Add(new FontKerning(n));
            }
        }

        public override void Load(BinaryReader br, SaveOption option)
        {
            throw new Exception("The method or operation is not implemented.");
        }


        internal FontChar GetFontChar(char c)
        {
            if (_charsDic.ContainsKey(c) == true)
            {
                return _charsDic[c];
            }

            return null;
        }

        protected override void CopyFrom(BaseObject ob)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override BaseObject Clone()
        {
            throw new Exception("The method or operation is not implemented.");
        }


    }

    public class FontInfo
    {
        public string Face
        {
            get;
            set;
        }

        public int Size
        {
            get;
            set;
        }

        public int Bold
        {
            get;
            set;
        }

        public int Italic
        {
            get;
            set;
        }

        public string CharSet
        {
            get;
            set;
        }

        public int Unicode
        {
            get;
            set;
        }

        public int StretchHeight
        {
            get;
            set;
        }

        public int Smooth
        {
            get;
            set;
        }

        //"aa"
        public int SuperSampling
        {
            get;
            set;
        }

        public Rectangle Padding
        {
            get;
            set;
        }

        /*[XmlAttribute("padding")]
        public String Padding
        {
            get
            {
                return _Padding.X + "," + _Padding.Y + "," + _Padding.Width + "," + _Padding.Height;
            }
            set
            {
                String[] padding = value.Split(',');
                _Padding = new Rectangle(Convert.ToInt32(padding[0]), Convert.ToInt32(padding[1]), Convert.ToInt32(padding[2]), Convert.ToInt32(padding[3]));
            }
        }*/

        public Point Spacing;
        /*[XmlAttribute("spacing")]
        public String Spacing
        {
            get
            {
                return _Spacing.X + "," + _Spacing.Y;
            }
            set
            {
                String[] spacing = value.Split(',');
                _Spacing = new Point(Convert.ToInt32(spacing[0]), Convert.ToInt32(spacing[1]));
            }
        }*/

        [XmlAttribute("outline")]
        public int OutLine
        {
            get;
            set;
        }

        public FontInfo(XmlNode node)
        {

        }

        public void Load(XmlNode node)
        {
            //face="Arial" size="32" bold="0" italic="0" charset="" unicode="1" stretchH="100" smooth="1" aa="1" padding="0,0,0,0" spacing="1,1" outline="0"

            string[] padding = node.Attributes["padding"].Value.Split(',');
            Padding = new Rectangle(Convert.ToInt32(padding[0]), Convert.ToInt32(padding[1]), Convert.ToInt32(padding[2]), Convert.ToInt32(padding[3]));

            string[] spacing = node.Attributes["spacing"].Value.Split(',');
            Spacing = new Point(Convert.ToInt32(spacing[0]), Convert.ToInt32(spacing[1]));
        }
    }

    public class FontCommon
    {
        [XmlAttribute("lineHeight")]
        public int LineHeight
        {
            get;
            set;
        }

        [XmlAttribute("base")]
        public int Base
        {
            get;
            set;
        }

        [XmlAttribute("scaleW")]
        public int ScaleW
        {
            get;
            set;
        }

        [XmlAttribute("scaleH")]
        public int ScaleH
        {
            get;
            set;
        }

        [XmlAttribute("pages")]
        public int Pages
        {
            get;
            set;
        }

        [XmlAttribute("packed")]
        public int Packed
        {
            get;
            set;
        }

        [XmlAttribute("alphaChnl")]
        public int AlphaChannel
        {
            get;
            set;
        }

        [XmlAttribute("redChnl")]
        public int RedChannel
        {
            get;
            set;
        }

        [XmlAttribute("greenChnl")]
        public int GreenChannel
        {
            get;
            set;
        }

        [XmlAttribute("blueChnl")]
        public int BlueChannel
        {
            get;
            set;
        }


        public FontCommon(XmlNode node)
        {
            Load(node);
        }

        public void Load(XmlNode node)
        {
            // lineHeight="32" base="26" scaleW="512" scaleH="512" pages="1" packed="0" alphaChnl="0" redChnl="4" greenChnl="4" blueChnl="4"

            LineHeight = int.Parse(node.Attributes["lineHeight"].Value);
            Base = int.Parse(node.Attributes["base"].Value);
        }

        public void Save(XmlNode node, SaveOption option)
        {
            XmlNode fontNode = node.OwnerDocument.CreateElement("Common");
            node.AppendChild(fontNode);

            fontNode.OwnerDocument.AddAttribute((XmlElement)fontNode, "lineHeight", LineHeight.ToString());
            fontNode.OwnerDocument.AddAttribute((XmlElement)fontNode, "base", Base.ToString());
        }
    }

    public class FontPage
    {
        [XmlAttribute("id")]
        public int Id
        {
            get;
            set;
        }

        [XmlAttribute("file")]
        public string File
        {
            get;
            set;
        }
    }

    public class FontChar
    {
        [XmlAttribute("id")]
        public int Id
        {
            get;
            set;
        }

        public char Char
        {
            get;
            set;
        }

        [XmlAttribute("x")]
        public int X
        {
            get;
            set;
        }

        [XmlAttribute("y")]
        public int Y
        {
            get;
            set;
        }

        [XmlAttribute("width")]
        public int Width
        {
            get;
            set;
        }

        [XmlAttribute("height")]
        public int Height
        {
            get;
            set;
        }

        [XmlAttribute("xoffset")]
        public int XOffset
        {
            get;
            set;
        }

        public int YOffset
        {
            get;
            set;
        }

        public int XAdvance
        {
            get;
            set;
        }

        public int Page
        {
            get;
            set;
        }

        public int Channel
        {
            get;
            set;
        }

        public FontChar(XmlNode node)
        {
            Load(node);
        }

        public void Load(XmlNode node)
        {
            Id = int.Parse(node.Attributes["id"].Value);
            Char = (char)Id;

            X = int.Parse(node.Attributes["x"].Value);
            Y = int.Parse(node.Attributes["y"].Value);
            Width = int.Parse(node.Attributes["width"].Value);
            Height = int.Parse(node.Attributes["height"].Value);
            XOffset = int.Parse(node.Attributes["xoffset"].Value);
            YOffset = int.Parse(node.Attributes["yoffset"].Value);
            XAdvance = int.Parse(node.Attributes["xadvance"].Value);
        }

        public void Save(XmlNode node, SaveOption option)
        {
            XmlNode charNode = node.OwnerDocument.CreateElement("Char");
            node.AppendChild(charNode);

            charNode.OwnerDocument.AddAttribute((XmlElement)charNode, "id", Id.ToString());
            charNode.OwnerDocument.AddAttribute((XmlElement)charNode, "x", X.ToString());
            charNode.OwnerDocument.AddAttribute((XmlElement)charNode, "y", Y.ToString());
            charNode.OwnerDocument.AddAttribute((XmlElement)charNode, "width", Width.ToString());
            charNode.OwnerDocument.AddAttribute((XmlElement)charNode, "height", Height.ToString());
            charNode.OwnerDocument.AddAttribute((XmlElement)charNode, "xoffset", XOffset.ToString());
            charNode.OwnerDocument.AddAttribute((XmlElement)charNode, "yoffset", YOffset.ToString());
            charNode.OwnerDocument.AddAttribute((XmlElement)charNode, "xadvance", XAdvance.ToString());
        }
    }

    public class FontKerning
    {
        public int First
        {
            get;
            set;
        }

        public int Second
        {
            get;
            set;
        }

        public int Amount
        {
            get;
            set;
        }

        public FontKerning(XmlNode node)
        {

        }

        public void Load(XmlNode node)
        {
            First = int.Parse(node.Attributes["first"].Value);
            Second = int.Parse(node.Attributes["second"].Value);
            Amount = int.Parse(node.Attributes["amount"].Value);
        }

        public void Save(XmlNode node, SaveOption option)
        {
            XmlNode kerningNode = node.OwnerDocument.CreateElement("Kerning");
            node.AppendChild(kerningNode);

            kerningNode.OwnerDocument.AddAttribute((XmlElement)kerningNode, "first", First.ToString());
            kerningNode.OwnerDocument.AddAttribute((XmlElement)kerningNode, "second", Second.ToString());
            kerningNode.OwnerDocument.AddAttribute((XmlElement)kerningNode, "amount", Amount.ToString());
        }
    }
}