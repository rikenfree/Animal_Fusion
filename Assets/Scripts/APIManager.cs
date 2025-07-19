using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class APIManager : MonoBehaviour
{
    public static APIManager Instance;

    public string firstChoice;
    public string secondChoice;

    public bool IsFirstClick = false;
    public bool IsSecondClick = false;

    public GameObject firstSection;
    public GameObject secondSection;
    public GameObject nextChoiceButton;

    public GameData gameData;
    public GameObject itemPrefabs;
    public Transform mainCategoryParent;
    public Transform CategoryParent;

    public GameObject VideoPlayer;
    public GameObject PhotoPlayer;

    public Image firstSelectionG1, firstSelectionG2;
    public Image secondSelectionG1, secondSelectionG2;

    [Space(10)]
    [Header("Manager")]
    public GameObject currentFirstSelection;
    public GameObject currentSecondSelection;
    public string currentVideoURL;
    public Sprite currentPhoto;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        ResetAnimal();
        CreateCategory();
    }

    public void OnClickNextChoice()
    {
        IsFirstClick = true;
        IsSecondClick = false;
        UIManager.Instance.OnPickUpComplete();

        CurrentItem();
    }

    public void ResetAnimal()
    {
        firstChoice = "";
        secondChoice = "";
        IsFirstClick = true;
        firstSection.SetActive(true);
        secondSection.SetActive(false);
        nextChoiceButton.SetActive(false);
    }

    public void CreateCategory()
    {
        for (int i = 0; i < gameData.mainCategories.Count; i++)
        {
            GameObject mainItem = Instantiate(itemPrefabs, mainCategoryParent);
            mainItem.transform.GetChild(0).GetComponent<Image>().sprite = gameData.mainCategories[i].mainAnimal;
            mainItem.name = gameData.mainCategories[i].mainCategory;
            mainItem.GetComponent<Item>().animalName = gameData.mainCategories[i].mainCategory;
            mainItem.GetComponent<Item>().animalSprite = gameData.mainCategories[i].mainAnimal;

            for (int j = 0; j < gameData.mainCategories[i].categories.Count; j++)
            {
                GameObject item = Instantiate(itemPrefabs, CategoryParent);
                item.transform.GetChild(0).GetComponent<Image>().sprite = gameData.mainCategories[i].categories[j].animal;
                item.name = gameData.mainCategories[i].categories[j].category;
                item.GetComponent<Item>().animalName = gameData.mainCategories[i].categories[j].category;
                item.GetComponent<Item>().animalSprite = gameData.mainCategories[i].categories[j].animal;
            }
        }
    }

    public void CurrentItem()
    {
        string A1 = currentFirstSelection.GetComponent<Item>().animalName;
        string A2 = currentSecondSelection.GetComponent<Item>().animalName;

        for (int i = 0; i < gameData.mainCategories.Count; i++)
        {
            if (A1 == gameData.mainCategories[i].mainCategory)
            {
                for (int j = 0; j < gameData.mainCategories[i].categories.Count; j++)
                {
                    if (A2 == gameData.mainCategories[i].categories[j].category)
                    {
                        currentVideoURL = gameData.mainCategories[i].categories[j].videoLink;
                        currentPhoto = gameData.mainCategories[i].categories[j].photoSprite;
                    }
                }
            }
        }
    }
}
