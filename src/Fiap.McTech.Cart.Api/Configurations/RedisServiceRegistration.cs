using Fiap.McTech.Cart.Api.DbContext;
using StackExchange.Redis;

namespace Fiap.McTech.Cart.Api.Configurations
{
    public static class RedisServiceRegistration
    {
        public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            string redisConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION")
                ?? configuration.GetConnectionString("RedisConnection")
                ?? throw new MissingFieldException("Redis connection is not configured. Set the 'REDIS_CONNECTION' environment variable or 'RedisConnection' in the configuration file.");

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                return ConnectionMultiplexer.Connect(redisConnectionString);
            });

            services.AddSingleton<RedisDataContext>();
        }
    }
}
