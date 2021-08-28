using UnityEngine;

public partial class PlayerControllerRB : MonoBehaviour
{
    public abstract class ActionStateBaseRB
    {
        public abstract void OnEnter(PlayerControllerRB owner);
        public abstract void OnUpdate(PlayerControllerRB owner);
        public abstract void OnExit(PlayerControllerRB owner);
    }
}
