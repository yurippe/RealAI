using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torque3D.Util;

namespace RealAI.Events
{

    class OnGetShotEventListener : EventListener
    {

        public Event trigger(FeatureVector vector, Brain brain)
        {
            return new OnGetShotEvent("hello from event");
        }

    }

    class OnGetShotEvent : Event
    {
        public string msg { get; }

        public OnGetShotEvent(string message)
        {
            msg = message;
        }

    }
}
