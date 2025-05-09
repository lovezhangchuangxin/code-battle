using Godot;

public partial class MapCamera : Camera2D
{
    /// <summary>
    /// 相机移动速度
    /// </summary>
    [Export]
    public float MoveSpeed { get; set; } = 500f;

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            // 鼠标滚轮缩放
            if (mouseButtonEvent.ButtonIndex == MouseButton.WheelUp)
            {
                Zoom *= 0.95f;
            }
            else if (mouseButtonEvent.ButtonIndex == MouseButton.WheelDown)
            {
                Zoom *= 1.05f;
            }
            Zoom = new Vector2(Mathf.Clamp(Zoom.X, 0.5f, 4f), Mathf.Clamp(Zoom.Y, 0.5f, 4f));
        }
    }

    // 处理物理更新，用于移动相机
    public override void _PhysicsProcess(double delta)
    {
        // 获取键盘输入方向
        Vector2 inputDir = Vector2.Zero;

        // 检测方向键和 WASD 键的输入
        if (Input.IsActionPressed("move_right"))
            inputDir.X += 1;
        if (Input.IsActionPressed("move_left"))
            inputDir.X -= 1;
        if (Input.IsActionPressed("move_down"))
            inputDir.Y += 1;
        if (Input.IsActionPressed("move_up"))
            inputDir.Y -= 1;

        // 归一化方向向量，确保斜向移动速度不会更快
        if (inputDir.LengthSquared() > 0)
            inputDir = inputDir.Normalized();

        // 考虑缩放级别调整移动速度（缩放越大，移动越慢）
        float adjustedSpeed = MoveSpeed / Zoom.X;

        // 应用移动
        GlobalPosition += inputDir * adjustedSpeed * (float)delta;
    }

    /// <summary>
    /// 移动视野到地图中心
    /// </summary>
    public void MoveToMapCenter(int length, int width)
    {
        // 计算地图中心位置
        Position = new Vector2(length * MapUtils.TILE_SIZE / 2, width * MapUtils.TILE_SIZE / 2);
    }
}
