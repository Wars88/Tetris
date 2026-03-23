using UnityEngine;

namespace Won
{
    public class TetrisController : MonoBehaviour
    {
        [Header("Settings")]
        public float fallTickRate = 1.0f;
        private float _fallTimer;

        // 현재 떨어지고 있는 4개의 블록의 게임오브젝트 배열
        private GameObject[] _fallingBlocks = new GameObject[4]; 
        
        // 바닥에 고정된 블록들을 시각적으로 추적하기 위한 배열 (라인 클리어 시 GameObject 파괴를 위해)
        private GameObject[,] _lockedBlocks = new GameObject[BoardManager.Height, BoardManager.Width];

        private void Start()
        {
            // 게임 시작: 배열 초기화 및 첫 블록 스폰
            Managers.BoardManager.Init();
            Managers.BoardManager.SpawnNextTetromino();

            // 현재 떨어질 4개의 시각용 큐브(프리팹)를 미리 생성해둡니다.
            for (int i = 0; i < 4; i++)
            {
                // Resource 폴더 안의 Prefabs/BlockPrefab 을 가져온다고 가정합니다.
                _fallingBlocks[i] = Managers.ResourceManager.Instantiate("BlockPrefab", this.transform);
                if (_fallingBlocks[i] == null)
                    Debug.LogWarning("Resources/Prefabs/BlockPrefab 을 찾을 수 없습니다! 큐브 프리팹을 만들어주세요.");
            }
        }

        private void Update()
        {
            HandleInput();
            HandleFall();

            // Update 마지막에 비주얼(GameObject)들의 좌표를 일괄 갱신합니다.
            SyncVisuals();
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                Managers.BoardManager.Move(Vector2Int.left);
            
            if (Input.GetKeyDown(KeyCode.RightArrow))
                Managers.BoardManager.Move(Vector2Int.right);
            
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
                Managers.BoardManager.Rotate();

            // 방향키 아래를 누르고 있으면 빠르게 내려감
            if (Input.GetKey(KeyCode.DownArrow))
                _fallTimer += Time.deltaTime * 10f; 
        }

        private void HandleFall()
        {
            _fallTimer += Time.deltaTime;

            if (_fallTimer >= fallTickRate)
            {
                _fallTimer = 0f;
                // 블록을 한 칸 아래로 이동 시도
                bool canMove = Managers.BoardManager.Move(Vector2Int.down);

                // 바닥이나 다른 블록에 닿아서 더 이상 못 내려간다면?
                if (!canMove)
                {
                    LockVisuals(); // 시각적으로 굳어진 블록(GameObject) 생성
                    Managers.BoardManager.LockPiece(); // 내부 논리 배열(Grid)에 고정하고 새로운 블록 스폰
                    SyncLockedVisuals(); // 라인이 지워졌을 수 있으므로 시각 오브젝트 갱싱
                }
            }
        }

        private void SyncVisuals()
        {
            // 1. 현재 떨어지는 블록(CurrentTetromino)의 논리적 위치를 가져와서,
            // 2. 4조각(GameObject)의 실제 Transform.position 에 대입합니다.

            var board = Managers.BoardManager;
            Vector2Int[,] cells = BlockData.Cells[board.CurrentTetromino];

            for (int i = 0; i < 4; i++)
            {
                // 현재 회전 상태의 오프셋
                Vector2Int offset = cells[board.CurrentRotation, i];
                
                // 중심좌표(CurrentPos) + 오프셋 = 실제 월드 좌표 (논리좌표)
                int x = board.CurrentPos.x + offset.x;
                int y = board.CurrentPos.y + offset.y;

                // 유니티 월드 상에 그대로 적용!
                if (_fallingBlocks[i] != null)
                    _fallingBlocks[i].transform.position = new Vector3(x, y, 0);
            }
        }

        private void LockVisuals()
        {
            var board = Managers.BoardManager;
            Vector2Int[,] cells = BlockData.Cells[board.CurrentTetromino];

            // 떨어지던 블록을 바닥에 박제(Instantiate) 합니다.
            for (int i = 0; i < 4; i++)
            {
                Vector2Int offset = cells[board.CurrentRotation, i];
                int x = board.CurrentPos.x + offset.x;
                int y = board.CurrentPos.y + offset.y;

                if (y >= 0 && y < BoardManager.Height && x >= 0 && x < BoardManager.Width)
                {
                    // 고정용 프리팹 생성 (색상 변경 등은 여기서 처리)
                    GameObject lockedPiece = Managers.ResourceManager.Instantiate("BlockPrefab", this.transform); // 또는 바위 프리팹
                    lockedPiece.transform.position = new Vector3(x, y, 0);
                    
                    _lockedBlocks[y, x] = lockedPiece; // 지워질 때 추적할 수 있도록 배열에 보관
                }
            }
        }

        private void SyncLockedVisuals()
        {
            var grid = Managers.BoardManager.Grid;

            // 실제 논리 배열(Grid)과 Unity 씬에 렌더링된(_lockedBlocks) 내역을 일치시킵니다.
            // (LockPiece 쪽에서 ClearLine이 일어났을 경우 블록들이 파괴되고 내려가야 하므로)
            for (int y = 0; y < BoardManager.Height; y++)
            {
                for (int x = 0; x < BoardManager.Width; x++)
                {
                    if (grid[y, x] == 0) // 논리적으로 비어있는데
                    {
                        if (_lockedBlocks[y, x] != null) // 실제 큐브가 화면에 있다면?
                        {
                            Destroy(_lockedBlocks[y, x]); // 삭제 로직
                            _lockedBlocks[y, x] = null;
                        }
                    }
                    else if (grid[y, x] == 1) // 논리적으로 채워져있는데
                    {
                        if (_lockedBlocks[y, x] != null)
                        {
                            // 줄이 지워져서 블록이 아래로 내려왔을 수 있으므로 Transform 갱신 (핵심!)
                            _lockedBlocks[y, x].transform.position = new Vector3(x, y, 0);
                        }
                    }
                }
            }
        }
    }
}
