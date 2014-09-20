using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.Threading;
using Binarysharp.MemoryManagement;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using WiNiFiX;

namespace Notepad
{
    public partial class Form1 : Form
    {
        Process wowProc;
        
        Thread ObjectManagerThread { get; set; }
        internal static bool IsExiting { get; set; }
        Logging log;
        public static WoWPlayerMe Me;

        private static uint ObjMgr;
        private static ulong PlayerGUID;
        private static uint CurObj;
        private static bool blnInitialized = false;
        private static int SleepDelay = 100;
        private static ulong previousTargetGUID = 0;
        private static byte previousIsLooting = 0;
        private static uint previousIsCasting = 0;
        public static List<WoWUnit> WoWUnitList;
        public static List<WoWUnit> WoWPlayerList;
        Memory wow;
               
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            log = new Logging(richTextBox1, this);
            Keyboard k = new Keyboard();                

            wowProc = Process.GetProcessesByName("Wow").FirstOrDefault();

            try
            {
                if (wowProc == null)
                {
                    log.LogActivity("Please ensure World of Warcraft is running.", Color.Red);
                    return;
                }

                string strFileName = wowProc.Modules[0].FileName;

                if (strFileName != "")
                {
                    FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(strFileName);
                    string version = versionInfo.FileVersion;

                    if (!versionInfo.FileVersion.Contains("18414"))
                    {
                        log.LogActivity("This program only supports WoW 18414 at present, you are running: " + versionInfo.FileVersion);
                        return;
                    }
                }

                wow = new Memory(wowProc);

                SleepDelay = 15000; // For initial loading

                ObjectManagerThread = new Thread(delegate()
                {
                    while (!IsExiting)
                    {
                        Pulse();
                        Thread.Sleep(SleepDelay);
                    }
                });
                ObjectManagerThread.Start();

            }
            catch (Exception ex)
            {
                log.LogActivity(ex.Message);
            }

        }
        
        public enum WoWClass : int
        {
            None = 0,
            Warrior = 1,
            Paladin = 2,
            Hunter = 3,
            Rogue = 4,
            Priest = 5,
            DeathKnight = 6,
            Shaman = 7,
            Mage = 8,
            Warlock = 9,
            Druid = 11,
        }
        
        enum WowObjectType : int
        {
            OBJECT = 0,
            ITEM = 1,
            CONTAINER = 2,
            UNIT = 3,
            PLAYER = 4,
            GAMEOBJECT = 5,
            DYNAMICOBJECT = 6,
            CORPSE = 7,
            AREATRIGGER = 8,
            SCENEOBJECT = 9,
            NUM_CLIENT_OBJECT_TYPES = 0xA
        };
        
        public string NameFromGUID(ulong GUID)
        {
            const ulong nameStorePtr = (ulong)Offsets.NameCache.NameCacheBase - 0x8;  // Player name database (NameCacheBase - 0x8 of Torpedoes posts)                                         
            const ulong nameMaskOffset = 0x24;    // Offset for the mask used with GUID to select a linked list
            const ulong nameBaseOffset = 0x18;    // Offset for the start of the name linked list
            const ulong nameStringOffset = 0x21;  // Offset to the C string in a name structure

            uint nMask = Memory.MemSharp.Read<uint>((IntPtr)(nameStorePtr + nameMaskOffset), true);
            uint nBase = Memory.MemSharp.Read<uint>((IntPtr)(nameStorePtr + nameBaseOffset));

            ulong nShortGUID = GUID & 0xFFFFFFFF; // only need part of the GUID (hex values 7 - 10) // http://www.wowwiki.com/API_UnitGUID

            ulong nOffset = (ulong)Offsets.NameCache.NameCacheGuid * (nMask & nShortGUID);

            uint nCurrentObject = Memory.MemSharp.Read<uint>((IntPtr)(nBase + (12 * (nMask & nShortGUID)) + 0x8), false);
            nOffset = Memory.MemSharp.Read<uint>((IntPtr)(nBase + nOffset), false);

            if ((nCurrentObject & 0x1) == 0x1)
                return "Unknown Player";

            uint nTestAgainstGUID = Memory.MemSharp.Read<uint>((IntPtr)(nCurrentObject), false);

            while (nTestAgainstGUID != nShortGUID)
            {
                nCurrentObject = Memory.MemSharp.Read<uint>((IntPtr)(nCurrentObject + nOffset + 0x4), false);

                if ((nCurrentObject & 0x1) == 0x1)
                    return "Unknown Player";

                nTestAgainstGUID = Memory.MemSharp.Read<uint>((IntPtr)(nCurrentObject), false);
            }

            return Memory.MemSharp.ReadString((IntPtr)(nCurrentObject + (uint)nameStringOffset), false);
        }
        
