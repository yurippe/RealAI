using System;
using Torque3D;
using Torque3D.Engine.Util.Enums;
using Torque3D.Util;

namespace RealAI
{
   public class AIClass
   {

        private static Brain brain = new Brain(); //We need some object that is not static to do the thinking, duh

      [ConsoleFunction]
      public static PlayerAction RealAI(FeatureVector vector)
      {
            // Implement your AI logic here, compile and then run Game.exe
            return brain.tick(vector);
      }
   }
}