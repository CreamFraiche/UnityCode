using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShopHolder
{
    public static Shop curShop;

    public static void SetShop(Shop shop)
    {
        curShop = shop;
    }

    public static Shop GetShop()
    {
        return curShop;
    }

    public static void BuyItem(int index)
    {
        curShop.BuyItem(index);
    }
}
