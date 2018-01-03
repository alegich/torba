using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace torba
{
   public class TorbaInterceptorClient: IInterceptor
   {
      private readonly ITorbaTransport transport;

      public TorbaInterceptorClient(ITorbaTransport transport)
      {
         this.transport = transport;
      }

      public void Intercept(IInvocation invocation)
      {
         string className = invocation.Proxy.GetType().GetInterfaces()[0].Name;
            //invocation.InvocationTarget.GetType().Name;
         //List<object> args = invocation.Arguments.ToList();
         //string retTypeName = invocation.Method.ReturnType.ToString();
         //Console.Out.WriteLine($"class: {className}, method: {invocation.Method.Name}, argumens: {string.Join(",", args)}, return type: {retTypeName}");

         ITorbaRequest request = new TorbaRequest(invocation.InvocationTarget, className, invocation.Method.Name, invocation.Arguments);
         ITorbaResponse response = transport.SendRequest(request);

         if (invocation.Method.ReturnType != typeof (void))
         {
            invocation.ReturnValue = response.GetReturnedResult() ??
                                     Activator.CreateInstance(invocation.Method.ReturnType);
         }

         //Console.Out.WriteLine($"Result of invocation: {invocation.ReturnValue}");
      }
   }
}
