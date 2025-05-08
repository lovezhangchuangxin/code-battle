using Godot;

public partial class SelectCursor : AnimatedSprite2D
{
    [Export]
    public MapEditor MapEditor { get; set; }

    [Export]
    public Label Label { get; set; }

    [Export]
    public Label Label2 { get; set; }

    [Export]
    public Label Label3 { get; set; }

    [Export]
    public Label Label4 { get; set; }

    /// <summary>
    /// 当前选择的地形
    /// </summary>
    public TerrainType selectedTerrain = TerrainType.None;

    public override void _Draw()
    {
    }

    public override void _Process(double delta)
    {
        Vector2I coords = MapUtils.GlobalPixelToTile(GetGlobalMousePosition());
        if (Input.IsActionPressed("left_click"))
        {
            MapEditor.SetTerrain(coords, selectedTerrain);
        }
        else if (Input.IsActionPressed("right_click"))
        {
            MapEditor.SetTerrain(coords, TerrainType.None);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        int offset = MapUtils.TILE_SIZE / 2;
        Position = MapUtils.GlobalPixelToTilePixel(GetGlobalMousePosition()) + new Vector2(offset, offset);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        Color color = new("#D9753D");

        var reset = () =>
        {
            Color white = new(1, 1, 1, 1);
            Label.Modulate = white;
            Label2.Modulate = white;
            Label3.Modulate = white;
            Label4.Modulate = white;
        };

        if (@event.IsActionPressed("key_0"))
        {
            selectedTerrain = TerrainType.None;
            reset();
            Label4.Modulate = color;
        }
        else if (@event.IsActionPressed("key_1"))
        {
            selectedTerrain = TerrainType.Wall;
            reset();
            Label.Modulate = color;
        }
        else if (@event.IsActionPressed("key_2"))
        {
            selectedTerrain = TerrainType.Swamp;
            reset();
            Label2.Modulate = color;
        }
        else if (@event.IsActionPressed("key_3"))
        {
            selectedTerrain = TerrainType.Lava;
            reset();
            Label3.Modulate = color;
        }
    }
}
