using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace V2RayW
{

    public partial class ConfigForm : Form
    {
        private static JavaScriptSerializer js = new JavaScriptSerializer();
        public List<ServerProfile> profiles = new List<ServerProfile>();
        public List<ServerProfile> subsProfiles = new List<ServerProfile>();
        public List<ServerProfile> addedProfiles = new List<ServerProfile>();
        private int localPort = 1081;
        private int httpPort = 8081;
        private bool udpSupport = false;
        private bool shareOverLan = false;
        private string dnsString = "localhost";
        private LogLevel logLevel = LogLevel.none;
        public int selectedServerIndex = 1;
        private int mainInboundType = 0;
        private bool alarmUnknown = true;
        private string suburl = "";
        private int selectedSubsIndex;

       
   
        public ServerProfile SelectedProfile()
        {
            return this.profiles[this.selectedServerIndex];
        }

        public ConfigForm()
        {
            InitializeComponent();
            I18N.InitControl(this);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void buttonSave_Click(object sender, EventArgs e)
        {
            Program.localPort = this.localPort;
            Program.httpPort = this.httpPort;
            Program.udpSupport = this.udpSupport;
            Program.shareOverLan = this.shareOverLan;
            Program.dnsString = String.Copy(this.dnsString);
            Program.logLevel = this.logLevel;
            Program.profiles.Clear();

            foreach (var p in this.profiles)
                if (Program.groupSubsMap.ContainsKey(p.groupId))
                    if (Program.groupSubsMap[p.groupId].VmessConfList.Count() != 0)
                        Program.groupSubsMap[p.groupId].VmessConfList.Clear();
            foreach (var p in this.profiles)
            {
                if (Program.groupSubsMap.ContainsKey(p.groupId))
                    Program.groupSubsMap[p.groupId].VmessConfList.Add(p.DeepCopy());
                Program.profiles.Add(p.DeepCopy());
            };
            if(this.selectedServerIndex < Program.profiles.Count && this.selectedServerIndex >= 0)
            {
                Program.selectedServerIndex = this.selectedServerIndex;
            } else
            {
                if(Program.profiles.Count > 0)
                {
                    Program.selectedServerIndex = 0;
                } else
                {
                    Program.selectedServerIndex = -1;
                }
            }
            Program.selectedServerIndex = this.selectedServerIndex;
            Program.mainInboundType = this.mainInboundType;
            Program.alarmUnknown = this.alarmUnknown;
            Program.configurationDidChange();
            this.Close();
        }

        private void buttonTS_Click(object sender, EventArgs e)
        {
            var tsWindow = new FormTransSetting();
            tsWindow.ShowDialog(this);
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            //copy settings
            this.localPort = Program.localPort;
            this.httpPort = Program.httpPort;
            this.udpSupport = Program.udpSupport;
            this.shareOverLan = Program.shareOverLan;
            this.dnsString = String.Copy(Program.dnsString);
            this.logLevel = Program.logLevel;
            this.profiles.Clear();
            foreach(var p in Program.profiles)
            {
                var tmp = p.DeepCopy();
                this.profiles.Add(tmp);
                if (tmp.groupId == -1)
                    addedProfiles.Add(tmp);
                else
                    subsProfiles.Add(tmp);
            };
            this.selectedServerIndex = Program.selectedServerIndex;
            this.mainInboundType = Program.mainInboundType;
            this.alarmUnknown = Program.alarmUnknown;
            
            // update views
            this.Icon = Properties.Resources.vw256;

            comboBoxInP.SelectedIndex = Program.mainInboundType;
            textBoxLocalPort.Text = this.localPort.ToString();
            checkBoxUDP.Checked = this.udpSupport;
            textBoxHttpPort.Text = this.httpPort.ToString();
            checkBoxShareOverLan.Checked = this.shareOverLan;
            textBoxDNS.Text = this.dnsString;
            comboBoxLog.SelectedIndex = (int)this.logLevel;
            checkBoxAlarm.Checked = this.alarmUnknown;

            //subscription list
            comboBoxSubs.DisplayMember = "remark";
            comboBoxSubs.ValueMember = "url";
            //comboBoxSubs.ValueMember = "groupId";
            comboBoxSubs.DataSource = Program.bs;

            loadProfiles();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            ServerProfile serverProfile = new ServerProfile();
            addedProfiles.Add(serverProfile);
            refreshListServBox(addedProfiles.Count() - 1);
        }

        private void loadProfiles()
        {
            listBoxServers.Items.Clear();
            foreach (var p in profiles)
            {
                listBoxServers.Items.Add(p.remark == "" ? p.address : p.remark);
            }
            if (selectedServerIndex >= 0 && selectedServerIndex < profiles.Count())
            {
                listBoxServers.SelectedIndex = selectedServerIndex;
            }
            listBoxServers_SelectedIndexChanged(null, null);
        }

        private void listBoxServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedServerIndex = listBoxServers.SelectedIndex;
            if (selectedServerIndex >= 0 && selectedServerIndex < this.profiles.Count())
            {
                var sp = profiles[selectedServerIndex];
                textBoxAddress.Text = sp.address;
                textBoxPort.Text = sp.port.ToString();
                textBoxUserId.Text = sp.userId;
                textBoxAlterID.Text = sp.alterId.ToString();
                textBoxRemark.Text = sp.remark;
                comboBoxNetwork.SelectedIndex = (int)sp.network;
                comboBoxSecurity.SelectedIndex = (int)sp.security;
                buttonTS.Enabled = true;
            }
            else
            {
                textBoxPort.Text = "";
                textBoxUserId.Text = "";
                textBoxAlterID.Text = "";
                textBoxRemark.Text = "";
                comboBoxNetwork.SelectedIndex = 0;
                comboBoxSecurity.SelectedIndex = 0;
                buttonTS.Enabled = false;
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (this.profiles.Count <= 0)
            {
                return;
            }
            if(selectedServerIndex >= 0 &&
                selectedServerIndex < this.profiles.Count())
            {
                this.profiles.RemoveAt(selectedServerIndex);
                if (selectedServerIndex >= 0 && selectedServerIndex < addedProfiles.Count())
                    addedProfiles.RemoveAt(selectedServerIndex);
                else if (selectedServerIndex >= addedProfiles.Count())
                {
                    int subsServerInd = selectedServerIndex - addedProfiles.Count();
                    if (Program.groupSubsMap.ContainsKey(subsProfiles[subsServerInd].groupId))
                    {
                        int i;
                        for (i = subsServerInd; i >= 0 && subsProfiles[i].groupId == subsProfiles[subsServerInd].groupId; i--) ;
                        //MessageBox.Show((subsServerInd - i - 1).ToString(), "group index");
                        Program.groupSubsMap[subsProfiles[subsServerInd].groupId]
                            .VmessConfList.RemoveAt(subsServerInd - i - 1);
                    }
                    subsProfiles.RemoveAt(subsServerInd);
                }

                if (this.profiles.Count() == 0)
                {
                    this.profiles.Add(new ServerProfile { remark = "placeholder" });
                }
                selectedServerIndex -= 1;
                if(selectedServerIndex == -1 && this.profiles.Count() > 0 )
                {
                    selectedServerIndex = 0;
                }
                loadProfiles();
            }
        }

        private void textBoxAddress_TextChanged(object sender, EventArgs e)
        {
            if(selectedServerIndex >= 0 && selectedServerIndex < this.profiles.Count())
            {
                this.profiles[selectedServerIndex].address = textBoxAddress.Text;
                loadProfiles();
            }
        }

        private void textBoxPort_TextChanged(object sender, EventArgs e)
        {
            if (selectedServerIndex >= 0 && selectedServerIndex < this.profiles.Count())
            {
                this.profiles[selectedServerIndex].port = Program.strToInt(textBoxPort.Text, 10086);
            }
        }

        private void textBoxUserId_TextChanged(object sender, EventArgs e)
        {
            if (selectedServerIndex >= 0 && selectedServerIndex < this.profiles.Count())
            {
                this.profiles[selectedServerIndex].userId = textBoxUserId.Text;
            }
        }

        private void textBoxAlterID_TextChanged(object sender, EventArgs e)
        {
            if (selectedServerIndex >= 0 && selectedServerIndex < this.profiles.Count())
            {
                this.profiles[selectedServerIndex].alterId = Program.strToInt(textBoxAlterID.Text, 0);
            }
        }

        private void textBoxRemark_TextChanged(object sender, EventArgs e)
        {
            if (selectedServerIndex >= 0 && selectedServerIndex < this.profiles.Count())
            {
                this.profiles[selectedServerIndex].remark = textBoxRemark.Text;
                loadProfiles();
            }
        }
        
        private void comboBoxNetwork_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedServerIndex >= 0 && selectedServerIndex < this.profiles.Count())
            {
                this.profiles[selectedServerIndex].network = (NetWorkType)comboBoxNetwork.SelectedIndex;
            }
        }

        private void comboBoxInP_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.mainInboundType = comboBoxInP.SelectedIndex; // 0: http, 1:socks
        }

        private void comboBoxSecurity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedServerIndex >= 0 && selectedServerIndex < this.profiles.Count())
            {
                this.profiles[selectedServerIndex].security = (SecurityType)comboBoxSecurity.SelectedIndex;
            }
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            var importWindow = new FormImport();
            importWindow.ShowDialog(this);
        }

        private void checkBoxShareOverLan_CheckedChanged(object sender, EventArgs e)
        {
            this.shareOverLan = checkBoxShareOverLan.Checked;
        }

        private void textBoxLocalPort_TextChanged(object sender, EventArgs e)
        {
            this.localPort = Program.strToInt(textBoxLocalPort.Text, 1081);
        }

        private void checkBoxUDP_CheckedChanged(object sender, EventArgs e)
        {
            this.udpSupport = checkBoxUDP.Checked;
        }

        private void textBoxHttpPort_TextChanged(object sender, EventArgs e)
        {
            this.httpPort = Program.strToInt(textBoxHttpPort.Text, 8081);

        }

        private void textBoxDNS_TextChanged(object sender, EventArgs e)
        {
            this.dnsString = textBoxDNS.Text.Trim();
        }

        private void checkBoxAlarm_CheckedChanged(object sender, EventArgs e)
        {
            this.alarmUnknown = checkBoxAlarm.Checked;
        }

        private void comboBoxLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.logLevel = (LogLevel)comboBoxLog.SelectedIndex;
        }

        private void refreshListServBox()
        {
            this.profiles.Clear();
            foreach (var ap in addedProfiles)
                this.profiles.Add(ap);
            foreach (var sp in subsProfiles)
                this.profiles.Add(sp);
            selectedServerIndex = this.profiles.Count() - 1;
            loadProfiles();
        }

        private void refreshListServBox(int selectedInd)
        {
            this.profiles.Clear();
            foreach (var ap in addedProfiles)
                this.profiles.Add(ap);
            foreach (var sp in subsProfiles)
                this.profiles.Add(sp);
            selectedServerIndex = selectedInd;
            loadProfiles();
        }

        private void buttonUpdateSub_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show(this.suburl, "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (res == DialogResult.Yes)
            {
                if (selectedSubsIndex > 0 && selectedSubsIndex < Program.subsProfiles.Count)
                {
                    updateUrl(Program.subsProfiles[selectedSubsIndex]);
                }
                else if (selectedSubsIndex == 0)
                {
                    updateUrl();
                }
            }
        }

        private void buttonAddSubs_Click(object sender, EventArgs e)
        {
            var ssWindow = new FormSubsSetting();
            ssWindow.ShowDialog(this);
        }

        private void comboBoxSubs_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedSubsIndex = comboBoxSubs.SelectedIndex;
            suburl = comboBoxSubs.SelectedValue.ToString();
            //MessageBox.Show("selected " + selectedSubsIndex);

            if (selectedSubsIndex > 0 && selectedSubsIndex < Program.subsProfiles.Count)
            {
                //suburl = Program.subsProfiles[selectedSubsIndex].url;
                subsProfiles.Clear();
                foreach (ServerProfile sp in Program.subsProfiles[selectedSubsIndex].VmessConfList)
                    //ImportVmess(vmess, Program.subsProfiles[selectedSubsIndex].groupId);
                    subsProfiles.Add(sp);
            }
            else if (selectedSubsIndex == 0)
            {
                //suburl = Program.subsProfiles[0].url;
                subsProfiles.Clear();
                foreach (SubsProfile subs in Program.subsProfiles[0].SubsRefList)
                    foreach (ServerProfile sp in subs.VmessConfList)
                        //ImportVmess(vmess, subs.groupId);
                        subsProfiles.Add(sp);
            }

            refreshListServBox();
        }

        private void updateUrl()
        {
            try
            {
                BackgroundWorker subscribeWorker = new BackgroundWorker();
                subscribeWorker.WorkerSupportsCancellation = true;
                subscribeWorker.DoWork += subscribeWorker_DoWork;
                if (subscribeWorker.IsBusy)
                    return;
                subscribeWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "request timeout");
                return;
            }
        }

        private void updateUrl(SubsProfile subsProfile)
        {
            try
            {
                BackgroundWorker subscribeWorker = new BackgroundWorker();
                subscribeWorker.WorkerSupportsCancellation = true;
                subscribeWorker.DoWork += subscribeWorker_DoWork2;
                if (subscribeWorker.IsBusy)
                    return;
                subscribeWorker.RunWorkerAsync(subsProfile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "request timeout");
                return;
            }
        }

        void subscribeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            subsProfiles.Clear();

            for (int i = 1; i < Program.subsProfiles.Count; i++)
            {
                SubsProfile item = Program.subsProfiles[i];
                if (item.VmessConfList.Count > 0)
                    item.VmessConfList.Clear();
                var tag = ImportURL(Utils.Base64Decode(Utils.GetUrl(item.url)), item);
            }

            refreshListServBox();
        }

        void subscribeWorker_DoWork2(object sender, DoWorkEventArgs e)
        {
            SubsProfile subsProfile = (SubsProfile)e.Argument;
            subsProfiles.Clear();
            if (subsProfile.VmessConfList.Count > 0)
                subsProfile.VmessConfList.Clear();
            var tag = ImportURL(Utils.Base64Decode(Utils.GetUrl(subsProfile.url)), subsProfile);

            refreshListServBox();
        }

        List<string> ImportURL(string importUrl, SubsProfile subsProfile)
        {
            List<string> linkMark = new List<string>();
            foreach (var link in importUrl.Split(Environment.NewLine.ToCharArray()))
            {
                if (link.StartsWith("vmess"))
                {
                    ServerProfile sp = ImportVmess(link);
                    linkMark.Add(sp.remark);
                    sp.groupId = subsProfile.groupId;
                    subsProfile.VmessConfList.Add(sp);
                    subsProfiles.Add(sp);
                }
            }
            Debug.WriteLine("importurl " + String.Join(",", linkMark));
            return linkMark;
        }

        public ServerProfile ImportVmess(string vmessUrl)
        {
            VmessConf vmessConf = js.Deserialize<VmessConf>(Utils.Base64Decode(vmessUrl.Substring(8)));
            ServerProfile tmp = Utils.GenVmessConfig(vmessConf);
            //tmp.groupId = groupId;
            //subsProfiles.Add(tmp);
            return tmp;
        }

        //public void ImportVmess(VmessConf vmessConf, int groupId)
        //{
        //    ServerProfile tmp = Utils.GenVmessConfig(vmessConf);
        //    tmp.groupId = groupId;
        //    subsProfiles.Add(tmp);
        //}

    }
}
