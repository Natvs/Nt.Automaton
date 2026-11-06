# Transitions

- [Structure](#structure)
- [Methods](#methods)
- [Events](#events)
- [Adding conditions to transitions](#adding-conditions-to-transitions)
    - [OnCondition after a constructor](#oncondition-after-a-constructor)
    - [OnCondition after setting an action](#oncondition-after-setting-an-action)
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

## Creating a transition

A transition is defined by the target state if the transition is taken. 
It is also possible to define a transition that is taken when a specific token is read. That defines two constructors for the transition:

```csharp
var target = new State<int>();
var token = new AutomatonToken<int>(1);

// Constructor 1: Transition to a target state without a specific token. This transition is taken by default except if a condition says otherwise.
var transition1 = new Transition<int>(target);

// Constructor 2: Transition to a target state when a specific token is read
var transition2 = new Transition<int>(token, target);
```

### Taking a transition

Transitions are linked to states with the `AddTransition` or `SetDefault` on states.
A state contains a list of transitions, and when reading a token, all transitions are tests in the order they have been added.
The first transition that accepts the token is taken.

The rules for each transitions to be taken are the following:

1. If the transition has no token, it is taken by default unless a condition says otherwise.
2. If the transition has a token and it corresponds to the token read, it is taken unless a condition says otherwise.
3. If the transition has not been taken, the next transition is tested.
4. For the last transition, if it has not been taken, the default transition is taken if it exists.
5. Depending on the type of automaton used, if the default transition doesn't exist, the automaton may throw an exception.

## Adding conditions to transitions

Transitions can be conditionned to be taken depending on a predefined condition. This conditions is a function that returns a boolean value.

There are two ways of conditioning a transition. This is done with the method `OnCondition`. The method behaves differently depending a the function that precedes it.

### OnCondition after a constructor

If the `OnCondition` method follows a constructor, then it defines whether the transition can be taken when transitioning from one state to an other.

Example:

```csharp
var target = new State<int>();
var default = new State<int>();
var token = new AutomatonToken<int>(1);

var state = new State<int>().SetDefault(new Transition<int>(default));
state.AddTransition(new Transition<int>(token, target).OnCondition(() => false));

var new_state = state.Read(token);
```

In this example, even though the token read is 1 and correponds to the transition to the target state, this transition is not taken because of the condition not being respected.
The next available transition is taken, which in this example is the default transition to the default state.

### OnCondition after setting an action

If the `OnCondition` method follows a call to `SetAction`, then it defines whether the action is triggered when the transition is taken.

Example:
```csharp
var target = new State<int>();
var token = new AutomatonToken<int>(1);
var trigger = random.Next(0, 2) == 1;

var state = new State<int>().AddTransition(token, target).SetAction(new MyAction()).OnCondition(() => trigger);
state.Read(token);
```

In this example the transition is always taken as long as the token read is 1, but the action is performed only if the condition is met (only half of the time here).

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