using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace V2RayW
{
    public partial class FormSubsSetting : Form
    {
        //public static List<SubProfile> subs = new List<SubProfile>();

        public FormSubsSetting()
        {
            InitializeComponent();
            I18N.InitControl(this);
        }

        private void FormSubsSetting_Load(object sender, EventArgs e)
        {
            foreach (SubsProfile item in Program.subsProfiles)
            {
                listBoxSubs.Items.Add(item.remark + "-" + item.url);
            }
        }

        private void buttonAddSub_Click(object sender, EventArgs e)
        {
            string url = textBoxSubUrl.Text;
            if (url.Length > 0)
            {
                string remark = textBoxRemark.Text;
                if (remark.Length == 0)
                {
                    remark = "new" + (Program.subsProfiles.Count);
                }
                SubsProfile sub = new SubsProfile(url, remark);
                sub.groupId = Program.subsProfiles.Count();
                while (Program.groupSubsMap.ContainsKey(sub.groupId))
                    sub.groupId++;
                Program.subsProfiles.Add(sub);
                Program.subsProfiles[0].SubsRefList.Add(sub);
                Program.groupSubsMap.Add(sub.groupId, sub);
                DataRow dr = Program.dt.NewRow();
                dr["remark"] = remark;
                dr["url"] = url;
                //dr["groupId"] = sub.groupId;
                Program.dt.Rows.Add(dr);
                Program.bs.ResetBindings(false);
                listBoxSubs.Items.Add(remark + "-" + url);
            }
        }

        //-
        private void textBoxSubUrl_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            var selectedIndex = listBoxSubs.SelectedIndex;
            if (selectedIndex > 0 && selectedIndex < Program.subsProfiles.Count)
            {
                listBoxSubs.Items.RemoveAt(selectedIndex);
                SubsProfile deletedSubs = Program.subsProfiles[selectedIndex];
                Program.subsProfiles.Remove(deletedSubs);
                Program.subsProfiles[0].SubsRefList.Remove(deletedSubs);
                deletedSubs.VmessConfList.Clear();
                if (Program.groupSubsMap.ContainsKey(deletedSubs.groupId))
                    Program.groupSubsMap.Remove(deletedSubs.groupId);

                Program.dt.Rows.RemoveAt(selectedIndex);
                Program.bs.ResetBindings(false);
            }
        }
    }
}
