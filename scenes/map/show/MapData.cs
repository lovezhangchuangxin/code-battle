using Godot;
using Godot.Collections;

public partial class MapData : Resource
{
    /// <summary>
    /// 地图长度，单位：瓦片
    /// </summary>
    [Export]
    public int MapLength { get; set; } = 40;

    /// <summary>
    /// 地图宽度，单位：瓦片
    /// </summary>
    [Export]
    public int MapWidth { get; set; } = 30;

    /// <summary>
    /// 每个位置的地形数据
    /// </summary>
    [Export]
    public Dictionary<Vector2I, TerrainType> TerrainData { get; set; } = [];

    /// <summary>
    /// 每个位置的对象数据
    /// </summary>
    [Export]
    public Dictionary<Vector2I, ObjectType> ObjectData { get; set; } = [];

    /// <summary>
    /// 文件保存目录
    /// </summary>
    public string SaveDirectory { get; set; } = "user://maps/";

    /// <summary>
    /// 保存地图数据到文件
    /// </summary>
    public void Save(string filename)
    {
        DirAccess.MakeDirRecursiveAbsolute(SaveDirectory);
        string filePath = SaveDirectory + filename;
        Error result = ResourceSaver.Save(this, filePath);
        if (result != Error.Ok)
        {
            GD.PrintErr($"Failed to save map data to {filePath}: {result}");
        }
        else
        {
            GD.Print($"Map data saved to {filePath}");
        }
    }

    /// <summary>
    /// 从文件中加载地图数据
    /// </summary>
    public MapData Load(string filename)
    {
        string filePath = SaveDirectory + filename;
        var loadedResource = ResourceLoader.Load(filePath);
        if (loadedResource is MapData mapData)
        {
            MapLength = mapData.MapLength;
            MapWidth = mapData.MapWidth;
            TerrainData = mapData.TerrainData;
            ObjectData = mapData.ObjectData;
            GD.Print($"Map data loaded from {filePath}");
        }
        else
        {
            GD.PrintErr($"Failed to load map data from {filePath}");
        }

        return this;
    }
}
