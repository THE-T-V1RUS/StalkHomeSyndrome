using UnityEngine;

public class TitleRevealController : MonoBehaviour
{
    [SerializeField] GameObject TitleRevealCutscene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TitleRevealCutscene.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
