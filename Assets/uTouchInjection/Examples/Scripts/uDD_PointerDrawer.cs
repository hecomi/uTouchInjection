using UnityEngine;
using State = uDD_TouchDispatcher.State;

[RequireComponent(typeof(uDD_TouchDispatcher))]
[RequireComponent(typeof(LineRenderer))]
public class uDD_PointerDrawer : MonoBehaviour
{
    uDD_TouchDispatcher dispatcher_;
    LineRenderer line_;

    [SerializeField] float nonHitAlpha = 0.2f;
    [SerializeField] float hitAlpha = 0.5f;
    [SerializeField] Color releaseColor = new Color(0.5f, 0.5f, 0.5f);
    [SerializeField] Color hoverColor = new Color(0.1f, 0.5f, 0.2f);
    [SerializeField] Color touchColor = new Color(0.8f, 0.2f, 0.1f);

    void Start()
    {
        dispatcher_ = GetComponent<uDD_TouchDispatcher>();
        line_ = GetComponent<LineRenderer>();
    }

    void Update()
    {
        UpdateLine();
        UpdateColor();
    }

    void UpdateLine()
    {
        line_.SetPosition(0, transform.position);

        if (dispatcher_.result.hit) {
            line_.SetPosition(1, dispatcher_.result.position);
        } else {
            line_.SetPosition(1, transform.position + transform.forward * 0.5f);
        }
    }

    void UpdateColor()
    {
        var color = Color.gray;

        switch (dispatcher_.state) {
            case State.Release : color = releaseColor; break;
            case State.Hover   : color = hoverColor;   break;
            case State.Touch   : color = touchColor;   break;
        }

        color.a = dispatcher_.result.hit ? hitAlpha : nonHitAlpha;
        color.a *= dispatcher_.isPrimaryPointer ? 1f : 0.5f;

        line_.SetColors(color, color);
    }
}