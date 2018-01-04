using System;
using System.Collections.Generic;

namespace torba
{
   class TorbaRequest: ITorbaRequest
   {
      private readonly string targetMethodName;
      private readonly List<object> arguments;
      private readonly object targetObject;

      public TorbaRequest(object targetObject, string targetMethodName, object[] arguments)
      {
         this.targetObject = targetObject;
         this.targetMethodName = targetMethodName;
         this.arguments = new List<object>(arguments);
      }

      public object GetObject()
      {
         return targetObject;
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
