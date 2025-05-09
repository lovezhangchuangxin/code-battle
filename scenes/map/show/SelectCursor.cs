using Godot;

public partial class SelectCursor : Sprite2D
{
    public override void _PhysicsProcess(double delta)
    {
        int offset = MapUtils.TILE_SIZE / 2;
        Position = MapUtils.GlobalPixelToTilePixel(GetGlobalMousePosition()) + new Vector2(offset, offset);
    }
}
