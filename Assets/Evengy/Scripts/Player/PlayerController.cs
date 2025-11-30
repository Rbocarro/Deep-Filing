using Evengy.GridBasedMovementController.Navigation;
using PrimeTween;
using System.Collections.Generic;
using UnityEngine;

namespace Evengy.GridBasedMovementController.Player
{
    public partial class PlayerController : MonoBehaviour
    {
        public static Vector3 PlayerWorldPosition { get; private set; }
        public bool CanPlayerMove { get; private set; }

        [SerializeField] private PlayerInteraction interactionBox;

        [Header("Movement Settings")]
        [SerializeField] bool smoothTransition = false;
        [SerializeField] float movementSpeed;
        [SerializeField] float rotationSpeed;
        [SerializeField] float precision;

        bool defaultTransition;
        float defaultMovementSpeed;

        [SerializeField] TileController grid;
        TileController targetTile;
        Direction currentDirection;
        Vector3 targetRotation;

        [Header("Camera Settings")]
        [SerializeField] Transform cameraTransform; // Drag Main Camera here
        [SerializeField] float shakeStrength = 0.15f;
        [SerializeField] float shakeDuration = 0.2f;
        [SerializeField] int shakeFrequency = 10;

        public bool IsBusy => Vector3.Distance(transform.position, targetTile.transform.position) > precision
            || Vector3.Distance(transform.rotation.eulerAngles, targetRotation) > precision;

        private Direction localDirection(Direction direction)
            => (Direction)(((int)currentDirection + (int)direction) % (int)Direction.Round);

        private void Start()
        {
            defaultTransition = smoothTransition;
            defaultMovementSpeed = movementSpeed;

            currentDirection = Direction.Forward;
            targetTile = grid;
            targetRotation = Vector3.up * (int)currentDirection;
            CanPlayerMove=true;
        }

        private void Update()
        {
            PlayerWorldPosition = transform.position;
            if (Input.GetKeyDown(KeyCode.J))
            {
                Debug.Log("Interact");
                if (interactionBox.CurrentEntity != null)
                    interactionBox.CurrentEntity.InteractWith();
            }
            if (!IsBusy) return;
            if (CanPlayerMove)
            {
                Move();
                Rotate();
            }
        }

        private void Move() => transform.position = smoothTransition ?
            Vector3.MoveTowards(transform.position, targetTile.transform.position, Time.deltaTime * movementSpeed)
            : targetTile.transform.position;

        private void Rotate() => transform.rotation = smoothTransition ?
            Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * rotationSpeed)
            : Quaternion.Euler(targetRotation);

        public void MoveTowards(Direction direction)
        {
            if (IsBusy) return;
            TileController nextTile = targetTile.GetTile(localDirection(direction));// Calculate the intended tile
            if (nextTile == targetTile) // Check if  are stuck (Next tile is the same as Current tile)
            {
                ShakeCamera();
                AudioManager.instance.Play("wallslam");
                return; 
            }
            // Move is valid
            targetTile = nextTile;
            transform.parent = targetTile.transform;

        }
        public void RotateTowards(Direction direction)
        {
            if (IsBusy) return;
            currentDirection = localDirection(direction);
            targetRotation = Vector3.up * (int)currentDirection;
        }

        private void ShakeCamera()
        {
            Tween.StopAll(cameraTransform);
            Tween.ShakeLocalPosition(
                target: cameraTransform,
                strength: new Vector3(shakeStrength, 0, shakeStrength*2),
                duration: shakeDuration,
                frequency: shakeFrequency
            );
        }
    }
}