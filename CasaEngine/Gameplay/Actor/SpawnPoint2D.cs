﻿using CasaEngine.Gameplay.Actor.Object;

namespace CasaEngine.Gameplay.Actor
{
    public class SpawnPoint2D
        : Actor2D
    {





        public SpawnPoint2D()
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
