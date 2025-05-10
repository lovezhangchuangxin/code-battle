export const run = () => {
  const bots = Object.values(Game.GetBots());
  if (bots.length < 5) {
    const spawn = Object.values(Game.GetObjects()).find(
      (o) => o.type === "spawn"
    );
    if (spawn) {
      const result = spawn.SpawnBot("keqing" + bots.length);
      if (result == 0) {
        console.log("Spawned bot: " + "keqing" + bots.length);
      }
    }
  }

  bots.forEach((bot) => {
    const result = bot.Move(1);
    console.log("Bot " + bot.BotName + " moved: " + result);
  });
};
