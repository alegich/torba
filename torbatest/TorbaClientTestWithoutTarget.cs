using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using torba;

namespace torbatest
{
   [TestClass]
   public class TorbaClientTestWithoutTarget
   {
      public interface IVoidMethod
      {
         void CallMe();
      }

      [TestMethod]
      public void TestTorbaClientWithoutTargetObject_VoidMethod()
      {
         TorbaClient<IVoidMethod> client = new TorbaClient<IVoidMethod>();

         try
         {
            client.CreateProxy().CallMe();
         }
         catch (Exception e)
         {
            Assert.Fail(e.ToString());
         }
      }

      public interface IVoidMethodWithArgs
      {
         void CallMe(int i, string str, object obj, bool b);
      }

      [TestMethod]
      public void TestTorbaClientWithoutTargetObject_VoidMethodWithArgs()
      {
         TorbaClient<IVoidMethodWithArgs> client = new TorbaClient<IVoidMethodWithArgs>();

         try
         {
            client.CreateProxy().CallMe(3, "some str", 5, true);
         }
         catch (Exception e)
         {
            Assert.Fail(e.ToString());
         }
      }

      public interface IStringMethodWithArgs
      {
         string CallMe(int i, string str, object obj, bool b);
      }

      [TestMethod]
      public void TestTorbaClientWithoutTargetObject_StringMethodWithArgs()
      {
         TorbaClient<IStringMethodWithArgs> client = new TorbaClient<IStringMethodWithArgs>();

         string result = client.CreateProxy().CallMe(3, "some str", 5, true);

         Assert.AreEqual(string.Empty, result);
      }

      public interface IIntMethodWithArgs
      {
         int CallMe(int i, string str, object obj, bool b);
      }

      [TestMethod]
      public void TestTorbaClientWithoutTargetObject_IntMethodWithArgs()
      {
         TorbaClient<IIntMethodWithArgs> client = new TorbaClient<IIntMethodWithArgs>();

         int result = client.CreateProxy().CallMe(3, "some str", 5, true);

         Assert.AreEqual(0, result);
      }

      public interface IObjMethodWithArgs
      {
         object CallMe(int i, string str, object obj, bool b);
      }

      [TestMethod]
      public void TestTorbaClientWithoutTargetObject_ObjMethodWithArgs()
      {
         TorbaClient<IObjMethodWithArgs> client = new TorbaClient<IObjMethodWithArgs>();

         object result = client.CreateProxy().CallMe(3, "some str", 5, true);

         Assert.IsNotNull(result);
         Assert.IsInstanceOfType(result, typeof(object));
      }


      public class CustomObject
      {
         public int Start { get; }

         public CustomObject(int start)
         {
            Start = start;
         }

         public override string ToString()
         {
            return $"Start={Start}";
         }
      }

      public interface ICustomObjMethodWithArgs
      {
         CustomObject CallMe(int i, string str, object obj, bool b);
      }

      [TestMethod]
      public void TestTorbaClientWithoutTargetObject_CustomObjMethodWithArgs()
      {
         TorbaClient<ICustomObjMethodWithArgs> client = new TorbaClient<ICustomObjMethodWithArgs>();

         CustomObject result = client.CreateProxy().CallMe(3, "some str", 5, true);

         Assert.IsNotNull(result);
         Assert.IsInstanceOfType(result, typeof(CustomObject));
         Assert.AreEqual(0, result.Start);
      }

      public class CustomObjWithoutPublicCtor
      {
         private CustomObjWithoutPublicCtor()
         {
         }
      }

      public interface ICustomPrivateObjMethodWithArgs
      {
         CustomObjWithoutPublicCtor CallMe(int i, string str, object obj, bool b);
      }

      [TestMethod]
      public void TestTorbaClientWithoutTargetObject_CustomPrivateObjMethodWithArgs()
      {
         TorbaClient<ICustomPrivateObjMethodWithArgs> client = new TorbaClient<ICustomPrivateObjMethodWithArgs>();

         try
         {
            client.CreateProxy().CallMe(3, "some str", 5, true);

            Assert.Fail("Expected exception is not thrown");
         }
         catch (MissingMethodException)
         {
         }
         catch (Exception e)
         {
            Assert.Fail("Incorrect exception is thrown: {0}", e);
         }
      }
   }
}
