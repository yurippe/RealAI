using System;
using Torque3D.Engine.Util.Enums;
using Torque3D.Util;

namespace RealAI
{ 

    public interface State
    {
        State tick(ref PlayerAction action, FeatureVector vector, Brain brain);
    }

}
