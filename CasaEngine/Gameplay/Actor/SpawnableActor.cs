﻿using CasaEngine.Gameplay.Actor.Object;

namespace CasaEngine.Gameplay.Actor
{
    public class SpawnableActor
        : Actor2D
    {





        public SpawnableActor()
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
