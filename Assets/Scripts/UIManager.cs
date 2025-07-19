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

    #region Loading
    [Header("LOADER")]
    public GameObject lodingFillImage;
    public float totalTime;
    public float speed;
    private float remainingTime;
    private Coroutine sliderCoroutine;
    private bool isStartSlider = false;
    #endregion

    #region UI Screens
    [Header("UI SCREENS")]
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
    #endregion

    #region ScrollSnap
    [Header("SimpleScrollSnap")]
    [SerializeField] private SimpleScrollSnap scrollSnap;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject getStartedButton;
    #endregion

    #region Template Selection
    [Header("Template Area")]
    public Image videoImage;
    public Image photoImage;
    public Sprite selectedTemplate;
    public Sprite unSelectedTemplate;
    public Template currentTemplate;
    #endregion

    #region Generate
    [Header("Generating")]
    public GameObject animal1;
    public GameObject animal2;
    public float totalTimeGenerate;
    public float speedGenerate;
    private float remainingTimeGenerate;
    private bool isGenerating = false;
    #endregion

    #region Unity Events
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

    private void Start()
    {
        remainingTime = totalTime;
        remainingTimeGenerate = totalTimeGenerate;
        isStartSlider = true;
        StartSlider();
    }

    private void Update()
    {
        HandleLoader();
        HandleGenerating();
    }
    #endregion

    #region Panel Control
    private void HandleLoader()
    {
        if (!isStartSlider) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime * speed;
        }
        else
        {
            isStartSlider = false;
            remainingTime = 0;
            StopSlider();

            if (IsTutorial == 0)
                OnOffPanels(lodingScreen, languageScreenStart);
            else
                OnOffPanels(lodingScreen, mainScreen);
        }
    }

    private void HandleGenerating()
    {
        if (!isGenerating) return;

        if (remainingTimeGenerate > 0)
        {
            remainingTimeGenerate -= Time.deltaTime * speedGenerate;
        }
        else
        {
            isGenerating = false;
            remainingTimeGenerate = 0;
            OnOffPanels(generateScreen, generate_Screen);
            VideoController.Instance.OnGeneratePlayVideo();

            if (currentTemplate == Template.VIDEO)
            {
                APIManager.Instance.VideoPlayer.SetActive(true);
                APIManager.Instance.PhotoPlayer.SetActive(false);
            }
            else
            {
                APIManager.Instance.PhotoPlayer.SetActive(true);
                APIManager.Instance.VideoPlayer.SetActive(false);
                APIManager.Instance.ApplyImage();
            }
        }
    }
    #endregion

    #region Slider
    public void StartSlider()
    {
        sliderCoroutine = StartCoroutine(MoveSlider());
    }

    public void StopSlider()
    {
        if (sliderCoroutine != null)
        {
            StopCoroutine(sliderCoroutine);
            sliderCoroutine = null;
        }
    }

    private IEnumerator MoveSlider()
    {
        lodingFillImage.transform.DOLocalMoveX(190, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            lodingFillImage.transform.DOLocalMoveX(-190, 1f).SetEase(Ease.Linear);
        });

        yield return new WaitForSeconds(2f);
        StartSlider();
    }
    #endregion

    #region Panel Transitions
    public void OnOffPanels(GameObject previousPanel, GameObject nextPanel)
    {
        nextPanel.SetActive(true);
        previousPanel.SetActive(false);
    }
    #endregion

    #region ScrollSnap Callback
    private void OnPanelCentered(int centeredPanel, int previousPanel)
    {
        int lastPanelIndex = scrollSnap.NumberOfPanels - 1;

        nextButton.SetActive(centeredPanel != lastPanelIndex);
        getStartedButton.SetActive(centeredPanel == lastPanelIndex);
    }
    #endregion

    #region Button Click Events
    public void OnClickSelectedLanguage() => OnOffPanels(languageScreenStart, tutorialScreen);

    public void OnClickLanguageSelect(bool isBack)
    {
        OnOffPanels(isBack ? languageScreen : settingScreen, isBack ? settingScreen : languageScreen);
    }

    public void OnClickGetStarted()
    {
        OnOffPanels(tutorialScreen, mainScreen);
        IsTutorial = 1;
    }

    public void OnClickSetting(bool isBack)
    {
        OnOffPanels(isBack ? settingScreen : mainScreen, isBack ? mainScreen : settingScreen);
    }

    public void OnClickCollection(bool isBack)
    {
        OnOffPanels(isBack ? collectionScreen : mainScreen, isBack ? mainScreen : collectionScreen);
    }

    public void OnClickMixAnimal(bool isBack)
    {
        OnOffPanels(isBack ? selectModeScreen : mainScreen, isBack ? mainScreen : selectModeScreen);
    }

    public void OnClickMix2(bool isBack)
    {
        OnOffPanels(isBack ? selectTemplateScreen : selectModeScreen, isBack ? selectModeScreen : selectTemplateScreen);
    }

    public void OnClickMix3(bool isBack)
    {
        // Unused - implement if needed
    }

    public void OnClickContinueButton(bool isBack)
    {
        OnOffPanels(isBack ? pick2ItemScreen : selectTemplateScreen, isBack ? selectTemplateScreen : pick2ItemScreen);
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

    public void OnClickVideo(bool isBack)
    {
        if (isBack)
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
    #endregion

    #region Generate Animation
    public void OnPickUpComplete()
    {
        remainingTimeGenerate = totalTimeGenerate;
        OnOffPanels(pick2ItemScreen, generateScreen);
        GenerateShake();
    }

    private void GenerateShake()
    {
        animal1.transform.DOShakePosition(1f, new Vector3(20f, 20f, 1f), 15, 90f, false, true)
              .SetEase(Ease.Linear)
              .SetLoops(-1, LoopType.Yoyo);

        animal2.transform.DOShakePosition(1f, new Vector3(20f, 20f, 1f), 15, 90f, false, true)
              .SetEase(Ease.Linear)
              .SetLoops(-1, LoopType.Yoyo);

        isGenerating = true;
    }
    #endregion

    #region Tutorial Flag
    public int IsTutorial
    {
        get => PlayerPrefs.GetInt("IsTutorial", 0);
        set => PlayerPrefs.SetInt("IsTutorial", value);
    }
    #endregion
}
