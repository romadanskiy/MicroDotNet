using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace RuOverflow.Questions.Infrastructure.Handlers;

public static class HandlerRegisterExtensions
{
        private static readonly Type[] AllHandlerTypes = new[]
            { typeof(IAsyncHandler<>), typeof(IAsyncHandler<,>), typeof(IHandler<>), typeof(IHandler<,>) };

        public static void RegisterHandlers(this IServiceCollection services, Assembly assembly)
        {
            Array.ForEach(AllHandlerTypes, x => services.AddHandlerFrom(assembly, x));
            services.AddDecoratorsFrom(assembly);
        }

        private static void AddHandlerFrom(this IServiceCollection services, Assembly assembly,
            Type openGenericInterfaceType, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            services.Scan(scan => scan
                .FromAssemblies(new[] { assembly })
                .AddClasses(classes =>
                    classes.AssignableTo(openGenericInterfaceType)
                        .Where(type => !Attribute.IsDefined(type, typeof(DecoratorAttribute))))
                .AsImplementedInterfaces()
                .WithLifetime(serviceLifetime));
        }

        private static void AddDecoratorsFrom(this IServiceCollection services, Assembly assembly)
        {
            var decorators = GetAllDecoratorTypesFromAssemblyInCorrectOrder(assembly);

            foreach (var decorator in decorators)
            {
                var handlerInterfaces = decorator.GetInterfaces();

                var openHandlerInterface = GetMostSpecificOpenGenericHandlerInterface(handlerInterfaces);

                var closedHandlerInterface =
                    GetMostSpecificClosedGenericHandlerInterface(handlerInterfaces, openHandlerInterface);

                services.Decorate(closedHandlerInterface, decorator);
            }
        }

        private static TypeInfo[] GetAllDecoratorTypesFromAssemblyInCorrectOrder(Assembly assembly)
        {
            var decorators = assembly.DefinedTypes
                .Where(x => Attribute.IsDefined(x, typeof(DecoratorAttribute)) && !x.IsAbstract)
                .OrderBy(x => x.GetCustomAttribute<DecoratorAttribute>()!.Order)
                .ToArray();

            return decorators;
        }

        private static Type GetMostSpecificOpenGenericHandlerInterface(Type[] handlerInterfaces)
        {
            var openHandlerInterface =
                AllHandlerTypes.First(x => handlerInterfaces.Any(z => z.GetGenericTypeDefinition() == x));

            return openHandlerInterface;
        }

        private static Type GetMostSpecificClosedGenericHandlerInterface(Type[] handlerInterfaces,
            Type openHandlerInterface)
        {
            var closedHandlerInterface =
                handlerInterfaces.First(x => x.GetGenericTypeDefinition() == openHandlerInterface);

            return closedHandlerInterface;
        }
}
