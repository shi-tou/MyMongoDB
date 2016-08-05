using MongoDB.Bson;
using MongoDB.Driver;
using MongodbView.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MongodbView
{
    /// <summary>
    /// 时间：2016-08-05 晚上
    /// 作者：斌仔
    /// </summary>
    public partial class MongoView : Form
    {
        /// <summary>
        /// Mongo服务
        /// </summary>
        public MongoService service = null;
        public string connectionString;
        public string currentCollection;

        public MongoView()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            this.textBox_MongoUrl.Text = ConfigurationManager.AppSettings["Mongodb"];
            this.pagerControl1.Enabled = false;
            Init();
        }
        /// <summary>
        /// 初始化(获取mongodb信息)
        /// </summary>
        public void Init()
        {
            this.treeView1.Nodes.Clear();
            connectionString = this.textBox_MongoUrl.Text;
            service = new MongoService(new MongoConnectionString(connectionString));
            //所有数据库
            var listDatabase = service.client.ListDatabases().ToList();
            foreach(BsonDocument dbDoc in listDatabase)
            {
                var dbName = dbDoc["name"].ToString();
                TreeNode dbNode = new TreeNode(dbName, 0, 0);
                dbNode.Tag = "database";

                var db = service.client.GetDatabase(dbName);
                //所有集合(可以理解为表)
                var listCollection = db.ListCollections().ToList();
                foreach (BsonDocument collDoc in listCollection)
                {
                    var collName = collDoc["name"].ToString();
                    TreeNode collNode = new TreeNode(collName,1,1);
                    collNode.Tag = "collection";
                    dbNode.Nodes.Add(collNode);
                }
                this.treeView1.Nodes.Add(dbNode);
            }
        }

        /// <summary>
        /// 选择节点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = e.Node;
            if (node.Tag.ToString() != "collection")
            {
                return;
            }
            TreeNode parentNode = node.Parent;
            string dbName = parentNode.Text;
            string collName = node.Text;
            currentCollection = collName;
            Hashtable hs = new Hashtable();
            
            switch (collName)
            {
                case "ErrorLog":
                    PagingList<ErrorLog> pagingList1 = service.PageList<ErrorLog>(hs, this.pagerControl1.PageIndex, this.pagerControl1.PageSize, "CreateTime", true);
                    this.dataGridView1.DataSource = pagingList1.ReturnData;
                    pagerControl1.DrawControl((int)pagingList1.TotalCount);
                    break;
                default:
                    break;
            }
            this.pagerControl1.Enabled = true;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Init();
        }
        /// <summary>
        /// 分页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pagerControl1_OnPageChanged(object sender, EventArgs e)
        {
            Hashtable hs = new Hashtable();
            switch (currentCollection)
            {
                case "ErrorLog":
                    PagingList<ErrorLog> pagingList1 = service.PageList<ErrorLog>(hs, this.pagerControl1.PageIndex, this.pagerControl1.PageSize, "CreateTime", true);
                    this.dataGridView1.DataSource = pagingList1.ReturnData;
                    pagerControl1.DrawControl((int)pagingList1.TotalCount);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 插入测试数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {
            List<ErrorLog> data1 = new List<ErrorLog>();
            List<InfoLog> data2 = new List<InfoLog>();
            List<WaringLog> data3 = new List<WaringLog>();
            for (int i=0; i < 100; i++)
            {
                data1.Add(new ErrorLog
                {
                    Code = Guid.NewGuid().ToString(),
                    CreateTime = DateTime.Now.ToString(),
                    Module = "ErrorLog",
                    RequestData = "",
                    ResponseData = ""
                });
               
            }
            service.Insert<ErrorLog>(data1);
        }
        /// <summary>
        /// 清空测试数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            service.Delete<ErrorLog>();
        }

        #region 测试例子
        /// <summary>
        /// 测试
        /// </summary>
        public void Test()
        {
            try
            {
                MongoService mongo = new MongoService("Mongodb");
                Hashtable hs = new Hashtable();
                //插入单条
                ErrorLog log = new ErrorLog { Code = "2016001", CreateTime = DateTime.Now.ToString(), Module = "ErrorLog", RequestData = "", ResponseData = "" };
                //mongo.Insert(log);
                //插入批量
                List<ErrorLog> insertData = new List<ErrorLog>();
                insertData.Add(new ErrorLog { Code = "2016002", CreateTime = DateTime.Now.ToString(), Module = "ErrorLog", RequestData = "", ResponseData = "" });
                insertData.Add(new ErrorLog { Code = "2016003", CreateTime = DateTime.Now.ToString(), Module = "ErrorLog", RequestData = "", ResponseData = "" });
                insertData.Add(new ErrorLog { Code = "2016004", CreateTime = DateTime.Now.ToString(), Module = "ErrorLog", RequestData = "", ResponseData = "" });
                insertData.Add(new ErrorLog { Code = "2016005", CreateTime = DateTime.Now.ToString(), Module = "ErrorLog", RequestData = "", ResponseData = "" });
                insertData.Add(new ErrorLog { Code = "2016006", CreateTime = DateTime.Now.ToString(), Module = "ErrorLog", RequestData = "", ResponseData = "" });
                insertData.Add(new ErrorLog { Code = "2016007", CreateTime = DateTime.Now.ToString(), Module = "ErrorLog", RequestData = "", ResponseData = "" });
                insertData.Add(new ErrorLog { Code = "2016008", CreateTime = DateTime.Now.ToString(), Module = "ErrorLog", RequestData = "", ResponseData = "" });
                insertData.Add(new ErrorLog { Code = "2016009", CreateTime = DateTime.Now.ToString(), Module = "ErrorLog", RequestData = "", ResponseData = "" });
                insertData.Add(new ErrorLog { Code = "2016010", CreateTime = DateTime.Now.ToString(), Module = "ErrorLog", RequestData = "", ResponseData = "" });
                //mongo.Insert(insertData);
                //获取数据
                List<ErrorLog> list1 = mongo.List<ErrorLog>();
                //获取指定条件数据
                hs.Clear();
                hs.Add("Code", "2016009");
                List<ErrorLog> list2 = mongo.List<ErrorLog>(hs);
                //分布获取数据
                hs.Clear();
                PagingList<ErrorLog> list3 = mongo.PageList<ErrorLog>(hs, 1, 8, "CreateTime", true);
                //删除数据
                //mongo.Delete();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

       
    }

}