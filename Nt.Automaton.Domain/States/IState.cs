using Nt.Automaton.Actions;
using Nt.Automaton.Tokens;
using Nt.Automaton.Transitions;

namespace Nt.Automaton.States
{
    public interface IState<T>
    {
        IAction<T>? Action { get; }
        IState<T> Read(IAutomatonToken<T> token);
        void AddTransition(ITransition<T> transition);
        public void AddTransitions(ICollection<ITransition<T>> transitions);
    }
}
