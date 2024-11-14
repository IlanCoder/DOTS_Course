using UnityEngine;
using UnityEngine.InputSystem;

public class MouseWorldPosition : MonoBehaviour {
    [SerializeField] LayerMask groundLayer;
        
    void Update() {
        //Debug.Log(GetPosition());
    }
        
    public Vector3 GetPosition() {
        Ray mouseCamRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        return Physics.Raycast(mouseCamRay, out RaycastHit hit, groundLayer) ? hit.point : Vector3.zero;
    }
}