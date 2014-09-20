using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Notepad
{
    public class WoWObject
    {
        public WoWObject(IntPtr BaseAddress)
        {
            this.BaseAddress = BaseAddress;
        }

        [Description("The objects descriptor base pointer."), Category("General")]
        public IntPtr DescriptorBase
        {
            get
            {
                return Memory.MemSharp.Read<IntPtr>((uint)this.BaseAddress + Offsets.ObjectManager.Descriptors, false);
            }
        }

        public T GetDescriptorField<T>(uint field) where T : struct
        {
            return Memory.MemSharp.Read<T>(new IntPtr((uint)this.DescriptorBase + field), false);
        }

        [Category("General"), Description("The objects base address pointer.")]
        public IntPtr BaseAddress { get; set; }

        [Category("Informations"), Description("Every object has an unique Guid. Use this to identify and access the object.")]
        public ulong Guid
        {
            get
            {
                return Memory.MemSharp.Read<ulong>(this.BaseAddress + 40, false);
            }
        }

        [Category("Informations"), Description("There are several ObjectTypes. Take a look at (enum) WoWObjectType")]
        public int Type
        {
            get
            {
                return Memory.MemSharp.Read<int>((uint)this.BaseAddress + Offsets.ObjectManager.Obj_TypeOffset, false);
            }
        }

    }
}
