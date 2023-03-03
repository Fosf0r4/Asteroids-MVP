using System;
using UnityEngine;

public class ShipPresenter : Presenter
{
    private Root _init;

    public uint ShipHealth = 3;

    public void Init(Root init)
    {
        _init = init;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (ShipHealth > 0)
            {
                _init.OnHit(1);

                if (ShipHealth == 0)
                {
                    _init.GameOver();
                }
            } 
        }
    }
}
