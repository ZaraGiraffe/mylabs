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

namespace naumlab1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.Rows.Add();
            cols = new List<string>();
            rows = new List<string>();
            cols.Add("A");
            rows.Add("1");
            dataGridView1.Columns[0].HeaderCell.Value = cols[0];
            dataGridView1.Rows[0].HeaderCell.Value = rows[0];
            dataGridView1[0, 0].Value = "";
        }

        public List<string> cols;
        public List<string> rows;

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void AddRow_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();
            rows.Add((Int32.Parse(rows[rows.Count - 1]) + 1).ToString());
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].HeaderCell.Value = rows[rows.Count - 1];
            for (int i = 0; i < dataGridView1.Columns.Count; i++) dataGridView1[i, dataGridView1.Rows.Count - 1].Value = "";
        }

        private void AddComlumn_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Add(new DataGridViewColumn(dataGridView1.Columns[0].CellTemplate));
            cols.Add(((char)((int)cols[cols.Count-1][0] + 1)).ToString());
            dataGridView1.Columns[dataGridView1.Columns.Count - 1].HeaderCell.Value = cols[cols.Count - 1];
            for (int i = 0; i < dataGridView1.Rows.Count; i++) dataGridView1[dataGridView1.Columns.Count-1, i].Value = "";
        }

        private void DeleteRow_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 1) return;
            rows.Remove(rows[rows.Count - 1]);
            dataGridView1.Rows.Remove(dataGridView1.Rows[dataGridView1.Rows.Count - 1]);
        }

        private void DeleteColumn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Columns.Count == 1) return;
            cols.Remove(cols[cols.Count - 1]);
            dataGridView1.Columns.Remove(dataGridView1.Columns[dataGridView1.Columns.Count - 1]);
        }

        public double calc = 0;
        public bool calculate(int ri,int ci)
        {
            Graph gr = new Graph();
            try
            {
                foreach (string r in rows)
                    foreach (string s in cols)
                    {
                        gr.AddVertex(new Pair(s, r));
                    }
                for (int i = 0; i < rows.Count; i++) for (int j = 0; j < cols.Count; j++)
                    {
                        gr.AddNewExpression(new Pair(cols[j], rows[i]), dataGridView1[j, i].Value.ToString());
                    }
                calc = gr.Eval()[new Pair(cols[ci], rows[ri])];
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool check = calculate(e.RowIndex, e.ColumnIndex);
            if (check) textBox1.Text = calc.ToString();
            else textBox1.Text = "error";
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            bool check = calculate(e.RowIndex, e.ColumnIndex);
            if (check) textBox1.Text = calc.ToString();
            else textBox1.Text = "error";
        }



        static class FileFuncs
        {
            public static string loadFile()
            {
                var fileContent = string.Empty;
                var filePath = string.Empty;
                var lines = string.Empty;
                using (OpenFileDialog openfile = new OpenFileDialog())
                {
                    openfile.InitialDirectory = "c:\\";
                    openfile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    openfile.FilterIndex = 2;
                    openfile.RestoreDirectory = true;

                    if (openfile.ShowDialog() == DialogResult.OK)
                        lines = File.ReadAllText(openfile.FileName);
                }
                return lines;
            }
            public static bool saveFile(string ar, out string path)
            {
                path = string.Empty;
                var filePath = string.Empty;
                using (SaveFileDialog savefile = new SaveFileDialog())
                {
                    savefile.InitialDirectory = "c:\\";
                    savefile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    savefile.FilterIndex = 2;
                    savefile.RestoreDirectory = true;

                    if (savefile.ShowDialog() == DialogResult.OK)
                    {
                        filePath = savefile.FileName;
                        var fileStream = savefile.OpenFile();
                        path = filePath;

                        using (StreamWriter s = new StreamWriter(fileStream))
                        {
                            s.WriteLine(ar);
                        }
                    }
                    else return false;
                }
                return true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string load = dataGridView1.Rows.Count.ToString() + "," +  dataGridView1.Columns.Count.ToString();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                load += "\n";
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    load += dataGridView1[j, i].Value.ToString().Replace(" ", "") + ",";
               }
            }
            string path = String.Empty;
            FileFuncs.saveFile(load, out path);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            while (dataGridView1.Columns.Count > 1) DeleteColumn_Click(sender, e);
            while (dataGridView1.Rows.Count > 1) DeleteRow_Click(sender, e);
            string all = FileFuncs.loadFile();

            List<string> lines = all.Split('\n').ToList();
            List<List<string>> cells = new List<List<string>>();
            for (int i = 0; i < lines.Count; i++)
            {
                cells.Add(lines[i].Split(',').ToList());
            }
            int r = Int32.Parse(cells[0][0]);
            int c = Int32.Parse(cells[0][1]);
            for (int j = 1; j <= c; j++)
            {
                if (j > 1) AddComlumn_Click(sender, e);
                for (int i = 1; i <= r; i++)
                {
                    if (i > 1 && j == 1) AddRow_Click(sender, e);
                    dataGridView1[j - 1, i - 1].Value = cells[i][j-1];
                }
            }

            try
            {
                Console.WriteLine("hyu");
            }
            catch(Exception ex)
            {
                MessageBox.Show("input in wrong format");
            }
        }
    }
}
