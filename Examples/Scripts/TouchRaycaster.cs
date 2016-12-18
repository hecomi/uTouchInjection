using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TouchRaycaster : MonoBehaviour
{
    static int currentId = 0;

    LineRenderer line_;
    uTouchInjection.Pointer pointer_;

    [SerializeField] float nonHitAlpha = 0.2f;
    [SerializeField] float hitAlpha = 0.5f;
    [SerializeField] Color releaseColor = new Color(0.5f, 0.5f, 0.5f);
    [SerializeField] Color hoverColor = new Color(0.1f, 0.5f, 0.2f);
    [SerializeField] Color touchColor = new Color(0.8f, 0.2f, 0.1f);
    [SerializeField] OVRInput.RawButton touchInputTrigger = OVRInput.RawButton.LIndexTrigger;
    [SerializeField] OVRInput.RawTouch hoverInputTrigger = OVRInput.RawTouch.LIndexTrigger;

    bool isHit_ = false;
    Vector2 hitPos_ = Vector2.zero;

    bool isPrimaryPointer
    {
        get { return pointer_ != null && pointer_.id == 0; }
    }

    enum State
    {
        Release,
        Hover,
        Touch,
    }
    State state_ = State.Release;

    void Start()
    {
        line_ = GetComponent<LineRenderer>();
    }

    void Update()
    {
        UpdateLine();
        UpdateTouch();
    }

    void UpdateLine()
    {
        line_.SetPosition(0, transform.position);

        var result = uDesktopDuplication.Texture.RayCastAll(transform.position, transform.forward * 9999f);
        isHit_ = result.hit;

        if (result.hit) {
            line_.SetPosition(1, result.position);
            hitPos_ = result.desktopCoords;
        } else {
            line_.SetPosition(1, transform.position + transform.forward * 0.5f);
        }

        var color = Color.gray;
        switch (state_) {
            case State.Release : color = releaseColor; break;
            case State.Hover   : color = hoverColor;   break;
            case State.Touch   : color = touchColor;   break;
        }
        color.a = result.hit ? hitAlpha : nonHitAlpha;
        color.a *= isPrimaryPointer ? 1f : 0.5f;
        line_.SetColors(color, color);
    }

    void GetPointer()
    {
        pointer_ = uTouchInjection.Manager.GetPointer(currentId);
        currentId++;
    }

    void ReleasePointer()
    {
        pointer_.Release();
        pointer_ = null;
        currentId--;
    }

    void UpdateTouch()
    {
        switch (state_) {
            case State.Release: {
                if (OVRInput.Get(hoverInputTrigger)) {
                    GetPointer();
                    state_ = State.Hover;
                }
                break;
            }
            case State.Hover: {
                pointer_.position = hitPos_;
                pointer_.Hover();

                if (OVRInput.Get(touchInputTrigger)) {
                    state_ = State.Touch;
                } else if (!OVRInput.Get(hoverInputTrigger)) {
                    ReleasePointer();
                    state_ = State.Release;
                }
                break;
            }
            case State.Touch: {
                pointer_.position = hitPos_;
                pointer_.Touch();

                if (!OVRInput.Get(touchInputTrigger)) {
                    state_ = State.Hover;
                }
                break;
            }
        }
    }
}