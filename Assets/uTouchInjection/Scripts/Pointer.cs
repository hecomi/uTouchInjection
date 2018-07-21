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

    public void Release(Vector2 position)
    {
        this.position = position;
        Lib.Release(id);
    }

    public void Hover()
    {
        Lib.Hover(id);
    }

    public void Hover(Vector2 position)
    {
        this.position = position;
        Lib.Hover(id);
    }

    public void Touch()
    {
        Lib.Touch(id);
    }

    public void Touch(Vector2 position)
    {
        this.position = position;
        Lib.Touch(id);
    }
}

}