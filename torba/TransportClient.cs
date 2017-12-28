using Castle.DynamicProxy;

namespace torba
{
   public class TransportClient<T> where T: class
   {
      private static readonly ProxyGenerator generator = new ProxyGenerator();

      public T Proxy { get; private set; }

      public TransportClient(T proxied)
      {
         Proxy = (T)generator.CreateClassProxyWithTarget(typeof (T), proxied, new TorbaInterceptorClient());
      }
   }
}
