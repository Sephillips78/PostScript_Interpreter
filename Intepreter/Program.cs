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

                try
                {
                    interpreter.Execute(input);
                    Console.WriteLine("Top of stack: " + interpreter.Pop());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}