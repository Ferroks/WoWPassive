using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad
{
    class Spells
    {




    }
}

//http://www.ownedcore.com/forums/world-of-warcraft/world-of-warcraft-bots-programs/wow-memory-editing/322754-code-release-cooldown-isgcd-spellready-unitaura-has-buff-stacks-timeleft.html

//using System;
//using System.Collections.Generic;
//using System.Runtime.InteropServices;
//using System.Linq;
//using System.Text;
//using ConsoleDruidHeals.Managers;

//namespace ConsoleDruidHeals.WoWFunctions
//{

//    public static class Cooldowns
//    {

//        /// <summary>
//        /// Returns Is spell on global cooldown
//        /// </summary>
//        /// <returns>Bool, Yes = on cooldown</returns>
//        public static bool IsGCD()
//        {
//            ulong CurrentTime = ProcessManager.WoWProcess.ReadUInt64((uint)Addresses.CoolDown.PerformanceCounter);
//            uint ObjectList = ProcessManager.WoWProcess.ReadUInt((uint)Addresses.CoolDown.GlobalCooldown + 0x8);

//            try
//            {
//                while (ObjectList != 0)
//                {
//                    uint StartTime = ProcessManager.WoWProcess.ReadUInt(ObjectList + 0x10);
//                    uint GlobalTime = ProcessManager.WoWProcess.ReadUInt(ObjectList + 0x2C);


//                    // Check spell on GCD
//                    if ((ulong)(StartTime + GlobalTime) > CurrentTime)
//                    {
//                        // Some reason bugs out hard. so if global under 10 secs lets just say we found
//                        // Cheat way but works ?
//                        if (GlobalTime < 10000 && GlobalTime != 0)
//                        {
//                            return true;
//                        }
//                    }

//                    ObjectList = ProcessManager.WoWProcess.ReadUInt(ObjectList + 4);
//                }
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//            return false;
//        }

//        public static bool SpellReady(int SpellID)
//        {
//            ulong CurrentTime = ProcessManager.WoWProcess.ReadUInt64((uint)Addresses.CoolDown.PerformanceCounter);
//            uint ObjectList = ProcessManager.WoWProcess.ReadUInt((uint)Addresses.CoolDown.GlobalCooldown + 0x8);

//            try
//            {
//                while (ObjectList != 0)
//                {

//                    try
//                    {
//                        uint ListSpellID = ProcessManager.WoWProcess.ReadUInt(ObjectList + 8);
//                        uint StartTime = ProcessManager.WoWProcess.ReadUInt(ObjectList + 0x10);
//                        uint GlobalTime = ProcessManager.WoWProcess.ReadUInt(ObjectList + 0x2C);
//                        uint GlobalCooldown = ProcessManager.WoWProcess.ReadUInt(ObjectList + 0x14) + ProcessManager.WoWProcess.ReadUInt(ObjectList + 0x20);

//                        // Hax fix ****ing heaps annoying and gay
//                         if (GlobalTime > 100000) GlobalTime = 0;

//                        if (ListSpellID == SpellID)
//                        {

//                            // Check spell on GCD
//                            if ((ulong)(StartTime + Math.Max(GlobalCooldown, GlobalTime)) > CurrentTime)
//                            {
//                                return false;
//                            }

//                        }

//                    }
//                    catch (Exception)
//                    {
//                    }

//                    ObjectList = ProcessManager.WoWProcess.ReadUInt(ObjectList + 4);
//                }
//            }
//            catch (Exception)
//            {
//                return true;
//            }

//            return true;

//        }


//    }
//}

// AURA

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using ConsoleDruidHeals.Managers;
//using ConsoleDruidHeals.Offsets;
//using ConsoleDruidHeals.WoWObjects;
//using System.Runtime.InteropServices;

//namespace ConsoleDruidHeals.WoWFunctions
//{

//    public static class UnitAura
//    {
//        /// <summary>
//        /// Returns If player has buff
//        /// </summary>
//        /// <param name="SpellID"></param>
//        /// <param name="PlayerBase"></param>
//        /// <returns></returns>
//        public static bool HasBuff(int SpellID, uint PlayerBase)
//        {
//            try
//            {
//                int Count = ProcessManager.WoWProcess.ReadInt(PlayerBase + (uint)Addresses.UnitBaseGetUnitAura.AURA_COUNT_1);
//                uint Table = PlayerBase + (uint)Addresses.UnitBaseGetUnitAura.AURA_TABLE_1;

