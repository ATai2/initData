using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace initData.model
{
    class ConfigModel
    {
        private string local;
        private string remote;
        private string pic;
        private string hisid;

        private string localip;
        private string localdatabase;
        private string localuser;
        private string localpwd;

        private string remoteip;
        private string remotedatabase;
        private string remoteuser;
        private string remotepwd;

        public string Local
        {
            get { return local; }

            set
            {
                local = value;
                string[] list = local.Split(';');
                localip = list[0].Split('=')[1];
                localdatabase = list[1].Split('=')[1];
                localuser = list[2].Split('=')[1];
                localpwd = list[3].Split('=')[1];
            }
        }

        public string Remote
        {
            get { return remote; }

            set
            {
                remote = value;
                string[] list = remote.Split(';');
                remoteip = list[0].Split('=')[1];
                remotedatabase = list[1].Split('=')[1];
                remoteuser = list[2].Split('=')[1];
                remotepwd = list[3].Split('=')[1];
            }
        }

        public string Pic
        {
            get { return pic; }

            set { pic = value; }
        }

        public string Localip
        {
            get { return localip; }

            set { localip = value; }
        }

        public string Localuser
        {
            get { return localuser; }

            set { localuser = value; }
        }

        public string Localpwd
        {
            get { return localpwd; }

            set { localpwd = value; }
        }

        public string Remoteip
        {
            get { return remoteip; }

            set { remoteip = value; }
        }

        public string Remoteuser
        {
            get { return remoteuser; }

            set { remoteuser = value; }
        }

        public string Remotepwd
        {
            get { return remotepwd; }

            set { remotepwd = value; }
        }

        public string Localdatabase
        {
            get { return localdatabase; }

            set { localdatabase = value; }
        }

        public string Remotedatabase
        {
            get { return remotedatabase; }

            set { remotedatabase = value; }
        }

        public string Hisid
        {
            get { return hisid; }

            set { hisid = value; }
        }

        public void save(String xmlPath)
        {
            if (!File.Exists(xmlPath))
            {
                return;
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);
            XmlNode root = xmlDoc.SelectSingleNode("Strings");
            XmlNodeList nodelist = root.ChildNodes;
            foreach (XmlNode xn in nodelist)
            {
                XmlElement xe = (XmlElement) xn;
                if (xe.GetAttribute("name").Equals("local"))
                {
                    xe.SetAttribute("value", local);
                }
                if (xe.GetAttribute("name").Equals("remote"))
                {
                    xe.SetAttribute("value", remote);
                }
                if (xe.GetAttribute("name").Equals("pic"))
                {
                    xe.SetAttribute("value", pic);
                }
                if (xe.GetAttribute("name").Equals("hisid"))
                {
                    xe.SetAttribute("value", hisid);
                }
            }
            xmlDoc.Save(xmlPath);
        }

        public void init(String xmlPath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);
            XmlNode root = xmlDoc.SelectSingleNode("Strings");
            XmlNodeList nodelist = root.ChildNodes;
            foreach (XmlNode xn in nodelist)
            {
                XmlElement xe = (XmlElement) xn;
                if (xe.GetAttribute("name").Equals("local"))
                {
                    this.Local = xe.GetAttribute("value");
                }
                if (xe.GetAttribute("name").Equals("remote"))
                {
                    this.Remote = xe.GetAttribute("value");
                }
                if (xe.GetAttribute("name").Equals("pic"))
                {
                    this.Pic = xe.GetAttribute("value");
                }
                if (xe.GetAttribute("name").Equals("hisid"))
                {
                    this.Hisid = xe.GetAttribute("value");
                }
            }
            // int i = 0;
        }
    }
}