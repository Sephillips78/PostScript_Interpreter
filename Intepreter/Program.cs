using System;

namespace Interpreter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var interpreter = new PSInterpreter();

            Console.WriteLine("Enter PostScript (or 'exit'):");

            while (true)
            {
                Console.Write("> ");
                var input = Console.ReadLine();

                if (input == "exit")
                    break;

                if (input == "static")
                {
                    interpreter = new PSInterpreter(PSInterpreter.ScopeMode.Static);
                    Console.WriteLine("Switched to STATIC scope (state reset)");
                    continue;
                }

                if (input == "dynamic")
                {
                    interpreter = new PSInterpreter(PSInterpreter.ScopeMode.Dynamic);
                    Console.WriteLine("Switched to DYNAMIC scope (state reset)");
                    continue;
                }

                try
                {
                    interpreter.Execute(input);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}