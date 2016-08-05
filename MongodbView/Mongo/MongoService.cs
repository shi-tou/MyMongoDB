using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MongodbView
{
    /// <summary>
    /// Mongodb服务类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MongoService
    {
        public MongoClient client = null;
        public IMongoDatabase database = null;
        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="connectionStringName"></param>
        public MongoService(string connectionStringName)
        {
            MongoConnectionString connectionString = new MongoConnectionString(ConfigurationManager.AppSettings[connectionStringName]);
            client = new MongoClient(connectionString.Host);
            database = client.GetDatabase(connectionString.DatabaseName);
        }
        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="connectionStringName"></param>
        public MongoService(MongoConnectionString connectionString)
        {
            client = new MongoClient(connectionString.Host);
            database = client.GetDatabase(connectionString.DatabaseName);
        }
        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <param name="t"></param>
        public void Insert<T>(T t)
        {
            IMongoCollection<T> collection = database.GetCollection<T>(typeof(T).Name);
            collection.InsertOne(t);
        }
        /// <summary>
        /// 插入单条数据(异步)
        /// </summary>
        /// <param name="t"></param>
        public void InsertAsync<T>(T t)
        {
            IMongoCollection<T> collection = database.GetCollection<T>(typeof(T).Name);
            collection.InsertOneAsync(t);
        }
        
        /// <summary>
        /// 插入多条数据
        /// </summary>
        /// <param name="list"></param>
        public void Insert<T>(List<T> list)
        {
            IMongoCollection<T> collection = database.GetCollection<T>(typeof(T).Name);
            collection.InsertMany(list);
        }
        /// <summary>
        /// 插入多条数据(异步)
        /// </summary>
        /// <param name="list"></param>
        public void InsertAsync<T>(List<T> list)
        {
            IMongoCollection<T> collection = database.GetCollection<T>(typeof(T).Name);
            collection.InsertManyAsync(list);
        }
        /// <summary>
        /// 删除所有数据
        /// </summary>
        public void Delete<T>()
        {
            IMongoCollection<T> collection = database.GetCollection<T>(typeof(T).Name);
            var filter =new BsonDocument();
            collection.DeleteMany(filter);
        }
        /// <summary>
        /// 删除所有数据(异步)
        /// </summary>
        public void DeleteAsync<T>()
        {
            IMongoCollection<T> collection = database.GetCollection<T>(typeof(T).Name);
            var filter = new BsonDocument();
            collection.DeleteManyAsync(filter);
        }
        /// <summary>
        /// 删除指定条件的数据
        /// </summary>
        public void Delete<T>(string key,object value)
        {
            IMongoCollection<T> collection = database.GetCollection<T>(typeof(T).Name);
            var filter = Builders<T>.Filter.Eq(key, value);
            collection.DeleteMany(filter);
        }
        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        public List<T> List<T>()
        {
            IMongoCollection<T> collection = database.GetCollection<T>(typeof(T).Name);
            var filter = new BsonDocument();
            return collection.Find(filter).ToList();
        }
        /// <summary>
        /// 查询数据（默认-条件与）
        /// </summary>
        /// <returns></returns>
        public List<T> List<T>(Hashtable hs)
        {
            IMongoCollection<T> collection = database.GetCollection<T>(typeof(T).Name);
            var builder = Builders<T>.Filter;
            var filter = builder.Empty;
            foreach (string key in hs.Keys)
            {
                filter &= builder.Eq(key, hs[key]);
            }
            return collection.Find(filter).ToList();
        }
        /// <summary>
        /// 查询数据（条件或）
        /// </summary>
        /// <returns></returns>
        public List<T> ListOr<T>(Hashtable hs)
        {
            IMongoCollection<T> collection = database.GetCollection<T>(typeof(T).Name);
            var builder = Builders<T>.Filter;
            var filter = builder.Empty;
            foreach (string key in hs.Keys)
            {
                filter |= builder.Eq(key, hs[key]);
            }
            return collection.Find(filter).ToList();
        }
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="hs">条件</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="sortFiled">排序字段</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        public PagingList<T> PageList<T>(Hashtable hs, int pageIndex, int pageSize, string sortFiled, bool isAsc)
        {
            IMongoCollection<T> collection = database.GetCollection<T>(typeof(T).Name);
            var builder = Builders<T>.Filter;
            var filter = builder.Empty;
            foreach (string key in hs.Keys)
            {
                filter &= builder.Eq(key, hs[key]);
            }
            var sort = isAsc ? Builders<T>.Sort.Ascending(sortFiled) : Builders<T>.Sort.Descending(sortFiled);
            PagingList<T> result = new PagingList<T>();
            result.TotalCount = collection.Count(filter);
            result.ReturnData = collection.Find(filter).Sort(sort).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
            return result;
        }
    }
}