//                if (Count == -1)
//                {
//                    Count = ProcessManager.WoWProcess.ReadInt(PlayerBase + (uint)Addresses.UnitBaseGetUnitAura.AURA_COUNT_2);
//                    Table = ProcessManager.WoWProcess.ReadUInt(PlayerBase + (uint)Addresses.UnitBaseGetUnitAura.AURA_TABLE_2);
//                }

//                uint SpellIndexID;
//                ulong SpellOwnerGUID;

//                // Loop threw all our buffs
//                for (int Index = 0; Index < Count - 1; Index++)
//                {
//                    SpellIndexID = GetSpellID(Index, Table);
//                    SpellOwnerGUID = GetSpellOwnerGUID(Index, Table);

//                    // If its not us, and not our spell go next
//                    if (WoWMe.GUID != SpellOwnerGUID) continue;
//                    if (SpellIndexID != SpellID) continue;

//                    // return true we found him!
//                    return true;
//                }

//                // nup no good didnt find him false
//                return false;


//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("TargetSite: {0}\nSource: {1}\nMessage:{2}", e.TargetSite, e.Source, e.Message);
//                return false;
//            }

//        }


//        /// <summary>
//        /// Returns Number of stacks SpellID has
//        /// </summary>
//        /// <param name="SpellID"></param>
//        /// <param name="PlayerBase"></param>
//        /// <returns></returns>
//        public static int Stacks(int SpellID, uint PlayerBase)
//        {
//            try
//            {
//                int Count = ProcessManager.WoWProcess.ReadInt(PlayerBase + (uint)Addresses.UnitBaseGetUnitAura.AURA_COUNT_1);
//                uint Table = PlayerBase + (uint)Addresses.UnitBaseGetUnitAura.AURA_TABLE_1;

//                if (Count == -1)
//                {
//                    Count = ProcessManager.WoWProcess.ReadInt(PlayerBase + (uint)Addresses.UnitBaseGetUnitAura.AURA_COUNT_2);
//                    Table = ProcessManager.WoWProcess.ReadUInt(PlayerBase + (uint)Addresses.UnitBaseGetUnitAura.AURA_TABLE_2);
//                }

//                uint SpellIndexID;
//                ulong SpellOwnerGUID;

//                // Loop threw all our buffs
//                for (int Index = 0; Index < Count - 1; Index++)
//                {
//                    SpellIndexID = GetSpellID(Index, Table);
//                    SpellOwnerGUID = GetSpellOwnerGUID(Index, Table);

//                    // If its not us, and not our spell go next
//                    if (WoWMe.GUID != SpellOwnerGUID) continue;
//                    if (SpellIndexID != SpellID) continue;

//                    return GetStacks(Index, Table);
//                }

//                return 0;


//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("TargetSite: {0}\nSource: {1}\nMessage:{2}", e.TargetSite, e.Source, e.Message);
//                return 0;
//            }
//        }


//        /// <summary>
//        /// Returns Number of stacks SpellID has
//        /// </summary>
//        /// <param name="SpellID"></param>
//        /// <param name="PlayerBase"></param>
//        /// <returns></returns>
//        public static int TimeLeft(int SpellID, uint PlayerBase)
//        {
//            try
//            {
//                int Count = ProcessManager.WoWProcess.ReadInt(PlayerBase + (uint)Addresses.UnitBaseGetUnitAura.AURA_COUNT_1);
//                uint Table = PlayerBase + (uint)Addresses.UnitBaseGetUnitAura.AURA_TABLE_1;

//                if (Count == -1)
//                {
//                    Count = ProcessManager.WoWProcess.ReadInt(PlayerBase + (uint)Addresses.UnitBaseGetUnitAura.AURA_COUNT_2);
//                    Table = ProcessManager.WoWProcess.ReadUInt(PlayerBase + (uint)Addresses.UnitBaseGetUnitAura.AURA_TABLE_2);
//                }

//                uint SpellIndexID;
//                ulong SpellOwnerGUID;

//                // Loop threw all our buffs
//                for (int Index = 0; Index < Count - 1; Index++)
//                {
//                    SpellIndexID = GetSpellID(Index, Table);
//                    SpellOwnerGUID = GetSpellOwnerGUID(Index, Table);

