using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public string animalName;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            OnClick();
        });
    }

    public void OnClick()
    {
        if (APIManager.Instance.IsFirstClick)
        {
            APIManager.Instance.firstChoice = animalName;
            APIManager.Instance.IsFirstClick = false;
            APIManager.Instance.IsSecondClick = true;
            return;
        }

        if (APIManager.Instance.IsSecondClick)
        {
            APIManager.Instance.secondChoice = animalName;
            APIManager.Instance.nextChoiceButton.SetActive(true);
            return;
        }
    }
}
