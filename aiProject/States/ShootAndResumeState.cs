using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torque3D.Engine.Util.Enums;
using Torque3D.Util;

namespace RealAI.States
{
    class ShootAndResumeState : State
    {
        private State resumeState;
        public ShootAndResumeState(State resumeState)
        {
            this.resumeState = resumeState;
        }

        public State tick(ref PlayerAction action, FeatureVector vector, Brain brain)
        {
            action = PlayerAction.Shoot;
            return resumeState;
        }
    }
}
