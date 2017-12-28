using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;

namespace torba
{
   public class TorbaInterceptorClient: IInterceptor
   {
      public void Intercept(IInvocation invocation)
      {
         string className = invocation.InvocationTarget.GetType().Name;
         List<object> args = invocation.Arguments.ToList();
         string retType = invocation.Method.ReturnType.ToString();
         Console.Out.WriteLine($"class: {className}, argumens: {string.Join(",", args)}, return type: {retType}");
         // with return type methods, NPE is thrown without Proceed
         invocation.Proceed();
      }
   }
}
