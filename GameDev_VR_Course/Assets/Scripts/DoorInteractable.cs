using UnityEngine;
using UnityEngine.Events;

public class DoorInteractable : SimpleHingeInteractable
{
    public UnityEvent OnOpen;

    [SerializeField]
    Transform doorTransform;

    [SerializeField]
    CombinationLock comboLock;

    [SerializeField]
    Vector3 rotationLimits;

    [SerializeField]
    Collider closedCollider;

    [SerializeField]
    Collider openCollider;

    Vector3 startRotation;

    [SerializeField]
    Vector3 endRotation;
    float startAngleX;
    bool isClosed;
    bool isOpen;


    protected override void Start()
    {
        base.Start();

        startRotation = transform.localEulerAngles;
        startAngleX = GetAngle(startRotation.x);

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
        isClosed = false;
        isOpen = false;

        float localAngleX = GetAngle(transform.localEulerAngles.x);
        if (localAngleX >= startAngleX + rotationLimits.x || localAngleX <= startAngleX - rotationLimits.x)
        {
            ReleaseHinge();
        }
    }

    float GetAngle(float angle)
    {
        if (angle >= 180)
        {
            angle -= 360;
        }

        return angle;
    }

    protected override void ResetHinge()
    {
        OnOpen?.Invoke();

        if (isClosed)
        {
            transform.localEulerAngles = startRotation;
        }
        else if (isOpen)
        {
            transform.localEulerAngles = endRotation;
        }
        else
        {
            transform.localEulerAngles = new Vector3(
                startAngleX,
                transform.localEulerAngles.y,
                transform.localEulerAngles.z
                );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if collider is a specific collider (alternative to endless tags)
        if (other == closedCollider)
        {
            isClosed = true;
            ReleaseHinge();
        }
        else if (other == openCollider)
        {
            isOpen = true;
            ReleaseHinge();
        }
    }
}
