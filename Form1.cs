using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSV_JSON_Converter
{
    public partial class Form1 : Form
    {
        private List<List<string>> csvData = new List <List<string>>();
        private string currentFileName = "";
        private Encoding currentFileEncoding = Encoding.UTF8;
        private bool isNowOpening = false;
        public Form1()
        {
            InitializeComponent();
            button2.Enabled = isNowOpening;
            statusStrip1.Items.Add(new ToolStripStatusLabel(currentFileEncoding.ToString()));
            statusStrip1.Items.Add(new ToolStripStatusLabel(currentFileName));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSVファイル|*.csv|JSONファイル|*.json";
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                csvData = new List<List<string>>();
                currentFileName = openFileDialog.FileName;
                using (FileStream fileStream = new FileStream(currentFileName, FileMode.Open, FileAccess.Read))
                {
                    StreamReader streamReader = new StreamReader(fileStream,true);
                    currentFileEncoding = streamReader.CurrentEncoding;
                    while (!streamReader.EndOfStream)
                    {
                        string row = streamReader.ReadLine();
                        textBox1.Text += row + "\r\n";
                        string[] rowArray = row.Split(',');
                        List<string> tmpRowList = new List<string>(rowArray);
                        csvData.Add(tmpRowList);
                    }
                }
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("[\r\n");
                for(int index2 = 0;index2 < csvData.Count;index2++)
                {
                    stringBuilder.Append("\t[\r\n");
                    for(int index = 0;index < csvData[index2].Count;index++)
                    {
                        if(index < csvData[index2].Count-1) stringBuilder.Append("\t\t\"" + csvData[index2][index] + "\",\r\n");
                        else stringBuilder.Append("\t\t\"" + csvData[index2][index] + "\"\r\n");
                    }
                    if (index2 < csvData.Count - 1) stringBuilder.Append("\t],\r\n");
                    else stringBuilder.Append("\t]\r\n");
                }
                stringBuilder.Append("]\r\n");
                textBox2.Text = stringBuilder.ToString();
                isNowOpening = true;
                button2.Enabled = isNowOpening;
                statusStrip1.Refresh();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string saveExtension = "";
            if (getFileNameExtension() == "csv") {
                saveFileDialog.Filter = "JSONファイル|*.json";
                saveExtension = "json";
            }
            else if (getFileNameExtension() == "json") {
                saveFileDialog.Filter = "CSVファイル|*.csv";
                saveExtension = "csv";
            }
            saveFileDialog.FileName = getFileNameWithoutExtension() + saveExtension;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write))
                {
                    StreamWriter streamWriter = new StreamWriter(fileStream);
                    streamWriter.Write(textBox2.Text);
                    streamWriter.Flush();
                }
            }
        }

        private string getFileNameExtension()
        {
            string[] dodSplited = currentFileName.Split('.');
            return dodSplited[dodSplited.Length - 1].ToLower();
        }
        private string getFileNameWithoutExtension()
        {
            string[] dodSplited = currentFileName.Split('.');
            dodSplited[dodSplited.Length - 1] = "";
            return String.Join(".", dodSplited);
        }
    }
}
