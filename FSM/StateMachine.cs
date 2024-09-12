using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulletz
{
    class StateMachine
    {
        private Dictionary<StateType, State> states;
        private State currentState;

        public StateMachine()
        {
            states = new Dictionary<StateType, State>();
            currentState = null;
        }

        public void AddState(StateType stateType, State state)
        {
            states[stateType] = state;
            state.SetStateMachine(this);
        }

        public void GoTo(StateType state)
        {
            if (currentState != null)
            {
                currentState.OnExit();
            }

            currentState = states[state];
            currentState.OnEnter();
        }

        public void Update()
        {
            currentState.Update();
        }
    }
}
