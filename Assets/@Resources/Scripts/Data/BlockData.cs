using UnityEngine;
using System.Collections.Generic;
using static Won.Define;

namespace Won
{
    public static class BlockData
    {
        // rotation 0, 1, 2, 3
        // each rotation contains 4 cell offsets from pivot
        public static readonly Dictionary<Tetromino, Vector2Int[][]> Cells = new()
        {
            {
                Tetromino.I, new Vector2Int[][]
                {
                    new Vector2Int[] { new Vector2Int(-1,  1), new Vector2Int( 0,  1), new Vector2Int( 1,  1), new Vector2Int( 2,  1) },
                    new Vector2Int[] { new Vector2Int( 1,  2), new Vector2Int( 1,  1), new Vector2Int( 1,  0), new Vector2Int( 1, -1) },
                    new Vector2Int[] { new Vector2Int(-1, -1), new Vector2Int( 0, -1), new Vector2Int( 1, -1), new Vector2Int( 2, -1) },
                    new Vector2Int[] { new Vector2Int( 0,  2), new Vector2Int( 0,  1), new Vector2Int( 0,  0), new Vector2Int( 0, -1) }
                }
            },
            {
                Tetromino.J, new Vector2Int[][]
                {
                    new Vector2Int[] { new Vector2Int(-1,  1), new Vector2Int(-1,  0), new Vector2Int( 0,  0), new Vector2Int( 1,  0) },
                    new Vector2Int[] { new Vector2Int( 1,  1), new Vector2Int( 0,  1), new Vector2Int( 0,  0), new Vector2Int( 0, -1) },
                    new Vector2Int[] { new Vector2Int(-1,  0), new Vector2Int( 0,  0), new Vector2Int( 1,  0), new Vector2Int( 1, -1) },
                    new Vector2Int[] { new Vector2Int( 0,  1), new Vector2Int( 0,  0), new Vector2Int( 0, -1), new Vector2Int(-1, -1) }
                }
            },
            {
                Tetromino.L, new Vector2Int[][]
                {
                    new Vector2Int[] { new Vector2Int( 1,  1), new Vector2Int(-1,  0), new Vector2Int( 0,  0), new Vector2Int( 1,  0) },
                    new Vector2Int[] { new Vector2Int( 1, -1), new Vector2Int( 0,  1), new Vector2Int( 0,  0), new Vector2Int( 0, -1) },
                    new Vector2Int[] { new Vector2Int(-1, -1), new Vector2Int(-1,  0), new Vector2Int( 0,  0), new Vector2Int( 1,  0) },
                    new Vector2Int[] { new Vector2Int(-1,  1), new Vector2Int( 0,  1), new Vector2Int( 0,  0), new Vector2Int( 0, -1) }
                }
            },
            {
                Tetromino.O, new Vector2Int[][]
                {
                    new Vector2Int[] { new Vector2Int( 0,  1), new Vector2Int( 1,  1), new Vector2Int( 0,  0), new Vector2Int( 1,  0) },
                    new Vector2Int[] { new Vector2Int( 0,  1), new Vector2Int( 1,  1), new Vector2Int( 0,  0), new Vector2Int( 1,  0) },
                    new Vector2Int[] { new Vector2Int( 0,  1), new Vector2Int( 1,  1), new Vector2Int( 0,  0), new Vector2Int( 1,  0) },
                    new Vector2Int[] { new Vector2Int( 0,  1), new Vector2Int( 1,  1), new Vector2Int( 0,  0), new Vector2Int( 1,  0) }
                }
            },
            {
                Tetromino.S, new Vector2Int[][]
                {
                    new Vector2Int[] { new Vector2Int( 0,  1), new Vector2Int( 1,  1), new Vector2Int(-1,  0), new Vector2Int( 0,  0) },
                    new Vector2Int[] { new Vector2Int( 0,  1), new Vector2Int( 0,  0), new Vector2Int( 1,  0), new Vector2Int( 1, -1) },
                    new Vector2Int[] { new Vector2Int( 0,  0), new Vector2Int( 1,  0), new Vector2Int(-1, -1), new Vector2Int( 0, -1) },
                    new Vector2Int[] { new Vector2Int(-1,  1), new Vector2Int(-1,  0), new Vector2Int( 0,  0), new Vector2Int( 0, -1) }
                }
            },
            {
                Tetromino.T, new Vector2Int[][]
                {
                    new Vector2Int[] { new Vector2Int( 0,  1), new Vector2Int(-1,  0), new Vector2Int( 0,  0), new Vector2Int( 1,  0) },
                    new Vector2Int[] { new Vector2Int( 0,  1), new Vector2Int( 0,  0), new Vector2Int( 1,  0), new Vector2Int( 0, -1) },
                    new Vector2Int[] { new Vector2Int(-1,  0), new Vector2Int( 0,  0), new Vector2Int( 1,  0), new Vector2Int( 0, -1) },
                    new Vector2Int[] { new Vector2Int( 0,  1), new Vector2Int(-1,  0), new Vector2Int( 0,  0), new Vector2Int( 0, -1) }
                }
            },
            {
                Tetromino.Z, new Vector2Int[][]
                {
                    new Vector2Int[] { new Vector2Int(-1,  1), new Vector2Int( 0,  1), new Vector2Int( 0,  0), new Vector2Int( 1,  0) },
                    new Vector2Int[] { new Vector2Int( 1,  1), new Vector2Int( 0,  0), new Vector2Int( 1,  0), new Vector2Int( 0, -1) },
                    new Vector2Int[] { new Vector2Int(-1,  0), new Vector2Int( 0,  0), new Vector2Int( 0, -1), new Vector2Int( 1, -1) },
                    new Vector2Int[] { new Vector2Int( 0,  1), new Vector2Int(-1,  0), new Vector2Int( 0,  0), new Vector2Int(-1, -1) }
                }
            }
        };
    }
}