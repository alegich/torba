using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using torbautils;

namespace torba
{
   class TorbaInterceptorClient: IInterceptor
   {
      private readonly ITorbaTransport transport;

      public TorbaInterceptorClient(ITorbaTransport transport)
      {
         this.transport = transport;
      }

      public void Intercept(IInvocation invocation)
      {
         ITorbaRequest request = new TorbaRequest(invocation.InvocationTarget, invocation.Method.Name, invocation.Arguments);
         ITorbaResponse response = transport.SendRequest(request);

         if (invocation.Method.ReturnType != typeof (void))
         {
            invocation.ReturnValue = response.GetReturnedResult() ??
                                     TorbaUtils.CreateDefaultInstance(invocation.Method.ReturnType);
         }
      }
   }
}
