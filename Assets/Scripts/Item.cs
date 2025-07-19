using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [Header("Animal Info")]
    public string animalName;
    public Sprite animalSprite;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();

        if (button != null)
            button.onClick.AddListener(OnClick);
        else
            Debug.LogWarning("Button component missing on Item GameObject.");
    }

    public void OnClick()
    {
        if (APIManager.Instance == null)
        {
            Debug.LogError("APIManager instance not found.");
            return;
        }

        if (APIManager.Instance.IsFirstClick)
        {
            SelectAsFirstChoice();
        }
        else if (APIManager.Instance.IsSecondClick)
        {
            SelectAsSecondChoice();
        }
    }

    private void SelectAsFirstChoice()
    {
        var api = APIManager.Instance;

        api.firstChoice = animalName;
        api.IsFirstClick = false;
        api.IsSecondClick = true;

        api.firstSection.SetActive(false);
        api.secondSection.SetActive(true);

        api.currentFirstSelection = gameObject;
        api.firstSelectionG1.sprite = animalSprite;
        api.firstSelectionG2.sprite = animalSprite;

        api.firstSelectionG1.gameObject.SetActive(true);
    }

    private void SelectAsSecondChoice()
    {
        var api = APIManager.Instance;

        api.secondChoice = animalName;
        api.nextChoiceButton.SetActive(true);

        api.currentSecondSelection = gameObject;
        api.secondSelectionG1.sprite = animalSprite;
        api.secondSelectionG2.sprite = animalSprite;

        api.secondSelectionG1.gameObject.SetActive(true);
    }

}
