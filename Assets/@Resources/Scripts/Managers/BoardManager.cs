using UnityEngine;
using System.Collections.Generic;
using static Won.Define;

namespace Won
{
    public class BoardManager
    {
        public const int Width = 10;
        public const int Height = 20;

        public int[,] Grid { get; private set; } = new int[Height, Width];

        public Tetromino CurrentTetromino { get; private set; }
        public Vector2Int CurrentPos { get; private set; }
        public int CurrentRotation { get; private set; }

        public Queue<Tetromino> NextBlockQueue { get; private set; } = new ();

        public void Init()
        {
            NextBlockQueue.Clear();
            RefillBag();
            ClearGrid();
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
            if (NextBlockQueue.Count < 7)
            {
                RefillBag();
            }
            return NextBlockQueue.Dequeue();
        }

        // UI 표시에 사용할 다음 블록들 미리보기 배열
        public Tetromino[] PeekNextBlocks(int count = 3)
        {
            if (NextBlockQueue.Count < count)
            {
                RefillBag();
            }
            
            Tetromino[] preview = new Tetromino[count];
            int idx = 0;
            foreach (var block in NextBlockQueue)
            {
                preview[idx++] = block;
                if (idx >= count)
                    break;
            }
            return preview;
        }

        public void SpawnNextTetromino()
        {
            CurrentTetromino = GetNextBlock();
            CurrentRotation = 0;
            // 상단 중앙에서 스폰
            CurrentPos = new Vector2Int(Width / 2, Height - 2); 

            if (!IsValidPosition(CurrentPos, CurrentRotation))
            {
                Debug.Log("Game Over: Block blocked on spawn!");
                // 게임 오버 이벤트 트리거
            }
        }

        public bool Move(Vector2Int direction)
        {
            Vector2Int newPos = CurrentPos + direction;
            if (IsValidPosition(newPos, CurrentRotation))
            {
                CurrentPos = newPos;
                return true;
            }
            return false;
        }

        public bool Rotate()
        {
            int newRotation = (CurrentRotation + 1) % 4;
            if (IsValidPosition(CurrentPos, newRotation))
            {
                CurrentRotation = newRotation;
                return true;
            }
            return false;
        }

        public void LockPiece()
        {
            Vector2Int[,] cells = BlockData.Cells[CurrentTetromino];
            for (int i = 0; i < 4; i++)
            {
                Vector2Int offset = cells[CurrentRotation, i];
                int x = CurrentPos.x + offset.x;
                int y = CurrentPos.y + offset.y;

                if (y >= 0 && y < Height && x >= 0 && x < Width)
                {
                    Grid[y, x] = 1;
                }
            }
            CheckLines();
            // 블록이 바닥에 고정되면 바로 다음 블록 스폰
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

        public bool IsValidPosition(Vector2Int pos, int rotation)
        {
            Vector2Int[,] cells = BlockData.Cells[CurrentTetromino];
            for (int i = 0; i < 4; i++)
            {
                Vector2Int offset = cells[rotation, i];
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