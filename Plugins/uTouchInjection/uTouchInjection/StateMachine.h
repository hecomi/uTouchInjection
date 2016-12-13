#pragma once

#include <functional>
#include <map>
#include "Debug.h"

template <class State>
class StateMachine
{
public:
    using StateFunc = std::function<void()>;
    using StateType = State;

    struct StateFunctions
    {
        StateFunc onStart;
        StateFunc onUpdate;
        StateFunc onEnd;
        StateFunctions(StateFunc&& update)
            : onUpdate(update) {}
        StateFunctions(StateFunc&& start, StateFunc&& update, StateFunc&& end)
            : onStart(start)
            , onUpdate(update)
            , onEnd(end) {}
    };

    StateMachine()
    {
    }

    void Init(State initialState, std::initializer_list<std::pair<State, StateFunctions>> states)
    {
        currentState_ = nextState_ = initialState;

        for (const auto& pair : states)
        {
            states_.insert(pair);
        }
    }

    void Update()
    {
        auto current = states_.find(currentState_);
        if (current == states_.end())
        {
            return;
        }

        if (currentState_ != nextState_)
        {
            current->second.onEnd();
            preState_ = currentState_;
            currentState_ = nextState_;
        }
        else if (isNewState)
        {
            current->second.onStart();
            isNewState = false;
        }
        else
        {
            current->second.onUpdate();
        }
    }

    void SetState(State state)
    {
        if (nextState_ == state) return;

        nextState_ = state;
        isNewState = true;
    }

    State GetCurrentState() const 
    {
        return currentState_;
    }

    State GetNextState() const 
    {
        return nextState_;
    }

    State GetPreviousState() const 
    {
        return preState_;
    }

private:
    State currentState_ = static_cast<State>(-1);
    State preState_ = static_cast<State>(-1);
    State nextState_ = static_cast<State>(-1);
    std::map<State, StateFunctions> states_;
    bool isNewState = false;
};