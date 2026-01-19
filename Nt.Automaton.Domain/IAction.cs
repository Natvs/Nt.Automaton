namespace Nt.Automaton
{
    public interface IAction 
    {
        void Perform(IAutomatonToken token);
    }

}
