using System;
using Torque3D.Engine.Util.Enums;
using Torque3D.Util;

using System.Collections.Generic;
using RealAI.Events;

namespace RealAI {
    public class Brain
    {

        private List<EventListener> eventListeners = new List<EventListener>();
        private State currentState = new InitialState();


        public Brain()
        {
            registerEvents();
        }

        public PlayerAction tick(FeatureVector vector)
        {
            triggerEvents(vector);
            //States return the next state; this is an infinite stream of states;
            PlayerAction action = PlayerAction.Prepare; //This is ALWAYS better than doing nothing
            State nextState = currentState.tick(ref action, vector, this);
            if(nextState == null)
            {
                //WTF, why don't you read the first comment even?
                nextState = new InitialState(); //Try to recover
            }
            currentState = nextState;
            return action; //This was passed by reference, so it should be updated by now
        }

        public State getCurrentState() { return currentState; }

        #region Set up

        //Events will be prioritized by order of registration
        private void registerEvents()
        { 
            eventListeners.Add(new HighChanceOfHitEventListener());
        }

        #endregion



        private void triggerEvents(FeatureVector vector)
        {
            if(vector.TickCount == 1) { return; } //simple hack to avoid having lastVector = null, skip first tick.
            foreach (EventListener e in eventListeners)
            {
                Event eventTriggered = e.trigger(vector, this);
                if(eventTriggered != null) {
                    //Trigger the event on the current state, if the current state has a listener for it
                    //This is reasonable, because if it listens for it, it assumes responsibility for it
                    //IFF it returns a new state
                    State newState = null; 
                    newState = triggerCurrentStateEvent(eventTriggered, vector);
                    if(newState != null)
                    {
                        currentState = newState;
                        return;
                    }
                    //Trigger the global event, this will happen IFF the current state
                    //did not return a new state
                    newState = eventTriggered.onTrigger(vector, this);
                    if (newState != null)
                    {
                        currentState = newState;
                        return;
                    }
                    //If this also returns null, try the next event
                }
            }
        }

        private State triggerCurrentStateEvent(Event triggerEvent, FeatureVector vector)
        {

            System.Type eListenerType = typeof(EventListenerAttribute);
            System.Type eStateType = typeof(State);

            foreach (System.Reflection.MethodInfo m in currentState.GetType().GetMethods())
            {

                if (m.GetCustomAttributes(eListenerType, false).Length > 0)
                {
                    System.Reflection.ParameterInfo[] parameters = m.GetParameters();
                    if (parameters.Length == 1)
                    {
                        System.Reflection.ParameterInfo info = m.GetParameters()[0];
                        if (info.ParameterType == triggerEvent.GetType())
                        {
                            if (m.ReturnType == eStateType)
                            {
                                return (State)m.Invoke(currentState, new object[] { triggerEvent });
                            }
                            else
                            {
                                m.Invoke(currentState, new object[] { triggerEvent });
                                return null;
                            }
                        }

                    }
                }


            }
            return null;
        }


    }
}
