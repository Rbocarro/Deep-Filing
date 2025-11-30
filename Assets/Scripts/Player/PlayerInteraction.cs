using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public IEntity CurrentEntity { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        IEntity entity = other.GetComponent<IEntity>();
        if (entity != null)
        {
            CurrentEntity = entity;
            Debug.Log("Entity found");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IEntity entity = other.GetComponent<IEntity>();
        if (entity != null && entity == CurrentEntity)
        {
            CurrentEntity = null;
        }
    }
}
