using NUnit.Framework;
using TinaX.Systems.Pipeline;

namespace TinaX.Tests.Core.Systems.Pipeline
{
    public interface IMeowHandler
    {
        bool SayMeow();
    }

    public class MeowHandler : IMeowHandler
    {

        public string Name = "";

        public MeowHandler() { }
        public MeowHandler(string name) { this.Name = name; }

        public bool SayMeow()
        {
            TestContext.WriteLine("Meow~~ :" + Name);
            return true;
        }
    }

    public class GeneralPipelineTest
    {
        [Test]
        public void DoPipelineTest()
        {
            XPipeline<IMeowHandler> pipeline = new XPipeline<IMeowHandler>();
            pipeline.AddFirst(new MeowHandler("step 1"));
            pipeline.AddFirst(new MeowHandler("step 0"));
            pipeline.AddLast(new MeowHandler("step 2"));
            pipeline.AddLast(new MeowHandler("step 3"));
            pipeline.AddLast(new MeowHandler("step 4"));

            TestContext.WriteLine("喵呜");
            pipeline.Start(handler =>
            {
                return handler.SayMeow();
            });

            TestContext.WriteLine("TENET");

            pipeline.StartReverse(handler =>
            {
                return handler.SayMeow();
            });
        }
    }
}
