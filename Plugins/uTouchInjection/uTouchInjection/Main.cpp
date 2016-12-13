#include <memory>
#include "IUnityInterface.h"
#include "IUnityGraphics.h"
#include "PointerManager.h"
#include "Debug.h"



namespace
{
    std::unique_ptr<PointerManager> g_manager;
}


extern "C"
{
    UNITY_INTERFACE_EXPORT void UNITY_INTERFACE_API Initialize(int touchNum)
    {
        if (g_manager) return;

        Debug::Initialize();
        g_manager = std::make_unique<PointerManager>();
        g_manager->Initialize(touchNum);
    }

    UNITY_INTERFACE_EXPORT void UNITY_INTERFACE_API Finalize()
    {
        if (!g_manager) return;

        g_manager.reset();
        Debug::Finalize();
    }

    UNITY_INTERFACE_EXPORT void UNITY_INTERFACE_API SetPosition(int id, int x, int y)
    {
        if (!g_manager) return;
        if (auto pointer = g_manager->GetPointer(id))
        {
            pointer->SetPosition(x, y);
        }
    }

    UNITY_INTERFACE_EXPORT void UNITY_INTERFACE_API SetPressure(int id, int pressure)
    {
        if (!g_manager) return;
        if (auto pointer = g_manager->GetPointer(id))
        {
            pointer->SetPressure(pressure);
        }
    }

    UNITY_INTERFACE_EXPORT void UNITY_INTERFACE_API SetAreaSize(int id, int areaSize)
    {
        if (!g_manager) return;
        if (auto pointer = g_manager->GetPointer(id))
        {
            pointer->SetAreaSize(areaSize);
        }
    }

    UNITY_INTERFACE_EXPORT void UNITY_INTERFACE_API SetOrientation(int id, int degree)
    {
        if (!g_manager) return;
        if (auto pointer = g_manager->GetPointer(id))
        {
            pointer->SetOrientation(degree);
        }
    }

    UNITY_INTERFACE_EXPORT void UNITY_INTERFACE_API Touch(int id)
    {
        if (!g_manager) return;
        if (auto pointer = g_manager->GetPointer(id))
        {
            pointer->Touch();
        }
    }

    UNITY_INTERFACE_EXPORT void UNITY_INTERFACE_API Hover(int id)
    {
        if (!g_manager) return;
        if (auto pointer = g_manager->GetPointer(id))
        {
            pointer->Hover();
        }
    }

    UNITY_INTERFACE_EXPORT void UNITY_INTERFACE_API Release(int id)
    {
        if (!g_manager) return;
        if (auto pointer = g_manager->GetPointer(id))
        {
            pointer->Release();
        }
    }

    UNITY_INTERFACE_EXPORT void UNITY_INTERFACE_API Update()
    {
        if (!g_manager) return;
        g_manager->Update();
    }
}