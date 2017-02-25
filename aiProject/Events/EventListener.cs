using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torque3D.Util;

namespace RealAI.Events
{
    interface EventListener
    {
        Event trigger(FeatureVector vector, Brain brain);
    }
}
