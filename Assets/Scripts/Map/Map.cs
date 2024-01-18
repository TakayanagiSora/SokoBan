using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    private GameObject _wall = default;
    [SerializeField]
    private GameObject _box = default;
    [SerializeField]
    private GameObject _goal = default;
    [SerializeField]
    private GameObject _player = default;

    private TileType[,] _mapData = default;
    private (int y, int x) _playerIndex = default;

    private void Awake()
    {
        CreateMap();
        InstancedMap();
    }

    /// <summary>
    /// マップを生成する
    /// <br>0 = マップ外,</br>
    /// <br>1 = 壁,</br>
    /// <br>2 = マップ内,</br>
    /// <br>3 = 箱,</br>
    /// <br>4 = ゴール,</br>
    /// <br>5 = プレイヤー</br>
    /// </summary>
    private void CreateMap()
    {
        const TileType _ = TileType.None;
        const TileType W = TileType.Wall;
        const TileType S = TileType.Space;
        const TileType B = TileType.Box;
        const TileType G = TileType.Goal;
        const TileType P = TileType.Player;

        _mapData = new TileType[11, 17]
        {
            {_, _, _, _, W, W, W, W, W, _, _, _, _, _, _, _, _ },
            {_, _, _, _, W, S, S, S, W, _, _, _, _, _, _, _, _ },
            {_, _, _, _, W, B, S, S, W, _, _, _, _, _, _, _, _ },
            {_, _, W, W, W, S, S, B, W, W, W, _, _, _, _, _, _ },
            {_, _, W, S, S, B, S, S, B, S, W, _, _, _, _, _, _ },
            {W, W, W, S, W, S, W, W, W, S, W, W, W, W, W, W, W },
            {W, S, S, S, W, S, W, W, W, S, W, W, S, S, G, G, W },
            {W, S, B, S, S, B, S, S, S, S, S, S, P, S, G, G, W },
            {W, W, W, W, W, S, W, W, W, W, S, W, S, S, G, G, W },
            {_, _, _, _, W, S, S, S, S, S, S, W, W, W, W, W, W },
            {_, _, _, _, W, W, W, W, W, W, W, W, _, _, _, _, _ },
        };

        _playerIndex = (7, 12);
    }

    private void DebugMap()
    {
        string mapData = default;

        for (int i = 0; i < _mapData.GetLength(0); i++)
        {
            for (int k = 0; k < _mapData.GetLength(1); k++)
            {
                mapData += $"{_mapData[i, k]}, ";
            }

            mapData += "\n";
        }

        mapData = mapData.Replace("None", "0");
        mapData = mapData.Replace("Wall", "1");
        mapData = mapData.Replace("Space", "2");
        mapData = mapData.Replace("Box", "3");
        mapData = mapData.Replace("Goal", "4");
        mapData = mapData.Replace("Player", "5");

        Debug.Log(mapData);
    }

    private void InstancedMap()
    {
        Vector2 spawnPos = new Vector2(-8f, 5f);

        for (int i = 0; i < _mapData.GetLength(0); i++)
        {
            for (int k = 0; k < _mapData.GetLength(1); k++)
            {
                switch (_mapData[i, k])
                {
                    case TileType.Wall:
                        Instantiate(_wall, spawnPos, Quaternion.identity);
                        break;

                    case TileType.Box:
                        Instantiate(_box, spawnPos, Quaternion.identity);
                        break;

                    case TileType.Goal:
                        Instantiate(_goal, spawnPos, Quaternion.identity);
                        break;

                    case TileType.Player:
                        Instantiate(_player, spawnPos, Quaternion.identity);
                        break;
                }

                spawnPos = new Vector2(spawnPos.x + 1f, spawnPos.y);
            }

            spawnPos = new Vector2(-8f, spawnPos.y - 1f);
        }
    }

    public void MapUpdate((int y, int x) moveDir)
    {
        _mapData[_playerIndex.y, _playerIndex.x] = TileType.Space;
        _mapData[-moveDir.y, moveDir.x] = TileType.Player;
    }
}
