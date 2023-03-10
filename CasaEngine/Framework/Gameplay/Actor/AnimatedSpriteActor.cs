using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Xml;
using CasaEngine.Framework.Game;
using CasaEngine.Framework.Entities;
using CasaEngine.Core.Logger;
using CasaEngine.Framework.Graphics2D;
using CasaEngine.Core.Design;
using CasaEngine.Framework.Assets.Graphics2D;

namespace CasaEngine.Framework.Gameplay.Actor
{
    public class AnimatedSpriteActor : Actor2D, IRenderable
    {

#if EDITOR
        public bool CompareTo(Entity other)
        {
            if (other is AnimatedSpriteActor)
            {
                var asa = (AnimatedSpriteActor)other;
                //asa._Animations
            }

            return false;
        }
#endif

        private Dictionary<int, Animation2D> _animations = new();
        private Animation2DPlayer _animation2DPlayer;
        private Renderer2DComponent _renderer2DComponent;



        public Animation2DPlayer Animation2DPlayer => _animation2DPlayer;

        public int Depth
        {
            get;
            set;
        }

        public float ZOrder
        {
            get;
            set;
        }

        public bool Visible
        {
            get;
            set;
        }



        public AnimatedSpriteActor()
            : base()
        {

        }

        public AnimatedSpriteActor(AnimatedSpriteActor src)
        {
            //return new 
        }



        public Entity Clone()
        {
            throw new NotImplementedException();
        }

        protected void CopyFrom(Entity ob)
        {
            base.CopyFrom(ob);

            var src = ob as AnimatedSpriteActor;

            _animations = new Dictionary<int, Animation2D>();

            foreach (var pair in src._animations)
            {
                _animations.Add(pair.Key, (Animation2D)pair.Value.Clone());
            }

            _animation2DPlayer = new Animation2DPlayer(_animations);
            _renderer2DComponent = src._renderer2DComponent;
        }

        public override void Update(float elapsedTime)
        {
            _animation2DPlayer.Update(elapsedTime);
        }

        public virtual void Draw(float elapsedTime)
        {
            _renderer2DComponent.AddSprite2D(
                _animation2DPlayer.CurrentAnimation.CurrentSpriteId,
                Position, 0.0f, Vector2.One, Color.White,
                1 - Position.Y / Game.Engine.Instance.Game.GraphicsDevice.Viewport.Height,
                SpriteEffects.None);
        }

        public override void Load(XmlElement el, SaveOption opt)
        {
            base.Load(el, opt);

            var animListNode = (XmlElement)el.SelectSingleNode("AnimationList");

            _animations.Clear();

            foreach (XmlNode node in animListNode.ChildNodes)
            {
                var anim2d = Game.Engine.Instance.Asset2DManager.GetAnimation2DByName(node.Attributes["name"].Value);
                if (anim2d != null)
                {
                    _animations.Add(int.Parse(node.Attributes["index"].Value), anim2d);
                }
                else
                {
                    LogManager.Instance.WriteLineError("CharacterActor2D.Load() : can't find the animation : '" + node.Attributes["name"].Value + "'");
                }
            }
        }

        public void LoadAnimation(int index, string anim2DName)
        {
            _animations.Add(
                index,
                (Animation2D)Game.Engine.Instance.ObjectManager.GetObjectByPath(anim2DName));

            foreach (var frame in _animations[index].GetFrames())
            {
                var sprite = (Sprite2D)Game.Engine.Instance.ObjectManager.GetObjectById(frame.SpriteId);
                //sprite.LoadTextureFile(CasaEngine.Game.Engine.Instance.Game.GraphicsDevice);
                sprite.LoadTexture(Game.Engine.Instance.Game.Content);
            }

            _animation2DPlayer = new Animation2DPlayer(_animations);
        }

        public virtual void Initialize()
        {
            _renderer2DComponent = Game.Engine.Instance.Game.GetDrawableGameComponent<Renderer2DComponent>();
        }

    }
}
