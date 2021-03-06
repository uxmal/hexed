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
        private string filename;

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
                    Save(new string[0]);
                }
            }
        }

        private void Save(string[] cmd)
        {
            if (string.IsNullOrEmpty(filename))
            {
                Console.WriteLine("No file has been loaded");
                return;
            }
            try
            {
                File.WriteAllBytes(filename, this.bytes);
            }
            catch
            {
                Console.WriteLine("An error occurred while writing to {0}.", filename);
            }
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
            else if (cmd[0] == "w")
            {
                WriteBytes(cmd);
            }
            else if (cmd[0] == "s")
            {
                Save(cmd);
            }
            else if (cmd[0] == "f")
            {
                Find(cmd);
            }
            else
            {
                Console.WriteLine("Unknown command '{0}'.", cmd[0]);
            }
        }

        private void WriteBytes(string[] cmd)
        {
            if (cmd.Length < 3)
            {
                Console.WriteLine("w command expects at least a position and some byte values.");
                return;
            }
            var writePosition = Convert.ToInt32(cmd[1], 16);
            for (int i = 2; i < cmd.Length; i = i + 1)
            {
                var stringByte = cmd[i];
                var b = Convert.ToInt32(stringByte, 16);
                bytes[writePosition] = (byte)b;
                writePosition = writePosition + 1;
            }
        }

        private void Find(string []cmd)
        {
            if (cmd.Length < 3)
            {
                Console.WriteLine("f command expects at least a position and some byte values");
            }
            var findPosition = Convert.ToInt32(cmd[1], 16);
            var patternBytes = new byte[cmd.Length - 2];
            for (int i = 2; i < cmd.Length; i = i + 1)
            {
                var stringByte = cmd[i];
                var b = Convert.ToInt32(stringByte, 16);
                patternBytes[i - 2] = (byte)b;
            }
            int foundPos = FindPattern(findPosition, patternBytes);
            if (foundPos <0)
            {
                Console.WriteLine("Failed to find pattern");
                return;
            }
            this.position = foundPos;
            DumpLines(16);
            this.position = foundPos;
        }

        private int FindPattern(int findPosition, byte[] patternBytes)
        {
            for (int i = findPosition; i <= bytes.Length - patternBytes.Length; ++i)
            {
                bool matched = true;
                for (int j = 0; j < patternBytes.Length; ++j)
                {
                    var patternByte = patternBytes[j];
                    var textByte = bytes[i + j];
                    if (patternByte != textByte)
                    {
                        matched = false;
                        break;
                    }
                }
                if (matched)
                    return i;
            }
            return -1;
        }

        private void Dump(string[] cmd)
        {
            if (cmd.Length > 1)
            {
                position = Convert.ToInt32(cmd[1], 16);
            }
            DumpLines(16);
        }

        private void DumpLines(int nLines)
        {
            for (int row = 0; row < nLines && position < bytes.Length; ++row)
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
                this.filename = cmd[1];
                Console.WriteLine("{0} bytes loaded", bytes.Length);
            } catch
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
