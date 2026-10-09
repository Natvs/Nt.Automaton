using Nt.Automaton.Actions;
using Nt.Automaton.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nt.Tests.Automaton.Transitions.Instances
{
    public class EmptyTokenAction<T> : ITokenAction<T>
    {

        public void Perform(IAutomatonToken<T> token)
        {
            // Do nothing
        }

    }
}
