using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class DoubleBonus : Bonus
{
    private void Start()
    {
        bonusType = BonusTypes.Double;
    }

    protected override void DoItsThing()
    {
        _isActive = true;
        MoveToUI();
        ManipulateWaves();
        Deactivate();
    }

    private void ManipulateWaves()
    {
        WaveManager.Instance.waveController.DoubleAndGiveItToTheNextWave();
    }
}