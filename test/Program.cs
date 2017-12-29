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
      public interface ITestClass
      {
         void VoidMethod(int a);
         int IntMethod();
      }

      public class TestClass: ITestClass
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
         ITestClass test = new TorbaClient<ITestClass>(/*new TestClass()*/).Proxy;
         test.VoidMethod(5);
         Console.Out.WriteLine($"IntMethod returned: {test.IntMethod()}");
         Console.ReadKey();
      }
   }
}
