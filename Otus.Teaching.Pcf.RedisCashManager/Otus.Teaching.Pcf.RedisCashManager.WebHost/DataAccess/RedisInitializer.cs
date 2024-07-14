using Microsoft.Extensions.Caching.Distributed;
using Otus.Teaching.Pcf.ReceivingFromPartner.Core.Domain;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;

namespace Otus.Teaching.Pcf.RedisCashManager.WebHost.DataAccess
{
    public class RedisInitializer(IDatabase cache) : IDbInitializer
    {
        private readonly IDatabase _cache = cache;

        public void InitializeDb()
        {
            foreach(var preference in FakeDataFactory.Preferences)
            {
                var key = $"preference:{preference.Id}";
                var value = JsonSerializer.Serialize(preference); ;

                //var options = new DistributedCacheEntryOptions
                //{
                //    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                //};

                _cache.StringSet(key, value);
            }
        }
    }
}
