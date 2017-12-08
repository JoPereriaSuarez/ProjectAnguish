using UnityEngine;
using System.Collections;
using System;

public abstract class ControllerBase : MonoBehaviour
{
    private bool _pauseController;
    public bool pauseController
    {
        get { return _pauseController; }
        set
        {
            if(value != _pauseController)
            {
                if (value == true)
                {
                    OnPaused();
                }
                else
                {
                    OnUnPaused();
                }
            }
            _pauseController = value;
        }
    }

    protected virtual void Update()
    {
        if (pauseController)
        {
            return;
        }
            
        UpdateController();
    }

    //para el update
    protected abstract void UpdateController();

    protected virtual void OnPaused()
    {
        return;
    }
    protected virtual void OnUnPaused()
    {
        return;
    }
}
