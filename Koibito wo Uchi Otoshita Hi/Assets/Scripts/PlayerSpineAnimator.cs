using UnityEngine;
using Spine.Unity;
using Spine;

public class PlayerSpineAnimator : MonoBehaviour
{
    #region 配置参数
    [Header("Spine 动画设置")]
    public SkeletonAnimation skeletonAnim; // Spine 动画组件
    public string idleAnimName = "idle";   // 待机动画名称（默认朝左）
    public string walkAnimName = "walk";   // 移动动画名称（默认朝左）
    public float animationBlendTime = 0.2f;// 动画混合时间（过渡平滑度）
    #endregion

    private Player player;
    private Spine.AnimationState animState;      // Spine 动画状态机
    private float currentScaleX = 1f;      // 当前 X 轴缩放（1=左，-1=右，适配默认朝左）

    void Awake()
    {
        player = GetComponent<Player>();
        animState = skeletonAnim.AnimationState;
        animState.SetAnimation(0, idleAnimName, true); // 初始播放待机（朝左）
    }

    void Update()
    {
        // 1. 处理角色翻转（根据移动方向，适配默认朝左）
        FlipCharacterByDirection();

        // 2. 处理动画切换（待机/移动）
        UpdateAnimationState();
    }

    #region 核心逻辑：翻转与动画切换
    /// <summary>
    /// 根据移动方向翻转角色（适配初始朝左的动画）
    /// </summary>
    void FlipCharacterByDirection()
    {
        float moveDir = Mathf.Sign(player.velocity.x); // 移动方向（1=右，-1=左，0=静止）

        if (moveDir == 1) // 向右移动 → 需要翻转朝右（ScaleX=-1）
        {
            if (currentScaleX != -1f)
            {
                currentScaleX = -1f;
                skeletonAnim.Skeleton.ScaleX = currentScaleX;
            }
        }
        else if (moveDir == -1) // 向左移动 → 保持默认朝左（ScaleX=1）
        {
            if (currentScaleX != 1f)
            {
                currentScaleX = 1f;
                skeletonAnim.Skeleton.ScaleX = currentScaleX;
            }
        }
        // 静止时保持当前方向（不处理）
    }

    /// <summary>
    /// 根据移动速度切换待机/移动动画
    /// </summary>
    void UpdateAnimationState()
    {
        bool isMoving = Mathf.Abs(player.velocity.x) > 0.1f; // 移动阈值（避免抖动）
        string targetAnim = isMoving ? walkAnimName : idleAnimName;

        TrackEntry currentTrack = animState.GetCurrent(0);
        if (currentTrack == null || currentTrack.Animation.Name != targetAnim)
        {
            TrackEntry newTrack = animState.SetAnimation(0, targetAnim, true);
            newTrack.MixDuration = animationBlendTime; // 平滑过渡
        }
    }
    #endregion
}