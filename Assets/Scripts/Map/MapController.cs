using System.Collections;
using UnityEngine;
using UniRx;
using System;

public class MapController : MonoBehaviour
{
    public class TileInfo
    {
        public GameObject _tile;
        public Vector2 _tilePos;
        public TileType _tileType;
    }

    [SerializeField]
    private GameObject _wall = default;
    [SerializeField]
    private GameObject _box = default;
    [SerializeField]
    private GameObject _goal = default;
    [SerializeField]
    private GameObject _player = default;

    private MapData _mapData = new();
    private TileInfo[,] _tiles = default;
    private float _createWaitTime = 0.08f;
    private bool _isFinishCreate = false;
    private Coroutine _timeCountCoroutine = default;

    public int NumberOfMoves { get; private set; } = 0;
    public float PlayTime { get; private set; } = 0f;

    private Subject<Unit> _onClear = new();

    public IObservable<Unit> OnClear => _onClear;

    public TileInfo this[MapIndexData mapIndexData]
    {
        get
        {
            return _tiles[mapIndexData.y, mapIndexData.x];
        }
        set
        {
            _tiles[mapIndexData.y, mapIndexData.x] = value;
        }
    }

    private void Awake()
    {
        MapIndexData mapSize = _mapData.CreateMap();
        _tiles = new TileInfo[mapSize.y, mapSize.x];

        StartCoroutine(InstancedMap());
    }


    /// <summary>
    /// マップをViewに表示する
    /// </summary>
    private IEnumerator InstancedMap()
    {
        Vector2 spawnPos = new(-8f, 5f);
        GameObject tile;

        for (int i = 0; i < _mapData.Map.GetLength(0); i++)
        {
            WaitForSeconds createWait = new(_createWaitTime);

            for (int k = 0; k < _mapData.Map.GetLength(1); k++)
            {
                switch (_mapData.Map[i, k])
                {
                    case TileType.Wall:
                        tile = Instantiate(_wall, spawnPos, Quaternion.identity);

                        yield return createWait;
                        break;

                    case TileType.Box:
                        tile = Instantiate(_box, spawnPos, Quaternion.identity);

                        yield return createWait;
                        break;

                    case TileType.Goal:
                        tile = Instantiate(_goal, spawnPos, Quaternion.identity);

                        yield return createWait;
                        break;

                    case TileType.Player:
                        tile = Instantiate(_player, spawnPos, Quaternion.identity);

                        yield return createWait;
                        break;

                    default:
                        tile = null;
                        break;
                }

                // タイル（GameObject）の情報を保存
                _tiles[i, k] = new TileInfo();
                _tiles[i, k]._tile = tile;
                _tiles[i, k]._tilePos = spawnPos;
                _tiles[i, k]._tileType = _mapData.Map[i, k];

                spawnPos = new Vector2(spawnPos.x + 1f, spawnPos.y);
            }

            // だんだん生成間の待機時間を短くする
            _createWaitTime *= 0.85f;
            spawnPos = new Vector2(-8f, spawnPos.y - 1f);
        }

        _isFinishCreate = true;
        _timeCountCoroutine = StartCoroutine(TimeCountAsync());
    }

    /// <summary>
    /// マップのViewおよびデータを更新する
    /// </summary>
    /// <param name="moveDir"></param>
    public void MapUpdate(MapIndexData moveDir)
    {
        // マップの生成が完了するまで弾く
        if (!_isFinishCreate) { return; }

        // 移動方向を配列の操作方向に変換（yだけ反転）
        moveDir.y = moveDir.y * -1;

        TileType moveDirTile = this[_mapData._playerIndex + moveDir]._tileType;

        // 移動先のタイルによって処理をわける
        if (moveDirTile == TileType.Wall)
        {
            return;
        }
        else if (moveDirTile == TileType.Box || moveDirTile == TileType.BoxOnGoal)
        {
            // マップデータ（二次元配列）を更新
            MapIndexData boxIndex = _mapData._playerIndex + moveDir;

            // Boxを押した先が壁もしくはBoxなら弾く
            if (this[boxIndex + moveDir]._tileType == TileType.Wall || this[boxIndex + moveDir]._tileType == TileType.Box || this[boxIndex + moveDir]._tileType == TileType.BoxOnGoal) { return; }

            // ゴールかどうか判定
            if (this[boxIndex + moveDir]._tileType == TileType.Goal)
            {
                this[boxIndex + moveDir]._tileType = TileType.BoxOnGoal;
                Goal();
            }
            else
            {
                _mapData[boxIndex + moveDir] = TileType.Box;
                this[boxIndex + moveDir]._tileType = TileType.Box;
            }

            // Viewの更新（既オブジェクトの削除と新オブジェクトの生成）
            // 更新情報を保持（次回以降の呼び出しの際に削除できるよう）
            Destroy(this[boxIndex]._tile);

            this[boxIndex + moveDir]._tile =
                Instantiate(_box, this[boxIndex + moveDir]._tilePos, Quaternion.identity);

            // 手数データのカウント
            NumberOfMoves++;
        }

        if (_mapData[_mapData._playerIndex] == TileType.Goal)
        {
            // マップデータ（二次元配列）を更新
            this[_mapData._playerIndex]._tileType = TileType.Goal;
        }
        else
        {
            this[_mapData._playerIndex]._tileType = TileType.Space;
        }

        this[moveDir + _mapData._playerIndex]._tileType = TileType.Player;

        // Viewの更新（既オブジェクトの削除と新オブジェクトの生成）
        // 更新情報を保持（次回以降の呼び出しの際に削除できるよう）
        Destroy(this[_mapData._playerIndex]._tile);

        this[_mapData._playerIndex + moveDir]._tile =
            Instantiate(_player, this[_mapData._playerIndex + moveDir]._tilePos, Quaternion.identity);
        this[_mapData._playerIndex + moveDir]._tileType = TileType.Player;

        // プレイヤーの位置を更新
        _mapData._playerIndex += moveDir;
    }

    /// <summary>
    /// ゴール時の処理
    /// </summary>
    public void Goal()
    {
        int nowGoalAmount = 0;

        for (int i = 0; i < _mapData.Map.GetLength(0); i++)
        {
            for (int k = 0; k < _mapData.Map.GetLength(1); k++)
            {
                if (_tiles[i, k]._tileType == TileType.BoxOnGoal)
                {
                    nowGoalAmount++;
                }
            }
        }

        if (_mapData.GoalAmount <= nowGoalAmount)
        {
            StopCoroutine(_timeCountCoroutine);
            // クリア
            _onClear.OnNext(Unit.Default);
        }
    }

    private IEnumerator TimeCountAsync()
    {
        while (true)
        {
            PlayTime += Time.deltaTime;
            yield return null;
        }
    }

    private void DebugMap()
    {
        string mapData = default;

        for (int i = 0; i < _mapData.Map.GetLength(0); i++)
        {
            for (int k = 0; k < _mapData.Map.GetLength(1); k++)
            {
                mapData += $"{_tiles[i, k]._tileType}, ";
            }

            mapData += "\n";
        }

        mapData = mapData.Replace("None", "_");
        mapData = mapData.Replace("Wall", "W");
        mapData = mapData.Replace("Space", "S");
        mapData = mapData.Replace("Box", "B");
        mapData = mapData.Replace("Goal", "G");
        mapData = mapData.Replace("Player", "P");
        mapData = mapData.Replace("BOnG", "@");

        Debug.Log(mapData);
    }
}
