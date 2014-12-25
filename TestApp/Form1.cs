using System;
using System.IO;
using System.Windows.Forms;

namespace TestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var sr = File.OpenText("v4schema.json"))
            {
                var s = sr.ReadToEnd();
                var validator = new FerroJson.Validator();
                validator.Validate(s, s);   
            }
        }
    }
}
