using System;
using System.Collections.Generic;

namespace Brkyzdmr.Services.ConfigService
{
    [Serializable]
    public class GameConfig
    {
        public int maxDiceCount;
        public List<string> levelOrder;
    }
}