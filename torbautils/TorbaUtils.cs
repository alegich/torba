using System;
using System.Linq;

namespace torbautils
{
    public static class TorbaUtils
    {
       public static object CreateDefaultInstance(Type type)
       {
         object retVal;

         if (type.IsPrimitive)
         {
            retVal = Activator.CreateInstance(type);
         }
         else
         {
            var constructor = type.GetConstructors().OrderBy(c => c.GetParameters().Length).FirstOrDefault();

            if (constructor == null)
            {
               throw new MissingMethodException($"No public constructor defined for object of type {type.FullName}");
            }

            retVal = constructor.Invoke(new object[constructor.GetParameters().Length]);
         }

         return retVal;
      }
   }
}
