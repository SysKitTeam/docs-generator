using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsGeneratorConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: DocsGeneratorConsole <input url> <output path>");
                return;
            }
            
            if (!Uri.IsWellFormedUriString(args[0], UriKind.RelativeOrAbsolute))
            {
                Console.WriteLine("Given url is not valid.");
                return;
            }
            if (!args[1].ToLower().EndsWith(".pdf"))
            {
                Console.WriteLine("Output file must be a .pdf!");
                return;
            }

            Console.WriteLine("Generating...");
            DocsGenerator.DocsGenerator.GenerateDocs(args[0], args[1]);
            Console.WriteLine("Pdf generated.");
            
        }
    }
}
