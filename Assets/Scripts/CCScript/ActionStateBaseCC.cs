using UnityEngine;

public partial class PlayerControllerCC : MonoBehaviour
{
    public abstract class ActionStateBaseCC
    {
        public abstract void OnEnter(PlayerControllerCC owner);
        public abstract void OnUpdate(PlayerControllerCC owner);
        public abstract void OnExit(PlayerControllerCC owner);
    }
}
