using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongodbView.Model
{
    public class InfoLog : BaseModel
    {
        public string Code;
        public string Module;
        public string RequestData;
        public string ResponseData;
        public string CreateTime;
    }
}
