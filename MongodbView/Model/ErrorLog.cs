using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongodbView.Model
{
    public class ErrorLog : BaseModel
    {
        public string Code { get; set; }
        public string Module { get; set; }
        public string RequestData { get; set; }
        public string ResponseData { get; set; }
        public string CreateTime { get; set; }
    }
}
