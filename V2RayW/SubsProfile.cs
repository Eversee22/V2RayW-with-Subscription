using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V2RayW
{
    public class SubsProfile
    {
        public string remark;
        public string url;
        public int groupId;
        //public int id;
        public List<ServerProfile> VmessConfList;
        public List<SubsProfile> SubsRefList;

        public SubsProfile(string url, string remark = "")
        {
            init(url, remark);
        }

        public SubsProfile()
        {
            init("", "");
        }

        private void init(string url, string remark)
        {
            this.remark = remark;
            this.url = url;
            groupId = 0;
            //id = -1;
            VmessConfList = new List<ServerProfile>();
            SubsRefList = new List<SubsProfile>();
        }
    }
}
