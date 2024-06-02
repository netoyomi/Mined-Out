using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Enemy : Cell
{
    public float stepSize = 1f;
    public bool playerCatched = false;
}
