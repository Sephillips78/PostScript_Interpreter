using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class Token
    {
        public enum TokenType
        {
            Number,
            Boolean,
            String,
            Name,
            Procedure,
            Operator
        }

        public TokenType Type;
        public object Value;
    }
}
