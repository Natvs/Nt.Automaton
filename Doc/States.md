# States

- [Structure](#structure)
- [Methods](#methods)
- [Events](#events)
- [Workflow](#workflow)
- [Customisable states](#customisable-states)

## Structure

All states have common features
- Two methods `AddTransition` and `OverwriteTransition` to add transitions from this state to an other state.
- A method `SetDefault` to set the default transition used when no transitions are valid.
- A method `SetAction` to set the action linked to the state.
- Two methods `Activate` and `Deactivate` to control the state's activity.
- Two events `OnReached` and `OnLeft` triggerred when the state is reached or left.
- A method `Read` to read a token and take a transition to the next state

The complete API description is available below.

## Methods

### void AddTransition(ITransition\<T\> transition)

Add a transition to the list of transitions. If a transition with the same token already exists, the already existing transition has priority over this one.

### void OverwriteTransition(ITransition\<T\> transition)

Same as `AddTransition` but overwrites the existing transition if one with the same token already exists.

### IState\<T\> SetDefault(ITransition\<T\> transition)

Set a transition to use when no transitions are valid.

Returns: The current state.

### IActionState\<T\> SetAction(ITokenAction\<T\> action)

Set an action to perform when the state is reached. 

If auto_perform is true, the action is performed when the state is reached.

Returns: A new new state with the action set.

### IFinalState\<T\> SetFinal()

Set the state as a final state.

Returns: A new state with the final flag set.

### IFinalState\<T\> OnCondition(Func\<bool\> condition)

This method is only available on final states. It sets a condition to check when the state is reached. If the condition returns false, the state is not considered as reached.

Returns: The current final state.

### void Activate()

Activate the state. When an action is linked to a state, this method performs the action.

### void Deactivate()

Deactivate the state.

### IState\<T\> Read(IAutomatonToken\<T\> token)

Read a token and return the target state of the first matching transition, or the default one if there is no such transition.

Returns: The target state of the transition, or the default one if there is no such transition.

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


## Customisable states

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