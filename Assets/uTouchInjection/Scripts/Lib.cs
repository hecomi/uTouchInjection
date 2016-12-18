using System.Runtime.InteropServices;

#pragma warning disable 114, 465

namespace uTouchInjection
{

public static class Lib
{
    [DllImport("uTouchInjection")]
    public static extern void Initialize(int touchNum);
    [DllImport("uTouchInjection")]
    public static extern void Finalize();
    [DllImport("uTouchInjection")]
    public static extern void Update();
    [DllImport("uTouchInjection")]
    public static extern void SetPosition(int id, int x, int y);
    [DllImport("uTouchInjection")]
    public static extern void SetPressure(int id, int pressure);
    [DllImport("uTouchInjection")]
    public static extern void SetAreaSize(int id, int areaSize);
    [DllImport("uTouchInjection")]
    public static extern void SetOrientation(int id, int degree);
    [DllImport("uTouchInjection")]
    public static extern void Touch(int id);
    [DllImport("uTouchInjection")]
    public static extern void Hover(int id);
    [DllImport("uTouchInjection")]
    public static extern void Release(int id);
    [DllImport("uTouchInjection")]
    public static extern void EnableLogOutput();
    [DllImport("uTouchInjection")]
    public static extern void DisableLogOutput();
}

}