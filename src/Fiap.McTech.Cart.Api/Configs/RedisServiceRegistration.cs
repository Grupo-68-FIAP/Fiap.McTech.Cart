using Fiap.McTech.Cart.Api.DbContext;
using StackExchange.Redis;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.McTech.Cart.Api.Configs
{
    [ExcludeFromCodeCoverage]
    public static class RedisServiceRegistration
    {
        public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            string redisConnectionString = configuration.GetConnectionString("RedisConnection");

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                return ConnectionMultiplexer.Connect(redisConnectionString);
            });

            services.AddSingleton<IRedisDataContext, RedisDataContext>();
        }
    }
}
