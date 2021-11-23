using System.Runtime.InteropServices;

#pragma warning disable 114, 465

namespace uTouchInjection
{

public enum DebugMode
{
    None = 0,
    File = 1,
    UnityLog = 2,
}

public static class Lib
{
    const string dllName = "uTouchInjection";

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DebugLogDelegate(string str);

    [DllImport(dllName)]
    public static extern void SetDebugMode(DebugMode mode);
    [DllImport(dllName)]
    public static extern void SetLogFunc(DebugLogDelegate func);
    [DllImport(dllName)]
    public static extern void SetErrorFunc(DebugLogDelegate func);
    [DllImport(dllName)]
    public static extern void Initialize(int touchNum);
    [DllImport(dllName)]
    public static extern void Finalize();
    [DllImport(dllName)]
    public static extern void Update();
    [DllImport(dllName)]
    public static extern void SetPosition(int id, int x, int y);
    [DllImport(dllName)]
    public static extern void SetPressure(int id, int pressure);
    [DllImport(dllName)]
    public static extern void SetAreaSize(int id, int areaSize);
    [DllImport(dllName)]
    public static extern void SetOrientation(int id, int degree);
    [DllImport(dllName)]
    public static extern void Touch(int id);
    [DllImport(dllName)]
    public static extern void Hover(int id);
    [DllImport(dllName)]
    public static extern void Release(int id);
    [DllImport(dllName)]
    public static extern void EnableLogOutput();
    [DllImport(dllName)]
    public static extern void DisableLogOutput();
}

}