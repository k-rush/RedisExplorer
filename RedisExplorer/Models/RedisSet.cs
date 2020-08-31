using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisSet
    {
        public string Key { get; set; }
        public string Type { get; set; }
    }
}
