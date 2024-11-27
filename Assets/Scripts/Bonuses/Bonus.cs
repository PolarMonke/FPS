using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bonus : MonoBehaviour
{

    protected string _name;
    protected string _description;
    protected string _imagePath;
    protected int _duration;
    
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public Image BGImage;
    public TextMeshProUGUI Duration;

    protected bool _isActive = false;

    public Bonus(string name, string description, string imagePath, int duration)
    {
        _name = name;
        _description = description;
        _imagePath = imagePath;
        _duration = duration;
    }

    protected void Start()
    {
        Name.text = _name;
        Description.text = _description;
        BGImage.sprite = Resources.Load<Sprite>(_imagePath);
        Duration.text = _duration.ToString();
    }

    void Update()
    {
        if (_isActive)
        {
            DoItsThing();
        }
    }

    public void SetActive()
    {
        _isActive = true;
    }

    protected virtual void DoItsThing()
    {
        
    }
}