//                    // If its not us, and not our spell go next
//                    if (WoWMe.GUID != SpellOwnerGUID) continue;
//                    if (SpellIndexID != SpellID) continue;

//                    return (int)GetTimeLeft(Index, Table);
//                }

//                return 0;

//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("TargetSite: {0}\nSource: {1}\nMessage:{2}", e.TargetSite, e.Source, e.Message);
//                return 0;
//            }
//        }





//// Shouldnt need to touch anything below here
//// this is where all the magic happens :p
//        #region MemoryReads

//        private static uint GetSpellID(int Index, uint Table)
//        {
//            try
//            {
//                return ProcessManager.WoWProcess.ReadUInt(
//                    Table + (uint)((uint)Addresses.UnitBaseGetUnitAura.AURA_SPELL_SIZE * Index)
//                    + (int)Addresses.UnitBaseGetUnitAura.AURA_SPELL_ID);
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("TargetSite: {0}\nSource: {1}\nMessage:{2}", e.TargetSite, e.Source, e.Message);
//                return 0;
//            }
//        }

//        private static int GetStacks(int Index, uint Table)
//        {
//            try
//            {
//                byte[] Flags;
//                Flags = ProcessManager.WoWProcess.ReadBytes(
//                    Table + (uint)((uint)Addresses.UnitBaseGetUnitAura.AURA_SPELL_SIZE * Index)
//                    + (uint)Addresses.UnitBaseGetUnitAura.AURA_SPELL_FLAGS, 3);

//                return (int)Flags[2];
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("TargetSite: {0}\nSource: {1}\nMessage:{2}", e.TargetSite, e.Source, e.Message);
//                return 0;
//            }
//        }

//        private static ulong GetSpellOwnerGUID(int Index, uint Table)
//        {
//            try
//            {
//                return ProcessManager.WoWProcess.ReadUInt64(
//                    Table + (uint)((int)Addresses.UnitBaseGetUnitAura.AURA_SPELL_SIZE * Index));
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("TargetSite: {0}\nSource: {1}\nMessage:{2}", e.TargetSite, e.Source, e.Message);
//                return 0;
//            }
//        }


//        private static uint GetTimeLeft(int Index, uint Table)
//        {
//            try
//            {
//                ulong CurrentTime = ProcessManager.WoWProcess.ReadUInt64((uint)Addresses.CoolDown.PerformanceCounter);
//                uint EndTime = ProcessManager.WoWProcess.ReadUInt((uint)Table + (uint)Addresses.UnitBaseGetUnitAura.AURA_SPELL_SIZE * (uint)Index + (uint)Addresses.UnitBaseGetUnitAura.AURA_SPELL_ENDTIME);

//                return (uint)(EndTime - CurrentTime);

//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("TargetSite: {0}\nSource: {1}\nMessage:{2}", e.TargetSite, e.Source, e.Message);
//                return 0;
//            }
//        }

//        #endregion

//    }
//}

//http://www.ownedcore.com/forums/world-of-warcraft/world-of-warcraft-bots-programs/wow-memory-editing/310223-example-check-if-spell-available-memory-reading.html

//of_enabled_action_btns = $9EDC58;
//of_no_mana_for_action_btns = $9EDA18;


////i - number of Action button.
//is_available = mr.readUInt( mem_baseaddr + of_enabled_action_btns + i*4);
//not_enough_mana = mr.readUInt( mem_baseaddr + of_no_mana_for_action_btns + i*4);

//  SKILLS_BAR_FROM = 60;
//  SKILLS_BAR_TO = 72;
//of_action_toolbar_start = $9EE0D4;
//of_spell_cooldowns = $980968;
//of_enabled_action_btns = $9EDC58;
//of_no_mana_for_action_btns = $9EDA18;



//{*******************************************}
//{**** SKILLS *******************************}
//procedure s_Combat.use_spell();
//var
//  i : word;
//  is_available, not_enough_mana : dword;
//  x : byte;
//  spid : dword;
//  s : AnsiString;
//begin

//  update_cooldowns();
//  if (on_gcd) then begin
//    if (gcd_left > 200) then sleep( gcd_left - 200 );
//    exit;
//  end;
//  x := 0;

