using System;

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
    }
}