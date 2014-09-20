using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad
{
/*
<?xml version="1.0" encoding="UTF-8"?>
<Offsets>
<CurrentWoWVersion>18414</CurrentWoWVersion>
<WoWVersionOffset>0xC7B6EF</WoWVersionOffset>
<PlayerName>0xEC4668</PlayerName>
<PlayerClass>0xEC47F1</PlayerClass>
<GetCurrentKeyBoardFocus>0xBB292C</GetCurrentKeyBoardFocus>
<GameState>0xD65B16</GameState>
<Lua_DoStringAddress>0x4FD12</Lua_DoStringAddress>
<Lua_GetLocalizedTextAddress>0x414267</Lua_GetLocalizedTextAddress>
<CVarBaseMgr>0xBA5DE8</CVarBaseMgr>
<CVarArraySize>0x400</CVarArraySize>
<ObjMgr>0xEC4628</ObjMgr>
<CurMgr>0x462C</CurMgr>
<ClntObjMgrGetActivePlayerObjAddress>0x4F84</ClntObjMgrGetActivePlayerObjAddress>
<LocalGUID>0xE8</LocalGUID>
<FirstObject>0xCC</FirstObject>
<NextObject>0x34</NextObject>
<Descriptors>0x4</Descriptors>
<Obj_TypeOffset>0xC</Obj_TypeOffset>
<Obj_X>0x838</Obj_X>
<Obj_TargetGUID>0x16</Obj_TargetGUID>
<ClickTerrain>0</ClickTerrain>
</Offsets>
*/

    class Offsets // 18414
    {
        public enum General : uint
        {
            GameState = 0xD65B16  // byte
        }

        public enum ObjectManager : uint
        {
            ClientConnection = 0xEC4628,
            CurMgr = 0x462c,
            LocalGUID = 0xE8,
            FirstObject = 0xCC,
            NextObject = 0x34,
            Descriptors = 0x4,
            Obj_TypeOffset = 0xC,
        }

        public enum WoWPlayerMe
        {
            Name = 0xEC4668,
            Class = 0xEC47F1,
            TargetGUID = 0xD65B40,
            IsLooting = 0xDD3D44,
            IsTexting = 0xBBE9AC,
            MouseGUID = 0xD65B28,
        }

        public enum NameCache : ulong
        {
            NameCacheBase	= 0xC86848,     // Used
            NameCacheNext	= 0x00,
            NameCacheGuid	= 0x0C,         // Used
            NameCacheName	= 0x15,
            NameCacheRace	= 0x5C,
            NameCacheClass	= 0x64
        }

        public enum WoWUnit : uint  
        {
            // Torpedoes @ Ownedcore.com

            UnitTransport = 0x830,
            UnitOrigin	  = 0x838,
            UnitAngle	  = 0x848,
            UnitCasting	  = 0xCB8,
            UnitChannel	  = 0xCD0,
            UnitCreator	  = 0x48,
            UnitHealth	  = 0x84,
            UnitPower	  = 0x88,
            UnitHealthMax = 0x9C,
            UnitPowerMax  = 0xA0,
            UnitLevel	  = 0xDC,
            UnitFlags	  = 0xF4,

            NamePointer   = 0x9B4,    // or NpcCache @ Torpedoes @ Ownedcore.com
            NameOffset    = 0x6C,     // or NpcName  @ Torpedoes @ Ownedcore.com
        }
    }
}
