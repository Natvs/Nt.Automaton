using Nt.Automaton.Actions;
using Nt.Automaton.Tokens;

namespace Nt.Tests.Automaton.States
{
    internal class ThrowStateErrorAction : IAction<string>
    {
        public void Perform(IAutomatonToken<string> token)
        {
            throw new StateErrorException();
        }
    }
}
