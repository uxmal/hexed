
using System;

namespace hexed
{
    class Program
    {
        // Handle different commands
        // l <filename>
        // s <filename>
        // s
        // d 0100
        // d 0100 0200
        // w 0023 'hello'
        // w 0023 48 46 4c 4c 4f
        // q
        static void Main(string[] args)
        {
            var hexed = new Hexed();
            hexed.Run();
        }
    }
}
