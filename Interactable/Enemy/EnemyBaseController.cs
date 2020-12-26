using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseController : Interactable
{
    protected string Name { get; set; }
    protected int Health { get; set; }
    protected int Damage { get; set; }

    public EnemyBaseController (string _name)
    {
        Name = _name;
    }

    protected virtual void Move()
    {
        // Move
    }

    public virtual void Attack() 
    {
        // Attack player within a range
    }
}
