using System;
using System.Collections.Generic;
using UnityEngine;
using static Won.Define;
using Random = UnityEngine.Random;

namespace Won
{
    public class Piece
    {
        public Tetromino Tetromino { get; set; }
        public Vector2Int Pos { get; set; }
        public int Rotation { get; set; }

        public Piece(Tetromino tetromino, Vector2Int pos, int rotation)
        {
            Tetromino = tetromino;
            Pos = pos;
            Rotation = rotation;
        }
    }
    
    public class Board
    {
        public const int Width = 10;
        public const int VisibleHeight  = 20;
        public const int HiddenHeight = 4;
        public const int Height = VisibleHeight + HiddenHeight;   

        public int[,] Grid { get; private set; } = new int[Height, Width];

        public Piece CurrentPiece { get; private set; }
        public Queue<Tetromino> NextBlockQueue { get; private set; } = new ();

        public Board()
        {
            NextBlockQueue.Clear();
            ClearGrid();
            RefillBag();
        }

        public void ClearGrid()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Grid[y, x] = 0;
                }
            }
        }

        private void RefillBag()
        {
            List<Tetromino> bag = new List<Tetromino>
            {
                Tetromino.I, Tetromino.J, Tetromino.L, Tetromino.O,
                Tetromino.S, Tetromino.T, Tetromino.Z
            };

            // Fisher-Yates 셔플
            for (int i = 0; i < bag.Count; i++)
            {
                int randomIndex = Random.Range(i, bag.Count);
                Tetromino temp = bag[i];
                bag[i] = bag[randomIndex];
                bag[randomIndex] = temp;
            }

            foreach (var piece in bag)
            {
                NextBlockQueue.Enqueue(piece);
            }
        }

        public Tetromino GetNextBlock()
        {
            if (NextBlockQueue.Count < Enum.GetValues(typeof(Tetromino)).Length)
                RefillBag();
            return NextBlockQueue.Dequeue();
        }

        public void SpawnNextTetromino()
        {
            Tetromino nextBlock = GetNextBlock();
            Vector2Int pos = new Vector2Int(Width / 2, Height - 2);
            int rotation = 0;

            Piece piece = new Piece(nextBlock, pos, rotation);
        
            // 상단 중앙에서 스폰
            if (!IsValidPosition(piece, pos, rotation))
            {
                Debug.Log("Game Over: Block blocked on spawn!");
                // 게임 오버 이벤트 트리거
                
                return;
            }
            CurrentPiece = piece;
        }

        public bool Move(Vector2Int direction)
        {
            Vector2Int newPos = CurrentPiece.Pos + direction;
            if (IsValidPosition(CurrentPiece, newPos, CurrentPiece.Rotation))
            {
                CurrentPiece.Pos = newPos;
                return true;
            }
            return false;
        }

        public bool Rotate()
        {
            int newRotation = (CurrentPiece.Rotation + 1) % 4;
            if (IsValidPosition(CurrentPiece, CurrentPiece.Pos, newRotation))
            {
                CurrentPiece.Rotation = newRotation;
                return true;
            }
            return false;
        }

        public void LockPiece()
        {
            Vector2Int[] cells = BlockData.Cells[CurrentPiece.Tetromino][CurrentPiece.Rotation];
            for (int i = 0; i < 4; i++)
            {
                Vector2Int offset = cells[i];
                int x = CurrentPiece.Pos.x + offset.x;
                int y = CurrentPiece.Pos.y + offset.y;

                if (y >= 0 && y < Height && x >= 0 && x < Width)
                {
                    Grid[y, x] = 1;
                }
            }
            CheckLines();
            
            // TODO: 고정하고 라인 클리어 다 하고 그제서야 내려오도록.
            SpawnNextTetromino();
        }

        private void CheckLines()
        {
            for (int y = 0; y < Height; y++)
            {
                bool isLineFull = true;
                for (int x = 0; x < Width; x++)
                {
                    if (Grid[y, x] == 0)
                    {
                        isLineFull = false;
                        break;
                    }
                }

                if (isLineFull)
                {
                    ClearLine(y);
                    y--; // 한 줄 당겼으므로 동일한 인덱스를 다시 검사
                }
            }
        }

        private void ClearLine(int row)
        {
            for (int y = row; y < Height - 1; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Grid[y, x] = Grid[y + 1, x];
                }
            }

            for (int x = 0; x < Width; x++)
            {
                Grid[Height - 1, x] = 0;
            }
        }

        public bool IsValidPosition(Piece piece, Vector2Int pos, int rot)
        {
            Vector2Int[] cells = BlockData.Cells[piece.Tetromino][rot];

            for (int i = 0; i < 4; i++)
            {
                Vector2Int offset = cells[i];
                int x = pos.x + offset.x;
                int y = pos.y + offset.y;

                if (x < 0 || x >= Width || y < 0 || y >= Height)
                    return false;

                if (Grid[y, x] != 0)
                    return false;
            }

            return true;
        }
           

    }
}