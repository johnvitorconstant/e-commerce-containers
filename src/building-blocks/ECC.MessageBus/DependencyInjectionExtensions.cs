using Microsoft.Extensions.DependencyInjection;

namespace ECC.MessageBus;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMessageBus(this IServiceCollection services, string connection)
    {
        if (string.IsNullOrWhiteSpace(connection)) throw new ArgumentException();

        services.AddSingleton<IMessageBus>(new MessageBus(connection));
        return services;
    }
    
}