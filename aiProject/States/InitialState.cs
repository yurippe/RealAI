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

        public State tick(ref PlayerAction action, FeatureVector vector, Brain brain)
        {
            action = PlayerAction.Prepare;
            return this; //What this returns decides how the program behaves basically
        }

    }


}
