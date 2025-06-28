using UnityEngine;
using Spine.Unity;
using Spine;

public class PlayerSpineAnimator : MonoBehaviour
{
    #region ���ò���
    [Header("Spine ��������")]
    public SkeletonAnimation skeletonAnim; // Spine �������
    public string idleAnimName = "idle";   // �����������ƣ�Ĭ�ϳ���
    public string walkAnimName = "walk";   // �ƶ��������ƣ�Ĭ�ϳ���
    public float animationBlendTime = 0.2f;// �������ʱ�䣨����ƽ���ȣ�
    #endregion

    private Player player;
    private Spine.AnimationState animState;      // Spine ����״̬��
    private float currentScaleX = 1f;      // ��ǰ X �����ţ�1=��-1=�ң�����Ĭ�ϳ���

    void Awake()
    {
        player = GetComponent<Player>();
        animState = skeletonAnim.AnimationState;
        animState.SetAnimation(0, idleAnimName, true); // ��ʼ���Ŵ���������
    }

    void Update()
    {
        // 1. �����ɫ��ת�������ƶ���������Ĭ�ϳ���
        FlipCharacterByDirection();

        // 2. �������л�������/�ƶ���
        UpdateAnimationState();
    }

    #region �����߼�����ת�붯���л�
    /// <summary>
    /// �����ƶ�����ת��ɫ�������ʼ����Ķ�����
    /// </summary>
    void FlipCharacterByDirection()
    {
        float moveDir = Mathf.Sign(player.velocity.x); // �ƶ�����1=�ң�-1=��0=��ֹ��

        if (moveDir == 1) // �����ƶ� �� ��Ҫ��ת���ң�ScaleX=-1��
        {
            if (currentScaleX != -1f)
            {
                currentScaleX = -1f;
                skeletonAnim.Skeleton.ScaleX = currentScaleX;
            }
        }
        else if (moveDir == -1) // �����ƶ� �� ����Ĭ�ϳ���ScaleX=1��
        {
            if (currentScaleX != 1f)
            {
                currentScaleX = 1f;
                skeletonAnim.Skeleton.ScaleX = currentScaleX;
            }
        }
        // ��ֹʱ���ֵ�ǰ���򣨲�����
    }

    /// <summary>
    /// �����ƶ��ٶ��л�����/�ƶ�����
    /// </summary>
    void UpdateAnimationState()
    {
        bool isMoving = Mathf.Abs(player.velocity.x) > 0.1f; // �ƶ���ֵ�����ⶶ����
        string targetAnim = isMoving ? walkAnimName : idleAnimName;

        TrackEntry currentTrack = animState.GetCurrent(0);
        if (currentTrack == null || currentTrack.Animation.Name != targetAnim)
        {
            TrackEntry newTrack = animState.SetAnimation(0, targetAnim, true);
            newTrack.MixDuration = animationBlendTime; // ƽ������
        }
    }
    #endregion
}