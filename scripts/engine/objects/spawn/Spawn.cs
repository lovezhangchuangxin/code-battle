using Godot;

public partial class Spawn : Sprite2D, IGameObject
{
    public string Id { get; set; } = "0";

    public string OwnerName { get; set; } = "user";

    public string Type { get; set; } = "spawn";


    public override void _EnterTree()
    {
        Game.Objects[Id] = this;
    }

    public override void _ExitTree()
    {
        Game.Objects.Remove(Id);
    }

    /// <summary>
    /// 生产 bot
    /// </summary>
    public SpawnBotResult SpawnBot(string botName)
    {
        string id = Game.GenerateId();
        string ownerName = OwnerName;
        Vector2I position = (Vector2I)Position + Vector2I.Right * MapUtils.TILE_SIZE;
        var bot = Bot.CreateBot(id, botName, ownerName, position);
        Game.Bots[bot.Id] = bot;
        Game.Map.Bots.AddChild(bot);
        return SpawnBotResult.Ok;
    }
}

public class SpawnApi
{
    public string Id { get; set; }
    public string OwnerName { get; set; }
    public string Type { get; set; } = "spawn";
    public Vector2I Position { get; set; }

    public SpawnApi(Spawn spawn)
    {
        Id = spawn.Id;
        OwnerName = spawn.OwnerName;
        Type = spawn.Type;
        Position = (Vector2I)spawn.Position;
    }

    public SpawnBotResult SpawnBot(string botName)
    {
        var spawn = Game.Objects[Id] as Spawn;
        if (spawn == null)
        {
            return SpawnBotResult.InvalidSpawn;
        }
        return spawn.SpawnBot(botName);
    }
}

public enum SpawnBotResult
{
    Ok,
    InvalidSpawn,
}