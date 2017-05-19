using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace Test
{
    [TestClass]
    public class PipelineTest
    {
        [TestMethod]
        public void TestOutput()
        {
            IAppBuilder app = new AppBuilder();
            PipelineDelegate coms = app.Build();
            Context context = new Context();
            coms(context).Wait();
            CompareLines(new string[] { "404" }, context.Result);

            context = new Context();
            app.Use(async (ctx, next) =>
            {
                ctx.AppendResult("403");
                await next();
                ctx.AppendResult("405");
            });
            coms = app.Build();
            coms(context).Wait();
            CompareLines(new string[] { "403", "404", "405" }, context.Result);
        }

        private void CompareLines(IEnumerable<string> values, string result)
        {
            StringReader reader = new StringReader(result);
            string line;
            foreach (string value in values)
            {
                line = reader.ReadLine();
                if (line == null)
                    Assert.Fail();
                if (line != value)
                    Assert.Fail();
            }
            line = reader.ReadLine();
            if (line != null)
                Assert.Fail();
        }

    }
}
