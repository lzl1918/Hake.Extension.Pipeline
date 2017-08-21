using System;
using System.Collections.Generic;

namespace Hake.Extension.Pipeline.Abstraction
{
    public abstract class PipelineBase<TDelegate, TImplement, TContext> : IPipeline<TDelegate, TImplement, TContext>
        where TContext : IContext
        where TImplement : class, IPipeline<TDelegate, TImplement, TContext>
    {
        private List<Func<TDelegate, TDelegate>> components = new List<Func<TDelegate, TDelegate>>();
        private TDelegate baseComponent;
        public PipelineBase(TDelegate baseComponent)
        {
            if (baseComponent == null)
                throw new ArgumentNullException(nameof(baseComponent));

            this.baseComponent = baseComponent;
        }

        public virtual TDelegate Build()
        {
            TDelegate app = baseComponent;
            foreach (Func<TDelegate, TDelegate> component in components)
                app = component(app);
            return app;
        }

        public virtual TImplement Use(Func<TDelegate, TDelegate> component)
        {
            components.Insert(0, component);
            return this as TImplement;
        }
    }
}
