#include <windows.h>
#include "StateMachine.h"

#pragma once

enum class PointerFlags : POINTER_FLAGS
{
    None       = POINTER_FLAG_NONE,
    HoverStart = POINTER_FLAG_UPDATE  | POINTER_FLAG_INRANGE,
    HoverMove  = POINTER_FLAG_UPDATE  | POINTER_FLAG_INRANGE,
    HoverEnd   = POINTER_FLAG_UPDATE,
    TouchStart = POINTER_FLAG_DOWN    | POINTER_FLAG_INRANGE   | POINTER_FLAG_INCONTACT,
    TouchMove  = POINTER_FLAG_UPDATE  | POINTER_FLAG_INRANGE   | POINTER_FLAG_INCONTACT,
    TouchEnd   = POINTER_FLAG_UP      | POINTER_FLAG_INRANGE,
    Cancel     = POINTER_FLAG_UPDATE  | POINTER_FLAG_CANCELED,
};

class Pointer
{
public:
    enum class State
    {
        Release,
        Hover,
        Touch,
    };

    explicit Pointer(UINT32 id);
    ~Pointer();
    void Update();

    void InitializeContact(UINT32 id);
    void InitializeState();

    void SetAreaSize(int size);
    void SetPressure(int pressure);
    void SetOrientation(int degree);
    void SetPosition(int x, int y);

    void Touch();
    void Hover();
    void Release();
    void Invalidate();
    
    const POINTER_TOUCH_INFO& GetData() const;

    bool ShouldBeUpdated() const;
    bool IsTouching() const;
    bool IsHovering() const;
    bool IsReleasing() const;

    void PrintDebugInfo() const;

private:
    void SetState(State state);
    void SetPointerFlags(PointerFlags flags);
    void RequireUpdate();
    void SetLastPosition();

    POINTER_TOUCH_INFO contact_ {};
    int areaSize_ = 0;
    StateMachine<State> state_;
    PointerFlags pointerFlags_ = PointerFlags::None;
    bool shouldBeUpdated_ = false;
    POINT lastPosition_;
};

