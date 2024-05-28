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
        public List<string> tiles;
        public List<int> rewards;
        public int diceCount;
        public List<GoalConfig> goals;
    }
}