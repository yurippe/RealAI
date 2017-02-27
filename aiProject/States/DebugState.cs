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
            if (vector.TickCount == 1) { Console.WriteLine(vector.DeltaMovedX + ", " + vector.DeltaMovedY + "," + vector.DeltaRot); }
            action = PlayerAction.TurnRight;
            return this;
            int i = (new Random()).Next(4);
            if(i == 0)
            {
                action = PlayerAction.MoveForward;
            } else if (i == 1)
            {
                action = PlayerAction.TurnLeft;
            } else if (i == 2 || i == 3)
            {
                action = PlayerAction.TurnRight;
            }
            
            return this;
        }
    }
}
