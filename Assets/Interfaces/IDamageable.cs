using UnityEngine;
public interface IDamageable
{
    public void OnHit(int damage, bool crit, Vector2 knockback);

    public void OnHit(int damage, bool crit);
}
