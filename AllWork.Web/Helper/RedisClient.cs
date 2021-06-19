using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllWork.Web.Helper.Redis
{
    /// <summary>
    /// Redis帮助类
    /// </summary>
    public class RedisClient
    {
        private ConnectionMultiplexer connectionMultiplexer;
        IDatabase db = null;
        static RedisClient _redisClient;
        static readonly object Locker = new object();

        public RedisClient()
        {

        }

        public static RedisClient redisClient
        {
            get
            {
                if (_redisClient == null)
                {
                    lock (Locker)
                    {
                        if (_redisClient == null)
                        {
                            _redisClient = new RedisClient();
                        }
                    }

                }
                return _redisClient;
            }
        }

        public void InitConnect(IConfiguration configuration)
        {
            try
            {
                var conStr = configuration.GetConnectionString("RedisConn");
                connectionMultiplexer = ConnectionMultiplexer.Connect(conStr);
                db = connectionMultiplexer.GetDatabase();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                connectionMultiplexer = null;
                db = null;
            }
        }

        public bool SetStringKey(string key, string value, TimeSpan? expiry = default)
        {
            return db.StringSet(key, value, expiry);
        }

        public string GetStringKey(string key)
        {
            return db.StringGet(key);
        }

        public T GetStringKey<T>(string key)
        {
            if (db == null)
            {
                return default;
            }
            var value = db.StringGet(key);
            if (value.IsNullOrEmpty)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(value);
        }

        public bool SetStringKey<T>(string key, T obj, TimeSpan? expiry = default)
        {
            if (db == null)
            {
                return false;
            }
            string json = JsonConvert.SerializeObject(obj);
            return db.StringSet(key, json, expiry);
        }

        public bool KeyDelete(string key)
        {
            if (db == null)
            {
                return false;
            }
            try
            {
                return db.KeyDelete(key);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
