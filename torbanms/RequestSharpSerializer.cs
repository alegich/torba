using System;
using System.Collections.Generic;
using System.IO;
using Apache.NMS;
using Polenter.Serialization;
using torba;
using torbautils;

namespace torbanms
{
   class RequestSharpSerializer: IRequestSerializer
   {
      public IMessage CreateMessage(ITorbaRequest request, IMessageProducer producer)
      {
         IMapMessage message = producer.CreateMapMessage();
         SharpSerializer serializer = new SharpSerializer(true);

         Type targetObjectType = request.GetObject().GetType();
         message.Body.SetBytes("targetObjectType", SerializeObject(targetObjectType, serializer));
         message.Body.SetString("targetObjectName", request.GetObject().GetType().Name);
         message.Body.SetString("targetMethodName", request.GetMethodName());
         message.Body.SetInt("targetMethodArgCount", request.GetArguments().Length);

         if (request.GetArguments().Length > 0)
         {
            for (int i = 0; i < request.GetArguments().Length; ++i)
            {
               byte[] serializedArg = SerializeObject(request.GetArguments()[i], serializer);
               message.Body.SetBytes($"targetArgument{i}", serializedArg);
               message.Body.SetBytes($"targetArgumentType{i}", SerializeObject(request.GetArguments()[i].GetType(), serializer));
            }
         }

         return message;
      }

      protected byte[] SerializeObject(object obj, SharpSerializer serializer)
      {
         byte[] retVal;

         using (MemoryStream objStream = new MemoryStream())
         {
            serializer.Serialize(obj, objStream);
            retVal = objStream.ToArray();
         }

         return retVal;
      }

      protected object DeserializeObject(byte[] data, SharpSerializer serializer)
      {
         object retVal;

         using (MemoryStream objStream = new MemoryStream(data))
         {
            retVal = serializer.Deserialize(objStream);
         }

         return retVal;
      }

      protected Type DeserializeType(byte[] data, SharpSerializer serializer)
      {
         object typeObj = DeserializeObject(data, serializer);

         return typeObj as Type;
      }

      public ITorbaRequest CreateRequest(IMessage message)
      {
         ITorbaRequest retVal = new TorbaRequest(null, string.Empty, new object[] { });
         var mapMessage = message as IMapMessage;
         if (mapMessage != null)
         {
            //string objectName = mapMessage.Body.GetString("targetObjectName");
            string methodName = mapMessage.Body.GetString("targetMethodName");
            int argCount = mapMessage.Body.GetInt("targetMethodArgCount");
            List<object> args = new List<object>(argCount);

            SharpSerializer serializer = new SharpSerializer(true);

            if (argCount > 0)
            {
               for (int i = 0; i < argCount; ++i)
               {
                  Type argType = DeserializeType(mapMessage.Body.GetBytes($"targetArgumentType{i}"), serializer);
                  object arg = DeserializeObject(mapMessage.Body.GetBytes($"targetArgument{i}"), serializer);
                  args.Add(arg);
               }
            }

            Type targetObjectType = DeserializeType(mapMessage.Body.GetBytes("targetObjectType"), serializer);
            object targetObject = TorbaUtils.CreateDefaultInstance(targetObjectType);

            retVal = new TorbaRequest(targetObject, methodName, args.ToArray());
         }

         return retVal;
      }
   }
}
