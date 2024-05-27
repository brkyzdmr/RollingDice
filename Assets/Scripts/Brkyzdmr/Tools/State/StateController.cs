using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Brkyzdmr.Tools.State
{
    public class StateController : MonoBehaviour
    {
        private IState _currentState;
        private IState _initialState;

        private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();
        private List<Transition> _currentTransitions = new List<Transition>();
        private List<Transition> _anyTransitions = new List<Transition>();
        private static List<Transition> _emptyTransitions = new List<Transition>();
        private List<IState> _allStates = new List<IState>();

        public IState CurrentState => _currentState;
        public string CurrentStateId { get; set; }
        public string ChangeStateId { get; set; }

        public string StateID { get; }

        public void Update()
        {
            Transition transition = GetTransition();
            if (transition != null) { ChangeState(transition.To); }

            _currentState?.OnUpdate();
            CurrentStateId = _currentState?.StateID;
        }

        public void FixedUpdate()
        {
            _currentState?.OnFixedUpdate();
        }

        public void SetStates(List<IState> states)
        {
            _allStates.Clear();
            _allStates = states;
        }

        public void SetInitialState(IState state)
        {
            _initialState = state;
        }

        public IState GetStateByID(string stateID)
        {
            return _allStates.Find(s => s.StateID == stateID);
        }

        public void AddTransition(IState from, IState to, Func<bool> predicate)
        {
            if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
            {
                transitions = new List<Transition>();
                _transitions[from.GetType()] = transitions;
            }

            transitions.Add(new Transition(to, predicate));
        }

        public void AddAnyTransition(IState state, Func<bool> predicate)
        {
            _anyTransitions.Add(new Transition(state, predicate));
        }

        public void ChangeState(IState state)
        {
            if (_currentState == state) { return; }

            Debug.Log($"Changing state from {_currentState?.StateID} to {state.StateID}");
            _currentState?.OnExit();
            _currentState = state;

            _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
            if (_currentTransitions == null) { _currentTransitions = _emptyTransitions; }

            _currentState.OnEnter();
        }

        private Transition GetTransition()
        {
            foreach (var transition in _anyTransitions)
            {
                if (transition.Condition()) { return transition; }
            }

            foreach (var transition in _currentTransitions)
            {
                if (transition.Condition()) { return transition; }
            }

            return null;
        }

        public void ResetStateService()
        {
            ChangeState(_initialState);
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(ChangeStateId)) { return; }
            var state = GetStateByID(ChangeStateId);
            if (state == null || state == _currentState) { return; }
            ChangeState(state);
            ChangeStateId = null;
#endif
        }
    }

    public class Transition
    {
        public Func<bool> Condition { get; }
        public IState To { get; }

        public Transition(IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }
}
