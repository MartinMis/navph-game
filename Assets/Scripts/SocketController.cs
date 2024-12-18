public class SocketController : Interactable
{
    private LampBossController _lampBoss;
    private float _damage = 50;

    public void Initialize(LampBossController lampBoss, float damage)
    {
        _lampBoss = lampBoss;
        _damage = damage;
    }

    public override void Interact(PlayerController player)
    {
        _lampBoss.TakeDamage(_damage);
        Destroy(gameObject);
    }
    
}
