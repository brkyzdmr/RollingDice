using System;
using System.Collections.Generic;

namespace Brkyzdmr.Services.ConfigService
{
    [Serializable]
    public class LevelConfig
    {
        public string id;
        public string name;
        public string boardId;
        public bool isRandom;
        // Tiles
        public int diceCount;
        public List<GoalConfig> goals;
    }
}