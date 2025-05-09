using Godot;

public partial class SelectCursor : AnimatedSprite2D
{
    [Export]
    public MapEditor MapEditor { get; set; }

    [Export]
    public Label Label1 { get; set; }

    [Export]
    public Label Label2 { get; set; }

    [Export]
    public Label Label3 { get; set; }

    [Export]
    public Label Label4 { get; set; }

    [Export]
    public Label Label0 { get; set; }

    /// <summary>
    /// 当前选择的地形
    /// </summary>
    public TerrainType selectedTerrain = TerrainType.Wall;

    /// <summary>
    /// 当前选择的对象
    /// </summary>
    public ObjectType selectedObject = ObjectType.None;

    public override void _Draw()
    {
    }

    public override void _Process(double delta)
    {
        Vector2I coords = MapUtils.GlobalPixelToTile(GetGlobalMousePosition());
        if (Input.IsActionPressed("left_click"))
        {
            MapEditor.Map.SetTerrain(coords, selectedTerrain);
            MapEditor.Map.SetObject(coords, selectedObject);
        }
        else if (Input.IsActionPressed("right_click"))
        {
            MapEditor.Map.SetTerrain(coords, TerrainType.None);
            MapEditor.Map.SetObject(coords, ObjectType.None);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        int offset = MapUtils.TILE_SIZE / 2;
        Position = MapUtils.GlobalPixelToTilePixel(GetGlobalMousePosition()) + new Vector2(offset, offset);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        Color color = new("#d89e62");

        void reset()
        {
            Color white = new(1, 1, 1, 1);
            Label0.Modulate = white;
            Label1.Modulate = white;
            Label2.Modulate = white;
            Label3.Modulate = white;
            Label4.Modulate = white;
        }

        if (@event.IsActionPressed("key_0"))
        {
            selectedTerrain = TerrainType.None;
            selectedObject = ObjectType.None;
            reset();
            Label0.Modulate = color;
        }
        else if (@event.IsActionPressed("key_1"))
        {
            selectedTerrain = TerrainType.Wall;
            selectedObject = ObjectType.None;
            reset();
            Label1.Modulate = color;
        }
        else if (@event.IsActionPressed("key_2"))
        {
            selectedTerrain = TerrainType.Swamp;
            selectedObject = ObjectType.None;
            reset();
            Label2.Modulate = color;
        }
        else if (@event.IsActionPressed("key_3"))
        {
            selectedTerrain = TerrainType.Lava;
            selectedObject = ObjectType.None;
            reset();
            Label3.Modulate = color;
        }
        else if (@event.IsActionPressed("key_4"))
        {
            selectedTerrain = TerrainType.None;
            selectedObject = ObjectType.Energy;
            reset();
            Label4.Modulate = color;
        }
    }
}
