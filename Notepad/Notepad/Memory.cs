using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binarysharp.MemoryManagement;

namespace Notepad
{    
    public class Memory
    {
        public static MemorySharp MemSharp;

        public Memory(System.Diagnostics.Process p)
        {
            MemSharp = new MemorySharp(p.Id);
        }
    }
}
