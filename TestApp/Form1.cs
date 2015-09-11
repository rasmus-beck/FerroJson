using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FerroJson;
using FerroJson.DynamicDictionary;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
			dynamic errors = null;
			validator.Validate(testData, testSchema, out errors);

			string json = JsonConvert.SerializeObject(errors, new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Formatting = Formatting.Indented
			});

			Console.WriteLine(json);
        }
    }


}
