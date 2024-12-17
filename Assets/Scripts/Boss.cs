using System;

namespace Assets.Scripts
{
    public interface Boss
    {
        public event Action OnDeath;
        public void TakeDamage(float damage);
    }
}