using UnityEngine;

public class PlayerBowAim : MonoBehaviour
{
    #region 配置参数
    [Header("弓与准星配置")]
    public Transform bowTransform;      // 弓的Transform（子对象，初始隐藏）
    public Transform arrowIndicator;    // 准星箭矢的Transform（子对象，初始隐藏）
    public float rotationOffset = 90f;  // 旋转偏移（调整弓的指向，根据美术需求）
    #endregion

    private Player player;
    private Vector3 mouseWorldPos;      // 鼠标世界坐标

    void Awake()
    {
        player = GetComponent<Player>();
        // 初始隐藏弓和准星
        bowTransform.gameObject.SetActive(false);
        arrowIndicator.gameObject.SetActive(false);
    }

    void Update()
    {
        if (player.isCharging)
        {
            // 1. 显示弓和准星（若未激活）
            if (!bowTransform.gameObject.activeSelf)
            {
                bowTransform.gameObject.SetActive(true);
                arrowIndicator.gameObject.SetActive(true);
            }

            // 2. 计算鼠标方向并旋转
            RotateToMouse();
        }
        else
        {
            // 3. 隐藏弓和准星（若已激活）
            if (bowTransform.gameObject.activeSelf)
            {
                bowTransform.gameObject.SetActive(false);
                arrowIndicator.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 旋转弓和准星到鼠标方向
    /// </summary>
    void RotateToMouse()
    {
        // 获取鼠标世界坐标（2D场景，Z轴置0）
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        // 计算方向向量（角色 → 鼠标）
        Vector3 direction = mouseWorldPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += rotationOffset;  // 调整偏移，使弓正确指向鼠标（如美术设计弓初始朝右）

        // 旋转弓和准星
        bowTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        arrowIndicator.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}