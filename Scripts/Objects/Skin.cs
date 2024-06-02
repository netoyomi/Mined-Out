using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
public class Skin : MonoBehaviour
{
    public RuntimeAnimatorController controller;
    public bool bought;
    public int price;
    public bool equiped;
    private Image holderImage;
    public Text buttonText;

    private void Start()
    {
        holderImage = GetComponent<Image>();
    }
    private void Update()
    {
        if (equiped)
        {
           holderImage.color = new Color(1f, 1f, 1f, 0.5f);
           buttonText.text = "Equiped";
        }
        else if(!equiped && bought)
        {
            holderImage.color = new Color(0.2f, 0.2f, 0.2f, 0.7f);
            buttonText.text = "Equip";
        }
        else
        {
            holderImage.color = new Color(0.2f, 0.2f, 0.2f, 0.7f);
            buttonText.text = price + " coins";
        }
    }
}