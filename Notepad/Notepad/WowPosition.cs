using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct WowPosition
    {
        public float X;
        public float Y;
        public float Z;
        public WowPosition(float x, float y, float z)
        {
            this = new WowPosition();
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static WowPosition Invalid
        {
            get
            {
                return new WowPosition();
            }
        }
        public override string ToString()
        {
            return string.Concat(new object[] { "[", (int)this.X, ", ", (int)this.Y, ", ", (int)this.Z, "]" });
        }
    }
}
