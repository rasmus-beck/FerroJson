using Irony.Parsing;

namespace FerroJson
{
    [Language("JSON", "1.0", "JSON data format")]
    public class JsonGrammar : Grammar
    {
        public JsonGrammar()
        {
            //Terminals
            var jstring = new StringLiteral("string", "\"");
            var jdecimal = new NumberLiteral("decimal");
			var jint = new NumberLiteral("int", NumberOptions.IntOnly | NumberOptions.NoDotAfterInt);
			var nullConst = new ConstantTerminal("null", typeof(StringLiteral));
			nullConst.Add("null", null);
			var boolConst = new ConstantTerminal("bool", typeof(StringLiteral));
			boolConst.Add("true", true);
			boolConst.Add("false", false);
            var comma = ToTerm(",");

            //Nonterminals
            var jobject = new NonTerminal("Object");
            var jobjectBr = new NonTerminal("ObjectBr");
            var jarray = new NonTerminal("Array");
            var jarrayBr = new NonTerminal("ArrayBr");
            var jvalue = new NonTerminal("Value");
            var jprop = new NonTerminal("Property");

            //Rules
			jvalue.Rule = jstring | jdecimal | jint | jobjectBr | jarrayBr | boolConst | nullConst;
            jobjectBr.Rule = "{" + jobject + "}";
            jobject.Rule = MakeStarRule(jobject, comma, jprop);
            jprop.Rule = jstring + ":" + jvalue;
            jarrayBr.Rule = "[" + jarray + "]";
            jarray.Rule = MakeStarRule(jarray, comma, jvalue);

            //Set grammar root
            Root = jvalue;
            MarkPunctuation("{", "}", "[", "]", ":", ",");
            MarkTransient(jvalue, jarrayBr, jobjectBr);
        }
    }
}
