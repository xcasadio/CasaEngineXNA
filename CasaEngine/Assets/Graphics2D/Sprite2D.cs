﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CasaEngineCommon.Extension;
using Microsoft.Xna.Framework.Content;
using System.Xml;
using CasaEngine.Math.Shape2D;
using CasaEngineCommon.Design;
using CasaEngine.Game;
using CasaEngine.Gameplay.Actor.Object;



#if EDITOR
using System.ComponentModel;
using CasaEngine.Editor.Assets;
using CasaEngine.Project;
#endif

namespace CasaEngine.Assets.Graphics2D
{
    public
#if EDITOR
    partial
#endif
    class Sprite2D
        : BaseObject
#if EDITOR
         , INotifyPropertyChanged, IAssetable
#endif
    {

        //constant
        private Texture2D m_Texture2D = null;
        private Rectangle m_PositionInTexture = new Rectangle();
        private Point m_Origin = Point.Zero;

#if EDITOR
        private readonly List<Shape2DObject> m_Collisions = new List<Shape2DObject>();
#else
        private Shape2DObject[] m_Collisions;
#endif

        private readonly Dictionary<string, Vector2> m_Sockets = new Dictionary<string, Vector2>();

        private readonly List<string> m_AssetFileNames = new List<string>();



#if EDITOR
        [Browsable(false)]
#endif
        public Texture2D Texture2D
        {
            get { return m_Texture2D; }
            internal set
            {
                m_Texture2D = value;

#if EDITOR                
                if (m_Texture2D != null)
                {
                    PositionInTexture = new Rectangle(0, 0, m_Texture2D.Width, m_Texture2D.Height);
                }
#endif
            }
        }

#if EDITOR
        [Browsable(false)]
#endif
        public Rectangle PositionInTexture
        {
            get { return m_PositionInTexture; }
            set
            {
                m_PositionInTexture = value;
#if EDITOR
                NotifyPropertyChanged("PositionInTexture");
#endif
            }
        }

#if EDITOR
        [Category("Sprite")]
#endif
        public Point HotSpot
        {
            get { return m_Origin; }
            set
            {
                //if (value != this.m_Origin)
                {
                    this.m_Origin = value;
#if EDITOR
                    NotifyPropertyChanged("HotSpot");
#endif
                }
            }
        }

#if EDITOR
        [Browsable(false)]
#endif
        public Shape2DObject[] Collisions
        {
            get
            {
#if EDITOR
                return m_Collisions.ToArray();
#else
                return m_Collisions;
#endif
            }
        }



        internal Sprite2D() { }

        public Sprite2D(Texture2D tex_)
        {
            Texture2D = tex_;
        }

        public Sprite2D(XmlElement node_, SaveOption option_)
        {
            Load(node_, option_);
        }

        public Sprite2D(Sprite2D sprite_)
        {
            CopyFrom(sprite_);
        }



        public override BaseObject Clone()
        {
            return new Sprite2D(this);
        }

#if EDITOR
        public
#else
		internal
#endif
        void CopyFrom(Sprite2D sprite_)
        {
            if (sprite_ == null)
            {
                throw new ArgumentNullException("Sprite2D.Copy() : Sprite2D is null");
            }

#if EDITOR
            m_Collisions.Clear();

            foreach (Shape2DObject o in sprite_.m_Collisions)
            {
                m_Collisions.Add(o.Clone());
            }
#else
            if (sprite_.m_Collisions != null)
            {
                this.m_Collisions = sprite_.m_Collisions;
                //this.m_Collisions = (Shape2DObject[])sprite_.m_Collisions.Clone();
            }
            else
            {
                this.m_Collisions = null;
            }
#endif

            this.m_Origin = sprite_.m_Origin;
            this.m_AssetFileNames.AddRange(sprite_.m_AssetFileNames);
            this.m_PositionInTexture = sprite_.m_PositionInTexture;

            base.CopyFrom(sprite_);
        }


        public override void Load(XmlElement node_, SaveOption option_)
        {
            base.Load(node_, option_);

            XmlNode rootNode = node_.SelectSingleNode("Sprite2D");

            uint version = uint.Parse(rootNode.Attributes["version"].Value);

            if (version == 1)
            {
                m_AssetFileNames.Add(rootNode.SelectSingleNode("AssetFileName").InnerText);
            }
            else
            {
                foreach (XmlNode child in rootNode.SelectNodes("AssetFiles/AssetFileName"))
                {
                    m_AssetFileNames.Add(child.InnerText);
                }
            }

            ((XmlElement)rootNode.SelectSingleNode("PositionInTexture")).Read(ref m_PositionInTexture);
            ((XmlElement)rootNode.SelectSingleNode("HotSpot")).Read(ref m_Origin);

            XmlNode collisionNode = rootNode.SelectSingleNode("CollisionList");

#if EDITOR
            m_Collisions.Clear();

            foreach (XmlNode node in collisionNode.ChildNodes)
            {
                m_Collisions.Add(Shape2DObject.CreateShape2DObject((XmlElement)node, option_));
            }
#else
            if (collisionNode.ChildNodes.Count > 0)
            {
                m_Collisions = new Shape2DObject[collisionNode.ChildNodes.Count];
                int i = 0;

                foreach (XmlNode node in collisionNode.ChildNodes)
                {
                    m_Collisions[i++] = (Shape2DObject.CreateShape2DObject((XmlElement)node, option_));
                }
            }
            else
            {
                m_Collisions = null;
            }
#endif

            //Sockets
            XmlNode socketNode = rootNode.SelectSingleNode("SocketList");

            foreach (XmlNode node in socketNode.ChildNodes)
            {
                Vector2 position = new Vector2();
                ((XmlElement)node.SelectSingleNode("Position")).Read(ref position);
                m_Sockets.Add(node.SelectSingleNode("Name").InnerText, position);
            }
        }

        public override void Load(BinaryReader br_, SaveOption option_)
        {
            base.Load(br_, option_);
            throw new NotImplementedException();
        }

        public void UnloadTexture()
        {
#if !EDITOR
			if (m_Texture2D == null)
			{
				throw new InvalidOperationException("Sprite2D.LoadTexture() : texture is null !");
			}
#endif

            //m_Texture2D.Dispose();
            m_Texture2D = null;
        }


        public Vector2 GetSocketByName(string name_)
        {
            return m_Sockets[name_];
        }

        public void LoadTexture(ContentManager content_)
        {
            if (m_Texture2D != null
                && m_Texture2D.IsDisposed == false)
            {
                return;
            }

            string assetFile = System.IO.Path.GetDirectoryName(m_AssetFileNames[0]) + System.IO.Path.DirectorySeparatorChar +
                System.IO.Path.GetFileNameWithoutExtension(m_AssetFileNames[0]);

            m_Texture2D = content_.Load<Texture2D>(assetFile);
        }

        public void LoadTextureFile(GraphicsDevice device_)
        {
            string assetFile;

#if EDITOR
            assetFile = Engine.Instance.ProjectManager.ProjectPath + System.IO.Path.DirectorySeparatorChar +
                ProjectManager.AssetDirPath + System.IO.Path.DirectorySeparatorChar + m_AssetFileNames[0];
#else
            assetFile = Engine.Instance.Game.Content.RootDirectory + System.IO.Path.DirectorySeparatorChar + m_AssetFileNames[0];
#endif

            if (m_Texture2D != null
                && m_Texture2D.IsDisposed == false
                && m_Texture2D.GraphicsDevice.IsDisposed == false)
            {
                return;
            }

            m_Texture2D = Texture2D.FromStream(device_, File.OpenRead(assetFile));
        }

    }
}
