using Godot;

public partial class Bot : Sprite2D, IGameObject
{
    /// <summary>
    /// 机器人场景路径
    /// </summary>
    public static readonly string BotScenePath = "res://scenes/objects/bot/bot.tscn";

    public string Id { get; set; }

    public string OwnerName { get; set; }

    public string Type { get; set; } = "bot";

    /// <summary>
    /// 机器人名称
    /// </summary>
    public string BotName { get; set; }

    /// <summary>
    /// 机器人的位置
    /// </summary>
    protected Vector2I _position;

    public override void _EnterTree()
    {
        Game.Bots[Id] = this;
    }

    public override void _ExitTree()
    {
        Game.Bots.Remove(Id);
    }

    /// <summary>
    /// 获取机器人的位置
    /// </summary>
    public Vector2I GetPos()
    {
        return _position;
    }

    /// <summary>
    /// 移动
    /// </summary>
    public BotMoveResult Move(int direction)
    {
        Vector2I dir;
        switch (direction)
        {
            case 0:
                dir = Vector2I.Up;
                break;
            case 1:
                dir = Vector2I.Right;
                break;
            case 2:
                dir = Vector2I.Down;
                break;
            case 3:
                dir = Vector2I.Left;
                break;
            default:
                return BotMoveResult.InvalidPosition;
        }

        Vector2I newPos = _position + dir * MapUtils.TILE_SIZE;
        _position = newPos;
        Position = newPos;
        return BotMoveResult.Ok;
    }

    /// <summary>
    /// 创建机器人
    /// </summary>
    public static Bot CreateBot(string id, string botName, string ownerName, Vector2I position)
    {
        var bot = GD.Load<PackedScene>(BotScenePath).Instantiate<Bot>();
        bot.Id = id;
        bot.BotName = botName;
        bot.OwnerName = ownerName;
        bot._position = position;
        bot.Position = position;
        return bot;
    }
}

public class BotApi
{
    public string Id { get; set; }
    public string BotName { get; set; }
    public string OwnerName { get; set; }
    public string Type { get; set; } = "bot";

    public BotApi(Bot bot)
    {
        Id = bot.Id;
        BotName = bot.BotName;
        OwnerName = bot.OwnerName;
        Type = bot.Type;
    }

    public Vector2I GetPos()
    {
        return Game.Bots[Id].GetPos();
    }

    public BotMoveResult Move(int direction)
    {
        return Game.Bots[Id].Move(direction);
    }
}

public enum BotMoveResult
{
    Ok,
    InvalidPosition,
}