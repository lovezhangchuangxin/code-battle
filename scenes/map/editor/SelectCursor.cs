using Godot;

public partial class SelectCursor : AnimatedSprite2D
{
    [Export]
    public MapEditor MapEditor { get; set; }

    public override void _Process(double delta)
    {
        Vector2I coords = MapUtils.GlobalPixelToTile(GetGlobalMousePosition());
        if (Input.IsActionPressed("left_click"))
        {
            MapEditor.SetTerrain(coords, TerrainType.Swamp);
        }
        else if (Input.IsActionPressed("right_click"))
        {
            MapEditor.SetTerrain(coords, TerrainType.Lava);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        int offset = MapUtils.TILE_SIZE / 2;
        Position = MapUtils.GlobalPixelToTilePixel(GetGlobalMousePosition()) + new Vector2(offset, offset);
    }
}
