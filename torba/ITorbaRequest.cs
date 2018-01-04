using System;

namespace torba
{
   public interface ITorbaRequest
   {
      object GetObject();
      string GetMethodName();
      object[] GetArguments();
   }
}
