using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "GameData", order = 1)]
public class GameData : ScriptableObject
{
    public List<MainCategory> mainCategories = new List<MainCategory>();
}

[System.Serializable]
public class MainCategory
{
    public string mainCategory;
    public Sprite mainAnimal;
    public List<Category> categories = new List<Category>();
}

[System.Serializable]
public class Category
{
    public string category;
    public Sprite animal;
    public string videoLink;
    public Sprite photoSprite;
}
