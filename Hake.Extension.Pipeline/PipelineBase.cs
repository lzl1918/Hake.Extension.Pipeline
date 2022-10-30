using System;
using System.Collections.Generic;

namespace Hake.Extension.Pipeline;

public abstract class PipelineBase<TDelegate, TContext>
    : IPipeline<TDelegate, TContext>
    where TDelegate : notnull, Delegate
    where TContext : notnull, IContext
{
    private readonly List<Func<TDelegate, TDelegate>> components = new List<Func<TDelegate, TDelegate>>();
    private readonly TDelegate baseComponent;

    public PipelineBase(TDelegate baseComponent)
    {
        if (baseComponent == null)
        {
            throw new ArgumentNullException(nameof(baseComponent));
        }

        this.baseComponent = baseComponent;
    }

    public virtual TDelegate Build()
    {
        TDelegate app = baseComponent;
        foreach (Func<TDelegate, TDelegate> component in components)
        {
            app = component(app);
        }

        return app;
    }

    public virtual IPipeline<TDelegate, TContext> Use(Func<TDelegate, TDelegate> component)
    {
        components.Insert(0, component);
        return this;
    }
}
