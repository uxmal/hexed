using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hexed
{
    class Hexed
    {
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
            //$TODO: something
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
