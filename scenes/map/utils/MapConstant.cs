/// <summary>
/// 地形类型
/// </summary>
public enum TerrainType
{
    /// <summary>
    /// 无地形
    /// </summary>
    None,
    /// <summary>
    /// 沼泽
    /// </summary>
    Swamp,
    /// <summary>
    /// 岩浆
    /// </summary>
    Lava,
    /// <summary>
    /// 墙壁
    /// </summary>
    Wall
}

/// <summary>
/// 对象类型
/// </summary>
public enum ObjectType
{
    /// <summary>
    /// 空对象
    /// </summary>
    None = 0,
    /// <summary>
    /// 能量
    /// </summary>
    Energy,
    /// <summary>
    /// 母巢
    /// </summary>
    Spawn,
    /// <summary>
    /// Npc 核心
    /// </summary>
    NpcCore
}