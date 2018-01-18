using Apache.NMS;
using torba;

namespace torbanms
{
   interface IRequestSerializer
   {
      IMessage CreateMessage(ITorbaRequest request, IMessageProducer producer);

      ITorbaRequest CreateRequest(IMessage message);
   }
}
