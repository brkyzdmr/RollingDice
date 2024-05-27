namespace Brkyzdmr.Helpers
{
    using System.Collections.Generic;
    using UnityEngine;
    using System.Linq;

    public static class GridHelper
    {
        public static List<Vector3> GenerateGridPositions(int count, int gridDimension, Transform startPosition, Vector3 objectSize)
        {
            List<Vector3> positions = new List<Vector3>();

            float gridCellSizeX = objectSize.x * 2f;
            float gridCellSizeY = objectSize.y * 2f;
            float gridCellSizeZ = objectSize.z * 2f;

            for (int x = 0; x < gridDimension; x++)
            {
                for (int y = 0; y < gridDimension; y++)
                {
                    for (int z = 0; z < gridDimension; z++)
                    {
                        if (positions.Count >= count) break;

                        float posX = startPosition.position.x + x * gridCellSizeX;
                        float posY = startPosition.position.y + y * gridCellSizeY;
                        float posZ = startPosition.position.z + z * gridCellSizeZ;

                        positions.Add(new Vector3(posX, posY, posZ));
                    }
                    if (positions.Count >= count) break;
                }
                if (positions.Count >= count) break;
            }

            // Shuffle the positions to randomize their order
            return positions.OrderBy(p => Random.value).ToList();
        }
    }
}