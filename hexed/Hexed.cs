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
        private int position;

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
            else if (cmd[0] == "d")
            {
                Dump(cmd);
            }
            else
            {
                Console.WriteLine("Unknown command '{0}'.", cmd[0]);
            }
        }

        private void Dump(string[] cmd)
        {
            for (int row = 0; row < 16 && position < bytes.Length; ++row)
            {
                DumpLine();
            }
        }

        private void DumpLine()
        {
            var stop = position + 16;
            if (stop > bytes.Length)
                stop = bytes.Length;
            Console.Write("{0:X4} ", position);
            for (int i = position; i < stop; i = i +1)
            {
                Console.Write("{0:X2} ", (int)bytes[i]);
            }
            if (stop < position + 16)
            {
                var blanks = position + 16 - stop;
                for (int i = 0; i < blanks; i = i + 1)
                {
                    Console.Write("   ");
                }
            }
            for (int i = position; i < stop; i = i + 1)
            {
                var b = bytes[i];
                if (0x20 <= b && b < 0x80)
                {
                    Console.Write((char)b);
                }
                else
                {
                    Console.Write('.');
                }
            }
            Console.WriteLine();
            position = stop;
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
                this.position = 0;
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
