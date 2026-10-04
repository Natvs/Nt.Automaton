# Transitions

- [Structure](#structure)
- [Methods](#methods)
- [Events](#events)
- [Customisable transitions](#customisable-transitions)

## Structure

Transitions have some common features
- A field `Value` that is the type of tokens to parse (`string`, `int`, `char`...)
- A field `Target` that points to the state the transitions leads to
- A method `Accepts` that checks if a token is accepted by the transition
- A method `SetAction` that sets the action associated to the transition
- A method `Trigger` that triggers the action associated to the transition

The complete API description is available below.

## Methods

### bool Accepts(IAutomatonToken token)

Bool that checks if the transition accepts the token.

### IActionTransition\<T\> SetAction(ITokenAction\<T\> action)

Set an action to perform when the transition is triggered.

Returns: The current transition.

### void Trigger(IAutomatonToken\<T\> token)

Triggers the transition and its associated action if any. A token is needed to trigger the action.

## Events

There are no events for now.

## Customisable transitions

In addition to the existing transitions, you can create your own transition by implementing the `ITransition` interface.

Example

```csharp
using Nt.Automaton.Transitions;
using Nt.Automaton.Tokens;
using Nt.Automaton.Actions;

#This transition accepts a token only if it is a number between 0 and 9
public class DigitTransition : Transition<int>
{
    public IAutomatonToken<int> Value { get; private set; }
    public IState<int> Target { get; private set; }
    private ITokenAction<int> action;

    public DigitTransition(IState<int> target) : base()
    {
        Target = target;
    }

    public override bool Accepts(IAutomatonToken<int> token)
    {
        return token.Value >= 0 && token.Value <= 9;
    }
}

```