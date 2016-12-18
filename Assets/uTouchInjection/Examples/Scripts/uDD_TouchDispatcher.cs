using UnityEngine;

public class uDD_TouchDispatcher : MonoBehaviour
{
    private static int currentId = 0;

    uTouchInjection.Pointer pointer_;

    [SerializeField] OVRInput.RawButton touchInputTrigger = OVRInput.RawButton.LIndexTrigger;
    [SerializeField] OVRInput.RawTouch hoverInputTrigger = OVRInput.RawTouch.LIndexTrigger;

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

    void Start()
    {
        state = State.Release;
    }

    void Update()
    {
        UpdateState();
        UpdateTouch();
    }

    void UpdateState()
    {
        switch (state) {
            case State.Release: {
                if (OVRInput.Get(hoverInputTrigger)) {
                    GetPointer();
                    state = State.Hover;
                }
                break;
            }
            case State.Hover: {
                pointer_.Hover();
                if (OVRInput.Get(touchInputTrigger)) {
                    state = State.Touch;
                } else if (!OVRInput.Get(hoverInputTrigger)) {
                    ReleasePointer();
                    state = State.Release;
                }
                break;
            }
            case State.Touch: {
                pointer_.Touch();
                if (!OVRInput.Get(touchInputTrigger)) {
                    state = State.Hover;
                }
                break;
            }
        }
    }

    void UpdateTouch()
    {
        result = uDesktopDuplication.Texture.RayCastAll(transform.position, transform.forward * 9999f);

        if (result.hit && pointer_ != null) {
            pointer_.position = result.desktopCoords;
        }
    }
}