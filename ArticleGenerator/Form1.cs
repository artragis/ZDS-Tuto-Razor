using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

namespace ArticleGenerator
{
    public partial class Form1 : Form
    {
        private List<Article> list = new List<Article>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text.Trim()) && !string.IsNullOrEmpty(textBox2.Text.Trim()))
            {
                list.Add(new Article
                {
                    Contenu = richTextBox1.Text,
                    Pseudo = textBox1.Text,
                    Titre = textBox2.Text
                });
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "Fichier JSON|*.json";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    using (var writer = new StreamWriter(dialog.FileName, false, Encoding.UTF8))
                    {
                        writer.Write(JsonConvert.SerializeObject(list));
                    }
                }
            }
        }
    }
}
