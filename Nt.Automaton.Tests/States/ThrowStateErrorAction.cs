using Nt.Automaton.Actions;
using Nt.Automaton.Tokens;

namespace Nt.Tests.Automaton.States
{
    internal class ThrowStateErrorAction : IAction
    {
        public void Perform()
        {
            throw new StateErrorException();
        }
    }
}
