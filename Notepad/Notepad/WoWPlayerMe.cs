using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Notepad
{
    public class WoWPlayerMe : WoWUnit
    {
        public WoWPlayerMe(IntPtr BaseAddress)
            : base(BaseAddress)
        {
        }

        public ulong TargetGUID
        {
            get
            {
                return Memory.MemSharp.Read<ulong>(Memory.MemSharp.Modules.MainModule.BaseAddress + (int)Offsets.WoWPlayerMe.TargetGUID, false);
            }
        }

        public ulong IsLooting
        {
            get
            {
                return Memory.MemSharp.Read<byte>(Memory.MemSharp.Modules.MainModule.BaseAddress + (int)Offsets.WoWPlayerMe.IsLooting, false);
            }
        }

        public int CurrentHealth
        {
            get
            {
                return base.GetDescriptorField<int>((uint)Offsets.WoWUnit.UnitHealth);
            }
        }

        public int MaxHealth
        {
            get
            {
                return base.GetDescriptorField<int>((uint)Offsets.WoWUnit.UnitHealthMax);
            }
        }

        public int HealthPercent
        {
            get
            {
                return (int)(((double)CurrentHealth / (double)MaxHealth) * 100);
            }
        }

        public int Power
        {
            get
            {
                return base.GetDescriptorField<int>((uint)Offsets.WoWUnit.UnitPower);
            }
        }

        public int PowerMax
        {
            get
            {
                return base.GetDescriptorField<int>((uint)Offsets.WoWUnit.UnitPowerMax);
            }
        }

        public Int64 Level
        {
            get
            {
                return (Int64)Memory.MemSharp.Read<int>((uint)base.DescriptorBase + Offsets.WoWUnit.UnitLevel, false);
            }
        }

        public Vector3 Position
        {
            get
            {
                return new Vector3
                {
                    X = Memory.MemSharp.Read<float>((uint)base.BaseAddress + Offsets.WoWUnit.UnitOrigin, false),      // Obj_X in PQR  // 18414
                    Y = Memory.MemSharp.Read<float>((uint)base.BaseAddress + Offsets.WoWUnit.UnitOrigin + 4, false),  // + 4
                    Z = Memory.MemSharp.Read<float>((uint)base.BaseAddress + Offsets.WoWUnit.UnitOrigin + 8, false)   // + 4
                };
            }
        }
    }
}
