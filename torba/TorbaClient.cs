using Castle.DynamicProxy;

namespace torba
{
   public class TorbaClient<T> where T: class
   {
      private static readonly ProxyGenerator generator = new ProxyGenerator();

      public T Proxy { get; private set; }

      public TorbaClient(T proxied)
      {
         Proxy = (T)generator.CreateClassProxyWithTarget(typeof (T), proxied, new TorbaInterceptorClient());
      }
   }
}
