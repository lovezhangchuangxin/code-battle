using Godot;
using Godot.Collections;

public partial class MapData : Resource
{
    /// <summary>
    /// 地图长度，单位：瓦片
    /// </summary>
    [Export]
    public int MapLength { get; set; } = 60;

    /// <summary>
    /// 地图宽度，单位：瓦片
    /// </summary>
    [Export]
    public int MapWidth { get; set; } = 40;

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
}
