using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Collections;
public class Shop: MonoBehaviour
{
    public GameObject playerPrefab;
    public Text Wallet;
    public Skin[] skins = new Skin[2];
    private string[] lines;
    private string[] shopInfo;
    public MineHints mineHints;
    public GameObject Message;

    private void Start()
    {
        Debug.Log("Start");
        lines = FileHandler.LoadData(FileHandler.resultsFile);

        mineHints.UpdateCount();
        Wallet.text = lines[0];
        Message.SetActive(false);
        LoadShopData();
    }
  
    public void LoadShopData()
    {
        shopInfo = FileHandler.LoadData(FileHandler.shopInfoFile);
        for (int i = 0; i < shopInfo.Length; i++)
        {
            string[] data = shopInfo[i].Split(',');
            if (data.Length == 2)
            {
                skins[i].bought = bool.Parse(data[0]);
                skins[i].equiped = bool.Parse(data[1]);
                if (skins[i].equiped)
                {
                    ChangePlayerAnimatorController(skins[i].controller);
                }
            }
        }
       
    }
    public void BuyHint()
    {
        int coins = int.Parse(lines[0]);
        if (coins >= 10)
        {
            coins -= 10;
            Wallet.text = coins.ToString();
            lines[0] = Wallet.text;
            FileHandler.SaveData(FileHandler.resultsFile, lines);
            mineHints.amount += 1;
            shopInfo[0] = mineHints.amount.ToString();
            SaveHintsData();
            mineHints.UpdateCount();
        }
        else
        {
            StartCoroutine(ShowMessage());
        }
    }
    private IEnumerator ShowMessage()
    {
        Message.SetActive(true);

        yield return new WaitForSeconds(0.5f);


        Message.SetActive(false);
    }

    private void SaveHintsData()
    {
        string[] info = new string[1];
        info[0] = mineHints.amount.ToString();
        FileHandler.SaveData(FileHandler.hintsInfoFile, info);
    }
    private void SaveShopData()
    {
        List<string> shopData = new List<string>{};
        foreach (var skin in skins)
        {
            shopData.Add($"{skin.bought},{skin.equiped}");
        }
        FileHandler.SaveData(FileHandler.shopInfoFile, shopData.ToArray());
    }
    public void BuyOrEquip(Skin skin)
    {
        if (!skin.bought)
        {
            int coins = int.Parse(Wallet.text);
            coins -= skin.price;
            Wallet.text = coins.ToString();
            lines[0] = Wallet.text;
            FileHandler.SaveData(FileHandler.shopInfoFile, lines.ToArray());
            skin.bought = true;
        }
        else if (!skin.equiped)
        {
            SetSkinsEquiped(false);
            skin.equiped = true;
            ChangePlayerAnimatorController(skin.controller);
        }
        SaveShopData();
    }
    private void SetSkinsEquiped(bool value)
    {
        for(int i = 0; i < skins.Length; i++)
        {
            skins[i].equiped = value;
        }
    }
    private void ChangePlayerAnimatorController(RuntimeAnimatorController newController)
    {

        Animator playerAnimator = playerPrefab.GetComponent<Animator>();

        if (playerAnimator != null)
        {
            playerAnimator.runtimeAnimatorController = newController;
            playerPrefab.AddComponent<BoxCollider2D>();
            
        }
        else
        {
            Debug.LogError("Animator component not found on player prefab.");
        }
    }

 
}
