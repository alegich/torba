using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testdll
{
   public class SharedClass
   {
      public int Start { get; }

      public SharedClass() { Start = 0; }

      public SharedClass(int start)
      {
         Start = start;
      }

      public override string ToString()
      {
         return $"Start={Start}";
      }
   }
}
