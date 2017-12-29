namespace torba
{
   public interface ITorbaRequest
   {
      object GetObject();
      string GetTargetObjectName();
      string GetMethodName();
      object[] GetArguments();
   }
}
