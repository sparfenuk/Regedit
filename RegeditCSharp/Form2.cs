using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace RegeditCSharp
{
    public partial class Form2 : Form
    {
        public string fullPath { get; set; }
        public string valueName { get; set; }
        public Form2(string fullPath,string valueName)
        {
            
            InitializeComponent();
            this.fullPath = fullPath;
            this.valueName = valueName;

            RegistryKey key = SetKey();

            textBox2.Text = Convert.ToString(key.GetValue(valueName));

        }

        private RegistryKey SetKey()
        {
            string[] elements = fullPath.Split('\\');
            string root = elements[0];
            string path = fullPath.Substring(fullPath.IndexOf('\\') + 1);
            RegistryKey key = null;
            switch (root)
            {
                case "HKEY_CURRENT_USER":
                    key = Registry.CurrentUser.OpenSubKey(path);
                    break;
                case "HKEY_CLASSES_ROOT":
                    key = Registry.ClassesRoot.OpenSubKey(path);
                    break;
                case "HKEY_LOCAL_MACHINE":
                    key = Registry.LocalMachine.OpenSubKey(path);
                    break;
                case "HKEY_USERS":
                    key = Registry.Users.OpenSubKey(path);
                    break;
                case "HKEY_CURRENT_CONFIG":
                    key = Registry.CurrentConfig.OpenSubKey(path);
                    break;
                default:
                    key = null;
                    break;

            }
            return key;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey key = SetKey();
                key.SetValue(valueName, textBox2.Text);
                this.Close();
            }
            catch (Exception ee)
            {
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RegistryKey key = SetKey();
            key.DeleteValue(valueName);
            this.Close();
        }
    }
}
