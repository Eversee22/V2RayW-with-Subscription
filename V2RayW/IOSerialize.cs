using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace V2RayW
{
    class IOSerialize
    {
        public static JavaScriptSerializer js = new JavaScriptSerializer();

        public static void writingJson(SubsProfile subsProfile, string dir = "")
        {
            string filename = Utils.MD5Sum(subsProfile.remark + "-" + subsProfile.url);
            string json = js.Serialize(subsProfile);
            save(string.Format("{0}\\{1}.json", dir, filename), json);
        }

        private static void save(string path, string jsonStr)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {

                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(jsonStr);
                }
            }
        }

        private static string getJsonStr(string path)
        {
            using (FileStream fsRead = new FileStream(path, FileMode.Open))
            {
                int fsLen = (int)fsRead.Length;
                byte[] heByte = new byte[fsLen];
                int r = fsRead.Read(heByte, 0, heByte.Length);
                return System.Text.Encoding.UTF8.GetString(heByte);
            }
        }

        public static SubsProfile readingJson(string path)
        {
            string jsonStr = getJsonStr(path);
            SubsProfile temp = js.Deserialize<SubsProfile>(jsonStr);
            return temp;
        }
    }
}
