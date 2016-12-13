#include <Windows.h>
#include "Pointer.h"
#include "Debug.h"


Pointer::Pointer(UINT32 id)
{
    InitializeContact(id);
    InitializeState();
}


Pointer::~Pointer()
{
}


void Pointer::InitializeContact(UINT32 id)
{
    memset(&contact_, 0, sizeof(POINTER_TOUCH_INFO));

    contact_.pointerInfo.pointerType = PT_TOUCH;
    contact_.pointerInfo.pointerId = id;
    contact_.touchFlags = TOUCH_FLAG_NONE;
    contact_.touchMask = TOUCH_MASK_CONTACTAREA | TOUCH_MASK_ORIENTATION | TOUCH_MASK_PRESSURE;

    SetAreaSize(2);
    SetPosition(100, 100);
    SetOrientation(90);
    SetPressure(32000);
}


void Pointer::InitializeState()
{
    state_.Init(State::Release, {
    { 
        State::Release, 
        {
            [this] 
            { 
                if (state_.GetPreviousState() != State::Hover)
                {
                    SetLastPosition();
                    SetPointerFlags(PointerFlags::Cancel);
                }
                else
                {
                    SetPointerFlags(PointerFlags::None);

                 }
            },
            [this] 
            { 
                /* do nothing */ 
            },
            [this] 
            { 
                /* do nothing */ 
            },
        }
    },
    { 
        State::Hover, 
        {
            [this] 
            {
                SetPointerFlags(PointerFlags::HoverStart);
            },
            [this] 
            {
                SetPointerFlags(PointerFlags::HoverMove);
            },
            [this] 
            {
                if (state_.GetNextState() == State::Release)
                {
                    SetLastPosition();
                    SetPointerFlags(PointerFlags::HoverEnd);
                }
            },
        }
    },
    { 
        State::Touch, 
        {
            [this] 
            {
                SetPointerFlags(PointerFlags::TouchStart);
            },
            [this] 
            {
                SetPointerFlags(PointerFlags::TouchMove);
            },
            [this] 
            {
                SetLastPosition();
                SetPointerFlags(PointerFlags::TouchEnd);
            },
        }
    },
    });
}


void Pointer::Update()
{
    shouldBeUpdated_ = false;

    state_.Update();

    if (contact_.pointerInfo.pointerFlags | POINTER_FLAG_UPDATE)
    {
        lastPosition_ = contact_.pointerInfo.ptPixelLocation;
    }
}


void Pointer::SetPosition(int x, int y)
{
    contact_.pointerInfo.ptPixelLocation.x = x;
    contact_.pointerInfo.ptPixelLocation.y = y;

    contact_.rcContact.top    = y - areaSize_;
    contact_.rcContact.bottom = y + areaSize_;
    contact_.rcContact.left   = x - areaSize_;
    contact_.rcContact.right  = x + areaSize_;
}


void Pointer::SetLastPosition()
{
    SetPosition(lastPosition_.x, lastPosition_.y);
}


void Pointer::SetAreaSize(int size)
{
    areaSize_ = size;
}


void Pointer::SetPressure(int pressure)
{
    contact_.pressure = pressure;
}


void Pointer::SetOrientation(int degree)
{
    contact_.orientation = degree;
}


void Pointer::SetState(State state)
{
    state_.SetState(state);
}


void Pointer::SetPointerFlags(PointerFlags flags)
{
    contact_.pointerInfo.pointerFlags = static_cast<POINTER_FLAGS>(flags);

    if (flags != PointerFlags::None)
    {
        shouldBeUpdated_ = true;
    }
}


void Pointer::Touch()
{
    SetState(State::Touch);
}


void Pointer::Hover()
{
    SetState(State::Hover);
}


void Pointer::Release()
{
    SetState(State::Release);
}


bool Pointer::ShouldBeUpdated() const
{
    return shouldBeUpdated_;
}


bool Pointer::IsTouching() const
{
    return state_.GetCurrentState() == State::Touch;
}


bool Pointer::IsHovering() const
{
    return state_.GetCurrentState() == State::Hover;
}


bool Pointer::IsReleasing() const
{
    return state_.GetCurrentState() == State::Release;
}


const POINTER_TOUCH_INFO& Pointer::GetData() const
{
    return contact_;
}
