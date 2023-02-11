using CasaEngine.AI.Navigation.SteeringsBehaviors;
using Microsoft.Xna.Framework;
using CasaEngineCommon.Helper;

namespace CasaEngine.AI.Navigation.Steeringsbehaviors
{
    public class Steeringbehaviors
    {
        protected internal MovingEntity owner;
        protected internal List<SteeringBehavior> behaviors;
        protected internal SumMethod sumAlgorithm;

        public Steeringbehaviors(MovingEntity owner, SumMethod sumAlgorithm)
        {
            owner = owner;
            this.sumAlgorithm = sumAlgorithm;

            behaviors = new List<SteeringBehavior>();
        }

        public SumMethod SumAlgorithm
        {
            get => sumAlgorithm;
            set => sumAlgorithm = value;
        }

        public void RegisterBehavior<T>(string name, float modifier)
        {
            object newBehavior;
            object[] parametters;

            foreach (SteeringBehavior behavior in behaviors)
                if (behavior is T && behavior.Name == name)
                {
                    return;
                }

            parametters = new object[3];
            parametters[0] = name;
            parametters[1] = owner;
            parametters[2] = modifier;

            newBehavior = Activator.CreateInstance(typeof(T), parametters);

            behaviors.Add((SteeringBehavior)newBehavior);
        }

        public void RegisterBehavior(SteeringBehavior behavior)
        {
            for (int i = 0; i < behaviors.Count; i++)
            {
                if (behaviors[i].GetType() == behavior.GetType() && behaviors[i].Name == behavior.Name)
                {
                    behaviors[i] = behavior;
                    return;
                }
            }

            behaviors.Add(behavior);
        }

        public void UnregisterBehavior<T>(string name)
        {
            for (int i = 0; i < behaviors.Count; i++)
            {
                if (behaviors[i] is T && behaviors[i].Name == name)
                {
                    behaviors.RemoveAt(i);
                }
            }
        }

        public void ActivateBehavior<T>(string name)
        {
            foreach (SteeringBehavior behavior in behaviors)
                if (behavior is T && behavior.Name == name)
                {
                    behavior.Active = true;
                }
        }

        public void DeactivateBehavior<T>(string name)
        {
            foreach (SteeringBehavior behavior in behaviors)
                if (behavior is T && behavior.Name == name)
                {
                    behavior.Active = false;
                }
        }

        public T GetBehavior<T>(string name) where T : SteeringBehavior
        {
            foreach (SteeringBehavior behavior in behaviors)
                if (behavior is T && behavior.Name == name)
                {
                    return (T)behavior;
                }

            return null;
        }

        public Vector3 Calculate()
        {
            return Vector3Helper.Truncate(sumAlgorithm(behaviors), owner.MaxForce);
        }

    }
}
