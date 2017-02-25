using System;
using Torque3D;
using Torque3D.Engine.Util.Enums;
using Torque3D.Util;

namespace GameAI
{
   public class AIClass
   {
      [ConsoleFunction]
      public static PlayerAction MyThinkMethod(FeatureVector vector)
      {
         // Implement your AI logic here, compile and then run Game.exe
         return PlayerAction.MoveForward;
      }
   }
}