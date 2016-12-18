#pragma once

#include <vector>
#include <memory>
#include "Pointer.h"


class PointerManager
{
public:
    PointerManager();
    ~PointerManager();
    void Initialize(int touchNum);
    DWORD Update();
    std::shared_ptr<Pointer> GetPointer(int id);
    void EnableLogOutput() { doesOutputLogs = true; }
    void DisableLogOutput() { doesOutputLogs = false; }

private:
    DWORD InjectAll();
    void InvalidateAll();

    std::vector<std::shared_ptr<Pointer>> pointers_;
    bool doesOutputLogs = false;
};