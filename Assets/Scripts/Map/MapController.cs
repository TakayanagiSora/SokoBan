using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int _nowGoalAmount = default;

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

        InstancedMap();
    }


    /// <summary>
    /// �}�b�v��View�ɕ\������
    /// </summary>
    private void InstancedMap()
    {
        Vector2 spawnPos = new(-8f, 5f);
        GameObject tile;

        for (int i = 0; i < _mapData.Map.GetLength(0); i++)
        {
            for (int k = 0; k < _mapData.Map.GetLength(1); k++)
            {
                switch (_mapData.Map[i, k])
                {
                    case TileType.Wall:
                        tile = Instantiate(_wall, spawnPos, Quaternion.identity);
                        break;

                    case TileType.Box:
                        tile = Instantiate(_box, spawnPos, Quaternion.identity);
                        break;

                    case TileType.Goal:
                        tile = Instantiate(_goal, spawnPos, Quaternion.identity);
                        break;

                    case TileType.Player:
                        tile = Instantiate(_player, spawnPos, Quaternion.identity);
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

            spawnPos = new Vector2(-8f, spawnPos.y - 1f);
        }
    }

    /// <summary>
    /// �}�b�v��View����уf�[�^���X�V����
    /// </summary>
    /// <param name="moveDir"></param>
    public void MapUpdate(MapIndexData moveDir)
    {
        // �ړ�������z��̑�������ɕϊ��iy�������]�j
        moveDir.y = moveDir.y * -1;

        // �ړ���̃^�C���ɂ���ď�����e��
        switch (this[_mapData._playerIndex + moveDir]._tileType)
        {
            // �ړ��ł��Ȃ�
            case TileType.None:
                return;

            // �ړ��ł��Ȃ�
            case TileType.Wall:
                return;

            // Box���ړ�������
            case TileType.Box:
                // �}�b�v�f�[�^�i�񎟌��z��j���X�V
                MapIndexData boxIndex = _mapData._playerIndex + moveDir;

                // Box���������悪�ǂ�������Box�Ȃ�e��
                if (_mapData[boxIndex + moveDir] == TileType.Wall || _mapData[boxIndex + moveDir] == TileType.Box) { return; }

                _mapData[boxIndex + moveDir] = TileType.Box;

                // View�̍X�V�i���I�u�W�F�N�g�̍폜�ƐV�I�u�W�F�N�g�̐����j
                // �X�V����ێ��i����ȍ~�̌Ăяo���̍ۂɍ폜�ł���悤�j
                Destroy(this[boxIndex]._tile);

                this[boxIndex + moveDir]._tile = 
                    Instantiate(_box, this[boxIndex + moveDir]._tilePos, Quaternion.identity);
                this[boxIndex + moveDir]._tileType = TileType.Box;
                break;
        }

        // �}�b�v�f�[�^�i�񎟌��z��j���X�V
        _mapData[_mapData._playerIndex] = TileType.Space;
        _mapData[moveDir + _mapData._playerIndex] = TileType.Player;

        // View�̍X�V�i���I�u�W�F�N�g�̍폜�ƐV�I�u�W�F�N�g�̐����j
        // �X�V����ێ��i����ȍ~�̌Ăяo���̍ۂɍ폜�ł���悤�j
        Destroy(this[_mapData._playerIndex]._tile);

        this[_mapData._playerIndex + moveDir]._tile = 
            Instantiate(_player, this[_mapData._playerIndex + moveDir]._tilePos, Quaternion.identity);
        this[_mapData._playerIndex + moveDir]._tileType = TileType.Player;

        // �v���C���[�̈ʒu���X�V
        _mapData._playerIndex += moveDir;

        GoalCheck(_mapData._playerIndex + moveDir);
    }

    /// <summary>
    /// �S�[���������ǂ����`�F�b�N����
    /// </summary>
    public void GoalCheck(MapIndexData boxMovedIndex)
    {
        print("A");
        // �S�[������Ȃ���Βe��
        if (_mapData[boxMovedIndex] != TileType.Goal) { return; }

        _nowGoalAmount++;
        print("B");
        if (_mapData.GoalAmount <= _nowGoalAmount)
        {
            // �N���A
            this[boxMovedIndex]._tile.GetComponent<Goal>().OnGoal();
            print("Clear!");
        }
    }

    //private void DebugMap()
    //{
    //    string mapData = default;

    //    for (int i = 0; i < _mapData.Map.GetLength(0); i++)
    //    {
    //        for (int k = 0; k < _mapData.Map.GetLength(1); k++)
    //        {
    //            mapData += $"{_mapData.Map[i, k]}, ";
    //        }

    //        mapData += "\n";
    //    }

    //    mapData = mapData.Replace("None", "_");
    //    mapData = mapData.Replace("Wall", "W");
    //    mapData = mapData.Replace("Space", "S");
    //    mapData = mapData.Replace("Box", "B");
    //    mapData = mapData.Replace("Goal", "G");
    //    mapData = mapData.Replace("Player", "P");

    //    Debug.Log(mapData);
    //}
}
