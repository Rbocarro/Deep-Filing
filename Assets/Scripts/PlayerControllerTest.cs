using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerControllerTest : MonoBehaviour
{

    //MOVEMENT
    public float transitionSpeed = 10f;
    public float transitionRotationSpeed=500f;

    public Vector3 targetGridPos;
    private Vector3 prevTargetGridPos;
    private Vector3 targetRotation;

    public float rayDistance = 1f; // The distance the ray will check
    public LayerMask wallLayer;    // Assign this in the inspector to only detect walls

    public Camera playerCamera;


    //INVENTORY
    public List<string> inventory = new List<string> { "Gun","Umbrella","L1 Acess Card" };

    private bool AtRest
    {
        get
        {
            if ((Vector3.Distance(transform.position, targetGridPos) < 0.05f) &&
                (Vector3.Distance(transform.eulerAngles, targetRotation) < 0.05f))
                return true;
            else
                return false;

        }
    }

    //PLAYER MOVEMENT
    public void MoveForward() { if (AtRest&& !Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, rayDistance, wallLayer)) Vector3Int.RoundToInt(targetGridPos += transform.forward); }
    public void MoveBack() { if (AtRest&& !Physics.Raycast(playerCamera.transform.position, (playerCamera.transform.forward*-1), out RaycastHit hit, rayDistance, wallLayer)) Vector3Int.RoundToInt(targetGridPos -= transform.forward); }
    public void MoveLeft() { if (AtRest&& !Physics.Raycast(playerCamera.transform.position, (playerCamera.transform.right*-1), out RaycastHit hit, rayDistance, wallLayer)) Vector3Int.RoundToInt(targetGridPos -= transform.right); }
    public void MoveRight() { if (AtRest && !Physics.Raycast(playerCamera.transform.position, playerCamera.transform.right, out RaycastHit hit, rayDistance, wallLayer)) Vector3Int.RoundToInt(targetGridPos += transform.right); }

    //PLAYER ROTATION
    public void RotateLeft() { if (AtRest) targetRotation -= Vector3.up * 90f; }
    public void RotateRight() { if (AtRest) targetRotation += Vector3.up * 90f; }


    public void Interact()
    {
        Ray ray = new Ray(playerCamera.transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            EntityInteractor entity = hit.collider.GetComponent<EntityInteractor>();
            if (entity != null)
            {   
                entity.transform.SetPositionAndRotation(entity.transform.position,Quaternion.LookRotation(playerCamera.transform.position,Vector3.up));
                Debug.Log("Entity Name: " + entity.entityName);
                string inv= "Inventory:";
                foreach (var item in entity.inventory)
                {
                    inv += "- " + item + ",";
                }
            }
        }
    }

    private void Start()
    {
        targetGridPos=Vector3Int.RoundToInt(transform.position);
        
    }

    private void FixedUpdate()
    {
            MovePlayer(); 
    }

    void MovePlayer()
    {   
            prevTargetGridPos = targetGridPos;
            Vector3 targetPosition = targetGridPos;

            if (targetRotation.y > 270f && targetRotation.y < 361f) targetRotation.y = 0f;
            if (targetRotation.y < 0f) targetRotation.y = 270f;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * transitionSpeed);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * transitionRotationSpeed);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(targetGridPos, 0.1f);
       // Gizmos.color = Color.green;
       // Gizmos.DrawSphere(prevTargetGridPos, 0.1f);
        //Gizmos.DrawLine(playerCamera.transform.position, targetGridPos);
    }


}
