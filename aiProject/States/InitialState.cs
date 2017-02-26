using RealAI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torque3D.Engine.Util.Enums;
using Torque3D.Util;

namespace RealAI.States
{
    [PreservePrepareMove]
    class InitialState : State
    {
        private bool b = true;

        public State tick(ref PlayerAction action, FeatureVector vector, Brain brain)
        {
            Console.WriteLine(vector.TickCount);
            Console.Write("isPreparedMove: ");
            Console.WriteLine(brain.isPreparedMove());
            Console.Write("hasCachedPrepareMove: ");
            Console.WriteLine(brain.hasCachedPrepareMove());
            Console.WriteLine("");
            action = b ? PlayerAction.Prepare : PlayerAction.None;
            b = false;
            return this;
        }


        [EventListener]
        public void testOnEvent(SampleEvent e)
        {
            Console.Out.WriteLine(e.msg);
        }
    }


}
