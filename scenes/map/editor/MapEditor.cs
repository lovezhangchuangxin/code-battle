using Godot;

public partial class MapEditor : Node2D
{
    /// <summary>
    /// 地图
    /// </summary>
    [Export]
    public MapShow Map { get; set; }

    [Export]
    public Label Label1 { get; set; }

    [Export]
    public Label Label2 { get; set; }

    [Export]
    public Label Label3 { get; set; }

    [Export]
    public Label Label4 { get; set; }

    [Export]
    public Label Label5 { get; set; }

    [Export]
    public Label Label6 { get; set; }

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

    public override void _Process(double delta)
    {
        Vector2I coords = MapUtils.GlobalPixelToTile(GetGlobalMousePosition());
        if (coords.X < 0 || coords.Y < 0 || coords.X >= Map.MapData.MapLength || coords.Y >= Map.MapData.MapWidth)
        {
            return;
        }

        if (Input.IsActionPressed("left_click"))
        {
            Map.SetTerrain(coords, selectedTerrain);
            Map.SetObject(coords, selectedObject);
        }
        else if (Input.IsActionPressed("right_click"))
        {
            Map.SetTerrain(coords, TerrainType.None);
            Map.SetObject(coords, ObjectType.None);
        }
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
            Label5.Modulate = white;
            Label6.Modulate = white;
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
        else if (@event.IsActionPressed("key_5"))
        {
            selectedTerrain = TerrainType.None;
            selectedObject = ObjectType.Spawn;
            reset();
            Label5.Modulate = color;
        }
        else if (@event.IsActionPressed("key_6"))
        {
            selectedTerrain = TerrainType.None;
            selectedObject = ObjectType.NpcCore;
            reset();
            Label6.Modulate = color;
        }
    }

    public void OnLengthChanged(string text)
    {
        try
        {
            MapData MapData = Map.MapData;
            MapData.MapLength = int.Parse(text);
            Map.MapData = MapData;
        }
        catch (System.Exception)
        {

        }
    }

    public void OnWidthChanged(string text)
    {
        try
        {
            MapData MapData = Map.MapData;
            MapData.MapWidth = int.Parse(text);
            Map.MapData = MapData;
        }
        catch (System.Exception)
        {

        }
    }

    public void OnSaveButtonPressed()
    {
        Map.MapData.Save("map.tres");
    }

    public void OnLoadButtonPressed()
    {
        Map.MapData.Load("map.tres");
        Map.LoadMapData();
    }
}
