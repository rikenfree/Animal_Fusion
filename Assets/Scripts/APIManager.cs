using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public void OnClickNextChoice()
    {
        IsFirstClick = true;
        IsSecondClick = false;
        UIManager.Instance.OnPickUpComplete();
       
        MoveAnimation();
    }

    public void ResetAnimal()
    {
        firstChoice = "";
        secondChoice = "";
        IsFirstClick = true;
        firstSection.SetActive(true);
        nextChoiceButton.SetActive(false);
    }

    public void MoveAnimation()
    {

    }
}
