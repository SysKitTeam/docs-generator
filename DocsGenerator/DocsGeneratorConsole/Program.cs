﻿using System;
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
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: DocsGeneratorConsole <input url> <output path> <version> <branch name (optional)>");
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
            DocsGenerator.DocsGenerator generator = new DocsGenerator.DocsGenerator();
            try
            {
                if (args.Length > 3)
                {
                    generator.GenerateDocs(args[0], args[1], args[2], string.Empty);
                }
                else
                {
                    generator.GenerateDocs(args[0], args[1], string.Empty, string.Empty);
                }
                
            }
            catch (Exception exc)
            {
                Console.WriteLine("Error generating docs. Error message:");
                Console.WriteLine(exc.Message);
                return;
            }
            
            Console.WriteLine("Pdf generated.");
            if (generator.HasUnprocessedDocuments())
            {
                Console.WriteLine("Some documents were not processed:");
                foreach(string line in generator.getUnprocessedDocumentsDetails())
                {
                    Console.WriteLine(line);
                }
            }
        }
    }
}
