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
        if (String.IsNullOrEmpty(APIManager.Instance.firstChoice))
        {
            APIManager.Instance.firstChoice = animalName;
            return;
        }

        if (String.IsNullOrEmpty(APIManager.Instance.secondChoice))
        {
            APIManager.Instance.secondChoice = animalName;
            return;
        }
    }
}
