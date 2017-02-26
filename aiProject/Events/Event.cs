using RealAI.States;
using System;
using Torque3D.Util;

namespace RealAI
{
    public interface Event
    {
        State onTrigger(FeatureVector vector, Brain brain);
    }

}
