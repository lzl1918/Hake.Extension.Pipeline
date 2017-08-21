using Hake.Extension.Pipeline.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public delegate Task PipelineDelegate(Context context);
    public class Context : IContext
    {
        private StringBuilder resultBuilder = new StringBuilder();
        public string Result { get { return resultBuilder.ToString(); } }

        public void AppendResult(string text)
        {
            resultBuilder.AppendLine(text);
        }
    }
    public interface IAppBuilder : IPipeline<PipelineDelegate, IAppBuilder, Context>
    {

    }
    public sealed class AppBuilder : PipelineBase<PipelineDelegate, IAppBuilder, Context>, IAppBuilder
    {
        private static PipelineDelegate baseComponent = (context) =>
        {
            context.AppendResult("404");
            return Task.CompletedTask;
        };

        public AppBuilder() : base(baseComponent)
        {

        }
    }
    public static class AppBuilderExtensions
    {
        public static IAppBuilder Use(this IAppBuilder builder, Func<Context, Func<Task>, Task> component)
        {
            return builder.Use(next =>
            {
                return context =>
                {
                    Func<Task> callnext = () => next(context);
                    return component(context, callnext);
                };
            });
        }
    }
}
