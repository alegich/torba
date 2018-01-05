using Castle.DynamicProxy;

namespace torba
{
   public class TorbaClient
   {
      protected static readonly ProxyGenerator generator = new ProxyGenerator();
   }

   public class TorbaClient<T>: TorbaClient where T: class
   {
      public T CreateProxy(T proxied)
      {
         return generator.CreateInterfaceProxyWithTarget(proxied,
            new TorbaInterceptorClient(new TorbaInvocationTransport()));
      }

      public T CreateProxy()
      {
         return generator.CreateInterfaceProxyWithoutTarget<T>(
               new TorbaInterceptorClient(new TorbaInvocationTransport()));
      }
   }
}
