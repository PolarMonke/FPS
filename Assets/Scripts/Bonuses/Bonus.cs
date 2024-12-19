using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;


public class Bonus : MonoBehaviour
{
    public enum BonusTypes
    {
        Double,
        Invincible,
        Chill,
        Those,
        Cheese,
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
    protected bool _isActivated = false;
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
            BGImage.sprite = AssetsReferences.Instance.GetBonusBackground(Path.GetFileNameWithoutExtension(_imagePath));
            Duration.text = _duration.ToString();

            pickedUp = !pickedUp;
        }   
        if (!GameObject.FindGameObjectWithTag("BonusUI").GetComponent<BonusUI>().AllSlotsAreFull())
        {
            if (Input.GetMouseButtonDown(0) && !_isActive)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(BGImage.rectTransform, Input.mousePosition))
                {
                    OnBonusClicked();
                }
            }
        }
        if (_isActivated)
        {
            DoItsThing();
            _isActivated = false;
        }
    }

    public void SetActive()
    {
        InventoryManager.Instance.RemoveFromInventory(bonusType);
        _isActivated = true;
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
        SetActive();

    }

    protected virtual void DoItsThing()
    {
        
    }

    protected void MoveToUI()
    {
        GameObject UI = GameObject.FindGameObjectWithTag("BonusUI");
        //transform.SetParent(UI.transform);
        //UI.GetComponent<BonusUI>().AlignChildrenVertically();
        UI.GetComponent<BonusUI>().PutBonusIntoSlot(gameObject);
    }

    protected void Deactivate()
    {
        _isActivated = false;
        Destroy(gameObject);
    }
}
