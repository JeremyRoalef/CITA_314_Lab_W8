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

    [SerializeField]
    Vector3 rotationLimits;

    Transform startRotation;
    float startAngleX;

    protected override void Start()
    {
        base.Start();

        startRotation = transform;
        startAngleX = startRotation.localEulerAngles.x;
        if (startAngleX >= 180)
        {
            startAngleX -= 360;
        }


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

        if (isSelected)
        {
            CheckLimits();
        }
    }

    void CheckLimits()
    {
        float localAngleX = transform.localEulerAngles.x;
        if (localAngleX >= 180)
        {
            localAngleX -= 360;
        }

        if (localAngleX >= startAngleX + rotationLimits.x || localAngleX <= startAngleX - rotationLimits.x)
        {
            ReleaseHinge();
            transform.localEulerAngles = new Vector3(
                startAngleX,
                transform.localEulerAngles.y,
                transform.localEulerAngles.z
                );
        }
    }
}
