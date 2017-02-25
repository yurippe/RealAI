using RealAI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealAI
{
    class InitialState : State
    {

        [EventListener]
        public void testOnEvent(OnGetShotEvent e)
        {
            Console.Out.WriteLine(e.msg);
        }

    }


}
