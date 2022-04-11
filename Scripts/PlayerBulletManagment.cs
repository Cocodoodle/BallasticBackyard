using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct BulletManager
{
    public int normalBullets;
    public int hyperModernBullets;

    public BulletManager(int normalBullets, int hyperModernBullets)
    {
        this.normalBullets = normalBullets;
        this.hyperModernBullets = hyperModernBullets;
    }
}
