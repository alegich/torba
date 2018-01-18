using System;
using System.Linq;
using System.Reflection;
using Castle.Core.Internal;

namespace torba
{
   public class TorbaInvocationTransport: ITorbaTransport
   {
      public ITorbaResponse SendRequest(ITorbaRequest request)
      {
         object target = request.GetObject();
         Type[] argTypes = request.GetArguments().Select(a => a.GetType()).ToArray();
         MethodInfo method = GetTargetMethod(target?.GetType(), request.GetMethodName(), argTypes);
         object result = method?.Invoke(target, request.GetArguments());
         return new TorbaResponse(result);
      }

      public void ProcessRequests()
      {
      }

      private MethodInfo GetTargetMethod(Type type, string methodName, Type[] argTypes)
      {
         MethodInfo method = type?.GetMethod(methodName, argTypes) ?? GetExplicitInterfaceImpl(type, methodName, argTypes);

         return method;
      }

      private MethodInfo GetExplicitInterfaceImpl(Type type, string methodName, Type[] argTypes)
      {
         MethodInfo[] methods = type?.GetMethods(
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

         return methods?.Find(m => m.IsFinal && m.IsPrivate && m.Name.EndsWith($".{methodName}") 
               && m.GetParameters().Select(p => p.ParameterType).SequenceEqual(argTypes));
      }
   }
}
