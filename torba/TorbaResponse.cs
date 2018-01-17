namespace torba
{
   public class TorbaResponse: ITorbaResponse
   {
      private readonly object response;

      public TorbaResponse(object response)
      {
         this.response = response;
      }

      public object GetReturnedResult()
      {
         return response;
      }
   }
}
