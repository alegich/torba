using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Apache.NMS;
using Polenter.Serialization;
using torba;

namespace torbanms
{
   public class TorbaNmsTransport : ITorbaTransport, IDisposable
   {
      private readonly IConnection connection;

      private ISession session;

      private readonly IRequestSerializer serializer;

      public TorbaNmsTransport()
      {
         serializer = new RequestSharpSerializer();
         //serializer = new RequestJsonSerializer();
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
         object responseRaw = null;

         using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
         {
            IDestination queue = Apache.NMS.Util.SessionUtil.GetQueue(session, "queue://timecalc.write");

            IQueue responseQueue = session.CreateTemporaryQueue();
            IMessageProducer producer = session.CreateProducer(queue);
            IMessageConsumer consumer = session.CreateConsumer(responseQueue);

            IMessage message = serializer.CreateMessage(request, producer);

            message.NMSReplyTo = responseQueue;
            message.NMSCorrelationID = responseQueue.QueueName;
            producer.Send(message);
            IMessage response = consumer.Receive(TimeSpan.FromSeconds(100));
            if (response is IObjectMessage)
            {
               responseRaw = (response as IObjectMessage).Body;
            }
         }

         return new TorbaResponse(responseRaw);
      }

      public void ProcessRequests()
      {
         session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);

         IDestination queue = Apache.NMS.Util.SessionUtil.GetQueue(session, "queue://timecalc.write");

         IMessageConsumer consumer = session.CreateConsumer(queue);

         while (true)
         {
            Consumer_Listener(consumer.Receive());
         }
      }

      protected ITorbaResponse InvokeRequest(ITorbaRequest request)
      {
         ITorbaTransport invocationTransport = new TorbaInvocationTransport();
         return invocationTransport.SendRequest(request);
      }

      private void Consumer_Listener(IMessage message)
      {
         ITorbaRequest request = serializer.CreateRequest(message);

         if (request != null)
         {
            ITorbaResponse response = InvokeRequest(request);
            Task.Factory.StartNew(
               () => SendResponse(response, message.NMSReplyTo, message.NMSCorrelationID));
         }
      }

      protected void SendResponse(ITorbaResponse response, IDestination replyTo, string correlationId)
      {
         IMessageProducer producer = session.CreateProducer(replyTo);
         IMessage responseMessage = producer.CreateObjectMessage(response.GetReturnedResult());
         responseMessage.NMSCorrelationID = correlationId;
         producer.Send(responseMessage);
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
