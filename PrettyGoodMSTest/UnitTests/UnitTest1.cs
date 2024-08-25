using PrettyGoodUtilities;
namespace MSTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            UserEnv userEnv = new UserEnv();
            userEnv.SetEnvironmentVariable("TestVariable", "TestValue");
            Assert.AreEqual("TestValue", Environment.GetEnvironmentVariable("TestVariable", EnvironmentVariableTarget.User));
        }
    }
}