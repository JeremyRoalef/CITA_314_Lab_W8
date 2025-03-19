using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DrawerInteractable : XRGrabInteractable
{
    [SerializeField]
    XRSocketInteractor socketInteractor;

    [SerializeField]
    bool isLocked = true;

    [SerializeField]
    Transform drawerTransform;

    Transform parentTransform;
    const string defaultLayer = "Default";
    const string grabLayer = "Grab";

    bool isGrabbed = false;
    Vector3 limitPositions;

    [SerializeField]
    Vector3 limitDistances = new Vector3(.01f,.01f,0);

    [SerializeField]
    float maxZPos = 0.9f;

    //XRSocketInteractable HAS AN AWAKE METHOD!!! DO NOT OVERRIDE ITS AWAKE METHOD WITHOUT CALLING IT
    protected override void Awake()
    {
        //Call XRGrabInteractable awake method
        base.Awake();

        //This script's awake code
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.AddListener(OnDrawerUnlocked);
            socketInteractor.selectExited.AddListener(OnDrawerLocked);
        }

        parentTransform = transform.parent.transform;
        limitPositions = drawerTransform.localPosition;
    }

    private void OnDrawerLocked(SelectExitEventArgs arg0)
    {
        isLocked = true;
        Debug.Log(isLocked);
    }

    private void OnDrawerUnlocked(SelectEnterEventArgs arg0)
    {
        isLocked = false;
        Debug.Log(isLocked);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (isLocked)
        {
            //Change interaction layer mask
            ChangeLayerMask(defaultLayer);
        }
        else
        {
            //Prevent the child object from disconneceting from the parent when grabbed
            //This will allow us to track its local z position
            transform.SetParent(parentTransform);
            isGrabbed = true;
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        ChangeLayerMask(grabLayer);
        isGrabbed = false;
        transform.localPosition = drawerTransform.localPosition;
    }

    void Update()
    {
        if (isGrabbed && drawerTransform != null)
        {
            drawerTransform.localPosition = new Vector3(
                drawerTransform.localPosition.x,
                drawerTransform.localPosition.y,
                transform.localPosition.z);

            CheckPositionLimits();
        }
    }

    void CheckPositionLimits()
    {
        if (transform.localPosition.x >= limitPositions.x + limitDistances.x ||
            transform.localPosition.x <= limitPositions.x - limitDistances.x)
        {
            ChangeLayerMask(defaultLayer);
        }
        else if (transform.localPosition.y >= limitPositions.y + limitDistances.y ||
            transform.localPosition.y <=  limitPositions.y - limitDistances.y)
        {
            ChangeLayerMask(defaultLayer);
        }
        else if (drawerTransform.localPosition.z <= limitPositions.z - limitDistances.z)
        {
            isGrabbed = false;
            ChangeLayerMask(defaultLayer);
            drawerTransform.localPosition = limitPositions;
        }
        else if (drawerTransform.localPosition.z >= maxZPos + limitDistances.z)
        {
            isGrabbed = false;
            drawerTransform.localPosition = new Vector3(
                drawerTransform.localPosition.x,
                drawerTransform.localPosition.y,
                maxZPos);
            ChangeLayerMask(defaultLayer);
        }
    }

    private void ChangeLayerMask(string layerMask)
    {
        interactionLayers = InteractionLayerMask.GetMask(layerMask);
    }
}
