using RealAI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torque3D.Engine.Util.Enums;
using Torque3D.Util;

namespace RealAI
{
    class InitialState : State
    {

        public State tick(ref PlayerAction action, FeatureVector vector, Brain brain)
        {
            return this;
        }


        [EventListener]
        public void testOnEvent(SampleEvent e)
        {
            Console.Out.WriteLine(e.msg);
        }
    }


}
