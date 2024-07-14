using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Otus.Teaching.Pcf.ReceivingFromPartner.Core.Domain;
using StackExchange.Redis;
using System;
using System.Text.Json;


namespace Otus.Teaching.Pcf.RedisCashManager.WebHost.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PreferencesController : ControllerBase
    {
        private readonly IDatabase _cache;
        private readonly ILogger<PreferencesController> _logger;

        public PreferencesController(ILogger<PreferencesController> logger, IDatabase cache)
        {
            _logger = logger;
            _cache = cache.Multiplexer.GetDatabase(); ;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var values = GetValuesFromPreferences();
            if (values == null || values.Count == 0)
                return NotFound();

            return Ok(values);
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> Get(Guid guid)
        {
            var key = $"preference:{guid}";
            if(!await _cache.KeyExistsAsync(key))
                return NotFound();

            return Ok(JsonSerializer.Deserialize<Preference>(await _cache.StringGetAsync(key)));
        }

        #region Tools

        private List<Preference>? GetValuesFromPreferences()
        {
            var keys = new List<string>();
            var prefix = "*preference*";

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            var server = redis.GetServer("localhost:6379");

            foreach (var key in server.Keys(pattern: prefix))
            {
                keys.Add(key);
            }

            var values = new List<Preference>();
            foreach (var key in keys)
            {
                values.Add(JsonSerializer.Deserialize<Preference>(_cache.StringGet(key)));
            }

            return values;
        }

        #endregion
    }
}
