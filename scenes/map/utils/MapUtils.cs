using System.Collections.Generic;
using Godot;

/// <summary>
/// 地图工具类
/// </summary>
public static class MapUtils
{
    /// <summary>
    /// 瓦片大小，长和宽是多少像素
    /// </summary>
    public const int TILE_SIZE = 16;

    /// <summary>
    /// 邻居位置
    /// </summary>
    public static readonly Vector2I[] NEIGHBOUR_POSITIONS = [
        new Vector2I(0, 0),
        new Vector2I(1, 0),
        new Vector2I(0, 1),
        new Vector2I(1, 1)
    ];

    /// <summary>
    /// 双网格系统周围四个瓦片对应的当前瓦片在 tileset source 的位置
    /// key 是一个 4 位二进制数，表示左上，右上，左下，右下四个方向的邻居是否存在
    /// </summary>
    public static readonly Dictionary<int, Vector2I> TileAtlasCoordsByNeighbours = new()
    {
        { 0b0000, new Vector2I(0, 3) },
        { 0b0001, new Vector2I(1, 3) },
        { 0b0010, new Vector2I(0, 0) },
        { 0b0011, new Vector2I(3, 0) },
        { 0b0100, new Vector2I(0, 2) },
        { 0b0101, new Vector2I(1, 0) },
        { 0b0110, new Vector2I(2, 3) },
        { 0b0111, new Vector2I(1, 1) },
        { 0b1000, new Vector2I(3, 3) },
        { 0b1001, new Vector2I(0, 1) },
        { 0b1010, new Vector2I(3, 2) },
        { 0b1011, new Vector2I(2, 0) },
        { 0b1100, new Vector2I(1, 2) },
        { 0b1101, new Vector2I(2, 2) },
        { 0b1110, new Vector2I(3, 1) },
        { 0b1111, new Vector2I(2, 1) }
    };

    /// <summary>
    /// 根据邻居的存在情况，获取当前瓦片在 tileset source 的位置
    /// </summary>
    public static Vector2I GetTileAtlasCoordsByNeighbours(bool up, bool right, bool down, bool left)
    {
        int neighbours = 0;
        if (up) neighbours |= 0b1000;
        if (right) neighbours |= 0b0100;
        if (down) neighbours |= 0b0010;
        if (left) neighbours |= 0b0001;
        return TileAtlasCoordsByNeighbours[neighbours];
    }

    /// <summary>
    /// 全局像素坐标转为瓦片坐标
    /// </summary>
    public static Vector2I GlobalPixelToTile(Vector2 pixelPosition)
    {
        return (Vector2I)(pixelPosition / TILE_SIZE);
    }

    /// <summary>
    /// 全局像素坐标转为瓦片的全局像素坐标
    /// </summary>
    public static Vector2 GlobalPixelToTilePixel(Vector2 pixelPosition)
    {
        return GlobalPixelToTile(pixelPosition) * TILE_SIZE;
    }
}