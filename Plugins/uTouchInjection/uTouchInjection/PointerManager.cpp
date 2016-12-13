#include "PointerManager.h"
#include "Debug.h"

#define STATUS_ACCESS_DENIED ((NTSTATUS)0xC0000022L)



PointerManager::PointerManager()
{
}


PointerManager::~PointerManager()
{
}


void PointerManager::Initialize(int touchNum)
{
    pointers_.clear();
    pointers_.reserve(touchNum);
    for (int i = 0; i < touchNum; ++i)
    {
        pointers_.push_back(std::make_shared<Pointer>(i));
    }

    InitializeTouchInjection(touchNum, TOUCH_FEEDBACK_DEFAULT);
}


DWORD PointerManager::Update()
{
    for (auto& pointer : pointers_)
    {
        pointer->Update();
    }

    return InjectAllTouches();
}


DWORD PointerManager::InjectAllTouches()
{
    std::vector<POINTER_TOUCH_INFO> contacts;

    for (const auto& pointer : pointers_)
    {
        if (pointer->ShouldBeUpdated())
        {
            contacts.push_back(pointer->GetData());
        }
    }

    if (contacts.empty()) return NO_ERROR;

    if (!::InjectTouchInput(static_cast<UINT32>(contacts.size()), &contacts[0]))
    {
        const auto error = GetLastError();
        switch (error)
        {
            case ERROR_INVALID_PARAMETER:
            {
                Debug::Error("InjectTouchInput() => ERROR_INVALID_PARAMETER");
                break;
            }
            case STATUS_ACCESS_DENIED:
            {
                Debug::Error("InjectTouchInput() => STATUS_ACCESS_DENIED");
                break;
            }
            case ERROR_TIMEOUT:
            {
                Debug::Error("InjectTouchInput() => ERROR_TIMEOUT");
                break;
            }
            case ERROR_NOT_READY:
            {
                Debug::Error("InjectTouchInput() => ERROR_NOT_READY");
                break;
            }
        }
        return error;
    }

    return NO_ERROR;
}


std::shared_ptr<Pointer> PointerManager::GetPointer(int id)
{
    if (id < 0 || id >= static_cast<int>(pointers_.size())) 
    {
        Debug::Error("PointerManager::GetPointer() => ", id, " is out of pointers array.");
        return nullptr;
    }
    return pointers_.at(id);
}