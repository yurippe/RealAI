using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torque3D.Engine.Util.Enums;
using Torque3D.Util;

namespace RealAI.States
{
    class DebugState : State
    {
        public State tick(ref PlayerAction action, FeatureVector vector, Brain brain)
        {
            //Console.Out.WriteLine(vector.TickCount);
            action = PlayerAction.Prepare;
            return this;
        }
    }
}
