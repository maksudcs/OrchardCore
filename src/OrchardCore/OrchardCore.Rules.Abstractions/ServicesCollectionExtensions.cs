using System;
using Microsoft.Extensions.DependencyInjection;

namespace OrchardCore.Rules
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCondition(this IServiceCollection services, Type condition, Type conditionEvaluator, Type conditionFactory)
        {
            services.Configure<ConditionOptions>(o =>
            {
                o.AddCondition(condition, conditionEvaluator, conditionFactory);
            });

            // Rules are scoped so that during a request rules like the script rule can build the scripting engine once.
            services.AddScoped(conditionEvaluator);            

            var descriptor = new ServiceDescriptor(typeof(IConditionFactory), conditionFactory, ServiceLifetime.Singleton);
            services.Add(descriptor);            

            return services;
        }

        public static IServiceCollection AddCondition<TCondition, TConditionEvaluator, TConditionFactory>(this IServiceCollection services)
            where TCondition : Condition
            where TConditionEvaluator : IConditionEvaluator
            where TConditionFactory : IConditionFactory
            => services.AddCondition(typeof(TCondition), typeof(TConditionEvaluator), typeof(TConditionFactory));            
    }
}