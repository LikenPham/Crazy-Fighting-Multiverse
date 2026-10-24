using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "DataConfig")]
public class AttackDataSO : ScriptableObject
{
    [SerializeField] private string nameAttack;
    [SerializeField] private AnimKey animationKey = AnimKey.NormalAttack;
    [SerializeField, Min(0)] private int damage = 10;
    [SerializeField] private Vector2 hitboxOffset = new Vector2(1f,1f);
    [SerializeField] private Vector2 hitboxSize = new Vector2(1f,1f);

    public string AttackName => nameAttack;
    public AnimKey AnimationKey => animationKey;
    public int Damage => damage;
    public Vector2 HitboxOffset => hitboxOffset;
    public Vector2 HitboxSize => hitboxSize;
}
