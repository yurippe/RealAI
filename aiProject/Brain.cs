using System;
using Torque3D.Engine.Util.Enums;
using Torque3D.Util;

using System.Collections.Generic;
using RealAI.Events;
using RealAI.States;

namespace RealAI {
    public class Brain
    {

        private List<EventListener> eventListeners;
        private State currentState;
        private MapManager mapManager;

        private bool _hasPrepareMove;
        private int _lastPrepareTick;

        private FeatureVector currentVector;
        private FeatureVector previousVector;

        public Brain()
        {
            Reset();
        }

        //Yeh, the game doesn't do this for us xD
        private void Reset()
        {
            eventListeners = new List<EventListener>();
            currentState = new InitialState(); //Reset our states
            registerEvents();
            mapManager = new MapManager(); //Reset the map
            //Reset fields
            _hasPrepareMove = false;
            _lastPrepareTick = -1;
            currentVector = null;
            previousVector = null;

        }

        public PlayerAction tick(FeatureVector vector)
        {
            if(vector.TickCount == 3200) { Reset(); return PlayerAction.Prepare; } //This is the reset condition, guess we also need to return a non-null value
            currentVector = vector;
            mapManager.tick(vector, this);
            triggerEvents();
            //States return the next state; this is an infinite stream of states;
            PlayerAction action = PlayerAction.Prepare; //This is ALWAYS better than doing nothing

            //Deal with caching prepareMove
            if(preservePrepare(currentState) && isPreparedMove()) { previousVector = vector; _lastPrepareTick = vector.TickCount; return PlayerAction.Prepare; }

            State nextState = currentState.tick(ref action, vector, this);
            if(nextState == null)
            {
                //WTF, why don't you read the first comment even?
                nextState = new InitialState(); //Try to recover
            }
            currentState = nextState;

            //If by this step the action is to prepare, we know we have a prepare for next round
            if(action == PlayerAction.Prepare)
            {
                _lastPrepareTick = vector.TickCount;
                _hasPrepareMove = true;
            }
            else if (_hasPrepareMove && _lastPrepareTick != vector.TickCount)
            {
                _hasPrepareMove = false;
            }
            previousVector = currentVector;
            return action; //This was passed by reference, so it should be updated by now
        }

        public State getCurrentState() { return currentState; }
        public FeatureVector getCurrentVector() { return currentVector;  }
        //return the first vector as the previous if it is the first tick, this works for most cases
        public FeatureVector getPreviousVector() { return currentVector.TickCount == 1 ? currentVector : previousVector; } 

        public MapManager getMapManager() { return mapManager; }

        //The prepare move is defined as the first of the 2 moves
        public bool isPreparedMove() { return _hasPrepareMove && getCurrentVector().TickCount != 1 && getCurrentVector().TickCount != getPreviousVector().TickCount; }
        public bool hasCachedPrepareMove() { return _hasPrepareMove; }

        #region Set up

        //Events will be prioritized by order of registration
        private void registerEvents()
        { 
            eventListeners.Add(new HighChanceOfHitEventListener());
        }

        #endregion


        private void triggerEvents()
        {
            if(currentVector.TickCount == 1) { return; } //simple hack to avoid having lastVector = null, skip first tick.
            foreach (EventListener e in eventListeners)
            {
                Event eventTriggered = e.trigger(currentVector, this);
                if(eventTriggered != null) {
                    //Trigger the event on the current state, if the current state has a listener for it
                    //This is reasonable, because if it listens for it, it assumes responsibility for it
                    //IFF it returns a new state
                    State newState = null; 
                    newState = triggerCurrentStateEvent(eventTriggered);
                    if(newState != null)
                    {
                        currentState = newState;
                        return;
                    }
                    //Trigger the global event, this will happen IFF the current state
                    //did not return a new state
                    newState = eventTriggered.onTrigger(currentVector, this);
                    if (newState != null)
                    {
                        currentState = newState;
                        return;
                    }
                    //If this also returns null, try the next event
                }
            }
        }

        private State triggerCurrentStateEvent(Event triggerEvent)
        {

            Type eListenerType = typeof(EventListenerAttribute);
            Type eStateType = typeof(State);

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

        private bool preservePrepare(State s)
        {
            Type ePreserveType = typeof(PreservePrepareMove);
            if(s.GetType().GetCustomAttributes(ePreserveType, false).Length > 0)
            {
                return true;
            }
            return false;
        } 


    }
}
