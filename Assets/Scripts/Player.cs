using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private double _wallet = 10000f;

    public double Wallet
    {
        get => Math.Round(_wallet, 2);
        set => _wallet = value;
    }

    public bool CanBuy(double price)
    {
        return Wallet >= price;
    }

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
