using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FerroJson;

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
	        string testData;
	        string testSchema;
	        using (var data = File.OpenText("testdata.json"))
	        {
		        testData = data.ReadToEnd();
	        }

			using (var schema = File.OpenText("v4schema.json"))
            {
				testSchema = schema.ReadToEnd();
            }

			var validator = new Validator();
			IEnumerable<IPropertyValidationError> errors = new List<IPropertyValidationError>();
			validator.Validate(testData, testSchema, out errors);

			foreach (var propertyValidationResult in errors)
			{
				foreach (var error in propertyValidationResult.Errors)
				{
					Console.WriteLine(error);
				}
			}
        }
    }
}
