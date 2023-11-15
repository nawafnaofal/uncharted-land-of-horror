using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDamageable : IDestroyable
{
    void TakeDamage(float damageTaken, GameObject damageGiver);
}

public interface IDestroyable
{
    void Destroy();
}

