using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SimpleUIControl : MonoBehaviour
{
    [SerializeField]
    ButtonInteractable buttonInteractable;

    [SerializeField]
    string[] messageStrings;

    [SerializeField]
    TMP_Text[] messageTexts;

    [SerializeField]
    GameObject keyInteractableLight;

    private void Awake()
    {
        if (buttonInteractable != null)
        {
            buttonInteractable.selectEntered.AddListener(ButtonInteractablePressed);
        }
    }

    //Method for when button is pressed
    private void ButtonInteractablePressed(SelectEnterEventArgs arg0)
    {
        //Temportaty array assignment
        SetText(messageStrings[1]);
        if (keyInteractableLight != null)
        {
            keyInteractableLight.SetActive(true);
        }
    }

    public void SetText(string message)
    {
        //Update all text elements that should change when button is pressed.
        for (int i = 0; i < messageTexts.Length; i++)
        {
            messageTexts[i].text = message;
        }
    }

}
