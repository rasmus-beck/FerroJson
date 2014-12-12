using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerroJson
{
    public interface IValidator
    {
        bool CanValidate();
        bool Validate();
    }
}
