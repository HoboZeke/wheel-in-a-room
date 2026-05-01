using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class FuelImporter : Interactable
{
    [SerializeField] Transform buttonTransform;
    [SerializeField] Vector3 buttonBasePos, buttonPressedPos;
    [SerializeField] TextMeshProUGUI fuelImportValue;
    [SerializeField] int[] importCosts;
    [SerializeField] float animDuration;
    int consecutiveImports;

    private void Start()
    {
        Furnance.main.OnFurnanceValueUpdate += FurnanceValuesChanged;
        UpdateImportUI();
    }

    int ImportCost(int mod = 0)
    {
        int spinsGenerated = Furnance.main.SpinsGenerated() + mod;
        if(spinsGenerated < importCosts.Length) { return importCosts[spinsGenerated]; }
        else 
        {
            float pow = spinsGenerated - (importCosts.Length-2);
            pow = (pow / 10) + 1;
            return Mathf.FloorToInt(Mathf.Pow(importCosts[importCosts.Length - 1], pow)); 
        }
    }

    void FurnanceValuesChanged(object s, EventArgs e) { consecutiveImports = 0; UpdateImportUI(); }

    void UpdateImportUI()
    {
        fuelImportValue.text = ImportCost(consecutiveImports) + "g for " + Furnance.main.NextInputAmount() + "f";
    }

    public override void Interact()
    {
        if(CoinScoop.main.CanAfford(ImportCost()))
        {
            CoinScoop.main.SpendCoin(ImportCost());
            fuelImportValue.text = "Importing....";
            StartCoroutine(PressButtonAnim());
        }
    }

    IEnumerator PressButtonAnim()
    {
        float timeElapsed = 0f;
        float dur = animDuration / 2f;

        while(timeElapsed < dur)
        {
            buttonTransform.localPosition = Vector3.Lerp(buttonBasePos, buttonPressedPos, timeElapsed / dur);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        buttonTransform.localPosition = buttonPressedPos;
        RewardShoot.main.SpawnFuelReward(Furnance.main.NextInputAmount());
        timeElapsed = 0f;

        while (timeElapsed < dur)
        {
            buttonTransform.localPosition = Vector3.Lerp(buttonPressedPos, buttonBasePos, timeElapsed / dur);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        buttonTransform.localPosition = buttonBasePos;
        consecutiveImports += 1;
        UpdateImportUI();
    }
}
