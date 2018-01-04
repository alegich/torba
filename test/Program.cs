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
      public class A
      {
         public int Start { get; }

         public A(int start)
         {
            Start = start;
         }

         public override string ToString()
         {
            return $"Start={Start}";
         }
      }

      public interface ITestClass
      {
         void VoidMethod(int a);
         int IntMethod();

         string StringMethod();

         A Correct(A inParam, int i, bool b);
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

         public string StringMethod()
         {
            return "hello";
         }

         public A Correct(A param, int i, bool b)
         {
            Console.Out.WriteLine($"<Correct> is called with i={i}, b={b}");
            return new A(i);
         }
      }
      static void Main(string[] args)
      {
         ITestClass test = new TorbaClient<ITestClass>(/*new TestClass()*/).Proxy;
         test.VoidMethod(5);
         Console.Out.WriteLine($"StringMethod returned: {test.StringMethod()}");
         Console.Out.WriteLine($"IntMethod returned: {test.IntMethod()}");
         A newA = test.Correct(new A(1), 7, true);
         Console.Out.WriteLine($"newA: {newA}");
         Console.ReadKey();
      }
   }
}
