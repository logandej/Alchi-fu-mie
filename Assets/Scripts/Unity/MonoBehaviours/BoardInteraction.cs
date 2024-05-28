using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardInteraction : MonoBehaviour
{
    private Camera _mainCamera;
    [HideInInspector] public Vector3 CurrentMousePosition;
    [SerializeField] float scale;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Table")) continue;
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.red);

            CurrentMousePosition = hit.point;

            break;
        }
    }
}
