using System.Collections.Generic;

namespace torba
{
   class TorbaRequest: ITorbaRequest
   {
      private readonly string targetName;
      private readonly string targetMethodName;
      private readonly List<object> arguments;
      private readonly object targetObject;

      public TorbaRequest(object targetObject, string targetObectName, string targetMethodName, object[] arguments)
      {
         this.targetObject = targetObject;
         targetName = targetObectName;
         this.targetMethodName = targetMethodName;
         this.arguments = new List<object>(arguments);
      }

      public object GetObject()
      {
         return targetObject;
      }

      public string GetTargetObjectName()
      {
         return targetName;
      }

      public string GetMethodName()
      {
         return targetMethodName;
      }

      public object[] GetArguments()
      {
         return arguments.ToArray();
      }
   }
}
