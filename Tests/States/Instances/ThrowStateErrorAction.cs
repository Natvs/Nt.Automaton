using Nt.Automaton.Actions;
using Nt.Tests.Automaton.States.Instances;

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
