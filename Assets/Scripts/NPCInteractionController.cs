using UnityEngine;

public class NPCInteractionController : MonoBehaviour, IEntity
{
    public void InteractWith()
    {
        Debug.Log("I am NPC " + gameObject.name);
    }
}
