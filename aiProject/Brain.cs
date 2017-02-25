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
            
            return PlayerAction.Prepare;
        }

        private void registerEvents()
        { //Events will be prioritized by order of registration
            eventListeners.Add(new OnGetShotEventListener());
        }

        private void triggerEvents(FeatureVector vector)
        {
            foreach (EventListener e in eventListeners)
            {
                Event eventTriggered = e.trigger(vector, this);
                if(eventTriggered != null) {
                    State newState = triggerCurrentStateEvent(eventTriggered, vector);
                    if(newState != null)
                    {
                        currentState = newState;
                        return;
                    }
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
