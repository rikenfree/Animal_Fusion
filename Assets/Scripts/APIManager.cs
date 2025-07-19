using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class APIManager : MonoBehaviour
{
    public static APIManager Instance;

    [Header("Selection State")]
    public string firstChoice;
    public string secondChoice;
    public bool IsFirstClick = false;
    public bool IsSecondClick = false;

    [Header("UI Panels")]
    public GameObject firstSection;
    public GameObject secondSection;
    public GameObject nextChoiceButton;

    [Header("Game Data")]
    public GameData gameData;
    public GameObject itemPrefabs;
    public Transform mainCategoryParent;
    public Transform categoryParent;

    [Header("Media Players")]
    public GameObject VideoPlayer;
    public GameObject PhotoPlayer;

    [Header("Selected Item Images")]
    public Image firstSelectionG1, firstSelectionG2;
    public Image secondSelectionG1, secondSelectionG2;

    [Header("Selection Tracking")]
    public GameObject currentFirstSelection;
    public GameObject currentSecondSelection;
    public string currentVideoURL;
    public Sprite currentPhoto;

    //[Header("Favourite_Area")]
    //public FavouriteData currentFavouriteData;
    //public List<FavouriteData> allFavouriteData = new List<FavouriteData>();
    //public List<GameObject> favoriteVideo = new List<GameObject>();
    //private bool IsFavouriteClick = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // Prevent duplicates
    }

    private void Start()
    {
        ResetAnimal();
        CreateCategory();
        //LoadFavouritesFromPrefs();
    }

    public void OnClickNextChoice()
    {
        IsFirstClick = true;
        IsSecondClick = false;

        UIManager.Instance.OnPickUpComplete();
        LoadCurrentItem();
    }

    public void ResetAnimal()
    {
        Debug.Log("Reset Animal");

        firstChoice = "";
        secondChoice = "";

        IsFirstClick = true;
        IsSecondClick = false;

        firstSection.SetActive(true);
        secondSection.SetActive(false);
        nextChoiceButton.SetActive(false);

        firstSelectionG1.sprite = null;
        secondSelectionG1.sprite = null;
        firstSelectionG1.gameObject.SetActive(false);
        secondSelectionG1.gameObject.SetActive(false);

        // Optional: Reset displayed photo
        // PhotoPlayer.GetComponent<Image>().sprite = null;
    }

    public void CreateCategory()
    {
        foreach (var mainCat in gameData.mainCategories)
        {
            // Create Main Category Item
            GameObject mainItem = Instantiate(itemPrefabs, mainCategoryParent);
            SetupItem(mainItem, mainCat.mainCategory, mainCat.mainAnimal);

            // Create Subcategory Items
            foreach (var subCat in mainCat.categories)
            {
                GameObject item = Instantiate(itemPrefabs, categoryParent);
                SetupItem(item, subCat.category, subCat.animal);
            }
        }
    }

    private void SetupItem(GameObject itemObject, string name, Sprite sprite)
    {
        itemObject.name = name;

        var image = itemObject.transform.GetChild(0).GetComponent<Image>();
        if (image != null)
            image.sprite = sprite;

        var item = itemObject.GetComponent<Item>();
        if (item != null)
        {
            item.animalName = name;
            item.animalSprite = sprite;
        }
    }

    public void LoadCurrentItem()
    {
        string A1 = currentFirstSelection?.GetComponent<Item>()?.animalName;
        string A2 = currentSecondSelection?.GetComponent<Item>()?.animalName;

        if (string.IsNullOrEmpty(A1) || string.IsNullOrEmpty(A2)) return;

        foreach (var mainCat in gameData.mainCategories)
        {
            if (mainCat.mainCategory == A1)
            {
                foreach (var subCat in mainCat.categories)
                {
                    if (subCat.category == A2)
                    {
                        currentVideoURL = subCat.videoLink;
                        currentPhoto = subCat.photoSprite;
                        return;
                    }
                }
            }
        }
    }

    public void ApplyImage()
    {
        if (currentPhoto != null && PhotoPlayer != null)
        {
            var image = PhotoPlayer.GetComponent<Image>();
            if (image != null)
            {
                image.sprite = currentPhoto;
            }
        }
    }


    // Favorite Area

    //public void OnClickFavourite()
    //{
    //    IsFavouriteClick = !IsFavouriteClick;

    //    if (IsFavouriteClick)
    //    {
    //        AddFavourite();
    //        Debug.Log("AddFavourite");
    //    }
    //    else
    //    {
    //        RemoveFavourite();
    //        Debug.Log("RemoveFavourite");
    //    }

    //    UpdateFavouriteButtonUI(IsFavouriteClick);
    //}

    //private void AddFavourite()
    //{
    //    if (currentFavouriteData != null && !IsAlreadyFavourite(currentFavouriteData))
    //    {
    //        allFavouriteData.Add(new FavouriteData
    //        {
    //            iconUrl = currentFavouriteData.iconUrl,
    //            videoUrl = currentFavouriteData.videoUrl,
    //            name = currentFavouriteData.name
    //        });

    //        SaveFavouritesToPrefs();
    //        LoadFavouritesFromPrefs();
    //    }
    //}

    //private void RemoveFavourite()
    //{
    //    for (int i = allFavouriteData.Count - 1; i >= 0; i--)
    //    {
    //        if (currentFavouriteData.videoUrl == allFavouriteData[i].videoUrl)
    //        {
    //            allFavouriteData.RemoveAt(i);
    //        }
    //    }

    //    SaveFavouritesToPrefs();
    //    LoadFavouritesFromPrefs();
    //}

    //public bool IsAlreadyFavourite(FavouriteData data)
    //{
    //    return allFavouriteData.Any(fav => fav.videoUrl == data.videoUrl);
    //}

    //public void SaveFavouritesToPrefs()
    //{
    //    string json = JsonConvert.SerializeObject(allFavouriteData);
    //    PlayerPrefs.SetString("favourites", json);
    //    PlayerPrefs.Save();
    //    Debug.Log("Favourites saved: " + json);
    //}

    //public void LoadFavouritesFromPrefs()
    //{
    //    for (int i = 0; i < favoriteVideo.Count; i++)
    //    {
    //        Destroy(favoriteVideo[i]);
    //    }

    //    favoriteVideo.Clear();
    //    GameObject currentParent = null;

    //    if (PlayerPrefs.HasKey("favourites"))
    //    {
    //        string json = PlayerPrefs.GetString("favourites");
    //        allFavouriteData = JsonConvert.DeserializeObject<List<FavouriteData>>(json);
    //        Debug.Log("Favourites loaded: " + json);

    //        for (int i = 0; i < allFavouriteData.Count; i++)
    //        {
    //            if (i % 2 == 0)
    //            {
    //                currentParent = Instantiate(rowParent, gridFavoriteSection);
    //                favoriteVideo.Add(currentParent);
    //            }

    //            ShowDataInFavouriteScreen(allFavouriteData[i].videoUrl, allFavouriteData[i].iconUrl, allFavouriteData[i].name, currentParent.transform);
    //        }
    //    }
    //}

    //public void ShowDataInFavouriteScreen(string data, string iconUrl, string name, Transform parent)
    //{
    //    GameObject videoObject = Instantiate(gridPrefab, parent);
    //    videoObject.transform.GetComponent<StoryScripts>().url = data;
    //    videoObject.transform.GetComponent<StoryScripts>().iconUrl = iconUrl;
    //    videoObject.transform.GetComponent<StoryScripts>().name = name;
    //    StartCoroutine(DownloadProfilePic(iconUrl, videoObject.GetComponent<ProceduralImage>()));
    //    videoObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
    //}

    //public void UpdateFavouriteButtonUI(bool isFavourite)
    //{
    //    IsFavouriteClick = isFavourite;

    //    if (favouriteIcon != null)
    //    {
    //        favouriteIcon.sprite = isFavourite ? favOnSprite : favOffSprite;
    //    }
    //}
}

//[System.Serializable]
//public class FavouriteData
//{
//    public string iconUrl;
//    public string videoUrl;
//    public string name;
//}
