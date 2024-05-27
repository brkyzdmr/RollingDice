using Brkyzdmr.Services.UIService;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCounterHandler : CounterHandler
{
    private readonly Button _increaseButton;
    private readonly Button _decreaseButton;

    public ButtonCounterHandler(Button increaseButton, Button decreaseButton, int min, int max) : base(min, max)
    {
        _increaseButton = increaseButton;
        _decreaseButton = decreaseButton;
        
        OnValueZero += HandleOnValueZero;
        OnValueValid += HandleOnValueValid;
        OnValueMax += HandleOnValueMax;
        OnValueNotValid += HandleOnValueNotValid;
    }

    private void HandleOnValueZero()
    {
        Debug.Log("HandleOnValueZero");
        _decreaseButton.interactable = false;
    }

    private void HandleOnValueValid()
    {
        Debug.Log("HandleOnValueValid");
        _increaseButton.interactable = true;
        _decreaseButton.interactable = true;
    }

    private void HandleOnValueMax()
    {
        Debug.Log("HandleOnValueMax");
        _increaseButton.interactable = false;
    }

    private void HandleOnValueNotValid()
    {
        Debug.Log("HandleOnValueNotValid");
        _increaseButton.interactable = false;
        _decreaseButton.interactable = false;
    }
}