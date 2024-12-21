using Gameplay;
using Interactables;

namespace Bosses
{
    /// <summary>
    /// Class implementing the sockets for the boss room.
    /// </summary>
    public class SocketController : Interactable
    {
        private LampBossController _lampBoss;
        private float _damage = 50;
        
        /// <summary>
        /// Initialization function for the sockets.
        /// </summary>
        /// <param name="lampBoss">The boss</param>
        /// <param name="damage">How much damage the socket do</param>
        public void Initialize(LampBossController lampBoss, float damage)
        {
            _lampBoss = lampBoss;
            _damage = damage;
        }
        
        /// <summary>
        /// Implementation of the interaction action.
        /// </summary>
        /// <param name="player">Player (Not Used in this case)</param>
        public override void Interact(PlayerController player)
        {
            _lampBoss.TakeDamage(_damage);
            Destroy(gameObject);
        }
    
    }
}
