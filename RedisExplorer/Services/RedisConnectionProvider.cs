using System.Collections.Concurrent;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace RedisExplorer.Services
{
    public class RedisConnectionProvider
    {
        private readonly ConcurrentDictionary<string, ConnectionMultiplexer> connections;

        public RedisConnectionProvider()
        {
            connections = new ConcurrentDictionary<string, ConnectionMultiplexer>();
        }

        public ConnectionMultiplexer TryGetConnection(string id)
        {
            connections.TryGetValue(id, out ConnectionMultiplexer connectionMultiplexer);
            return connectionMultiplexer;
        }

        private void TrySetConnection(string id, ConnectionMultiplexer connection)
        {
            connections.TryAdd(id, connection);
        }

        public async Task<ConnectionMultiplexer> ConnectAsync(string id, ConfigurationOptions options)
        {
            var connection = await ConnectionMultiplexer.ConnectAsync(options);
            TrySetConnection(id, connection);
            return connection;
        }
    }
}
