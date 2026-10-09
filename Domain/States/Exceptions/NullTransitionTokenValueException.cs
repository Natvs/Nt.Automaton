using System;
using System.Collections.Generic;
using System.Text;

namespace Nt.Automaton.States.Exceptions
{
    public class NullTransitionTokenValueException() : Exception("Transition token value cannot be null.")
    {
    }
}
