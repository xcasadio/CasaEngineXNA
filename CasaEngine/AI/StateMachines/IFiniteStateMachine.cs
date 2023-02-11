using CasaEngine.AI.Messaging;


namespace CasaEngine.AI.StateMachines
{
    public interface IFiniteStateMachine<T> : IMessageable where T : /*BaseEntity,*/ IFsmCapable<T>
    {

        IState<T> CurrentState
        {
            get;
            set;
        }

        IState<T> GlobalState
        {
            get;
            set;
        }



        void Update(float elpasedTime);

        void Transition(IState<T> newState);

        void RevertStateChange();

        bool IsInState(IState<T> state);

    }
}
