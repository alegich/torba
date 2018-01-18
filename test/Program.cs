using System;
using testdll;
using torba;
using torbanms;

namespace test
{
   public class Program
   {
      static void Main(string[] args)
      {
         ITestClass test = new TorbaClient<ITestClass>(new TorbaNmsTransport()).CreateProxy(new TestClass());
         test.VoidMethod(5);
         Console.Out.WriteLine($"StringMethod returned: {test.StringMethod()}");
         Console.Out.WriteLine($"IntMethod returned: {test.IntMethod()}");
         SharedClass newA = test.Correct(new SharedClass(1), 7, true);
         Console.Out.WriteLine($"newA: {newA}");
         Console.ReadKey();
      }
   }
}
