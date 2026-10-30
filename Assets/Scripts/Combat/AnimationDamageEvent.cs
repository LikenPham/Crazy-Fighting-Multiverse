using UnityEngine;

public class AnimationDamageEvent : MonoBehaviour
{
    [SerializeField] private Hitbox hitbox;

    private void Awake()
    {
        if (hitbox == null) hitbox = GetComponentInParent<Hitbox>();
    }

    public void HitboxOn() => hitbox.HitboxOn();
    public void HitboxOff() => hitbox.HitboxOff();
}
