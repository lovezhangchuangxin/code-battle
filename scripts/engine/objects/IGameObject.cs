public interface IGameObject
{
    /// <summary>
    /// 游戏对象 ID
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// 游戏对象类型
    /// </summary>
    string Type { get; set; }

    /// <summary>
    /// 游戏对象的拥有者
    /// </summary>
    string OwnerName { get; set; }
}