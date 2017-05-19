using System;
using System.Threading.Tasks;

namespace Hake.Extension.Pipeline.Abstraction
{
    public interface IPipeline<TDelegate, TImplement, TContext>
        where TContext : IContext
        where TImplement : class, IPipeline<TDelegate, TImplement, TContext>
    {
        TDelegate Build();

        TImplement Use(Func<TDelegate, TDelegate> component);
    }
}
