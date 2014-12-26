using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service;

namespace FixJournal
{
    class Program
    {

        static void Main(string[] args)
        {
            // Redirect Console to Text file
            FileStream ostrm = null;
            StreamWriter writer = null;
            TextWriter oldOut = Console.Out;
            try
            {
                Console.WriteLine("Generating ConsoleLogGL.txt file....");
                ostrm = new FileStream("./ConsoleLogGL.txt", FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(ostrm);
                Console.SetOut(writer);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open ConsoleLogGL.txt for writing");
                Console.WriteLine(e.Message);
                //return;
            }

            //for (int i = 1; i < 400; i++) Console.WriteLine(i);
            //Console.ReadLine();
            FixFunctions rb = new FixFunctions();
            Console.WriteLine("Fixing TransactionDate on GL Journal...");
            rb.FixJournal(null, null);
            Console.WriteLine("Done");

            // Restore original Console
            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();
            //Console.ReadLine();
        }
    }
}
