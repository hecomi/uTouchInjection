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

private:
    DWORD InjectAllTouches();

    std::vector<std::shared_ptr<Pointer>> pointers_;
};