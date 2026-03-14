using UnityEngine;



[RequireComponent(typeof(SpriteRenderer))]
public class Seed : MonoBehaviour
{

    /// True after the seed has been watered and converted to a vine.
    public bool IsWatered { get; private set; }

    private SpriteRenderer _spriteRenderer;

    public Sprite seedSprite;

    public Sprite grownSprite;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        // ensure we start in the seed state even if a Vine component was left behind
        ResetToSeed();
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerRespawn += ResetToSeed;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerRespawn -= ResetToSeed;
    }

  
    /// Called by a watering system when the player waters this object.
   
    public void Water()
    {
        if (IsWatered)
            return;

        IsWatered = true;
        StartGrowing();
    }

    private void StartGrowing()
    {
        // add the vine component if it doesn't already exist
        if (GetComponent<Vine>() == null)
        {
            gameObject.AddComponent<Vine>();
        }

        // swap the sprite if one was provided
        if (_spriteRenderer != null && grownSprite != null)
        {
            _spriteRenderer.sprite = grownSprite;
        }
        else if (_spriteRenderer != null)
        {
            // optionally hide the seed graphic if nothing to show
            _spriteRenderer.enabled = false;
        }
    }


 
    public void ResetToSeed()
    {
        IsWatered = false;

        // destroy any vine behaviour
        Vine vine = GetComponent<Vine>();
        if (vine != null)
        {
            Destroy(vine);
        }

        // restore seed visuals
        if (_spriteRenderer != null)
        {
            if (seedSprite != null)
                _spriteRenderer.sprite = seedSprite;
            _spriteRenderer.enabled = true;
        }
    }
}
