using System;

namespace Brkyzdmr.Services.ConfigService
{
    [Serializable]
    public class BoardConfig
    {
        public string id;
        public BoardType boardType;
        public int tileCount;
        public string prefabPath;
    }
}