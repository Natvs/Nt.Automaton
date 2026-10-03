namespace Nt.Automaton.States.Decorators
{
    public interface IFinalState<T> : IState<T>
    {
        public IFinalState<T> OnCondition(Func<bool> condition);
    }
}
