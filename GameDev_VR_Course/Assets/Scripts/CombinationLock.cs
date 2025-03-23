using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class CombinationLock : MonoBehaviour
{
    public UnityAction UnlockAction;
    void OnUnlocked() => UnlockAction?.Invoke();

    public UnityAction LockAction;
    void OnLocked() => LockAction?.Invoke();

    [SerializeField]
    string numberCombination = "0412";

    [SerializeField]
    ButtonInteractable[] comboButtons = new ButtonInteractable[4];

    [SerializeField]
    TMP_Text textInput;

    [SerializeField]
    bool isLocked = true;

    [SerializeField]
    Color unlockedButtonColor = Color.green;

    [SerializeField]
    Color incorrectComboButtonColor = Color.red;

    const string defaultInputText = "0000";
    string userInput = "";

    private void Start()
    {
        for (int i = 0; i < comboButtons.Length; i++)
        {
            comboButtons[i].selectEntered.AddListener(OnComboButtonPressed);
        }

        textInput.text = defaultInputText;
    }

    private void OnComboButtonPressed(SelectEnterEventArgs arg0)
    {
        //Use SelectEnterEventArgs
        for (int i = 0; i < comboButtons.Length; i++)
        {
            //Find the object's name in the array to retrieve the index/number for the object
            if (arg0.interactableObject.transform.name == comboButtons[i].transform.name)
            {
                userInput += i.ToString();
                textInput.text = userInput;
                if (userInput.Length == numberCombination.Length)
                {
                    CheckCombination();
                }

            }

            //Reset the color of other objects to normal color when button is pressed
            comboButtons[i].SetColorToNormal();
        }
    }

    private void CheckCombination()
    {
        //Check if the combination is correct. If it is, prevent the buttons from being interacted with,
        //isLocked is false, & set the button colors to a green color for user feedback

        //Debug.Log($"Combination Code: {numberCombination}");
        //Debug.Log($"User Guessed: {userInput}");
        //Debug.Log(numberCombination.CompareTo(userInput));

        if (numberCombination.CompareTo(userInput) == 0)
        {
            //Debug.Log("You found the combination");
            isLocked = false;
            OnUnlocked();
            for (int i = 0;i < comboButtons.Length; i++)
            {
                comboButtons[i].GetComponent<ButtonInteractable>().interactionLayers = InteractionLayerMask.GetMask("Nothing");
                Invoke("SetButtonsToUnlockedColor", .1f);
            }
        }
        //Combination has not been found. Reset the string & flash the buttons red for user feedback
        else
        {
            OnLocked();
            for(int i = 0; i < comboButtons.Length; i++)
            {
                comboButtons[i].SetButtonColor(incorrectComboButtonColor);
            }

            Invoke("ResetCombination", 0.5f);
        }
    }

    void ResetCombination()
    {
        for (int i = 0; i < comboButtons.Length; i++)
        {
            comboButtons[i].SetColorToNormal();
        }
        textInput.text = defaultInputText;
        userInput = string.Empty;
    }

    void SetButtonsToUnlockedColor()
    {
        for (int i = 0; i < comboButtons.Length; i++)
        {
            comboButtons[i].SetButtonColor(unlockedButtonColor);
        }
    }
}
