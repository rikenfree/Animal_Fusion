using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public string animalName;
    public Sprite animalSprite;

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

            APIManager.Instance.firstSection.SetActive(false);
            APIManager.Instance.secondSection.SetActive(true);

            APIManager.Instance.currentFirstSelection = this.gameObject;
            APIManager.Instance.firstSelectionG1.sprite = animalSprite;
            APIManager.Instance.firstSelectionG2.sprite = animalSprite;
            return;
        }

        if (APIManager.Instance.IsSecondClick)
        {
            APIManager.Instance.secondChoice = animalName;
            APIManager.Instance.nextChoiceButton.SetActive(true);

            APIManager.Instance.currentSecondSelection = this.gameObject;
            APIManager.Instance.secondSelectionG1.sprite = animalSprite;
            APIManager.Instance.secondSelectionG2.sprite = animalSprite;

            return;
        }
    }

}