//  for i := SKILLS_BAR_FROM to SKILLS_BAR_TO do begin
//    inc(x);
//    is_available := mr.readUInt( mem_baseaddr + of_enabled_action_btns + i*4);
//    not_enough_mana := mr.readUInt( mem_baseaddr + of_no_mana_for_action_btns + i*4);

//    if (is_available = 1) and ( not_enough_mana <> 1 )    then begin
//      //get spell ID from btn
//      spid := mr.readUInt(mem_baseaddr + of_action_toolbar_start + (i+1)*4);

//      if not( on_cooldown(spid) ) then begin
//        log(' btn '+inttostr(x)+' available ('+inttostr(spid)+')');
//        s := inttostr(x);
//        SendKey( wow_hwnd, s[1] );
//        break;
//      end;
//    end;
//  end;
  
//end;


//procedure s_Combat.update_cooldowns();
//var
//  perfCount, frequency, currentTime: Int64;
//  startTime, cooldown1, cooldown2, cooldownLength, globalLength, left: integer;
//  curItem,spellId : dword;
//begin
//  n_cooldowns := 0;
//  on_gcd := false;

//  QueryPerformanceFrequency(frequency);
//  QueryPerformanceCounter(perfCount);
//  currentTime := round( (perfCount * 1000) / frequency );

//  curItem := mr.readUInt(mem_baseaddr + of_spell_cooldowns + $8);
//  while ((curItem <> 0) and ((curItem and 1) = 0)) do begin
//    spellId := mr.readUInt(curItem + $8);
//    startTime := mr.readInt(curItem + $10);
//    cooldown1 := mr.readInt(curItem + $14);
//    cooldown2 := mr.readInt(curItem + $20);
//    globalLength  := mr.readInt( curItem + $2C );

//    if ( (startTime + globalLength) > currentTime) then begin
//      on_gcd := true;
//      gcd_left :=  (startTime + globalLength) - currentTime;
//      exit;
//    end;


//    cooldownLength := max(cooldown1, cooldown2);
//    left := (startTime + cooldownLength) - currentTime ;
//    if left > 0 then begin
//      //log( Format(' +sp CD: %x %d', [spellId, left]) );
//      inc(n_cooldowns);
//      spells_on_cooldown[ n_cooldowns ] := spellId;
//    end;
//    curItem := mr.readUInt(curItem + 4);
//  end;
//end;


//function s_Combat.on_cooldown(spid:dword) : boolean;
//var
//  i : word;
//begin
//  result := false;
//  for i := 1 to n_cooldowns do begin
//    if spells_on_cooldown[ i ] = spid then begin
//      result := true;
//      exit;
//    end;
//  end;
//end;

//http://www.ownedcore.com/forums/world-of-warcraft/world-of-warcraft-bots-programs/wow-memory-editing/310223-example-check-if-spell-available-memory-reading.html

//[DllImport("kernel32.dll")]
//static extern bool QueryPerformanceCounter(out long lpPerformanceCounter);
//[DllImport("kernel32.dll")]
//static extern bool QueryPerformanceFrequency(out long lpFrequency);
//public static bool IsOnCooldown(int SpellId)
//{
//    //long perfCount;
//    long frequency;
//    //QueryPerformanceFrequency(out perfCount);
//    QueryPerformanceFrequency(out frequency);
//    long perfCount = Memory.ReadRelative<long>(0x008A7F54);

//    long currentTime = (perfCount * 1000) / frequency;

//    var CurrentObject = Memory.ReadRelative<uint>(0x00980968 + 0x8);
//    while ((CurrentObject != 0) && ((CurrentObject & 1) == 0))
//    {
//        var spellId = Memory.Read<uint>(CurrentObject + 0x8);
//        if (spellId == SpellId)
//        {
//            var startTime = Memory.Read<uint>(CurrentObject + 0x10);
//            var cooldown1 = Memory.Read<uint>(CurrentObject + 0x14);
//            var cooldown2 = Memory.Read<uint>(CurrentObject + 0x20);
//            var globalCooldown = Memory.Read<uint>(CurrentObject + 0x2C);

//            if ((startTime + globalCooldown) > currentTime)
//                return true; // On global cooldown

//            var cooldownLength = Math.Max(cooldown1, cooldown2);
//            if ((startTime + cooldownLength) - currentTime > 0)
//                return true; // On regular cooldown
//        }

//        CurrentObject = Memory.Read<uint>(CurrentObject + 0x4);
//    }
//    return false;
//}