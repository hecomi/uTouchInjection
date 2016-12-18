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

    struct StateInfo
    {
        StateFunc onStart;
        StateFunc onUpdate;
        StateFunc onEnd;
        std::map<State, State> proxies;

        StateInfo(StateFunc&& start, StateFunc&& update, StateFunc&& end)
            : onStart(start)
            , onUpdate(update)
            , onEnd(end) 
        {
        }

        StateInfo(StateFunc&& start, StateFunc&& update, StateFunc&& end, std::initializer_list<std::pair<State, State>>&& map)
            : onStart(start)
            , onUpdate(update)
            , onEnd(end) 
        {
            for (const auto& proxy : map)
            {
                proxies.insert(proxy);
            }
        }
    };

    StateMachine()
    {
    }

    void Init(State initialState, std::initializer_list<std::pair<State, StateInfo>> states)
    {
        initialState_ = currentState_ = preState_ = nextState_ = initialState;

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

        auto& state = current->second;

        if (!isStarted)
        {
            state.onStart();
            isStarted = true;
        }
        else if (!isUpdated)
        {
            state.onUpdate();
            isUpdated = true;
        }
        else if (currentState_ != nextState_)
        {
            state.onEnd();

            auto it = state.proxies.find(nextState_);
            if (it != state.proxies.end())
            {
                const auto proxyState = it->second;
                preState_ = currentState_;
                currentState_ = proxyState;
            }
            else
            {
                preState_ = currentState_;
                currentState_ = nextState_;
            }

            isStarted = false;
            isUpdated = false;
        }
        else
        {
            state.onUpdate();
        }
    }

    void SetState(State state)
    {
        nextState_ = state;
    }

    void Reset()
    {
        currentState_ = preState_ = nextState_ = initialState_;
        isStarted = false;
        isUpdated = false;
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
    State initialState_ = static_cast<State>(-1);
    State currentState_ = static_cast<State>(-1);
    State preState_ = static_cast<State>(-1);
    State nextState_ = static_cast<State>(-1);
    std::map<State, StateInfo> states_;
    bool isStarted = false;
    bool isUpdated = false;
};