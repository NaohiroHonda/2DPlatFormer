using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Adds
{
    /// <summary>
    /// AI Bomb:OnTriggerExitで爆発する凶悪なボム。
    /// </summary>
    public enum BombType
    {
        Timer,
        Collision,
        AI
    }

    public static class PowerUpManager
    {
        private static int killCountBomb = 0;

        //長さ4
        private static int[] bombDamage = new int[] { 1, 1, 2, 2 };
        private static float[] bombRadius = new float[] { 6f, 8f, 8f, 10f };

        private static int bombLevel = 0; 

        public static BombType BombType { get; private set; }

        public static void BombKill()
        {
            killCountBomb++;

            bombLevel = killCountBomb / 16;
            if (bombLevel > 3)
            {
                if (BombType < BombType.AI)
                {
                    bombLevel = 0;
                    BombType++;
                    killCountBomb = 0;
                }
                else
                {
                    bombLevel = 3;
                }
            }
        }

        public static void Init()
        {
            killCountBomb = 0;
            bombLevel = 0;
            BombType = BombType.Timer;
        }

        public static int GetBombDamage { get { return bombDamage[bombLevel]; } }

        public static float GetBombRadius { get { return bombRadius[bombLevel]; } }
    }
}
