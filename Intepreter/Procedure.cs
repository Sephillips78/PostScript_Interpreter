using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class Procedure
    {
        public List<Token> Body { get; }
        public Dictionary<string, object>? CapturedEnv { get; }

        public Procedure(List<Token> body, Dictionary<string, object>? env)
        {
            Body = body;
            CapturedEnv = env;
        }
    }
}
