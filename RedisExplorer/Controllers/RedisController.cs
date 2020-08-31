using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExplorer.Models;
using RedisExplorer.Services;
using StackExchange.Redis;

namespace RedisExplorer.Controllers
{
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly RedisConnectionProvider connectionProvider;

        public RedisController(RedisConnectionProvider connectionProvider)
        {
            this.connectionProvider = connectionProvider;
        }

        [HttpPost]
        [Route("/connect/")]
        public async Task<ActionResult<bool>> Connect([FromBody]RedisConnectionRequest redisConnectionRequest)
        {
            var options = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                Ssl = true,
                Password = redisConnectionRequest.Password,
                ConnectTimeout = 60000,
                SyncTimeout = 60000,
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };

            options.EndPoints.Add(redisConnectionRequest.Host, 6380);

            await connectionProvider.ConnectAsync("test", options);
            return true;
        }

        [HttpGet]
        [Route("/keys/")]
        public async Task<ActionResult<List<RedisSet>>> GetKeys()
        {
            var connection = connectionProvider.TryGetConnection("test");
            var keys = new List<RedisSet>();
            foreach (var endPoint in connection.GetEndPoints())
            {
                var server = connection.GetServer(endPoint);
                var serverKeys = server.KeysAsync();
                await foreach (var key in serverKeys)
                {
                    var set = new RedisSet
                    {
                        Key = key,
                        Type = (await connection.GetDatabase().KeyTypeAsync(key)).ToString()
                    };

                    keys.Add(set);
                }
            }
            return keys;
        }

        [HttpGet]
        [Route("/hashset/")]
        public async Task<ActionResult<Dictionary<string, string>>> GetHashSet(string key)
        {
            var connection = connectionProvider.TryGetConnection("test");
            var set = await connection.GetDatabase().HashGetAllAsync(key);
            var hashSet = new Dictionary<string, string>();

            foreach(var entry in set)
            {
                var unzippedValue = await DecompressAsync(entry.Value);
                hashSet.Add(entry.Name, unzippedValue);
            }

            return hashSet;
        }

        private async Task<string> DecompressAsync(byte[] zippedValue)
        {
            using (var memoryStream = new MemoryStream(zippedValue))
            using (var deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(deflateStream))
            {
                return await streamReader.ReadToEndAsync();
            }
        }
    }
}
