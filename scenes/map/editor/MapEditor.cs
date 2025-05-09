using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class MapEditor : Node2D
{
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
    /// 每个位置的地形数据
    /// </summary>
    public Dictionary<Vector2I, TerrainType> TerrainData { get; set; } = [];

    public override void _Ready()
    {
        // 设置沼泽层和岩浆层的偏移量
        int offset = -MapUtils.TILE_SIZE / 2;
        SwampLayer.Position = new Vector2(offset, offset);
        LavaLayer.Position = new Vector2(offset, offset);
        WallLayer.Position = new Vector2(offset, offset);
    }

    /// <summary>
    /// 设置某个位置的地形数据，位置是以瓦片为单位
    /// </summary>
    public void SetTerrain(Vector2I coords, TerrainType terrain)
    {
        if (terrain == TerrainType.None)
        {
            RemoveTerrain(coords);
            return;
        }

        // 更新地形数据
        if (TerrainData.TryGetValue(coords, out TerrainType existingTerrain))
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
        TerrainData[coords] = terrain;

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
        if (!TerrainData.TryGetValue(coords, out TerrainType terrain))
        {
            return;
        }

        // 移除地形数据
        TerrainData.Remove(coords);
        // 更新周围四个邻居的瓦片
        foreach (var neighbour in MapUtils.NEIGHBOUR_POSITIONS)
        {
            Vector2I neighbourPosition = coords + neighbour;
            SetTile(neighbourPosition, terrain);
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
            TerrainData.TryGetValue(neighbourPosition, out TerrainType neighbourTerrain);
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
        else if (terrain == TerrainType.Wall)
        {
            WallLayer.SetCell(position, 2, tileAtlasCoords);
        }
    }
}
