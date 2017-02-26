using RealAI.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torque3D.Util;

namespace RealAI.Events
{
    class HighChanceOfHitEventListener : EventListener
    {
        public Event trigger(FeatureVector vector, Brain brain)
        {
            if(vector.DamageProb > 0.8f && vector.ShootDelay <= 1) //<= 1 is a bug, and it should be <= 0 or just == 0, but let's exploit this while we can
            {
                return new HighChanceOfHitEvent();
            }
            return null;
        }
    }

    class HighChanceOfHitEvent : Event
    {
        public State onTrigger(FeatureVector vector, Brain brain)
        {
            return new ShootAndResumeState(brain.getCurrentState());
        }
    }
}
