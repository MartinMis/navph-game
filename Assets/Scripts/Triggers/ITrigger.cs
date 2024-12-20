using System;

namespace Triggers
{
    public interface ITrigger
    {
        public event Action OnTriggered;
    }
}