        public void Initialize()
        {
            richTextBox1.Clear();

            string strPlayerName = Memory.MemSharp.ReadString(Offsets.WoWPlayerMe.Name, true);
            byte PlayerClass = Memory.MemSharp.Read<byte>(Offsets.WoWPlayerMe.Class, true);

            log.LogActivity("Player Name     : " + strPlayerName);
            log.LogActivity("Player Class    : " + (WoWClass)PlayerClass);
            

            ObjMgr = Memory.MemSharp.Read<uint>(new IntPtr((uint)Memory.MemSharp.Read<ulong>(new IntPtr((uint)Memory.MemSharp.Modules.MainModule.BaseAddress + (uint)Offsets.ObjectManager.ClientConnection), false) + (uint)Offsets.ObjectManager.CurMgr), false);
            PlayerGUID = Memory.MemSharp.Read<ulong>(new IntPtr((uint)ObjMgr + (uint)Offsets.ObjectManager.LocalGUID), false);
            CurObj = Memory.MemSharp.Read<uint>(new IntPtr((uint)ObjMgr + (uint)Offsets.ObjectManager.FirstObject), false);
                      
            int NearbyPlayerCount = 0;
            int NearbyNPCCount = 0;

            WoWUnitList = new List<WoWUnit>();
            WoWPlayerList = new List<WoWUnit>();

            while ((CurObj != 0) && ((CurObj & 1) == 0))
            {
                uint next = Memory.MemSharp.Read<uint>(new IntPtr((uint)CurObj + (uint)Offsets.ObjectManager.NextObject), false);
                WoWObject currentObj = new WoWObject((IntPtr)CurObj);

                if (currentObj.Guid == PlayerGUID)
                {
                    Me = new WoWPlayerMe((IntPtr)CurObj);
                }
                else
                {
                    if (currentObj.Type == (int)WowObjectType.PLAYER)
                    {
                        if (currentObj.Guid != 0)
                        {
                            log.LogActivity("Nearby Players  : " + NameFromGUID(currentObj.Guid));
                            WoWUnit currentUnit = new WoWUnit((IntPtr)CurObj);
                            WoWPlayerList.Add(currentUnit);
                            NearbyPlayerCount++;
                        }
                    }
                    else if (currentObj.Type == (int)WowObjectType.UNIT)
                    {
                        WoWUnit currentUnit = new WoWUnit((IntPtr)CurObj);
                        log.LogActivity("Nearby Unit Name: " + currentUnit.Name);
                        WoWUnitList.Add(currentUnit);
                        NearbyNPCCount++;
                    }
                }
                
                CurObj = next;
            }

            log.LogActivity("Nearby Players  : " + NearbyPlayerCount);
            log.LogActivity("Nearby NPC's    : " + NearbyNPCCount);                                            
            log.LogActivity("My Level        : " + Me.UnitLevel);
            //log.LogActivity("My HP Current   : " + Me.BaseHealth);
            //log.LogActivity("My HP Max       : " + Me.MaxHealth);

            pbHealth.Maximum = 100;
            pbHealth.Value = Me.HealthPercent;

            pbResource.Maximum = Me.PowerMax;
            pbResource.Value = Me.Power;

            SleepDelay = 100;
        }

        public void Pulse()
        {
            //intPulseCount++;
            //LogActivity("Pulse: " + intPulseCount);

            byte GameState = 0;
            try
            {
                GameState = Memory.MemSharp.Read<byte>(Offsets.General.GameState, true);
            }
            catch
            {
                IsExiting = true;
            }
           
            if (GameState == 1)  // We are on not on the login screen
            {
                txtLoadScreen.Text = "In Game";
                txtLoadScreen.BackColor = System.Drawing.Color.Green;

                if (!blnInitialized)
                {
                    Initialize();
                    blnInitialized = true;
                }

                if (Me != null)
                {
                    txtX.Text = Me.Position.X.ToString();
                    txtY.Text = Me.Position.Y.ToString();
                    txtZ.Text = Me.Position.Z.ToString();

                    pbHealth.Value = Me.HealthPercent;

                    //if (pbHealth.Value < 90)
                    //{
                    //    Keyboard.PressKey(Keyboard.Keys.H);
                    //}

                    pbResource.Value = Me.Power;

                    uint currentCastingID = Me.IsCasting;

                    if (currentCastingID != previousIsCasting)
                    {                  
                        log.LogActivity("Casting         : " + Me.IsCasting);
                        previousIsCasting = currentCastingID;
                    }                    

                    ulong currentTargetGUID = Me.TargetGUID;

                    if (currentTargetGUID != previousTargetGUID)
                    {                  
                        log.LogActivity("My Target GUID  : 0x" + currentTargetGUID.ToString("X"));
                        previousTargetGUID = currentTargetGUID;
                    }

                    if ((Me.IsLooting == 1) && (previousIsLooting == 0))
                    {
                        log.LogActivity("Looting");
                        previousIsLooting = 1;
                    }
                    else
                    {
                        previousIsLooting = 0;
                    }
                }                
            }           
            else
            {
                txtLoadScreen.Text = "Login Screen";
                txtLoadScreen.BackColor = System.Drawing.Color.Red;
                blnInitialized = false;
            }
        }

        private void cmdT_Click(object sender, EventArgs e)
        {
            VirtualControl.ControlSendMessage("World of Warcraft", VirtualControl.VirtualKeyStates.T, false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            VirtualControl.ControlSendMessage("World of Warcraft", VirtualControl.VirtualKeyStates.H, false);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.ScrollToCaret();
        }
    }
}
