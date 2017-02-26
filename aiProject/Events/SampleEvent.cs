using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torque3D.Util;

namespace RealAI.Events
{

    class SampleEventListener : EventListener
    {

        public Event trigger(FeatureVector vector, Brain brain)
        {
            return new SampleEvent("hello from event");
        }

    }

    class SampleEvent : Event
    {
        public string msg { get; }

        public SampleEvent(string message)
        {
            msg = message;
        }

        public State onTrigger(FeatureVector vector, Brain brain)
        {
            return null;
        }
    }
}
