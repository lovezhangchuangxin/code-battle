using System;
using System.Collections.Generic;

/// <summary>
/// 游戏全局对象
/// </summary>
public static class Game
{
    /// <summary>
    /// 游戏 tick 数
    /// </summary>
    public static int Tick { get; set; } = 0;

    /// <summary>
    /// 1 tick多长，单位是秒
    /// </summary>
    public static float TickTime { get; set; } = 1f;

    /// <summary>
    /// 所有机器人
    /// </summary>
    public static Dictionary<string, Bot> Bots { get; set; } = [];

    /// <summary>
    /// 除机器人之外的所有对象
    /// </summary>
    public static Dictionary<string, IGameObject> Objects { get; set; } = [];

    /// <summary>
    /// 地图的名字
    /// </summary>
    public static string MapName { get; set; } = "map";

    /// <summary>
    /// 游戏地图场景
    /// </summary>
    public static MapShow Map { get; set; }

    /// <summary>
    /// 往引擎上设置 api
    /// </summary>
    public static void SetEngineApi(Jint.Engine engine)
    {
        engine.SetValue("Game", new GameApi());
        engine.SetValue("Map", new MapApi(Map));
    }

    /// <summary>
    /// 生成唯一的 ID
    /// </summary>
    public static string GenerateId(int length = 10)
    {
        const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        Random random = new();
        char[] id = new char[length];

        for (int i = 0; i < length; i++)
        {
            id[i] = chars[random.Next(chars.Length)];
        }

        return new string(id);
    }
}

public class GameApi
{
    /// <summary>
    /// 获取游戏 tick
    /// </summary>
    public int GetTick()
    {
        return Game.Tick;
    }

    /// <summary>
    /// 获取游戏每 tick 时间
    /// </summary>
    public float GetTickTime()
    {
        return Game.TickTime;
    }

    /// <summary>
    /// 获取所有的机器人
    /// </summary>
    public Dictionary<string, BotApi> GetBots()
    {
        Dictionary<string, BotApi> bots = [];
        foreach (var bot in Game.Bots)
        {
            bots[bot.Key] = new BotApi(bot.Value);
        }
        return bots;
    }

    /// <summary>
    /// 获取所有的对象
    /// </summary>
    public Dictionary<string, object> GetObjects()
    {
        Dictionary<string, object> objects = [];
        foreach (var obj in Game.Objects)
        {
            object o = null;
            if (obj.Value is Spawn spawn)
            {
                o = new SpawnApi(spawn);
            }
            objects[obj.Key] = o;
        }
        return objects;
    }
}