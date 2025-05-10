using Godot;
using System.Linq;

public partial class MapShow : Node2D
{
    /// <summary>
    /// 地图地面
    /// </summary>
    [Export]
    public ColorRect Ground { get; set; }

    /// <summary>
    /// 沼泽层
    /// </summary>
    [Export]
    public TileMapLayer SwampLayer { get; set; }

    /// <summary>
    /// 岩浆层
    /// </summary>
    [Export]
    public TileMapLayer LavaLayer { get; set; }

    /// <summary>
    /// 墙壁层
    /// </summary>
    [Export]
    public TileMapLayer WallLayer { get; set; }

    /// <summary>
    /// 对象层
    /// </summary>
    [Export]
    public TileMapLayer ObjectLayer { get; set; }

    /// <summary>
    /// 相机
    /// </summary>
    [Export]
    public MapCamera Camera { get; set; }

    /// <summary>
    /// 机器人都挂在这个节点下面
    /// </summary>
    [Export]
    public Node2D Bots { get; set; }

    /// <summary>
    /// 地图数据
    /// </summary>
    private MapData _mapData = new MapData();

    /// <summary>
    /// 地图数据
    /// </summary>
    public MapData MapData
    {
        get => _mapData;
        set
        {
            _mapData = value;
            ResizeMap(_mapData.MapLength, _mapData.MapWidth);
        }
    }

    public override void _EnterTree()
    {
        if (Game.MapName != null)
        {
            MapData.Load(Game.MapName + ".tres");
            Game.Map = this;
        }
    }

    public override void _Ready()
    {
        // 设置沼泽层和岩浆层的偏移量
        int offset = -MapUtils.TILE_SIZE / 2;
        SwampLayer.Position = new Vector2(offset, offset);
        LavaLayer.Position = new Vector2(offset, offset);
        ResizeMap(MapData.MapLength, MapData.MapWidth);
        if (Game.MapName != null)
        {
            LoadMapData();
        }
    }

    public override void _ExitTree()
    {
        Game.Map = null;
    }

    /// <summary>
    /// 加载地图数据
    /// </summary>
    public void LoadMapData()
    {
        // 清空所有层
        SwampLayer.Clear();
        LavaLayer.Clear();
        WallLayer.Clear();
        ObjectLayer.Clear();

        // 加载地形数据
        foreach (var kvp in MapData.TerrainData)
        {
            var coords = kvp.Key;
            var terrain = kvp.Value;
            SetTerrain(coords, terrain, true);
        }

        // 加载对象数据
        foreach (var kvp in MapData.ObjectData)
        {
            var coords = kvp.Key;
            var objectType = kvp.Value;
            SetObject(coords, objectType);
        }
    }

    /// <summary>
    /// 设置某个位置的地形数据，位置是以瓦片为单位
    /// </summary>
    public void SetTerrain(Vector2I coords, TerrainType terrain, bool isNew = false)
    {
        if (terrain == TerrainType.None)
        {
            RemoveTerrain(coords);
            return;
        }

        // 更新地形数据
        if (!isNew && MapData.TerrainData.TryGetValue(coords, out TerrainType existingTerrain))
        {
            // 移除旧的地形数据
            if (existingTerrain != terrain)
            {
                RemoveTerrain(coords);
            }
            else
            {
                return;
            }
        }
        MapData.TerrainData[coords] = terrain;

        if (terrain == TerrainType.Wall)
        {
            WallLayer.SetCell(coords, 3, Vector2I.Zero);
            return;
        }

        // 更新周围四个邻居的瓦片
        foreach (var neighbour in MapUtils.NEIGHBOUR_POSITIONS)
        {
            Vector2I neighbourPosition = coords + neighbour;
            SetTile(neighbourPosition, terrain);
        }
    }

    /// <summary>
    /// 移除某个位置的地形数据
    /// </summary>
    public void RemoveTerrain(Vector2I coords)
    {
        if (!MapData.TerrainData.TryGetValue(coords, out TerrainType terrain))
        {
            return;
        }

        // 移除地形数据
        MapData.TerrainData.Remove(coords);

        if (terrain == TerrainType.Wall)
        {
            WallLayer.EraseCell(coords);
            return;
        }

        // 更新周围四个邻居的瓦片
        foreach (var neighbour in MapUtils.NEIGHBOUR_POSITIONS)
        {
            Vector2I neighbourPosition = coords + neighbour;
            SetTile(neighbourPosition, terrain);
        }
    }

    /// <summary>
    /// 设置某个位置的对象
    /// </summary>
    public void SetObject(Vector2I coords, ObjectType objectType)
    {
        if (objectType == ObjectType.None)
        {
            MapData.ObjectData.Remove(coords);
            ObjectLayer.EraseCell(coords);
            return;
        }

        if (objectType == ObjectType.Energy)
        {
            MapData.ObjectData[coords] = objectType;
            ObjectLayer.SetCell(coords, 1, Vector2I.Zero, 1);
            return;
        }

        if (objectType == ObjectType.Spawn)
        {
            MapData.ObjectData[coords] = objectType;
            ObjectLayer.SetCell(coords, 1, Vector2I.Zero, 2);
            return;
        }

        if (objectType == ObjectType.NpcCore)
        {
            MapData.ObjectData[coords] = objectType;
            ObjectLayer.SetCell(coords, 1, Vector2I.Zero, 3);
            return;
        }
    }

    /// <summary>
    /// 设置瓦片
    /// </summary>
    public void SetTile(Vector2I position, TerrainType terrain)
    {
        int hash = 0;
        foreach (var neighbour in MapUtils.NEIGHBOUR_POSITIONS.Reverse())
        {
            Vector2I neighbourPosition = position - neighbour;
            hash <<= 1;
            MapData.TerrainData.TryGetValue(neighbourPosition, out TerrainType neighbourTerrain);
            hash |= neighbourTerrain == terrain ? 1 : 0;
        }

        Vector2I tileAtlasCoords = MapUtils.TileAtlasCoordsByNeighbours[hash];
        if (terrain == TerrainType.Swamp)
        {
            SwampLayer.SetCell(position, 0, tileAtlasCoords);
        }
        else if (terrain == TerrainType.Lava)
        {
            LavaLayer.SetCell(position, 1, tileAtlasCoords);
        }
    }

    /// <summary>
    /// 调整地图大小
    /// </summary>
    public void ResizeMap(int length, int width)
    {
        _mapData.MapLength = length;
        _mapData.MapWidth = width;
        Vector2 size = new(length * MapUtils.TILE_SIZE, width * MapUtils.TILE_SIZE);
        Ground.Size = size;
        ShaderMaterial material = Ground.Material as ShaderMaterial;
        material?.SetShaderParameter("resolution", size);
        material?.SetShaderParameter("grid_size", MapUtils.TILE_SIZE);
        Camera.MoveToMapCenter(length, width);
    }
}

public class MapApi
{
    public int Length { get; set; }
    public int Width { get; set; }
    public TerrainType[][] Terrains { get; set; }

    public MapApi(MapShow map)
    {
        MapData mapData = map.MapData;
        Length = mapData.MapLength;
        Width = mapData.MapWidth;
        Terrains = new TerrainType[Length][];
        for (int i = 0; i < Length; i++)
        {
            Terrains[i] = new TerrainType[Width];
            for (int j = 0; j < Width; j++)
            {
                Vector2I coords = new(i, j);
                if (mapData.TerrainData.TryGetValue(coords, out TerrainType terrain))
                {
                    Terrains[i][j] = terrain;
                }
                else
                {
                    Terrains[i][j] = TerrainType.None;
                }
            }
        }
    }
}