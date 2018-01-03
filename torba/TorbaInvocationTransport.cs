namespace torba
{
   class TorbaInvocationTransport: ITorbaTransport
   {
      public ITorbaResponse SendRequest(ITorbaRequest request)
      {
         object target = request.GetObject();
         object result = target?.GetType().GetMethod(request.GetMethodName())?.Invoke(target, request.GetArguments());
         return new TorbaResponse(result);
      }
   }
}
