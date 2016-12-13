using UnityEngine;
using uTouchInjection;

public class Test : MonoBehaviour
{
    public int frame = 0;
    public int x = 400;
    public int y = 300;

    Pointer pointer0;
    Pointer pointer1;

    void Start()
    {
        pointer0 = Manager.pointers[0];
        pointer1 = Manager.pointers[1];
    }

    void Update()
    {
        if (frame < 100) {
            pointer0.Release();
        } else if (frame < 200) {
            pointer0.Hover();
            ++x;
        } else if (frame < 300) {
            pointer0.Touch();
            ++x;
            ++y;
        } else if (frame < 400) {
            pointer0.Hover();
            ++y;
        } else if (frame < 500) {
            pointer0.Release();
        } else {
            UnityEditor.EditorApplication.ExecuteMenuItem("Edit/Play");
        }

        pointer0.position = new Vector2(x, y);

        ++frame;
    }
}
