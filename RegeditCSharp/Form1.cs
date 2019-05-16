using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace RegeditCSharp
{
    public partial class Form1 : Form
    {
        private string Path;
        public Form1()
        {
            InitializeComponent();


            FillTree();
        }

        private RegistryKey SetKey(string fullPath)
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
        private void FillTree()
        {


            this.treeView1.Nodes.Add(Registry.ClassesRoot.Name, Registry.ClassesRoot.Name);
            this.treeView1.Nodes[Registry.ClassesRoot.Name].Nodes.Add(string.Empty);
            this.treeView1.Nodes[Registry.ClassesRoot.Name].Tag = Registry.ClassesRoot;

            this.treeView1.Nodes.Add(Registry.CurrentUser.Name, Registry.CurrentUser.Name);
            this.treeView1.Nodes[Registry.CurrentUser.Name].Nodes.Add(string.Empty);
            this.treeView1.Nodes[Registry.CurrentUser.Name].Tag = Registry.CurrentUser;

            this.treeView1.Nodes.Add(Registry.LocalMachine.Name, Registry.LocalMachine.Name);
            this.treeView1.Nodes[Registry.LocalMachine.Name].Nodes.Add(string.Empty);
            this.treeView1.Nodes[Registry.LocalMachine.Name].Tag = Registry.LocalMachine;

            this.treeView1.Nodes.Add(Registry.Users.Name, Registry.Users.Name);
            this.treeView1.Nodes[Registry.Users.Name].Nodes.Add(string.Empty);
            this.treeView1.Nodes[Registry.Users.Name].Tag = Registry.Users;

            this.treeView1.Nodes.Add(Registry.CurrentConfig.Name, Registry.CurrentConfig.Name);
            this.treeView1.Nodes[Registry.CurrentConfig.Name].Nodes.Add(string.Empty);
            this.treeView1.Nodes[Registry.CurrentConfig.Name].Tag = Registry.CurrentConfig;

            this.treeView1.BeforeExpand += new TreeViewCancelEventHandler(treeView1_BeforeExpand);
        }

        void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == string.Empty)
            {
                e.Node.Nodes.Clear();
                RegistryKey key = e.Node.Tag as RegistryKey;
                if (key != null)
                {
                    foreach (string name in key.GetSubKeyNames())
                    {

                        e.Node.Nodes.Add(name, name);

                        if (name != "SECURITY" && name != "SAM")
                        {

                            RegistryKey subkey = key.OpenSubKey(name);

                            e.Node.Nodes[name].Tag = subkey;

                            if (subkey.SubKeyCount > 0)
                                e.Node.Nodes[name].Nodes.Add(string.Empty);

                        }

                    }

                }

            }

        }


        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            listBox1.Items.Clear();
            
            this.Path = e.Node.FullPath;

           
            RegistryKey key = SetKey(e.Node.FullPath);
            
            if (key != null)
            {
                foreach(string valuename in key.GetValueNames())
                {
                    listBox1.Items.Add(valuename);
                    listBox2.Items.Add(key.GetValueKind(valuename));
                    listBox3.Items.Add(key.GetValue(valuename));
                }
                
            }
            //RegistryKey currentUserKey = Registry.CurrentUser;
            //RegistryKey key = currentUserKey.OpenSubKey(e.Node.FullPath);





        }
       

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Form2 f2 = new Form2(Path, listBox1.SelectedItem as string);
            f2.ShowDialog();
        }


    }
}
