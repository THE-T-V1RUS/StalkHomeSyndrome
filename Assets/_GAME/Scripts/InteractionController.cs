using Hertzole.GoldPlayer;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    [SerializeField] Image img_Crosshair;
    [SerializeField] Sprite spr_NormalCrosshair, srp_InteractCrosshair, spr_InspectCrosshair;
    [SerializeField] float interactDistance = 3f;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] GoldPlayerController playerController;

    private void Start()
    {
        img_Crosshair.sprite = spr_NormalCrosshair;
        img_Crosshair.color = new Color(1f, 1f, 1f, 0.15f);
        img_Crosshair.transform.localScale = Vector3.one * 0.22f;
    }

    private void Update()
    {
        if (!playerController.Movement.CanMoveAround || !playerController.Camera.CanLookAround)
        {
            SetNormalCrosshair();
            return;
        }

        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayer))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                SetInteractCrosshair();

                if (Input.GetMouseButtonDown(0))
                {
                    var obj_interaction = hit.transform.GetComponent<ObjectInteraction>();
                    if (obj_interaction == null) return;
                    obj_interaction.PerformInteractionEvent();
                }

                return;
            }
        }

        SetNormalCrosshair();

        Color rayColor = Color.red;

        if (Physics.Raycast(ray, out hit, interactDistance, interactableLayer))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                rayColor = Color.green;
            }

            Debug.DrawRay(ray.origin, ray.direction * hit.distance, rayColor);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * interactDistance, rayColor);
        }
    }

    void SetNormalCrosshair()
    {
        img_Crosshair.sprite = spr_NormalCrosshair;
        img_Crosshair.color = new Color(1f, 1f, 1f, 0.15f);
        img_Crosshair.transform.localScale = Vector3.one * 0.22f;
    }

    void SetInteractCrosshair()
    {
        img_Crosshair.sprite = srp_InteractCrosshair;
        img_Crosshair.color = new Color(1f, 1f, 1f, 0.5f);
        img_Crosshair.transform.localScale = Vector3.one * 0.5f;
    }
}
