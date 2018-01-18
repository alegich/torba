namespace testdll
{
   public interface ITestClass
   {
      void VoidMethod(int a);
      int IntMethod();
      string MyStringMethod();
      string StringMethod();
      SharedClass Correct(SharedClass inParam, int i, bool b);
   }
}
