using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RedisLib
{
    public class StackExchangeRedisClient
    {
        #region 初始化 客户端
        private static ConnectionMultiplexer _instance;
        private static object _locker = new object();

        //[ConnectionMultiplexer对象是StackExchange.Redis最中枢的对象。这个类的实例需要被整个应用程序域共享和重用的]
        public static ConnectionMultiplexer redisClient
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null || !_instance.IsConnected)
                        {
                            _instance = GetManager();
                        }
                    }
                }
                return _instance;
            }
        }

        //通过字符串配置连接 
        private static ConnectionMultiplexer GetManager(string connectionString = null)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = GetDefaultConnectionString();
                //带密码 连接配置
                string password = (ConfigurationManager.AppSettings["redisPassword"] ?? "").Trim();
                connectionString = connectionString + ",password=" + password;

                //可以将一个字符串转换为ConfigurationOptions 或者 将一个ConfigurationOptions转换为字符串 。
                //推荐的用法是将基础信息保存在一个字符串中，然后在运行是通过ConfigurationOptions改变其他信息。
                //ConfigurationOptions options = ConfigurationOptions.Parse(connectionString);
                //string configString = options.ToString();
                //options.AllowAdmin = true;
                //options.Password = password;
            }
            ConnectionMultiplexer connect = ConnectionMultiplexer.Connect(connectionString);
            //此处可以 注入事件 [ConnectionRestored ErrorMessage ConfigurationChanged HashSlotMoved InternalError]
            //_redis.ConnectionFailed += MuxerConnectionFailed;
            return connect;
        }

        private static string GetDefaultConnectionString()
        {
            return (ConfigurationManager.AppSettings["redisServer"] ?? "").Trim();
            //多个连接通过逗号分割 redis0:6380,redis1:6380[主从]
        }

        #endregion

        #region 通用方法

        #region Get & Set [Data Structures 1 : String]

        public static string StringGet(string key, int dbIndex = 0)
        {
            string value = GetDB(dbIndex).StringGet(key);
            return value;
        }

        public static T StringGet<T>(string key, int dbIndex = 0)
        {
            try
            {
                string value = GetDB(dbIndex).StringGet(key);
                if (string.IsNullOrWhiteSpace(value))
                {
                    return default(T);
                }
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception)
            {
                return default(T);
            }
            
        }

        public static void StringSet(string key, string value, int dbIndex = 0, TimeSpan? expiry = null)
        {
            bool flag = GetDB(dbIndex).StringSet(key, value, expiry);
        }

        public static void StringSet<T>(string key, T value, int dbIndex = 0, TimeSpan? expiry = null)
        {
            //小技巧 命名redis的key的时候最好的加上前缀，并且使用 ：来分割前缀 ,这里在使用可视化工具查看的时候就比较好区分，比如我的的前缀是 Demo:XXXXXX
            string josnString = JsonConvert.SerializeObject(value);
            bool flag = GetDB(dbIndex).StringSet(key, josnString, expiry);
        }

        public static void StringSet<T>(string key, T value, int dbIndex = 0, DateTime? expiryDate = null)
        {
            //小技巧 命名redis的key的时候最好的加上前缀，并且使用 ：来分割前缀 ,这里在使用可视化工具查看的时候就比较好区分，比如我的的前缀是 Demo:XXXXXX
            string josnString = JsonConvert.SerializeObject(value);

            bool flag = GetDB(dbIndex).StringSet(key, josnString, expiryDate == null ? null : expiryDate - DateTime.Now);

        }
           

        //保存多个key value
        public static bool StringSet(List<KeyValuePair<RedisKey, RedisValue>> keyValues, int dbIndex = 0)
        {
            bool flag = redisClient.GetDatabase(dbIndex).StringSet(keyValues.ToArray());
            return flag;
        }


        //Set 的异步设置
        public static async Task StringSetAsync(string key, object value, int dbIndex = 0)
        {
            await redisClient.GetDatabase(dbIndex).StringSetAsync(key, JsonConvert.SerializeObject(value));
        }

        #region Increment & Decrement
        public static long StringIncrement(string key, long incrementStep, int dbIndex = 0)
        {

            //CommandFlags的模式 
            //CommandFlags.FireAndForget Fire - and - Forget,就是发送命令，然后完全不关心最终什么时候完成命令操作。
            //即发即弃：通过配置 CommandFlags 来实现即发即弃功能，在该实例中该方法会立即返回，如果是string则返回null 如果是int则返回0.这个操作将会继续在后台运行，一个典型的用法页面计数器的实现
            long result = GetDB(dbIndex).StringIncrement(key, incrementStep);
            return result;
        }

        public static long StringDecrement(string key, long incrementStep, int dbIndex = 0)
        {
            long result = GetDB(dbIndex).StringDecrement(key, incrementStep);
            return result;
        }

        #endregion

        #endregion


        #region [Data Structures 2 : List 列表 (队列)] 
        //List 里面的value 可以重复 

        //获取queue 长度
        public static long ListLength(string key, int dbIndex = 0)
        {
            long result = GetDB(dbIndex).ListLength(key);
            return result;
        }

        public static long ListLeftPush(string key, string value, int dbIndex = 0)
        {
            long result = GetDB(dbIndex).ListLeftPush(key, value);
            return result;
        }

        public static string ListRightPop(string key, int dbIndex = 0)
        {
            RedisValue result = GetDB(dbIndex).ListRightPop(key);
            return result;
        }

        // 从 source 队列 里面 导出 数据 ->导入到 destination 队列
        //通过使用RPOPLPUSH命令，消费者程序在从主消息队列中取出消息之后再将其插入到备份队列中，直到消费者程序完成正常的处理逻辑后再将该消息从备份队列中删除。
        //同时我们还可以提供一个守护进程，当发现备份队列中的消息过期时，可以重新将其再放回到主消息队列中，以便其它的消费者程序继续处理。
        public static string ListRightPopLeftPush(string source, string destination, int dbIndex = 0)
        {
            RedisValue result = GetDB(dbIndex).ListRightPopLeftPush(source, destination);
            return result;
        }

        // 移除指定ListId的内部List的值【若存在相同值 全部删除】
        public static long ListRemove(string key, string value, int dbIndex = 0)
        {
            long result = GetDB(dbIndex).ListRemove(key, value);
            return result;
        }

        //检索一些最近插入的条目
        public static RedisValue[] ListRange(string key, long start = 0, long stop = -1, int dbIndex = 0)
        {
            RedisValue[] result = GetDB(dbIndex).ListRange(key, start, stop);
            return result;
        }

        #endregion

        #region [Data Structures 3 : Set 集合 ]

        public static bool SetAdd(string key, string value, int dbIndex = 0)
        {
            bool result = GetDB(dbIndex).SetAdd(key, value);
            return result;
        }


        #endregion

        #region [Data Structures 4 : Hashes 哈希 ]
        public static bool HashExists(string key, string hashField, int dbIndex = 0)
        {
            bool flag = GetDB(dbIndex).HashExists(key, hashField);
            return flag;
        }

        //存储 单个 数据到hash表
        public static bool HashSet<T>(string key, string hashField, T value, int dbIndex = 0)
        {
            string josnString = JsonConvert.SerializeObject(value);
            bool flag = GetDB(dbIndex).HashSet(key, hashField, josnString);
            return flag;
        }

        //获取hash中的单个值
        public static T HashGet<T>(string key, string hashField, int dbIndex = 0)
        {

            string value = GetDB(dbIndex).HashGet(key, hashField);
            T t = JsonConvert.DeserializeObject<T>(value);
            return t;
        }

        //获取hash中的 所有key值
        public static List<T> HashGet<T>(string key, int dbIndex = 0)
        {
            RedisValue[] values = GetDB(dbIndex).HashKeys(key);
            List<T> t = ConvetList<T>(values);
            return t;
        }

        //移除 hash中的某个值
        public static bool HashDelete(string key, string hashField, int dbIndex = 0)
        {
            bool flag = GetDB(dbIndex).HashDelete(key, hashField);
            return flag;
        }

        //移除 hash中的多个值
        public static long HashDelete(string key, List<RedisValue> hashFields, int dbIndex = 0)
        {
            long num = GetDB(dbIndex).HashDelete(key, hashFields.ToArray());
            return num;
        }

        #region Increment & Decrement
        public static long HashIncrement(string key, string hashField, long incrementStep, int dbIndex = 0)
        {
            long result = GetDB(dbIndex).HashIncrement(key, hashField, incrementStep);
            return result;
        }
        public static long HashDecrement(string key, string hashField, long incrementStep, int dbIndex = 0)
        {
            long result = GetDB(dbIndex).HashDecrement(key, hashField, incrementStep);
            return result;
        }
        #endregion

        #endregion

        #region [Data Structures 5 : Sorted Sets 有序集合 ]

        #endregion

        #region TTL 相关

        //设置过期时间
        public static bool KeyExpire(string key, DateTime expiry, int dbIndex = 0, CommandFlags flags = CommandFlags.None)
        {
            bool value = GetDB(dbIndex).KeyExpire(key, expiry, flags);
            return value;
        }
        public static bool KeyExpire(string key, TimeSpan expiry, int dbIndex = 0, CommandFlags flags = CommandFlags.None)
        {
            bool value = GetDB(dbIndex).KeyExpire(key, expiry, flags);
            return value;
        }

        //获取 key 有效时间
        public static double KeyTimeToLive(string key, int dbIndex = 0)
        {
            double result = 0;
            TimeSpan? time = GetDB(dbIndex).KeyTimeToLive(key);
            if (time != null)
            {
                result = time.Value.TotalSeconds;
            }
            return result;
        }

        #endregion

        #region 其他

        //正则查询 符合条件的 key【存在 server】
        //此方法会阻塞 服务器，尽量避免使用
        public static List<string> KeysQuery(string query)//此处的匹配模式  "*ZS_RedisKey_QRCodeStatistics*" ？
        {
            List<string> keys = new List<string>();
            string connectionString = GetDefaultConnectionString();
            IServer server = redisClient.GetServer(connectionString);
            foreach (var key in server.Keys(pattern: query))
            {
                keys.Add(key);
            }
            return keys;
        }

        //获取全部终结点
        public static EndPoint[] GetEndPoints()
        {

            EndPoint[] endpoints = redisClient.GetEndPoints();
            return endpoints;
        }

        //删除 单个key
        public static bool KeyDelete(string key, int dbIndex = 0)
        {
            return GetDB(dbIndex).KeyDelete(key);
        }

        //删除 多个个key
        public static long KeyDelete(List<string> keys, int dbIndex = 0)
        {
            RedisKey[] redisKeys = ConvertRedisKeys(keys);
            return GetDB(dbIndex).KeyDelete(redisKeys);
        }

        //判断 key 是否存在
        public static bool KeyExists(string key, int dbIndex = 0)
        {
            return GetDB(dbIndex).KeyExists(key);
        }

        //重命名key
        public static bool KeyRename(string key, string newKey, int dbIndex = 0)
        {
            return GetDB(dbIndex).KeyRename(key, newKey);
        }

        #endregion

        #region Publish & Subscribe [当作消息代理中间件使用 一般使用更专业的消息队列[RabbitMQ]来处理这种业务场景]

        public static long Publish(string channel, string message)
        {
            ISubscriber sub = redisClient.GetSubscriber();
            return sub.Publish(channel, message);
        }

        public static void Sunscribe(string channelFrom)
        {
            ISubscriber sub = redisClient.GetSubscriber();
            sub.Subscribe(channelFrom, (channel, message) =>
            {
            });

        }

        #endregion


        #region Transaction
        private static ITransaction CreateTransaction(int dbIndex = -1)
        {
            ITransaction transaction = redisClient.GetDatabase(dbIndex).CreateTransaction();
            return transaction;
        }

        #endregion

        #endregion

        #region 私有方法 
        private static IDatabase GetDB(int dbIndex = -1)
        {
            IDatabase db = redisClient.GetDatabase(dbIndex);
            return db;
        }

        //将List<string> keys 转换成 RedisKey[]
        private static RedisKey[] ConvertRedisKeys(List<string> keys)
        {
            return keys.Select(key => (RedisKey)key).ToArray();

        }

        //将RedisValue[] keys 转换成 List<T> 
        private static List<T> ConvetList<T>(RedisValue[] values)
        {
            List<T> result = new List<T>();
            foreach (var item in values)
            {
                var model = JsonConvert.DeserializeObject<T>(item);
                result.Add(model);
            }
            return result;
        }

        #endregion

        #region redis 事件 【eg：ConnectionFailed ...】
        private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            //添加相关日志
            //e.EndPoint;e.FailureType;e.Exception;
        }


        #endregion
    }
}
