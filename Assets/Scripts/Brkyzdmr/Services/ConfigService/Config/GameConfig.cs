using System;
using System.Collections.Generic;

namespace Brkyzdmr.Services.ConfigService
{
    [Serializable]
    public class GameConfig
    {
        public int maxDiceCount;
        public List<string> levelOrder;
        public List<string> boards;
        public List<string> items;
        public List<string> avatars;
    }
}