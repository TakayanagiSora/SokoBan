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
    /// �}�b�v��View�ɕ\������
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

                // �^�C���iGameObject�j�̏���ۑ�
                _tiles[i, k] = new TileInfo();
                _tiles[i, k]._tile = tile;
                _tiles[i, k]._tilePos = spawnPos;
                _tiles[i, k]._tileType = _mapData.Map[i, k];

                spawnPos = new Vector2(spawnPos.x + 1f, spawnPos.y);
            }

            // ���񂾂񐶐��Ԃ̑ҋ@���Ԃ�Z������
            _createWaitTime *= 0.85f;
            spawnPos = new Vector2(-8f, spawnPos.y - 1f);
        }

        _isFinishCreate = true;
    }

    /// <summary>
    /// �}�b�v��View����уf�[�^���X�V����
    /// </summary>
    /// <param name="moveDir"></param>
    public void MapUpdate(MapIndexData moveDir)
    {
        // �}�b�v�̐�������������܂Œe��
        if (!_isFinishCreate) { return; }

        // �ړ�������z��̑�������ɕϊ��iy�������]�j
        moveDir.y = moveDir.y * -1;

        TileType moveDirTile = this[_mapData._playerIndex + moveDir]._tileType;

        // �ړ���̃^�C���ɂ���ď������킯��
        if (moveDirTile == TileType.Wall)
        {
            return;
        }
        else if (moveDirTile == TileType.Box || moveDirTile == TileType.BoxOnGoal)
        {
            // �}�b�v�f�[�^�i�񎟌��z��j���X�V
            MapIndexData boxIndex = _mapData._playerIndex + moveDir;

            // Box���������悪�ǂ�������Box�Ȃ�e��
            if (this[boxIndex + moveDir]._tileType == TileType.Wall || this[boxIndex + moveDir]._tileType == TileType.Box || this[boxIndex + moveDir]._tileType == TileType.BoxOnGoal) { return; }

            // �S�[�����ǂ�������
            if (this[boxIndex + moveDir]._tileType == TileType.Goal)
            {
                this[boxIndex + moveDir]._tileType = TileType.BoxOnGoal;
                Goal(boxIndex + moveDir);
            }
            else
            {
                _mapData[boxIndex + moveDir] = TileType.Box;
                this[boxIndex + moveDir]._tileType = TileType.Box;
            }

            // View�̍X�V�i���I�u�W�F�N�g�̍폜�ƐV�I�u�W�F�N�g�̐����j
            // �X�V����ێ��i����ȍ~�̌Ăяo���̍ۂɍ폜�ł���悤�j
            Destroy(this[boxIndex]._tile);

            this[boxIndex + moveDir]._tile =
                Instantiate(_box, this[boxIndex + moveDir]._tilePos, Quaternion.identity);
        }

        if (_mapData[_mapData._playerIndex] == TileType.Goal)
        {
            // �}�b�v�f�[�^�i�񎟌��z��j���X�V
            this[_mapData._playerIndex]._tileType = TileType.Goal;
        }
        else
        {
            this[_mapData._playerIndex]._tileType = TileType.Space;
        }

        this[moveDir + _mapData._playerIndex]._tileType = TileType.Player;

        // View�̍X�V�i���I�u�W�F�N�g�̍폜�ƐV�I�u�W�F�N�g�̐����j
        // �X�V����ێ��i����ȍ~�̌Ăяo���̍ۂɍ폜�ł���悤�j
        Destroy(this[_mapData._playerIndex]._tile);

        this[_mapData._playerIndex + moveDir]._tile =
            Instantiate(_player, this[_mapData._playerIndex + moveDir]._tilePos, Quaternion.identity);
        this[_mapData._playerIndex + moveDir]._tileType = TileType.Player;

        // �v���C���[�̈ʒu���X�V
        _mapData._playerIndex += moveDir;
    }

    /// <summary>
    /// �S�[�����̏���
    /// </summary>
    public void Goal(MapIndexData boxMovedIndex)
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
            // �N���A
            print("Clear!");
            _onClear.OnNext(Unit.Default);
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
