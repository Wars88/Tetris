using UnityEngine;
using static Won.Define;

namespace Won
{
    public class TetrisController : BaseController
    {
        private BlockColorData _colorData;
        // ── 낙하 설정 ──────────────────────────────────────
        [SerializeField] private float _fallTickRate = 1.0f;
        private float _fallTimer;

        // ── DAS 설정 ───────────────────────────────────────
        [SerializeField] private float _dasDelay = 0.15f;  // 첫 입력 후 반복 시작까지 대기
        [SerializeField] private float _dasInterval = 0.05f;  // 반복 이동 속도
        [SerializeField] private float _softDropMul = 10f;
        private float _dasTimer;
        private int _lastDir = 0; // -1: 왼쪽, 1: 오른쪽, 0: 없음

        // ── 비주얼 ─────────────────────────────────────────
        private GameObject[] _fallingBlocks = new GameObject[4];
        private GameObject[,] _lockedBlocks = new GameObject[BoardManager.Height, BoardManager.Width];

        // ─────────────────────────────────────────────────
        public override bool Init()
        {
            if (base.Init() == false) return false;

            // 이벤트 구독 (중복 방지를 위해 -= 후 +=)
            Managers.InputManager.OnMoveLeft -= HandleOnMoveLeft;
            Managers.InputManager.OnMoveLeft += HandleOnMoveLeft;
            Managers.InputManager.OnMoveRight -= HandleOnMoveRight;
            Managers.InputManager.OnMoveRight += HandleOnMoveRight;
            Managers.InputManager.OnCanceledLeft -= HandleOnCanceledLeft;
            Managers.InputManager.OnCanceledLeft += HandleOnCanceledLeft;
            Managers.InputManager.OnCanceledRight -= HandleOnCanceledRight;
            Managers.InputManager.OnCanceledRight += HandleOnCanceledRight;
            Managers.InputManager.OnRotate -= HandleOnRotate;
            Managers.InputManager.OnRotate += HandleOnRotate;
            Managers.InputManager.OnHardDrop -= HandleOnHardDrop;
            Managers.InputManager.OnHardDrop += HandleOnHardDrop;

            // 보드 초기화 및 첫 블록 스폰
            Managers.BoardManager.Init();
            Managers.BoardManager.SpawnNextTetromino();

            _colorData = Managers.ResourceManager.Load<BlockColorData>(BlockColorDataPath);

            SpawnBlock();
            return true;
        }

        private void OnDestroy()
        {
            if (Managers.Instance == null) return;

            Managers.InputManager.OnMoveLeft -= HandleOnMoveLeft;
            Managers.InputManager.OnMoveRight -= HandleOnMoveRight;
            Managers.InputManager.OnCanceledLeft -= HandleOnCanceledLeft;
            Managers.InputManager.OnCanceledRight -= HandleOnCanceledRight;
            Managers.InputManager.OnRotate -= HandleOnRotate;
            Managers.InputManager.OnHardDrop -= HandleOnHardDrop;
        }

        // ── 게임 루프 ──────────────────────────────────────
        public override void UpdateController()
        {
            HandleDAS();
            HandleFall();
            SyncVisuals();
        }

        #region InputHandler
        // ── 입력 이벤트 핸들러 ─────────────────────────────
        private void HandleOnMoveLeft()
        {
            _lastDir = -1;
            _dasTimer = 0f;
            Managers.BoardManager.Move(Vector2Int.left); // 첫 1회 즉시 이동
        }

        private void HandleOnMoveRight()
        {
            _lastDir = 1;
            _dasTimer = 0f;
            Managers.BoardManager.Move(Vector2Int.right); // 첫 1회 즉시 이동
        }

        // 키를 뗄 때 → 반대쪽이 눌려있으면 그쪽으로 복귀 (Last Input Wins)
        private void HandleOnCanceledLeft()
        {
            if (_lastDir == -1)
            {
                _lastDir = Managers.InputManager.IsPressingRight ? 1 : 0;
                _dasTimer = 0f;
            }
        }

        private void HandleOnCanceledRight()
        {
            if (_lastDir == 1)
            {
                _lastDir = Managers.InputManager.IsPressingLeft ? -1 : 0;
                _dasTimer = 0f;
            }
        }

