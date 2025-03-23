using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SimpleHingeInteractable : XRSimpleInteractable
{
    Transform grabHand;

    [SerializeField]
    bool isLocked = true;

    const string Default_Layer = "Default";
    const string Grab_Layer = "Grab";

    public void UnlockHinge()
    {
        isLocked = false;
    }

    public void LockHinge()
    {
        isLocked = true;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!isLocked)
        {
            base.OnSelectEntered(args);
            grabHand = args.interactorObject.transform;
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        grabHand = null;
        ChangeLayerMask(Grab_Layer);
    }

    //Virtual b/c of DoorInteractable
    protected virtual void Update()
    {
        if (grabHand != null)
        {
            transform.LookAt(grabHand, transform.forward);
        }
    }

    public void ReleaseHinge()
    {
        ChangeLayerMask(Default_Layer);
    }

    void ChangeLayerMask(string mask)
    {
        interactionLayers = InteractionLayerMask.GetMask(mask);
    }
}
