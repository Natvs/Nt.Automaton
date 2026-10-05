using System;
using System.Collections.Generic;
using System.Text;

namespace Nt.Automaton.Automatons.Exceptions
{
    public class NullStateException(string message) : Exception(message) { }
}
