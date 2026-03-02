using UnityEngine;

public class Vine : MonoBehaviour
{
    [SerializeField] private float growthSpeed = 0.5f;
    [SerializeField] private float maxHeight = 10f;
    [SerializeField] private LineRenderer lineRenderer;
    
    private float currentHeight = 0f;
    private bool isGrowing = false;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void Water()
    {
        isGrowing = true;
    }

    private void Update()
    {
        if (isGrowing && currentHeight < maxHeight)
        {
            currentHeight += growthSpeed * Time.deltaTime;
            currentHeight = Mathf.Min(currentHeight, maxHeight);
            
            UpdateVineVisuals();
            
            if (currentHeight >= maxHeight)
            {
                isGrowing = false;
            }
        }
    }

    private void UpdateVineVisuals()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + Vector3.up * currentHeight);
    }
}