﻿using CasaEngine.Gameplay.Actor.Object;

namespace CasaEngine.Gameplay.Actor
{
    public class StaticSpriteActor
        : Actor2D
    {





        public StaticSpriteActor()
            : base()
        {

        }



        public override BaseObject Clone()
        {
            throw new NotImplementedException();
        }

#if EDITOR
        public override bool CompareTo(BaseObject other_)
        {
            throw new NotImplementedException();
        }
#endif

    }
}
