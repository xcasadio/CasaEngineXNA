using Microsoft.Xna.Framework;
//using CasaEngine.GameLogic;
//using CasaEngine.PhysicEngine;
//using BEPUphysics;
//using CasaEnginePhysics;


namespace CasaEngine.Framework.AI.Navigation.SteeringsBehaviors
{
    public class WallAvoidance : SteeringBehavior
    {

        public WallAvoidance(string name, MovingObject owner, float modifier)
            : base(name, owner, modifier)
        { }



        public override Vector3 Calculate()
        {
            if (PhysicEngine.Physic == null)
            {
                throw new NullReferenceException("MovingObject.CanMoveBetween() : PhysicEngine.Physic not defined");
            }

            Vector3 force, position, overShoot, contactPoint, contactNormal;
            Vector3[] feelers;
            float nearIntersectionDist, scale;

            feelers = new Vector3[3];
            scale = 2.0f + owner.Speed * 0.5f;
            force = Vector3.Zero;

            //Create the feelers
            feelers[0] = owner.Position + owner.Look * scale;
            feelers[1] = owner.Position + Vector3.TransformNormal(owner.Look, Matrix.CreateRotationY(MathHelper.ToRadians(-40.0f))) * scale * 0.5f;
            feelers[2] = owner.Position + Vector3.TransformNormal(owner.Look, Matrix.CreateRotationY(MathHelper.ToRadians(40.0f))) * scale * 0.5f;

            nearIntersectionDist = float.MaxValue;

            for (var i = 0; i < feelers.Length; i++)
            {
                position = owner.Position;

                //Test for a collision
                owner.Position = position;

                //If there was a collision see the collision distance
                if (PhysicEngine.Physic.NearBodyWorldRayCast(ref position, ref feelers[i], out contactPoint, out contactNormal))
                {
                    var intersectionDist = (contactPoint - owner.Position).Length();

                    //If it was closer than the the closer collision so far, update the values
                    if (intersectionDist < nearIntersectionDist)
                    {
                        nearIntersectionDist = intersectionDist;
                        overShoot = contactPoint - feelers[i];
                        force = contactNormal * overShoot.Length() * scale;
                    }
                }
            }

            return force;
        }

    }
}
