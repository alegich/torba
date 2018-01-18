using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Apache.NMS;
using torba;
using torbautils;

namespace torbanms
{
   class RequestJsonSerializer: IRequestSerializer
   {
      public IMessage CreateMessage(ITorbaRequest request, IMessageProducer producer)
      {
         IMapMessage message = producer.CreateMapMessage();
         JavaScriptSerializer serializer = new JavaScriptSerializer();

         message.Body.SetString("targetObjectType", serializer.Serialize(request.GetObject().GetType()));
         message.Body.SetString("targetObjectName", request.GetObject().GetType().Name);
         message.Body.SetString("targetMethodName", request.GetMethodName());
         message.Body.SetInt("targetMethodArgCount", request.GetArguments().Length);

         if (request.GetArguments().Length > 0)
         {
            for (int i = 0; i < request.GetArguments().Length; ++i)
            {
               object origArg = request.GetArguments()[i];
               string serializedArg = serializer.Serialize(origArg);
               string serializedArgType = serializer.Serialize(origArg.GetType());
               message.Body.SetString($"targetArgument{i}", serializedArg);
               message.Body.SetString($"targetArgumentType{i}", serializedArgType);
               object arg = serializer.Deserialize(serializedArg, origArg.GetType());
               Type argType = serializer.Deserialize<Type>(serializedArgType);
            }
         }

         return message;
      }

      public ITorbaRequest CreateRequest(IMessage message)
      {
         ITorbaRequest retVal = new TorbaRequest(null, string.Empty, new object[] {});
         var mapMessage = message as IMapMessage;
         if (mapMessage != null)
         {
            //string objectName = mapMessage.Body.GetString("targetObjectName");
            string methodName = mapMessage.Body.GetString("targetMethodName");
            int argCount = mapMessage.Body.GetInt("targetMethodArgCount");
            List<object> args = new List<object>(argCount);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            if (argCount > 0)
            {
               for (int i = 0; i < argCount; ++i)
               {
                  Type argType = serializer.Deserialize<Type>(mapMessage.Body.GetString($"targetArgumentType{i}"));
                  object arg = serializer.Deserialize(mapMessage.Body.GetString($"targetArgument{i}"), argType);
                  args.Add(arg);
               }
            }

            Type targetObjectType = serializer.Deserialize<Type>(mapMessage.Body.GetString("targetObjectType"));
            object targetObject = TorbaUtils.CreateDefaultInstance(targetObjectType);

            retVal = new TorbaRequest(targetObject, methodName, args.ToArray());
         }

         return retVal;
      }
   }
}
