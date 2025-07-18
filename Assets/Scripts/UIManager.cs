using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine.UI;

public enum Template
{
    VIDEO,
    PHOTO
}

public class UIManager : MonoBehaviour
{
    [Header("LODER")]
    public GameObject lodingFillImage;
    public float totalTime;
    public float remainingTime;
    public float speed;
    private Coroutine sliderCoroutine;
    private bool IsStartSlider = false;


    [Space(10)]
    [Header("UI SCREEN")]
    public GameObject lodingScreen;
    public GameObject languageScreenStart;
    public GameObject tutorialScreen;
    public GameObject mainScreen;
    public GameObject languageScreen;
    public GameObject settingScreen;
    public GameObject selectModeScreen;
    public GameObject selectTemplateScreen;
    public GameObject pick2ItemScreen;
    public GameObject generateScreen;
    public GameObject generate_Screen;
    public GameObject playVideoScreen;
    public GameObject collectionScreen;

    [Space(10)]
    [Header("SimpleScrollSnap")]
    [SerializeField] SimpleScrollSnap scrollSnap;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject getStartedButton;

    [Space(10)]
    [Header("TemplateArea")]
    public Image videoImage;
    public Image photoImage;
    public Sprite selectedTemplate;
    public Sprite unSelectedTemplate;
    public Template currentTemplate;

    private void Awake()
    {
        scrollSnap.OnPanelCentered.AddListener(OnPanelCentered);
    }

    private void OnDestroy()
    {
        scrollSnap.OnPanelCentered.RemoveListener(OnPanelCentered);
    }

    private void OnPanelCentered(int centeredPanel, int previousPanel)
    {
        int lastPanelIndex = scrollSnap.NumberOfPanels - 1;
        // Change the sprite according to the centered panel

        if (centeredPanel == lastPanelIndex)
        {
            nextButton.SetActive(false);
            getStartedButton.SetActive(true);
        }
        else
        {
            nextButton.SetActive(true);
            getStartedButton.SetActive(false);
        }
    }

    private void Start()
    {
        remainingTime = totalTime;
        IsStartSlider = true;
        StartSlider();
    }

    private void Update()
    {
        if (IsStartSlider)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime * speed;
            }
            else
            {
                IsStartSlider = false;
                remainingTime = 0;
                StopSlider();

                if (IsTutorial == 0)
                {
                    OnOffPanels(lodingScreen, languageScreenStart);
                }
                else
                {
                    OnOffPanels(lodingScreen, mainScreen);
                }
            }
        }
    }

    public void StartSlider()
    {
        sliderCoroutine = StartCoroutine(MoveSlider());
    }

    public void StopSlider()
    {
        StopCoroutine(sliderCoroutine);
    }

    public IEnumerator MoveSlider()
    {
        lodingFillImage.transform.DOLocalMoveX(190, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            lodingFillImage.transform.DOLocalMoveX(-190, 1f).SetEase(Ease.Linear);
        });

        yield return new WaitForSeconds(2f);
        StartSlider();
    }

    public void OnOffPanels(GameObject previousPanel, GameObject nextPanel)
    {
        nextPanel.SetActive(true);
        previousPanel.SetActive(false);
    }

    public void OnClickSelectedLanguage()
    {
        OnOffPanels(languageScreenStart, tutorialScreen);
    }

    public void OnClickGetStarted()
    {
        OnOffPanels(tutorialScreen, mainScreen);
        IsTutorial = 1;
    }

    public void OnClickSetting(bool IsBack)
    {
        if (IsBack)
        {
            OnOffPanels(settingScreen, mainScreen);
        }
        else
        {
            OnOffPanels(mainScreen, settingScreen);
        }
    }

    public void OnClickCollection(bool IsBack)
    {
        if (IsBack)
        {
            OnOffPanels(collectionScreen, mainScreen);
        }
        else
        {
            OnOffPanels(mainScreen, collectionScreen);
        }
    }

    public void OnClickMixAnimal(bool IsBack)
    {
        if (IsBack)
        {
            OnOffPanels(selectModeScreen, mainScreen);
        }
        else
        {
            OnOffPanels(mainScreen, selectModeScreen);
        }
    }

    public void OnClickMix2(bool IsBack)
    {
        if (IsBack)
        {
            OnOffPanels(selectTemplateScreen, selectModeScreen);
        }
        else
        {
            OnOffPanels(selectModeScreen, selectTemplateScreen);
        }
    }

    public void OnClickMix3(bool IsBack)
    {
        //if (IsBack)
        //{
        //    OnOffPanels(selectModeScreen, mainScreen);
        //}
        //else
        //{
        //    OnOffPanels(mainScreen, selectModeScreen);
        //}
    }

    public void OnClickContinueButton(bool IsBack)
    {
        if (IsBack)
        {
            OnOffPanels(pick2ItemScreen, selectTemplateScreen);
        }
        else
        {
            OnOffPanels(selectTemplateScreen, pick2ItemScreen);
        }
    }

    public void OnClickVideoScreen()
    {
        videoImage.sprite = selectedTemplate;
        photoImage.sprite = unSelectedTemplate;

        currentTemplate = Template.VIDEO;
    }

    public void OnClickPhotoScreen()
    {
        videoImage.sprite = unSelectedTemplate;
        photoImage.sprite = selectedTemplate;

        currentTemplate = Template.PHOTO;
    }

    public int IsTutorial
    {
        get
        {
            return PlayerPrefs.GetInt("IsTutorial", 0);
        }
        set
        {
            PlayerPrefs.SetInt("IsTutorial", value);
        }
    }
}
