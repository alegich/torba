using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

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
                                     CreateDefaultInstance(invocation.Method.ReturnType);
         }
      }

      private object CreateDefaultInstance(Type type)
      {
         object retVal = null;

         if (type.IsPrimitive)
         {
            retVal = Activator.CreateInstance(type);
         }
         else
         {
            var constructor = type.GetConstructors().OrderBy(c => c.GetParameters().Length).FirstOrDefault();

            if (constructor == null)
            {
               throw new MissingMethodException($"No public constructor defined for object of type {type.FullName}");
            }

            retVal = constructor.Invoke(new object[constructor.GetParameters().Length]);
         }

         return retVal;
      }
   }
}
