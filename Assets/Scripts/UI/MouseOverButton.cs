using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    private Vector3 originalScale;
    public float scaleFactor = 1.2f;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    void Start()
    {
        if (originalScale == Vector3.zero)
        {
            originalScale = new Vector3(1f, 1f, 1f); 
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * scaleFactor;
        //FindObjectOfType<AudioManager>().Play("UIMove");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIMoveSound, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (originalScale == Vector3.zero) return;
        transform.localScale = originalScale * scaleFactor;
        //FindObjectOfType<AudioManager>().Play("UIMove");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIMoveSound, transform.position);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        transform.localScale = originalScale;
    }
}