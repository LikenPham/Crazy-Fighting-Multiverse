using UnityEngine;

public class AnimationDamageEvent : MonoBehaviour
{
    [SerializeField] private Hitbox hitbox;

    private void Awake()
    {
        if (hitbox == null) hitbox = GetComponentInParent<Hitbox>();
    }

    public void HitboxOn(int index) => hitbox.HitboxOn(index);
    public void HitboxOff() => hitbox.HitboxOff();
}
