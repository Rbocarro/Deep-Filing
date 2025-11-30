using UnityEngine;

namespace Evengy.GridBasedMovementController.Helpers
{
    public class TriggerOnTag : MonoBehaviour
    {
        [SerializeField] Tag triggerTag;
        public bool IsTriggered => isTriggered;
        public GameObject TriggerObject { get; private set; }

        bool isTriggered;

        SphereCollider triggerCollider;
        private Color gizmoColor;

        private void OnValidate()
        {
            triggerCollider = GetComponent<SphereCollider>();
            SetGizmoColorFromTag();
        }

        private void Awake()
        {
            SetGizmoColorFromTag();
        }
        public Tag GetTriggerTag() { 
            
            return triggerTag;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals(triggerTag.ToString()))
            {
                TriggerObject = other.gameObject;
                isTriggered = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals(triggerTag.ToString()))
            {
                TriggerObject = null;
                isTriggered = false;
            }
        }

        private void SetGizmoColorFromTag()
        {
            switch (triggerTag)
            {
                case Tag.Tile:
                    gizmoColor = Color.green;
                    break;
                case Tag.Obstacle:
                    gizmoColor = Color.red;
                    break;
                case Tag.Player:
                    // Blue for Player
                    gizmoColor = Color.blue;
                    break;
                default:
                    gizmoColor = Color.white;
                    break;
            }
            gizmoColor.a = 0.7f;// Add a little transparency so it looks like a wireframe
        }

        private void OnDrawGizmosSelected()
        {
            if (triggerCollider != null)
            {
                Gizmos.color = gizmoColor;
                Matrix4x4 oldMatrix = Gizmos.matrix;// Apply the object's transform to the Gizmo drawing
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawWireSphere(triggerCollider.center, triggerCollider.radius);
                Gizmos.matrix = oldMatrix;// Reset the matrix to prevent it from affecting other Gizmos
            }
        }

    }
}