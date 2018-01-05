using Castle.DynamicProxy;

namespace torba
{
   public class TorbaClient
   {
      protected static readonly ProxyGenerator generator = new ProxyGenerator();
   }

   public class TorbaClient<T>: TorbaClient where T: class
   {
      private readonly ITorbaTransport transport;

      public TorbaClient()
      {
         transport = new TorbaInvocationTransport();
      }

      public TorbaClient(ITorbaTransport transport)
      {
         this.transport = transport;
      }

      public T CreateProxy(T proxied)
      {
         return generator.CreateInterfaceProxyWithTarget(proxied,
            new TorbaInterceptorClient(transport));
      }

      public T CreateProxy()
      {
         return generator.CreateInterfaceProxyWithoutTarget<T>(
               new TorbaInterceptorClient(transport));
      }
   }
}
