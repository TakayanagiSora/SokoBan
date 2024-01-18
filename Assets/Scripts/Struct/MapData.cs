
public struct MapData
{
    private TileType[,] _map;
    public MapIndexData _playerIndex;


    public TileType[,] Map => _map;

    public TileType this[MapIndexData mapIndexData]
    {
        get
        {
            return _map[mapIndexData.y, mapIndexData.x];
        }
        set
        {
            _map[mapIndexData.y, mapIndexData.x] = value;
        }
    }


    /// <summary>
    /// É}ÉbÉvÇê∂ê¨Ç∑ÇÈ
    /// </summary>
    public void CreateMap()
    {
        const TileType _ = TileType.None;
        const TileType W = TileType.Wall;
        const TileType S = TileType.Space;
        const TileType B = TileType.Box;
        const TileType G = TileType.Goal;
        const TileType P = TileType.Player;

        _map = new TileType[11, 17]
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

        _playerIndex = new(7, 12);
    }
}
