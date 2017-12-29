using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using torba;

namespace test
{
   public class Program
   {
      public class TestClass
      {
         virtual public void VoidMethod(int a)
         {
            Console.Out.WriteLine($"VoidMethod is called with {a}");
         }

         virtual public int IntMethod()
         {
            Console.Out.WriteLine("IntMethod is called");
            return 22;
         }
      }
      static void Main(string[] args)
      {
         TestClass test = new TorbaClient<TestClass>(new TestClass()).Proxy;
         test.VoidMethod(5);
         Console.Out.WriteLine($"IntMethod returned: {test.IntMethod()}");
         Console.ReadKey();
      }
   }
}
