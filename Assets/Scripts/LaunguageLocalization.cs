using DG.Tweening;
using I2.Loc;
using System;

//using Main.Controller;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Launguage
{
    public string Name;
    public string Code;
}

public class LaunguageLocalization : MonoBehaviour
{
    public static LaunguageLocalization instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        InitializeLanguageButtons();
        ApplyInitialLanguageSelection();
    }

    public string currentlanguage
    {
        get
        {
            return PlayerPrefs.GetString("language", "en");
        }
        set
        {
            PlayerPrefs.SetString("language", value);
            PlayerPrefs.Save();
        }
    }

    [Space]
    [Header("<color=yellow> I2 Localize Refrence")]

    public Color selected;
    public Color unSelected;

    public Color selectedText;
    public Color unSelectedText;

    public Sprite selectedSprite;
    public Sprite unSelectedSprite;

    public Transform languagebtnparent;
    public List<Launguage> Language = new List<Launguage>();
    public int currentId;
    //public List<TMP_FontAsset> tMP_Fonts = new List<TMP_FontAsset>();

    private int oldSelectedNob = -1;

    private void InitializeLanguageButtons()
    {
        for (int i = 0; i < Language.Count; i++)
        {
            Transform t = languagebtnparent.GetChild(i);
            t.GetChild(0).GetComponent<TextMeshProUGUI>().text = Language[i].Name;

            Button button = t.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            int index = i;
            button.onClick.AddListener(() => AddEvent(index));
        }
    }

    private void ApplyInitialLanguageSelection()
    {
        string currentLang = currentlanguage;
        for (int i = 0; i < Language.Count; i++)
        {
            if (Language[i].Code == currentLang)
            {
                oldSelectedNob = i;
                UpdateLanguageSelection(i, true);
                currentId = i;
                return;
            }
        }

        oldSelectedNob = 0;
        UpdateLanguageSelection(0, true);
        currentId = 0;
    }

    public void AddEvent(int index)
    {
        Debug.Log("AddEvent => " + index);
        OnLanguageSelect(index);
    }

    public void OnLanguageSelect(int index)
    {
        if (oldSelectedNob != index)
        {
            UpdateLanguageSelection(index);
            currentId = index;
        }
    }

    private void UpdateLanguageSelection(int index, bool initialSetup = false)
    {
        if (oldSelectedNob >= 0 && oldSelectedNob < languagebtnparent.childCount)
        {
            Transform oldT = languagebtnparent.GetChild(oldSelectedNob);
            oldT.GetComponent<Image>().color = unSelected;
            oldT.GetChild(0).GetComponent<TextMeshProUGUI>().color = unSelectedText;
            oldT.GetChild(1).GetComponent<Image>().sprite = unSelectedSprite;
        }

        Transform newT = languagebtnparent.GetChild(index);
        if (!initialSetup)
        {
            newT.DOScale(new Vector3(1.05f, 1.05f, 1.05f), 0.1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
        }
        newT.GetComponent<Image>().color = selected;
        newT.GetChild(0).GetComponent<TextMeshProUGUI>().color = selectedText;
        newT.GetChild(1).GetComponent<Image>().sprite = selectedSprite;




        //currentlanguage = Language[index].Code;
        //LocalizationManager.CurrentLanguageCode = currentlanguage;
        oldSelectedNob = index;
    }

    public void OnClickCorrect()
    {
        currentlanguage = Language[currentId].Code;
        LocalizationManager.CurrentLanguageCode = currentlanguage;
        //oldSelectedNob = currentId;
    }

    public void SetToDefault()
    {
        OnLanguageSelect(0);
    }
}