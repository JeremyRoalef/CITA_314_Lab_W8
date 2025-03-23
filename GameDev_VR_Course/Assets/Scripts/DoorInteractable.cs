using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : SimpleHingeInteractable
{
    [SerializeField]
    Transform doorTransform;

    [SerializeField]
    CombinationLock comboLock;

    // Start is called before the first frame update
    void Start()
    {
        if (comboLock != null)
        {
            comboLock.UnlockAction += OnUnlocked;
            comboLock.LockAction += OnLocked;
        }
    }

    private void OnLocked()
    {
        LockHinge();
    }

    private void OnUnlocked()
    {
        UnlockHinge();
    }

    //Inheriting from SimpleHingeInteractable
    protected override void Update()
    {
        base.Update();

        if (doorTransform != null)
        {
            doorTransform.localEulerAngles = new Vector3(
                doorTransform.localEulerAngles.x,
                transform.localEulerAngles.y,
                doorTransform.localEulerAngles.z
                );
        }
    }

}
