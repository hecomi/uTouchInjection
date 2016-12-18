using UnityEngine;
using State = uDD_TouchDispatcher.State;

[RequireComponent(typeof(uDD_TouchDispatcher))]
[RequireComponent(typeof(LineRenderer))]
public class uDD_PointerDrawer : MonoBehaviour
{
    uDD_TouchDispatcher dispatcher_;
    LineRenderer line_;

    [SerializeField] GameObject cursor;
    [SerializeField] float nonHitAlpha = 0.2f;
    [SerializeField] float hitAlpha = 0.5f;
    [SerializeField] Color releaseColor = new Color(0.5f, 0.5f, 0.5f);
    [SerializeField] Color hoverColor = new Color(0.1f, 0.5f, 0.2f);
    [SerializeField] Color touchColor = new Color(0.8f, 0.2f, 0.1f);

    Color color_;
    Material cursorMaterial_;

    void Start()
    {
        dispatcher_ = GetComponent<uDD_TouchDispatcher>();
        line_ = GetComponent<LineRenderer>();
    }

    void Update()
    {
        UpdateColor();
        UpdateLine();
        UpdateCursor();
    }

    void UpdateColor()
    {
        switch (dispatcher_.state) {
            case State.Release : color_ = releaseColor; break;
            case State.Hover   : color_ = hoverColor;   break;
            case State.Touch   : color_ = touchColor;   break;
        }

        color_.a = dispatcher_.result.hit ? hitAlpha : nonHitAlpha;
        color_.a *= dispatcher_.isPrimaryPointer ? 1f : 0.5f;
    }

    void UpdateLine()
    {
        line_.SetPosition(0, transform.position);

        if (dispatcher_.result.hit) {
            line_.SetPosition(1, dispatcher_.result.position);
        } else {
            line_.SetPosition(1, transform.position + transform.forward * 0.5f);
        }

        line_.SetColors(color_, color_);
    }

    void UpdateCursor()
    {
        if (cursor == null) return;

        if (cursorMaterial_ == null) {
            cursorMaterial_ = cursor.GetComponent<Renderer>().material;
        }

        if (dispatcher_.state == State.Release) {
            cursor.SetActive(false);
            return;
        }

        var result = dispatcher_.result;
        var texture = result.texture;
        var coord = dispatcher_.filteredDesktopCoord;
        coord.x -= texture.monitor.left;
        coord.y -= texture.monitor.top;
        var pos = texture.GetWorldPositionFromCoord(coord);
        cursor.transform.position = pos + result.normal * 0.01f;
        cursor.transform.rotation = Quaternion.LookRotation(result.normal, result.texture.transform.up);
        cursor.SetActive(true);

        var color = color_;
        color.a *= 0.2f;
        cursorMaterial_.SetColor("_TintColor", color);
    }
}