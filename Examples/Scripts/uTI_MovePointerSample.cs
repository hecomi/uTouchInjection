﻿using UnityEngine;

namespace uTouchInjection
{

public class uTI_MovePointerSample : MonoBehaviour
{
    Pointer pointer0;
    Pointer pointer1;

    [SerializeField] int areaSize = 5;
    [SerializeField] float t0 = 0f;
    [SerializeField] float t1 = 2f;
    [SerializeField] float t2 = 4f;
    [SerializeField] float t3 = 6f;
    [SerializeField] float t4 = 8f;
    [SerializeField] Vector2 start0 = new Vector2(400, 300);
    [SerializeField] Vector2 end0 = new Vector2(400, 800);
    [SerializeField] Vector2 start1 = new Vector2(500, 300);
    [SerializeField] Vector2 end1 = new Vector2(500, 800);
    float t_ = 0f;

    void Start()
    {
        pointer0 = Manager.pointers[0];
        pointer0.areaSize = areaSize;
        pointer1 = Manager.pointers[1];
    }

    void Update()
    {
        if (t_ < t0) 
        {
            pointer0.Release(start0);
            pointer1.Release(start1);
        }
        else if (t_ < t1) 
        {
            pointer0.Hover(start0);
            pointer1.Hover(start1);
        }
        else if (t_ < t2) 
        {
            var a = (t_ - t1) / (t3 - t2);
            pointer0.Touch(start0 + (end0 - start0) * a);
            pointer1.Touch(start1 + (end1 - start1) * a);
        } 
        else if (t_ < t3) 
        {
            pointer0.Hover(end0);
            pointer1.Hover(end1);
        } 
        else if (t_ < t4) 
        {
            pointer0.Release(end0);
            pointer1.Release(end1);
        }
        else 
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExecuteMenuItem("Edit/Play");
#else
            Application.Quit();
#endif
        }

        t_ += Time.deltaTime;
    }
}

}