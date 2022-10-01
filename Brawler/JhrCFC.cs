using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DragonEngineLibrary;
using Newtonsoft.Json;

namespace Brawler
{

    internal struct YazawaWeaponSetChunk
    {
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public AssetArmsCategoryID Category;
        public JhrMovesetChunk Moveset;
    }

    internal struct JhrStyleChunk
    {
        public JhrMovesetChunk Moves;
        public MotionID SwapAnim;
    }

    internal struct JhrMovesetChunk
    {
        public JhrCommandChunk[] Chunks;
        public uint RepelCommand;
    }

    internal struct JhrCommandChunk
    {
        public JhrCommandChunkInfo Move;
    }

    internal struct JhrCommandChunkInfo
    {
        public MoveType MoveType;

        public MoveInput[] inputKeys;
        public MoveSimpleConditions skillConditions;
        public float cooldown;
        public MotionID Motion;
        public RPGSkillID rpgSkill;

        public MoveString.AttackFrame[] attackFrames;
    }

    public static class JhrCommand
    {
        internal static JhrStyleChunk ToJhrCFC(Style style)
        {
            JhrStyleChunk styleChunk = new JhrStyleChunk();
            styleChunk.Moves = ToJhrCFC(style.CommandSet);
            styleChunk.SwapAnim = style.SwapAnimation;

            return styleChunk;
        }

        internal static JhrMovesetChunk ToJhrCFC(Moveset moveset)
        {
            JhrMovesetChunk moveChunk = new JhrMovesetChunk();

            List<JhrCommandChunk> chunks = new List<JhrCommandChunk>();

            foreach (MoveBase move in moveset.Moves)
                chunks.Add(ToJhrCFCChunk(move));

            moveChunk.RepelCommand = (uint)moveset.RepelCounter;
            moveChunk.Chunks = chunks.ToArray();

            return moveChunk;
        }

        internal static YazawaWeaponSetChunk[] ToJhrCFC(Dictionary<AssetArmsCategoryID, Moveset> WeaponMovesets)
        {
            List<YazawaWeaponSetChunk> chunks = new List<YazawaWeaponSetChunk>();

            foreach (var kv in WeaponMovesets)
            {
                chunks.Add(new YazawaWeaponSetChunk() { Category = kv.Key, Moveset = ToJhrCFC(kv.Value) });
            }

            return chunks.ToArray();
        }

        internal static JhrCommandChunk ToJhrCFCChunk(MoveBase move)
        {
            JhrCommandChunk chunk = new JhrCommandChunk();

            chunk.Move = new JhrCommandChunkInfo();
            chunk.Move.MoveType = move.MoveType;
            chunk.Move.inputKeys = move.inputKeys;
            chunk.Move.skillConditions = move.skillConditions;
            chunk.Move.cooldown = move.cooldown;

            switch (move.MoveType)
            {
                case MoveType.MoveComboString:
                    chunk.Move.attackFrames = (move as MoveString).Attacks;
                    break;
                case MoveType.MoveRPG:
                    chunk.Move.rpgSkill = (move as MoveRPG).ID;
                    break;
                case MoveType.MoveGMTOnly:
                    chunk.Move.Motion = (move as MoveGMTOnly).Motion;
                    break;
            }


            return chunk;
        }

        internal static MoveBase ToMove(JhrCommandChunk chunk)
        {
            MoveBase move = null;

            switch (chunk.Move.MoveType)
            {
                default:
                    move = new MoveBase(chunk.Move.cooldown, chunk.Move.inputKeys, chunk.Move.skillConditions);
                    break;
                case MoveType.MoveRPG:
                    move = new MoveRPG(chunk.Move.rpgSkill, chunk.Move.cooldown, chunk.Move.inputKeys, chunk.Move.skillConditions);
                    break;
                case MoveType.MoveComboString:
                    move = new MoveString(chunk.Move.attackFrames, chunk.Move.inputKeys, chunk.Move.skillConditions);
                    break;
                case MoveType.MoveGMTOnly:
                    move = new MoveGMTOnly(chunk.Move.Motion, chunk.Move.cooldown, chunk.Move.inputKeys, chunk.Move.skillConditions);
                    break;
            }

            return move;
        }

        internal static Moveset ToMoveSet(JhrMovesetChunk chunk)
        {
            List<MoveBase> moves = new List<MoveBase>();

            foreach (JhrCommandChunk moveChunk in chunk.Chunks)
                moves.Add(ToMove(moveChunk));

            return new Moveset((RPGSkillID)chunk.RepelCommand, moves.ToArray());
        }

        internal static Style ToStyle(JhrStyleChunk chunk)
        {
            return new Style(chunk.SwapAnim, ToMoveSet(chunk.Moves));
        }

        internal static Dictionary<AssetArmsCategoryID, Moveset> ToWeaponSets(YazawaWeaponSetChunk[] chunk)
        {
            Dictionary<AssetArmsCategoryID, Moveset> sets = new Dictionary<AssetArmsCategoryID, Moveset>();

            foreach (YazawaWeaponSetChunk weaponSetChunk in chunk)
                sets.Add(weaponSetChunk.Category, ToMoveSet(weaponSetChunk.Moveset));

            return sets;
        }
    }
}
