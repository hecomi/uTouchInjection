﻿using UnityEngine;
using System.Collections.Generic;

namespace uTouchInjection
{

public class Manager : MonoBehaviour 
{
    public int touchNum = 2;

    private static Manager instance_;
    public static Manager instance 
    {
        get { return CreateInstance(); }
    }

    public static Manager CreateInstance()
    {
        if (instance_ != null) return instance_;

        var manager = FindObjectOfType<Manager>();
        if (manager) {
            instance_ = manager;
            return manager;
        }

        var go = new GameObject("uTouchInjection");
        instance_ = go.AddComponent<Manager>();
        return instance_;
    }

    List<Pointer> pointers_ = new List<Pointer>();
    public static List<Pointer> pointers
    {
        get { return instance.pointers_; }
    }

    Pointer GetPointer(int id)
    {
        if (id < 0 || id >= Manager.pointers.Count) {
            Debug.LogErrorFormat("There is no pointer whose id is {0}.", id);
            return null;
        }
        return pointers_[id];
    }

    void CheckInstance()
    {
        if (instance_ == this) {
            return;
        }

        if (instance_ != null && instance_ != this) {
            Destroy(gameObject);
            return;
        }

        instance_ = this;
    }

    void Awake()
    {
        CheckInstance();

        Lib.Initialize(touchNum);

        for (int i = 0; i < touchNum; ++i) {
            pointers_.Add(new Pointer(i));
        }
    }

    void Update()
    {
        Lib.Update();
    }

    void OnApplicationQuit()
    {
        Lib.Finalize();
    }

}

}