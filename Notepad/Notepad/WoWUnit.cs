using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Notepad
{
    public class WoWUnit : WoWObject
    {
        public WoWUnit(IntPtr BaseAddress)
            : base(BaseAddress)
        {
        }

        [Category("Informations"), Description("The name of the unit. Never use this if you're planning to write a bot due to different languages.")]
        public string Name
        {
            get
            {
                ulong NamePtr = (ulong)Memory.MemSharp.Read<uint>((IntPtr)((uint)base.BaseAddress + (uint)Offsets.WoWUnit.NamePointer), false);
                if (NamePtr == 0)
                    return "Not Found";
                ulong NamePtrOffset = (ulong)Memory.MemSharp.Read<uint>((IntPtr)(NamePtr + (uint)Offsets.WoWUnit.NameOffset), false);
                return Memory.MemSharp.ReadString((IntPtr)NamePtrOffset, false);
            }
        }

        public uint IsCasting
        {
            get
            {              
                return Memory.MemSharp.Read<uint>((IntPtr)((uint)base.BaseAddress + (uint)Offsets.WoWUnit.UnitCasting), false);
            }
        }

        public uint IsCastingChanneled
        {
            get
            {
                return Memory.MemSharp.Read<uint>((IntPtr)((uint)base.BaseAddress + (uint)Offsets.WoWUnit.UnitChannel), false);
            }
        }

        public int UnitHealth
        {
            get
            {
                return Memory.MemSharp.Read<int>((IntPtr)((uint)base.BaseAddress + (uint)Offsets.WoWUnit.UnitHealth), false);
            }
        }

        public int UnitHealthMax
        {
            get
            {
                return Memory.MemSharp.Read<int>((IntPtr)((uint)base.BaseAddress + (uint)Offsets.WoWUnit.UnitHealthMax), false);
            }
        }

        public long UnitLevel
        {
            get
            {
                return Memory.MemSharp.Read<long>((IntPtr)((uint)base.BaseAddress + (uint)Offsets.WoWUnit.UnitLevel), false);
            }
        }
    }
}
