using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongodbView
{
    /// <summary>
    /// Mongodb连接配置信息（）
    /// </summary>
    public sealed class MongoConnectionString
    {
        /// <summary>
        /// 原始连接字符串
        /// </summary>
        private readonly string originalConnectionString;
        private string host;
        private string databaseName;

        /// <summary>
        /// 主机地址
        /// </summary>
        public string Host { get { return host; } }
        /// <summary>
        /// 数据库名
        /// </summary>
        public string DatabaseName { get { return databaseName; } }

        /// <summary>
        /// 补始化实例
        /// </summary>
        /// <param name="connectionString"></param>
        public MongoConnectionString(string connectionString)
        {
            originalConnectionString = connectionString;
            Parse();
        }
        /// <summary>
        /// 解析连接字符串
        /// </summary>
        private void Parse()
        {
            try
            {
                int index = originalConnectionString.LastIndexOf("/");
                host = originalConnectionString.Substring(0, index);
                databaseName = originalConnectionString.Substring(index + 1);
            }
            catch
            {
                var message = string.Format("The connection string '{0}' is not valid.", originalConnectionString);
                throw new MongoConfigurationException(message);
            }
        }
        /// <summary>
        /// 重载ToString方法，返回连接字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return originalConnectionString;
        }
    }
}
