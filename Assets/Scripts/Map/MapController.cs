using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField]
    private GameObject _wall = default;
    [SerializeField]
    private GameObject _box = default;
    [SerializeField]
    private GameObject _goal = default;
    [SerializeField]
    private GameObject _player = default;

    private MapData _mapData = new();

    private void Awake()
    {
        _mapData.CreateMap();
        InstancedMap();
    }

    //private void DebugMap()
    //{
    //    string mapData = default;

    //    for (int i = 0; i < _mapData.GetLength(0); i++)
    //    {
    //        for (int k = 0; k < _mapData.GetLength(1); k++)
    //        {
    //            mapData += $"{_mapData[i, k]}, ";
    //        }

    //        mapData += "\n";
    //    }

    //    mapData = mapData.Replace("None", "0");
    //    mapData = mapData.Replace("Wall", "1");
    //    mapData = mapData.Replace("Space", "2");
    //    mapData = mapData.Replace("Box", "3");
    //    mapData = mapData.Replace("Goal", "4");
    //    mapData = mapData.Replace("Player", "5");

    //    Debug.Log(mapData);
    //}

    private void InstancedMap()
    {
        Vector2 spawnPos = new Vector2(-8f, 5f);

        for (int i = 0; i < _mapData.Map.GetLength(0); i++)
        {
            for (int k = 0; k < _mapData.Map.GetLength(1); k++)
            {
                switch (_mapData.Map[i, k])
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

    public void MapUpdate(MapIndexData moveDir)
    {
        _mapData[moveDir] = TileType.Space;
        _mapData.Map[-moveDir.y, moveDir.x] = TileType.Player;
    }
}
