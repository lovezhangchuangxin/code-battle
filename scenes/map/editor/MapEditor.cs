using Godot;

public partial class MapEditor : Node2D
{
    /// <summary>
    /// 地图
    /// </summary>
    [Export]
    public MapShow Map { get; set; }
}
