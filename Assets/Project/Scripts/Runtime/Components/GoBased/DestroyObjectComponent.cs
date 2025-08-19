using Project.Scripts.Model;
using UnityEngine;

namespace Project.Scripts.Components
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToDestroy;
        [SerializeField] private RestoreStateComponent _state;
        
        public void DestroyObject()
        {
            Destroy(_objectToDestroy);
            if(_state != null)
                FindObjectOfType<GameSessionModel>().StoreState(_state.Id);
        }
    }
}
