﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using System.Xml;
using CasaEngine.Game;
using CasaEngineCommon.Design;
using CasaEngineCommon.Extension;
using CasaEngine.Gameplay.Actor.Object;
using CasaEngine.Project;

#if EDITOR
using System.ComponentModel;
using CasaEngine.Editor.Assets;
#endif

namespace CasaEngine.Asset.Fonts
{
    /// <summary>
    /// 
    /// </summary>
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
        Dictionary<char, FontChar> m_CharsDic;
        List<string> m_TexturesFileNames;

        #region Properties

        /// <summary>
        /// Gets
        /// </summary>
        public GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Texture[] Textures
        { 
            get;
            internal set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool UseKerning
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FontInfo Info
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FontCommon Common
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<FontPage> Pages
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<FontChar> Chars
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<FontKerning> Kernings
        {
            get;
            set;
        }  
        
        /// <summary>
        /// Gets or sets the vertical distance (in pixels) between the base lines of
        /// two consecutive lines of text. Line spacing includes the blank space between
        /// lines as well as the height of the characters.
        /// </summary>
        //public int LineSpacing { get; set; }

        /// <summary>
        /// Gets or sets the spacing of the font characters
        /// </summary>
        //public float Spacing { get; set; }

        /// <summary>
        /// Gets or sets the vertical distance (in pixels) between the base lines of two consecutive lines of text.
        /// Line spacing includes the blank space between lines as well as the height of the characters.
        /// </summary>
        public int LineSpacing
        {
            get { return Common.LineHeight; }
            set { Common.LineHeight = value; }
        } // LineSpacing

        /// <summary>
        /// Gets or sets the spacing of the font characters.
        /// </summary>
        public int Spacing
        {
            get { return Info.Spacing.X; }
            set { Info.Spacing.X = value; }
        } // Spacing

        /// <summary>
        /// Gets or sets the default character for the font.
        /// </summary>
        public char? DefaultCharacter
        {
            get { return '¤'; }//Resource.DefaultCharacter; }
            //set { Resource.DefaultCharacter = value; }
        } // DefaultCharacter

        #endregion

        #region Constructors

        private Font()
        {
            m_CharsDic = new Dictionary<char, FontChar>();
            m_TexturesFileNames = new List<string>();
            Pages = new List<FontPage>();
            Chars = new List<FontChar>();
            Kernings = new List<FontKerning>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        /// <param name="option_"></param>
        public Font(XmlElement node_, SaveOption option_)
            : this()
        {
            Load(node_, option_);
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str_"></param>
        /// <returns></returns>
        public Vector2 MeasureString(StringBuilder str_)
        {
            return MeasureString(str_.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public Vector2 MeasureString(string text)
        {
            float width = 0.0f;

            foreach (char c in text.ToCharArray())
            {
                if (m_CharsDic.ContainsKey(c) == true)
                {
                    width += m_CharsDic[c].Width;
                }
                else
                {
                    width += Info.Spacing.X;
                }
            }

            return new Vector2(width, Common.LineHeight);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphicsDevice_"></param>
        public void LoadTexture(string path_, GraphicsDevice graphicsDevice_)
        {
            GraphicsDevice = graphicsDevice_;

            if (Textures == null)
            {
                Textures = new Texture[m_TexturesFileNames.Count];
            }

            int i = 0;

            foreach (string texFileName in m_TexturesFileNames)
            {
                //string fileName = path_ + Path.DirectorySeparatorChar + ProjectManager.AssetDirPath + Path.DirectorySeparatorChar + texFileName;
                //fileName = fileName.Replace(Engine.Instance.AssetContentManager.RootDirectory + Path.DirectorySeparatorChar, "");
                Textures[i] = new Texture(graphicsDevice_, texFileName); //fileName);
                i++;
            }

            /*
            string assetFile;

#if EDITOR
            assetFile = Engine.Instance.ProjectManager.ProjectPath + System.IO.Path.DirectorySeparatorChar +
                ProjectManager.AssetDirPath + System.IO.Path.DirectorySeparatorChar + m_AssetFileName;
#else
            assetFile = Engine.Instance.Game.Content.RootDirectory + System.IO.Path.DirectorySeparatorChar + m_AssetFileName;
#endif

            if (m_Texture2D != null
                && m_Texture2D.IsDisposed == false
                && m_Texture2D.GraphicsDevice.IsDisposed == false)
            {
                return;
            }

            m_Texture2D = Texture2D.FromStream(device_, File.OpenRead(assetFile));
            */
        }

        #region BaseObject

        #region Save/Load

        /// <summary>
        /// 
        /// </summary>
        /// <param name="el_"></param>
        /// <param name="opt_"></param>
        public override void Load(XmlElement el_, SaveOption opt_)
        {
            base.Load(el_, opt_);

            Common = new FontCommon(el_.SelectSingleNode("Font/Common"));

            foreach (XmlNode n in el_.SelectNodes("Font/Pages/Page"))
            {
                m_TexturesFileNames.Add(n.Attributes["file"].Value);
            }

            foreach (XmlNode n in el_.SelectNodes("Font/Chars/Char"))
            {
                FontChar f = new FontChar(n);
                Chars.Add(f);
                m_CharsDic.Add((char)f.ID, f);
            }

            foreach (XmlNode n in el_.SelectNodes("Font/Kernings/Kerning"))
            {
                Kernings.Add(new FontKerning(n));
            }
        }

        public override void Load(BinaryReader br_, SaveOption option_)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        internal FontChar GetFontChar(char c)
        {
            if (m_CharsDic.ContainsKey(c) == true)
            {
                return m_CharsDic[c];
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob_"></param>
        protected override void CopyFrom(BaseObject ob_)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override BaseObject Clone()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class FontInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public String Face
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 Size
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 Bold
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 Italic
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public String CharSet
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 Unicode
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 StretchHeight
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 Smooth
        {
            get;
            set;
        }

        //"aa"
        /// <summary>
        /// 
        /// </summary>
        public Int32 SuperSampling
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
        public Int32 OutLine
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public FontInfo(XmlNode node_)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public void Load(XmlNode node_)
        {
            //face="Arial" size="32" bold="0" italic="0" charset="" unicode="1" stretchH="100" smooth="1" aa="1" padding="0,0,0,0" spacing="1,1" outline="0"

            String[] padding = node_.Attributes["padding"].Value.Split(',');
            Padding = new Rectangle(Convert.ToInt32(padding[0]), Convert.ToInt32(padding[1]), Convert.ToInt32(padding[2]), Convert.ToInt32(padding[3]));

            String[] spacing = node_.Attributes["spacing"].Value.Split(',');
            Spacing = new Point(Convert.ToInt32(spacing[0]), Convert.ToInt32(spacing[1]));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FontCommon
    {
        [XmlAttribute("lineHeight")]
        public Int32 LineHeight
        {
            get;
            set;
        }

        [XmlAttribute("base")]
        public Int32 Base
        {
            get;
            set;
        }

        [XmlAttribute("scaleW")]
        public Int32 ScaleW
        {
            get;
            set;
        }

        [XmlAttribute("scaleH")]
        public Int32 ScaleH
        {
            get;
            set;
        }

        [XmlAttribute("pages")]
        public Int32 Pages
        {
            get;
            set;
        }

        [XmlAttribute("packed")]
        public Int32 Packed
        {
            get;
            set;
        }

        [XmlAttribute("alphaChnl")]
        public Int32 AlphaChannel
        {
            get;
            set;
        }

        [XmlAttribute("redChnl")]
        public Int32 RedChannel
        {
            get;
            set;
        }

        [XmlAttribute("greenChnl")]
        public Int32 GreenChannel
        {
            get;
            set;
        }

        [XmlAttribute("blueChnl")]
        public Int32 BlueChannel
        {
            get;
            set;
        }


        public FontCommon(XmlNode node_)
        {
            Load(node_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public void Load(XmlNode node_)
        {
            // lineHeight="32" base="26" scaleW="512" scaleH="512" pages="1" packed="0" alphaChnl="0" redChnl="4" greenChnl="4" blueChnl="4"

            LineHeight = int.Parse(node_.Attributes["lineHeight"].Value);
            Base = int.Parse(node_.Attributes["base"].Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        /// <param name="option_"></param>
        public void Save(XmlNode node_, SaveOption option_)
        {
            XmlNode fontNode = node_.OwnerDocument.CreateElement("Common");
            node_.AppendChild(fontNode);

            fontNode.OwnerDocument.AddAttribute((XmlElement)fontNode, "lineHeight", LineHeight.ToString());
            fontNode.OwnerDocument.AddAttribute((XmlElement)fontNode, "base", Base.ToString());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FontPage
    {
        [XmlAttribute("id")]
        public Int32 ID
        {
            get;
            set;
        }

        [XmlAttribute("file")]
        public String File
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FontChar
    {
        [XmlAttribute("id")]
        public Int32 ID
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
        public Int32 X
        {
            get;
            set;
        }

        [XmlAttribute("y")]
        public Int32 Y
        {
            get;
            set;
        }

        [XmlAttribute("width")]
        public Int32 Width
        {
            get;
            set;
        }

        [XmlAttribute("height")]
        public Int32 Height
        {
            get;
            set;
        }

        [XmlAttribute("xoffset")]
        public Int32 XOffset
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 YOffset
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 XAdvance
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 Page
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 Channel
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public FontChar(XmlNode node_)
        {
            Load(node_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public void Load(XmlNode node_)
        {
            ID = int.Parse(node_.Attributes["id"].Value);
            Char = (char)ID;

            X = int.Parse(node_.Attributes["x"].Value);
            Y = int.Parse(node_.Attributes["y"].Value);
            Width = int.Parse(node_.Attributes["width"].Value);
            Height = int.Parse(node_.Attributes["height"].Value);
            XOffset = int.Parse(node_.Attributes["xoffset"].Value);
            YOffset = int.Parse(node_.Attributes["yoffset"].Value);
            XAdvance = int.Parse(node_.Attributes["xadvance"].Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        /// <param name="option_"></param>
        public void Save(XmlNode node_, SaveOption option_)
        {
            XmlNode charNode = node_.OwnerDocument.CreateElement("Char");
            node_.AppendChild(charNode);

            charNode.OwnerDocument.AddAttribute((XmlElement)charNode, "id", ID.ToString());
            charNode.OwnerDocument.AddAttribute((XmlElement)charNode, "x", X.ToString());
            charNode.OwnerDocument.AddAttribute((XmlElement)charNode, "y", Y.ToString());
            charNode.OwnerDocument.AddAttribute((XmlElement)charNode, "width", Width.ToString());
            charNode.OwnerDocument.AddAttribute((XmlElement)charNode, "height", Height.ToString());
            charNode.OwnerDocument.AddAttribute((XmlElement)charNode, "xoffset", XOffset.ToString());
            charNode.OwnerDocument.AddAttribute((XmlElement)charNode, "yoffset", YOffset.ToString());
            charNode.OwnerDocument.AddAttribute((XmlElement)charNode, "xadvance", XAdvance.ToString());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FontKerning
    {
        /// <summary>
        /// 
        /// </summary>
        public Int32 First
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 Second
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 Amount
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public FontKerning(XmlNode node_)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public void Load(XmlNode node_)
        {
            First = int.Parse(node_.Attributes["first"].Value);
            Second = int.Parse(node_.Attributes["second"].Value);
            Amount = int.Parse(node_.Attributes["amount"].Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        /// <param name="option_"></param>
        public void Save(XmlNode node_, SaveOption option_)
        {
            XmlNode kerningNode = node_.OwnerDocument.CreateElement("Kerning");
            node_.AppendChild(kerningNode);

            kerningNode.OwnerDocument.AddAttribute((XmlElement)kerningNode, "first", First.ToString());
            kerningNode.OwnerDocument.AddAttribute((XmlElement)kerningNode, "second", Second.ToString());
            kerningNode.OwnerDocument.AddAttribute((XmlElement)kerningNode, "amount", Amount.ToString());
        }
    }
}