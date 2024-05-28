using System;
using Brkyzdmr.Services.ConfigService;

namespace RollingDice.Runtime.Board
{
    [Serializable]
    public class ItemData
    {
        public ItemConfig config;
        public int count;
    }
}