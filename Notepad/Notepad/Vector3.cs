using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad
{
    public class Vector3
    {
        public float X;
        public float Y;
        public float Z;

        public override string ToString()
        {
            return string.Format("X = {0} Y = {1} Z = {2}", X, Y, Z);
        }
    }
}
