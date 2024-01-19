using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private class TileInfo
    {
        public GameObject _tile;
        public Vector2 _tilePos;
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

    private void Awake()
    {
        MapIndexData _mapSize = _mapData.CreateMap();

        InstancedMap();
    }


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
