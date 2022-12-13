using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace naumlab2
{
    public partial class Form1 : Form
    {
        public string path = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (path == "")
            {
                MessageBox.Show("You haven't selected a file");
                return;
            }
            Resourse query = new Resourse(
                Rname.Text,
                Info.Text,
                Annotation.Text,
                Type.Text,
                Address.Text,
                Conditions.Text,
                new Author(
                    Aname.Text,
                    Faculty.Text,
                    Cathedra.Text
                    )
                );
            List<Resourse> res = new List<Resourse>();
            IStrategy cl;
            if (Linq2XmlButton.Checked) cl = new Linq2Xml();
            else if (DomButton.Checked) cl = new DOM();
            else cl = new SAX();
            res = cl.search(query, path);
            List<string> ret = new List<string>();
            foreach (var a in res) ret.Add(a.converter());
            richTextBox1.Text = String.Join("\n", ret);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "xml files (*.xml)|*.xml";
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK) filePath = openFileDialog.FileName;
            }
            if (filePath == string.Empty) MessageBox.Show("error occured");
            else
            {
                MessageBox.Show("Succesfully selected file at" + filePath);
                path = filePath;
                FileBox.Text = path;
            }
        }
    }
}
