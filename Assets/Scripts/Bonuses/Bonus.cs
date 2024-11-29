using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Bonus : MonoBehaviour
{
    public enum BonusTypes
    {
        Double,
        Invincible,
        Chill,
        Those,
    }

    public BonusTypes bonusType;

    protected string _name;
    protected string _description;
    protected string _imagePath;
    protected int _duration;
    
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public Image BGImage;
    public TextMeshProUGUI Duration;
    
    public Animator bonusAnimator;

    public bool pickedUp = false;
    protected bool _isActive = false;

    public void Create(string name, string description, string imagePath, int duration)
    {
        _name = name;
        _description = description;
        _imagePath = imagePath;
        _duration = duration;
    }

    void Update()
    {
        if (pickedUp)
        {
            Name.text = _name;
            Description.text = _description;
            BGImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Textures/Bonuses/" + _imagePath);
            Duration.text = _duration.ToString();

            pickedUp = !pickedUp;
        }   
        if (Input.GetMouseButtonDown(0))
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(BGImage.rectTransform, Input.mousePosition))
            {
                OnBonusClicked();
            }
        }
        if (_isActive)
        {
            DoItsThing();
        }
    }

    public void SetActive()
    {
        _isActive = true;
    }

    public void CloneBonus(Bonus bonus)
    {
        Name = bonus.Name;
        Description = bonus.Description;
        BGImage = bonus.BGImage;
        Duration = bonus.Duration;
        bonusAnimator = bonus.bonusAnimator;
    }

     public void OnBonusClicked()
    {
        Debug.Log("Bonus clicked!");
        _isActive = true;
    }

    protected virtual void DoItsThing()
    {
        
    }
}
