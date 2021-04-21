using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hexed
{
    class Hexed
    {
        private byte[] bytes;

        public bool IsDirty { get; set; }

        public void Run()
        {
            IsDirty = false;
            WriteBanner();
            var cmd = ReadCommand();
            while (cmd[0] != "q")
            {
                HandleCommand(cmd);
                cmd = ReadCommand();
            }
            if (IsDirty)
            {
                if (PromptForSaving())
                {
                    Save();
                }
            }

        }

        private void Save()
        {
            throw new NotImplementedException();
        }

        private bool PromptForSaving()
        {
            throw new NotImplementedException();
        }

        private void HandleCommand(string[] cmd)
        {
            if (cmd[0] == "l")
            {
                Load(cmd);
            }
            else
            {
                Console.WriteLine("Unknown command '{0}'.", cmd[0]);
            }
        }

        // Loading a file.
        // l <filename>
        private void Load(string[] cmd)
        {
            if (cmd.Length != 2)
            {
                Console.WriteLine("l command expects a single filename.");
                return;
            }
            try
            {
                this.bytes = File.ReadAllBytes(cmd[1]);
                Console.WriteLine("{0} bytes loaded", bytes.Length);
            }catch
            {
                Console.WriteLine("Unable to read file '{0}'.", cmd[1]);

            }
        }

        private string[] ReadCommand()
        {
            Console.Write("Hexed> ");
            var line = Console.ReadLine();
            if (line == null)
                return new string[] { "q" };
            var cmd = line.Split();
            return cmd;
        }

        private void WriteBanner()
        {
            Console.WriteLine("Hexed v1.0");
        }
    }
}
