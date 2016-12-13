using UnityEngine;

namespace uTouchInjection
{

public class Pointer
{
    public Pointer(int id)
    {
        this.id = id;
    }

    public int id
    {
        get; 
        private set;
    }

    public int pressure
    {
        set { Lib.SetPressure(id, value); }
    }

    public int orientation
    {
        set { Lib.SetOrientation(id, value); }
    }

    public int areaSize
    {
        set { Lib.SetAreaSize(id, value); }
    }

    public Vector2 position
    {
        set { Lib.SetPosition(id, (int)value.x, (int)value.y); }
    }

    public void Release()
    {
        Lib.Release(id);
    }

    public void Hover()
    {
        Lib.Hover(id);
    }

    public void Touch()
    {
        Lib.Touch(id);
    }
}

}