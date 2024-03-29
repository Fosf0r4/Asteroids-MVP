﻿using Asteroids.Model;
using System;
using UnityEngine;

public class EnemyPresenter : Presenter
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            DestroyCompose();
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            DestroyCompose();
        }
    }
}
