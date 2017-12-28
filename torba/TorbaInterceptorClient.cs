using Castle.DynamicProxy;

namespace torba
{
    public class TorbaInterceptorClient: IInterceptor
    {
       public void Intercept(IInvocation invocation)
       {
          invocation.Proceed();
       }
    }
}
