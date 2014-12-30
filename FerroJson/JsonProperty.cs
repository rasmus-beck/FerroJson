using System;
using System.Collections.Generic;

namespace FerroJson
{
    public class JsonProperty
    {
        public string ErrorMessageFormat { get; set; }
        public IList<Func<object, bool>> ValidationRules { get; set; }
    }
}