using System;
using Apache.NMS;
using torba;

namespace torbanms
{
   public class TorbaNmsTransport : ITorbaTransport, IDisposable
   {
      private readonly IConnection connection;

      public TorbaNmsTransport()
      {
         try
         {
            IConnectionFactory factory = new NMSConnectionFactory("activemq:failover:(tcp://localhost:5672)");
            connection = factory.CreateConnection();
            connection.Start();
         }
         catch (Exception)
         {
            CleanUp();
            throw;
         }
      }

      public ITorbaResponse SendRequest(ITorbaRequest request)
      {
         ITorbaResponse retVal = null;

         using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
         {
            IDestination queue = Apache.NMS.Util.SessionUtil.GetQueue(session, "queue://timecalc.write");

            IQueue responseQueue = session.CreateTemporaryQueue();
            IMessageProducer producer = session.CreateProducer(queue);
            IMessageConsumer consumer = session.CreateConsumer(responseQueue);

            IMapMessage message = producer.CreateMapMessage();
            message.Body.SetString("targetObjectName", request.GetObject().GetType().Name);
            message.Body.SetString("targetMethodName", request.GetMethodName());
            message.Body.SetList("targetMethodArguments", request.GetArguments());

            message.NMSReplyTo = responseQueue;
            message.NMSCorrelationID = responseQueue.QueueName;
            producer.Send(message);
            IMessage response = consumer.Receive(TimeSpan.FromSeconds(10));
            if (response is IObjectMessage)
            {
               object responseRaw = (response as IObjectMessage).Body;
               retVal = new TorbaResponse(responseRaw);
            }
         }

         return retVal;
      }

      private void CleanUp()
      {
         connection?.Dispose();
      }
      public void Dispose()
      {
         CleanUp();
      }
   }
}
