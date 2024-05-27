using System.Collections.Generic;
using UnityEngine;

namespace RollingDice.Runtime.Board
{
    public class Board : MonoBehaviour
    {
        public List<Tile> Tiles;
        
        public Tile GetTileAtIndex(int index)
        {
            if (Tiles == null || index < 0 || index >= Tiles.Count)
            {
                return null;
            }
            return Tiles[index];
        }
    }
}