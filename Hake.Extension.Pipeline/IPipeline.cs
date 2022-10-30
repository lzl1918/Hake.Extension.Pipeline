using System;

namespace Hake.Extension.Pipeline;

public interface IPipeline<TDelegate, TContext>
    where TDelegate : notnull, Delegate
    where TContext : notnull, IContext
{
    TDelegate Build();

    IPipeline<TDelegate, TContext> Use(Func<TDelegate, TDelegate> component);
}
