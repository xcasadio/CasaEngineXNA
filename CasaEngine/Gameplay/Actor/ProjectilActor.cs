﻿using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using CasaEngine.AI.Messaging;
using CasaEngine.Core.Math.Shape2D;
using CasaEngine.Entities;
using CasaEngine.Helpers;
using CasaEngineCommon.Helper;

namespace CasaEngine.Gameplay.Actor
{
    public class ProjectilActor
        : AnimatedSpriteActor, IAttackable
    {

        private Body _body;
        private List<Shape2DObject> _shape2DObjectList = new();
        private TeamInfo _teamInfo;

#if !FINAL
        private ShapeRendererComponent _shapeRendererComponent;
#endif

        //life?
        //distance Max
        Vector3 _start;



        public Shape2DObject[] Shape2DObjectList => _shape2DObjectList.ToArray();

        public new Vector2 Position => _body.Position;

        public TeamInfo TeamInfo
        {
            get => _teamInfo;
            set => _teamInfo = value;
        }

        public Actor2D Owner
        {
            get;
            set;
        }

        public Vector2 Velocity
        {
            get;
            set;
        }



        public ProjectilActor()
            : base()
        {
        }

        public ProjectilActor(ProjectilActor src)
        {
            CopyFrom(src);
        }



        public Entity Clone()
        {
            return new ProjectilActor(this);
        }

        protected void CopyFrom(Entity ob)
        {
            base.CopyFrom(ob);

            var src = ob as ProjectilActor;

            //_Body
            //_Shape2DObjectList
            _teamInfo = src.TeamInfo;
            _shapeRendererComponent = src._shapeRendererComponent;
            _start = src._start;
            Owner = src.Owner;
            Velocity = src.Velocity;
        }

        public virtual void Initialize(FarseerPhysics.Dynamics.World world)
        {
            base.Initialize();

            _body = new Body(world);
            _body.IsBullet = true;

            _body.Restitution = 0.0f;
            _body.SleepingAllowed = false;
            _body.IgnoreGravity = true;
            _body.Friction = 0.0f;
            _body.IsStatic = false;
            _body.FixedRotation = true;
            _body.UserData = world;
        }

        public void DoANewAttack()
        {

        }

        public bool CanAttackHim(IAttackable other)
        {
            return true;
        }

        public bool HandleMessage(Message message)
        {
            switch (message.Type)
            {
                case (int)MessageType.Hit:
                    //Hit((HitInfo)message.ExtraInfo);
                    ToBeRemoved = true;
                    break;

                case (int)MessageType.HitSomeone:
                    var hitInfo = (HitInfo)message.ExtraInfo;
                    ToBeRemoved = true;
                    break;
            }

            return true;
        }

        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);
            base.Position = _body.Position;
            _body.LinearVelocity = Velocity;
        }

        public override void Draw(float elapsedTime)
        {
            base.Draw(elapsedTime);

            if (ShapeRendererComponent.DisplayCollisions)
            {
                var geometry2DObjectList = Shape2DObjectList;
                if (geometry2DObjectList != null)
                {
                    foreach (var g in geometry2DObjectList)
                    {
                        _shapeRendererComponent.AddShape2DObject(g, g.Flag == 0 ? Color.Green : Color.Red);
                    }
                }
            }
        }

        public void SetTransform(Vector2 position, Vector2 direction)
        {
            var rot = Vector2Helper.GetAngleBetweenVectors(Vector2.UnitX, direction);
            _body.SetTransform(ref position, rot);
        }

    }
}
