
public struct MapIndexData
{
    public int x;
    public int y;

    public MapIndexData(int y, int x)
    {
        this.y = y;
        this.x = x;
    }

    public static MapIndexData operator +(MapIndexData left, MapIndexData right)
    {
        MapIndexData mapIndexData = default;
        mapIndexData.x = left.x + right.x;
        mapIndexData.y = left.y + right.y;
        return mapIndexData;
    }

    public static MapIndexData operator -(MapIndexData left, MapIndexData right)
    {
        MapIndexData mapIndexData = default;
        mapIndexData.x = left.x + right.x;
        mapIndexData.y = left.y + right.y;
        return mapIndexData;
    }

    public static bool operator ==(MapIndexData left, MapIndexData right)
    {
        if (left.x == right.x && left.y == right.y)
        {
            return true;
        }
        return false;
    }

    public static bool operator !=(MapIndexData left, MapIndexData right)
    {
        if (!(left.x == right.x && left.y == right.y))
        {
            return true;
        }
        return false;
    }
}
