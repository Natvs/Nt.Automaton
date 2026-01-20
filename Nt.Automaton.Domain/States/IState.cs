using Nt.Automaton.Actions;
using Nt.Automaton.Tokens;

namespace Nt.Automaton.States
{
    public interface IState
    {
        IAction? Action { get; }
        IState Read(IAutomatonToken token);
    }
}
