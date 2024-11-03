using StackExchange.Redis;

namespace Fiap.McTech.Cart.Api.DbContext
{
    public interface IRedisDataContext : IDisposable
    {
        IDatabase Database { get; }
        Task<bool> SetValueAsync(string key, string value, TimeSpan? expiry = null);
        Task<string> GetValueAsync(string key);
        Task<bool> DeleteKeyAsync(string key);
    }

    public class RedisDataContext : IRedisDataContext
    {
        private readonly ConnectionMultiplexer _connection;
        private readonly IDatabase _database;

        public RedisDataContext(string connectionString)
        {
            _connection = ConnectionMultiplexer.Connect(connectionString);
            _database = _connection.GetDatabase();
        }

        public IDatabase Database => _database;

        public async Task<bool> SetValueAsync(string key, string value, TimeSpan? expiry = null)
        {
            return await _database.StringSetAsync(key, value, expiry);
        }

        public async Task<string> GetValueAsync(string key)
        {
            return await _database.StringGetAsync(key);
        }

        public async Task<bool> DeleteKeyAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