        private void HandleOnRotate() => Managers.BoardManager.Rotate();
        private void HandleOnHardDrop() => HardDrop();

        #endregion

        private void SpawnBlock()
        {
            if (_colorData == null)
            {
                Debug.Log("BlockColorData not found.");
                return;
            }

            Tetromino tetrominoType = Managers.BoardManager.CurrentTetromino;

            for (int i = 0; i < 4; i++)
            {
                _fallingBlocks[i] = Managers.ResourceManager.Instantiate(BlockPath, this.transform);
                var block = _fallingBlocks[i].GetComponent<Block>();
                if (block == null)
                {
                    Debug.Log("Block not found.");
                    return;
                }

                block.Init(_colorData.GetColor(tetrominoType));
            }
        }



        // ── DAS 반복 이동 ──────────────────────────────────
        private void HandleDAS()
        {
            if (_lastDir == 0) { _dasTimer = 0f; return; }

            _dasTimer += Time.deltaTime;
            if (_dasTimer < _dasDelay + _dasInterval) return;

            _dasTimer = _dasDelay; // 딜레이 이후부터는 Interval 속도로 반복
            Managers.BoardManager.Move(_lastDir < 0 ? Vector2Int.left : Vector2Int.right);
        }

        // ── 자동 낙하 ──────────────────────────────────────
        private void HandleFall()
        {
            // 소프트 드롭 중이면 빠르게 누적
            if (Managers.InputManager.IsSoftDropping)
                _fallTimer += Time.deltaTime * _softDropMul;

            _fallTimer += Time.deltaTime;

            if (_fallTimer < _fallTickRate) return;

            _fallTimer = 0f;
            bool canMove = Managers.BoardManager.Move(Vector2Int.down);

            if (!canMove)
            {
                LockVisuals();
                Managers.BoardManager.LockPiece();
                SyncLockedVisuals();
            }
        }

        // ── 하드 드롭 ──────────────────────────────────────
        private void HardDrop()
        {
            // 더 이상 못 내려갈 때까지 한 번에 끝까지 내림
            while (Managers.BoardManager.Move(Vector2Int.down)) { }
            LockVisuals();
            Managers.BoardManager.LockPiece();
            SyncLockedVisuals();
        }

        // ── 시각화 ─────────────────────────────────────────
        private void SyncVisuals()
        {
            var board = Managers.BoardManager;
            Vector2Int[,] cells = BlockData.Cells[board.CurrentTetromino];

            for (int i = 0; i < 4; i++)
            {
                Vector2Int offset = cells[board.CurrentRotation, i];
                int x = board.CurrentPos.x + offset.x;
                int y = board.CurrentPos.y + offset.y;

                if (_fallingBlocks[i] != null)
                    _fallingBlocks[i].transform.position = new Vector3(x, y, 0);
            }
        }

        private void LockVisuals()
        {
            var board = Managers.BoardManager;
            Vector2Int[,] cells = BlockData.Cells[board.CurrentTetromino];

            for (int i = 0; i < 4; i++)
            {
                Vector2Int offset = cells[board.CurrentRotation, i];
                int x = board.CurrentPos.x + offset.x;
                int y = board.CurrentPos.y + offset.y;

                if (y < 0 || y >= BoardManager.Height || x < 0 || x >= BoardManager.Width) continue;

                GameObject locked = Managers.ResourceManager.Instantiate("BlockPrefab", this.transform);
                locked.transform.position = new Vector3(x, y, 0);
                _lockedBlocks[y, x] = locked;
            }
        }

        private void SyncLockedVisuals()
        {
            var grid = Managers.BoardManager.Grid;

            for (int y = 0; y < BoardManager.Height; y++)
            {
                for (int x = 0; x < BoardManager.Width; x++)
                {
                    if (grid[y, x] == 0)
                    {
                        if (_lockedBlocks[y, x] != null)
                        {
                            Managers.ResourceManager.Destroy(_lockedBlocks[y, x]);
                            _lockedBlocks[y, x] = null;
                        }
                    }
                    else if (_lockedBlocks[y, x] != null)
                    {
                        _lockedBlocks[y, x].transform.position = new Vector3(x, y, 0);
                    }
                }
            }
        }
    }
}
