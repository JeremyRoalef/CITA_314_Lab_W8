using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Test : XRGrabInteractable
{
    [SerializeField]
    XRSocketInteractor socketInteractor;

    [SerializeField]
    bool isLocked = true;

    private void Awake()
    {
        if (socketInteractor != null)
        {
            //socketInteractor.selectEntered.AddListener(OnDrawerUnlocked);
            //socketInteractor.selectExited.AddListener(OnDrawerLocked);
        }
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


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
