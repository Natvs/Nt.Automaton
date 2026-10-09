using System;
using System.Collections.Generic;
using System.Text;
using Nt.Automaton.Actions;

namespace Nt.Tests.Automaton.States.Instances
{
    internal class EmptyAction : IAction
    {
        public void Perform()
        {
            // Do nothing
        }
    }
}
