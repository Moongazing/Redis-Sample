using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    public class RedisService
    {
        private readonly ConnectionMultiplexer connectionMultiplexer;
        
        public RedisService(string url)
        {
            connectionMultiplexer = ConnectionMultiplexer.Connect(url);
        }
        public IDatabase GetDb(int dbIndex)
        {
            return connectionMultiplexer.GetDatabase(dbIndex);
        }
    }
}
