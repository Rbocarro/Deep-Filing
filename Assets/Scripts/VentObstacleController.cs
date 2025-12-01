using Evengy.GridBasedMovementController.Player;
using UnityEngine;

public class VentObstacleController : MonoBehaviour, IEntity
{


    Vector3 playerPos;
    bool hasMoved=false;
    [SerializeField] private BoxCollider ventCollider;
    [SerializeField] private float frontCenterZ = 0.5f;
    [SerializeField] private float backCenterZ = -0.5f;



    void Update()
    {
        playerPos=PlayerController.PlayerWorldPosition;
        // Compare global positions along the vent's forward axis
        Vector3 toPlayer = transform.InverseTransformPoint(playerPos);

        if (toPlayer.z > 0f)
            ventCollider.center = new Vector3(ventCollider.center.x, ventCollider.center.y, backCenterZ);
        else
            ventCollider.center = new Vector3(ventCollider.center.x, ventCollider.center.y, frontCenterZ);
    }

    public void InteractWith()
    {   if (hasMoved) return;
        this.transform.position = new Vector3(0f, 1000f, 1f);
        //this.gameObject.SetActive(false);
        hasMoved = true;
    }
}
