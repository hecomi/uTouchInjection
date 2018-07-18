using UnityEngine;

namespace uTouchInjection
{

public class uDD_TouchDispatcher : MonoBehaviour
{
    private static int currentId = 0;

    uTouchInjection.Pointer pointer_;
    bool isFirstTouch_ = true;

    [SerializeField] OVRInput.RawButton touchInputTrigger = OVRInput.RawButton.LIndexTrigger;
    [SerializeField] OVRInput.RawTouch hoverInputTrigger = OVRInput.RawTouch.LIndexTrigger;
    [SerializeField, Range(0f, 1f)] float filter = 0.8f;
    [SerializeField] float maxRayDistance = 9999f;

    public uDesktopDuplication.Texture.RayCastResult result 
    { 
        get; 
        private set; 
    }

    public bool isPrimaryPointer
    {
        get { return pointer_ != null && pointer_.id == 0; }
    }

    public enum State
    {
        Release,
        Hover,
        Touch,
    }

    public State state 
    { 
        get; 
        set; 
    }

    public Vector2 filteredDesktopCoord 
    { 
        get; 
        private set; 
    }

    void GetPointer()
    {
        if (pointer_ != null) return;

        pointer_ = uTouchInjection.Manager.GetPointer(currentId);
        currentId++;
    }

    void ReleasePointer()
    {
        if (pointer_ == null) return;

        pointer_.Release();
        pointer_ = null;
        currentId--;
    }

    void Start()
    {
        state = State.Release;
    }

    void Update()
    {
        UpdateTouch();
        UpdateState();
    }

    void UpdateTouch()
    {
        result = uDesktopDuplication.Texture.RayCastAll(transform.position, transform.forward * maxRayDistance);

        if (pointer_ == null) return;

        if (result.hit) {
            if (isFirstTouch_) {
                filteredDesktopCoord = result.desktopCoord;
                isFirstTouch_ = false;
            } else {
                filteredDesktopCoord += (result.desktopCoord - filteredDesktopCoord) * (Time.deltaTime * 60) * (1f - filter);
            }
        }

        pointer_.position = filteredDesktopCoord;
    }

    void UpdateState()
    {
        if (!result.hit) {
            StartRelease();
            return;
        }

        switch (state) {
            case State.Release:
                if (OVRInput.Get(hoverInputTrigger)) {
                    StartHover();
                }
                break;
            case State.Hover:
                Hover();
                if (OVRInput.Get(touchInputTrigger)) {
                    StartTouch();
                } else if (!OVRInput.Get(hoverInputTrigger)) {
                    StartRelease();
                }
                break;
            case State.Touch:
                Touch();
                if (!OVRInput.Get(touchInputTrigger)) {
                    StartHover();
                }
                break;
        }
    }

    void StartRelease()
    {
        ReleasePointer();
        state = State.Release;
    }

    void StartHover()
    {
        GetPointer();
        state = State.Hover;
    }

    void StartTouch()
    {
        isFirstTouch_ = true;
        state = State.Touch;
    }

    void Hover()
    {
        GetPointer();
        pointer_.Hover();
    }

    void Touch()
    {
        GetPointer();
        pointer_.Touch();
    }
}

}