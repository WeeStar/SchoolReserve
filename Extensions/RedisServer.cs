using CSRedis;
using Wee_XYYY.Model.Common;

namespace Wee_XYYY.Extensions;

public static class RedisServer
{
    public static CSRedisClient? Cache;

    public static void Initalize()
    {
        Cache = new CSRedisClient(AppSettings.Configuration["RedisServer:Cache"]);
    }
}