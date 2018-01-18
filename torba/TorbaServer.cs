namespace torba
{
   public class TorbaServer<T> where T : class
   {
      private readonly ITorbaTransport transport;

      public TorbaServer(ITorbaTransport transport)
      {
         this.transport = transport;
      }

      public int Run()
      {
         transport.ProcessRequests();
         return 0;
      }
   }
}
