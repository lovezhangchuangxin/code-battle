using Godot;

public partial class StartGame : CanvasLayer
{
    public override void _Ready()
    {

    }

    /// <summary>
    /// 点击开始游戏按钮
    /// </summary>
    public void OnStartButtonPressed()
    {
    }

    /// <summary>
    /// 点击设置按钮
    /// </summary>
    public void OnSettingButtonPressed()
    {
    }

    /// <summary>
    /// 点击退出按钮
    /// </summary>
    public void OnExitButtonPressed()
    {
        GetTree().Quit();
    }
}
