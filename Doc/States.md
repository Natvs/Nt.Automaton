# States

- [Structure](#structure)
- [Methods](#methods)
- [Events](#events)
- [Creating states](#creating-states)
- [Stating conditions to states](#stating-conditions-to-states)
	- [Example](#example)
	- [Combining conditions](#combining-conditions)
- [Workflow](#workflow)
- [Advanced features](#advanced-features)
	- [Evolutive states](#evolutive-states)
	- [Customisable states](#customisable-states)

## Structure

All states have common features
- A method `AddTransition` to add transitions from this state to an other state.
- A method `SetDefault` to set the default transition used when no transitions are valid.
- A method `SetAction` to set the action linked to the state.
- Two methods `Activate` and `Deactivate` to control the state's activity.
- Two events `OnReached` and `OnLeft` triggerred when the state is reached or left.
- A method `Read` to read a token and take a transition to the next state

The complete API description is available below.

## Methods

### IState\<T\> AddTransition(ITransition\<T\> transition)

Add a transition to the list of transitions. If a transition with the same token already exists, the already existing transition has priority over this one.

### void ClearTransitions()

Clear all transitions from the state. This method keeps the default transition.

### void GetTransitionsCount()

Get the number of transitions from the state. The total count does not include the default transition.

### IEnumerable\<T\> GetTransitions()

Get a list of tokens that lead to actual transitions once read. Tokens to default transition are not included in the list.

### ITransition\<T\>? GetTransition(T token)

Get the transition triggered when the given token is read. Returns null if no transition is found.


### IState\<T\> SetDefault(ITransition\<T\> transition)

Set a transition to use when no transitions are valid.

### IActionState\<T\> SetAction(ITokenAction\<T\> action)

Set an action to perform when the state is reached. 

### IFinalState\<T\> SetFinal()

Set the state as a final state.

### void Activate()

Activate the state. When an action is linked to a state, this method performs the action.

### void Deactivate()

Deactivate the state.

### IState\<T\> Read(IAutomatonToken\<T\> token)

Read a token and return the target state of the first matching transition, or the default one if there is no such transition.

## Events

### EventHandler? Activated

Event triggered once the state is activated

### EventHandler? Deactivated

Event triggered once the state is deactivated

### EventHandler\<StateEventArgs\<T\>\>? Leave

Event triggered when a state is left (before a transition).

### EventHandler\<StateEventArgs\<T\>\>? Reach

Event triggered when a state is reached (after a transition).

## Workflow

When a token is read and a transition is found, the following methods are called in order:

1. The `Deactivate` method of the current state is called.
2. The event `Leave` is raised from the current state.
3. The action linked to the transition is performed, if any.
4. The event `Reach` is raised from the target state.
5. The `Activate` method of the target state is called.

When a token is read and no transition is found, the following methods are called in order:

1. A runtime exception is thrown if no default state is set.
2. The `Deactivate` method of the current state is called.
3. The event `Leave` is raised from the current state.
4. The default action is performed, if any.
5. The event `Reach` is raised from the default state.
6. The `Activate` method of the default state is called.

## Creating states

Some methods are designed for chaining when creating states. Some of these chaining method DO NOT apply modifications to the state, but instead return a new one.

The methods returning a new state are:
- `SetAction` returns an `IActionState<T>` instance.
- `SetFinal` returns an `IFinalState<T>` instance.

As a consequence, it is recommended to call all the chaining methods needed when declaring the state:

```csharp
var state = new State<int>()
	.SetDefault(new MyDefaultTransition())
	.AddTransition(new MyTransition())o
	.SetAction(new MyAction())
	.SetFinal();
```

> Here state will be of type `IFinalState<int>` as `SetFinal` is the last method called.

If this is not possible, you can store the returned state in a variable and call the next method on it:

```csharp
var state = new State<int>()
	.SetDefault(new MyDefaultTransition())
	.AddTransition(new MyTransition());

var actionState = state.SetAction(new MyAction());
var finalState = actionState.SetFinal();
```

Here `actionState` is not marked as final, but `finalState` is and contains an action. 
The original `state` variable is not marked as final either nor contains any action. But all states contain the same transitions and default transitions.

To keep things simple, you can just define one state variable `state`:

```csharp
var state = new State<int>()
	.SetDefault(new MyDefaultTransition())
	.AddTransition(new MyTransition());

state = state.SetAction(new MyAction());
state = state.SetFinal();
```


## Stating conditions to states
The type of state can be conditionned and dependent on the runtime environment. For example, a state can be set as final only if a certain condition is met.

For that purpose, the method `OnCondition` is provided. It takes a function returning a boolean as parameter. The behavior of this method depends on the method called before it.

After calling `SetAction`, the method `OnCondition` sets a condition for the action to be executed.

After calling `SetFinal`, the method `OnCondition` sets a condition for the state to be considered as final.

### Example
Here is an example of an action performed only if the score of the player is greater than 1000:

```csharp
var state = new State<int>().SetAction(new MyAction()).OnCondition(() => player.Score > 1000);
```

In this example, the action `MyAction` will be executed only if the player has a score of 1000 or greater.

### Combining conditions
You can combine conditions for actions and final states by following each correponding method with a `OnCondition` call.

```csharp
var state = new State<int>()
	.SetAction(new MyAction()).OnCondition(() => player.Score > 1000)
	.SetFinal().OnCondition(() => player.Level == 10);
```

The action will be performed only if the player has a score of 1000 or greater, and the state will be considered as final only if the player is at level 10.

## Advanced features

### Evolutive states

Sometimes the number and nature of transitions cannot be predicted before the state is reached.
The following patterns explain how to dynamically add or remove transitions from a state when it is reached.

#### 1. Subscribe to the `Reach` or `Activated` event of the state

The event `Reach` is raised before any action is performed, and the event `Activated` is raised after the action is performed.
Following the scenario, you must subscribe to one of these events.

```csharp
var state = new State<int>();
state.Reach += OnStateReached;
```

#### 2. Edit the transitions from the event handler

Define the logic to add or remove transitions from the state in the event handler. For that, use these methods:
- `AddTransition` to add a transition to the state.
- `OverwriteTransition` to add a transition to the state and overwrite an existing one if it exists.
- `ClearTransitions` to remove all transitions from the state.
- `GetTransition` to get a specific transition from the state.

```csharp
private OnStateReached(object? sender, TransitionEventArgs<int> e) {
   var state = e.Transition.Target;

   # edit transitions, using commands like
   state.ClearTransitions();

   # followed by
   state.AddTransition(new Transition(...)):

   # or
   state.GetTransition(...)?.SetAction(...);

}
```

### Customisable states

In addition to the existing states, you can override the default behavior by implementing the `Nt.Automaton.States.State` class, or create a new state from scratch by implementing the `Nt.Automaton.States.IState` interface.

Example:

```csharp
using Nt.Automaton.States;
using Nt.Automaton.Actions;

# This state lets you define an action triggered when leaving the state
public class MyState<T> : State<T>
{
	private Action { get; set; }
	
	public MyState() : base() { }

	public override IState<T> SetAction(ITokenAction<T> action)
	{
		Action = action;
		return this;
	}

	public override void Deactivate()
	{
		Action?.Perform();
		base.Deactivate();
	}
}

```