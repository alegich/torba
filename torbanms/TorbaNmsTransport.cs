using System;
using System.Collections;
using System.IO;
using Apache.NMS;
using Polenter.Serialization;
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
         object responseRaw = null;

         using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
         {
            IDestination queue = Apache.NMS.Util.SessionUtil.GetQueue(session, "queue://timecalc.write");

            IQueue responseQueue = session.CreateTemporaryQueue();
            IMessageProducer producer = session.CreateProducer(queue);
            IMessageConsumer consumer = session.CreateConsumer(responseQueue);

            IMessage message = CreateMessage(request, producer);

            message.NMSReplyTo = responseQueue;
            message.NMSCorrelationID = responseQueue.QueueName;
            producer.Send(message);
            IMessage response = consumer.Receive(TimeSpan.FromSeconds(1));
            if (response is IObjectMessage)
            {
               responseRaw = (response as IObjectMessage).Body;
            }
         }

         return new TorbaResponse(responseRaw);
      }

      protected virtual IMessage CreateMessage(ITorbaRequest request, IMessageProducer producer)
      {
         IMapMessage message = producer.CreateMapMessage();
         message.Body.SetString("targetObjectName", request.GetObject().GetType().Name);
         message.Body.SetString("targetMethodName", request.GetMethodName());

         if (request.GetArguments().Length > 0)
         {
            SharpSerializer serializer = new SharpSerializer(true);

            for (int i = 0; i < request.GetArguments().Length; ++i)
            {
               using (MemoryStream argStream = new MemoryStream())
               {
                  serializer.Serialize(request.GetArguments()[i], argStream);
                  byte[] serializedArg = argStream.ToArray();
                  message.Body.SetBytes($"targetArgument{i}", serializedArg);
               }
            }
         }

         return message;
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
