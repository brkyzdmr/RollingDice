using System.Collections.Generic;
using UnityEngine;

namespace Brkyzdmr.Services.DiceService
{
    public interface IDiceService
    {
        int diceCount { get; set; }
        List<DiceData> diceDataList { get; }
        List<DiceValue> targetedResult { get; }
        void InitializeStartPositions(Transform startPositionRoot);
        void RollTheDice(GameObject dicePrefab);
        List<int> GetDiceResults();
    }
}

