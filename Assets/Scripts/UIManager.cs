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
    public static UIManager Instance;

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

    [Space(10)]
    [Header("Generating")]
    public GameObject animal1;
    public GameObject animal2;
    public float totalTimeGenerate;
    public float remainingTimeGenerate;
    public float speedGenerate;
    private bool IsGenerate = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

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
        remainingTimeGenerate = totalTimeGenerate;
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

        if (IsGenerate)
        {
            if (remainingTimeGenerate > 0)
            {
                remainingTimeGenerate -= Time.deltaTime * speed;
            }
            else
            {
                IsGenerate = false;
                remainingTimeGenerate = 0;
                OnOffPanels(generateScreen, generate_Screen);
                VideoController.Instance.OnGeneratePlayVideo();

                if (currentTemplate == Template.VIDEO)
                {
                    APIManager.Instance.VideoPlayer.SetActive(true);
                }
                else
                {
                    APIManager.Instance.PhotoPlayer.SetActive(true);
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

    public void OnPickUpComplete()
    {
        remainingTimeGenerate = totalTimeGenerate;
        OnOffPanels(pick2ItemScreen, generateScreen);
        GenerateShake();
    }

    public void GenerateShake()
    {
        animal1.transform.DOShakePosition(
            duration: 1f,
            strength: new Vector3(20f, 20f, 1f),
            vibrato: 15,
            randomness: 90f,
            snapping: false,
            fadeOut: true
        )
        .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Yoyo);

        animal2.transform.DOShakePosition(
            duration: 1f,
            strength: new Vector3(20f, 20f, 1f),
            vibrato: 15,
            randomness: 90f,
            snapping: false,
            fadeOut: true
        )
        .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Yoyo);

        IsGenerate = true;
    }

    public void OnClickVideo(bool IsBack)
    {
        if (IsBack)
        {
            OnOffPanels(playVideoScreen, pick2ItemScreen);
            APIManager.Instance.ResetAnimal();
        }
        else
        {
            OnOffPanels(generate_Screen, playVideoScreen);
        }
    }

    public void OnClickHomeButton()
    {
        OnOffPanels(playVideoScreen, mainScreen);
        APIManager.Instance.ResetAnimal();
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
