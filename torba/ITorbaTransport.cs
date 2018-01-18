namespace torba
{
   public interface ITorbaTransport
   {
      ITorbaResponse SendRequest(ITorbaRequest request);

      void ProcessRequests();
   }
}
