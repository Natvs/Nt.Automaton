using Nt.Automaton.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nt.Automaton.Transitions
{
    public class TransitionEventsArgs<T> : EventArgs
    {

        public IAutomatonToken<T> Token { get; }

        public TransitionEventsArgs(IAutomatonToken<T> token)
        {
            Token = token;
        }

    }
}
