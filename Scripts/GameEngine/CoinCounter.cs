using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
public class CoinCounter: MonoBehaviour
{

    public Text textField;
    public GameObject CoinMessage;
    public Text messageText;
    private void Start()
    {
        CoinMessage.SetActive(false);
    }
    public void UpdateCoinCounter(int count)
    {
        textField.text = count.ToString();
    }

    public void CallCoinMessage(string message)
    {
        StartCoroutine(ShowCoinMessage(message));
    }

    private IEnumerator ShowCoinMessage(string m)
    {
        Time.timeScale = 0f;
        messageText.text = m;
        CoinMessage.SetActive(true);
        yield return new WaitForSecondsRealtime(1f); 
        CoinMessage.SetActive(false);
        Time.timeScale = 1f; 
    }
}