using UnityEngine;

[System.Serializable]
public class SpellContext
{
    // Who is casting
    public Transform casterTransform;

    // Where projectiles spawn from
    public Transform firePoint;

    // Direction the caster is facing (2D)
    public Vector2 facing;

    // Optional future stuff
    public float mana;
    public GameObject casterObject;
}
