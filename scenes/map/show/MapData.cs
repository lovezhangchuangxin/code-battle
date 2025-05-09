using Godot;
using Godot.Collections;

public partial class MapData : Resource
{
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
