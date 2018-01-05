using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using torba;

namespace torbatest
{
   [TestClass]
   public class TorbaClientTestWithTarget
   {
      public interface IVoidMethod
      {
         void CallMe();
      }
      public interface IVoidMethodWithArgs
      {
         void CallMeVoid(int i, string str, object obj, bool b);
      }
      public interface IStringMethodWithArgs
      {
         string CallMeString(int i, string str, object obj, bool b);
      }
      public interface IIntMethodWithArgs
      {
         int CallMeInt(int i, string str, object obj, bool b);
      }
      public interface IObjMethodWithArgs
      {
         object CallMeObj(int i, string str, object obj, bool b);
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
         CustomObject CallMeCustomObj(int i, string str, object obj, bool b);
      }

      public interface IOverloadedMethod
      {
         string CallOverload();
         string CallOverload(int over);
      }

      public interface IOverloadedMethodExplicit
      {
         string CallOverloadExplicit();
         string CallOverloadExplicit(int over);
      }

      public class Impl : IVoidMethod, IVoidMethodWithArgs, IStringMethodWithArgs,
         IIntMethodWithArgs, IObjMethodWithArgs, ICustomObjMethodWithArgs, 
         IOverloadedMethod, IOverloadedMethodExplicit
      {
         public void CallMe()
         {
         }

         public void CallMeVoid(int i, string str, object obj, bool b)
         {
         }

         string IStringMethodWithArgs.CallMeString(int i, string str, object obj, bool b)
         {
            return "CallMeString";
         }

         public int CallMeInt(int i, string str, object obj, bool b)
         {
            return 25;
         }

         public object CallMeObj(int i, string str, object obj, bool b)
         {
            return "CallMeObj";
         }

         public CustomObject CallMeCustomObj(int i, string str, object obj, bool b)
         {
            return new CustomObject(88);
         }

         public string CallOverload()
         {
            return "CallOverload";
         }

         public string CallOverload(int over)
         {
            return $"CallOverload {over}";
         }

         string IOverloadedMethodExplicit.CallOverloadExplicit()
         {
            return "CallOverloadExplicit";
         }

         string IOverloadedMethodExplicit.CallOverloadExplicit(int over)
         {
            return $"CallOverloadExplicit {over}";
         }
      }

      [TestMethod]
      public void TestTorbaClientWithTargetObject_VoidMethod()
      {
         TorbaClient<IVoidMethod> client = new TorbaClient<IVoidMethod>(new Impl());

         try
         {
            client.Proxy.CallMe();
         }
         catch (Exception e)
         {
            Assert.Fail(e.ToString());
         }
      }

      [TestMethod]
      public void TestTorbaClientWithTargetObject_VoidMethodWithArgs()
      {
         TorbaClient<IVoidMethodWithArgs> client = new TorbaClient<IVoidMethodWithArgs>(new Impl());

         try
         {
            client.Proxy.CallMeVoid(3, "some str", 5, true);
         }
         catch (Exception e)
         {
            Assert.Fail(e.ToString());
         }
      }

      [TestMethod]
      public void TestTorbaClientWithTargetObject_StringMethodWithArgs()
      {
         TorbaClient<IStringMethodWithArgs> client = new TorbaClient<IStringMethodWithArgs>(new Impl());

         string result = client.Proxy.CallMeString(3, "some str", new object(), true);

         Assert.AreEqual("CallMeString", result);
      }

      [TestMethod]
      public void TestTorbaClientWithTargetObject_IntMethodWithArgs()
      {
         TorbaClient<IIntMethodWithArgs> client = new TorbaClient<IIntMethodWithArgs>(new Impl());

         int result = client.Proxy.CallMeInt(3, "some str", 5, true);

         Assert.AreEqual(25, result);
      }

      [TestMethod]
      public void TestTorbaClientWithTargetObject_ObjMethodWithArgs()
      {
         TorbaClient<IObjMethodWithArgs> client = new TorbaClient<IObjMethodWithArgs>(new Impl());

         object result = client.Proxy.CallMeObj(3, "some str", 5, true);

         Assert.IsNotNull(result);
         Assert.IsInstanceOfType(result, typeof(string));
         Assert.AreEqual("CallMeObj", result);
      }

      [TestMethod]
      public void TestTorbaClientWithTargetObject_CustomObjMethodWithArgs()
      {
         TorbaClient<ICustomObjMethodWithArgs> client = new TorbaClient<ICustomObjMethodWithArgs>(new Impl());

         CustomObject result = client.Proxy.CallMeCustomObj(3, "some str", 5, true);

         Assert.IsNotNull(result);
         Assert.IsInstanceOfType(result, typeof(CustomObject));
         Assert.AreEqual(88, result.Start);
      }

      [TestMethod]
      public void TestTorbaClientWithTargetObject_OverloadedMethod()
      {
         TorbaClient<IOverloadedMethod> client = new TorbaClient<IOverloadedMethod>(new Impl());

         {
            string result = client.Proxy.CallOverload();

            Assert.IsNotNull(result);
            Assert.AreEqual("CallOverload", result);
         }
         {
            string result = client.Proxy.CallOverload(5);

            Assert.IsNotNull(result);
            Assert.AreEqual("CallOverload 5", result);
         }
      }

      [TestMethod]
      public void TestTorbaClientWithTargetObject_OverloadedExcplicitMethod()
      {
         TorbaClient<IOverloadedMethodExplicit> client = new TorbaClient<IOverloadedMethodExplicit>(new Impl());

         {
            string result = client.Proxy.CallOverloadExplicit();

            Assert.IsNotNull(result);
            Assert.AreEqual("CallOverloadExplicit", result);
         }
         {
            string result = client.Proxy.CallOverloadExplicit(7);

            Assert.IsNotNull(result);
            Assert.AreEqual("CallOverloadExplicit 7", result);
         }
      }
   }
}
