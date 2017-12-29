using Castle.DynamicProxy;

namespace torba
{
   public class TorbaClient
   {
      protected static readonly ProxyGenerator generator = new ProxyGenerator();
   }

   public class TorbaClient<T>: TorbaClient where T: class
   {
      public T Proxy { get; private set; }

      public TorbaClient(T proxied)
      {
         Proxy = (T)generator.CreateClassProxyWithTarget(typeof (T), proxied, 
            new TorbaInterceptorClient(new TorbaInvocationTransport()));
      }
   }
}
