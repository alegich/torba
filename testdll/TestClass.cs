using System;

namespace testdll
{
   public class TestClass : ITestClass
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

      string ITestClass.MyStringMethod()
      {
         return "goodbye";
      }
      string ITestClass.StringMethod()
      {
         return "hello";
      }

      public SharedClass Correct(SharedClass param, int i, bool b)
      {
         Console.Out.WriteLine($"<Correct> is called with i={i}, b={b}");
         return new SharedClass(i);
      }
   }
}
