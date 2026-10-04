using Nt.Automaton.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nt.Automaton.Events
{
    public class TokenEventArgs<T> : EventArgs
    {

        public IAutomatonToken<T> Token { get; }

        public TokenEventArgs(IAutomatonToken<T> token)
        {
            Token = token;
        }

    }
}
