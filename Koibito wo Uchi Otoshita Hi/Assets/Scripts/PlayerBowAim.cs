using UnityEngine;

public class PlayerBowAim : MonoBehaviour
{
    #region ���ò���
    [Header("����׼������")]
    public Transform bowTransform;      // ����Transform���Ӷ��󣬳�ʼ���أ�
    public Transform arrowIndicator;    // ׼�Ǽ�ʸ��Transform���Ӷ��󣬳�ʼ���أ�
    public float rotationOffset = 90f;  // ��תƫ�ƣ���������ָ�򣬸�����������
    #endregion

    private Player player;
    private Vector3 mouseWorldPos;      // �����������

    void Awake()
    {
        player = GetComponent<Player>();
        // ��ʼ���ع���׼��
        bowTransform.gameObject.SetActive(false);
        arrowIndicator.gameObject.SetActive(false);
    }

    void Update()
    {
        if (player.isCharging)
        {
            // 1. ��ʾ����׼�ǣ���δ���
            if (!bowTransform.gameObject.activeSelf)
            {
                bowTransform.gameObject.SetActive(true);
                arrowIndicator.gameObject.SetActive(true);
            }

            // 2. ������귽����ת
            RotateToMouse();
        }
        else
        {
            // 3. ���ع���׼�ǣ����Ѽ��
            if (bowTransform.gameObject.activeSelf)
            {
                bowTransform.gameObject.SetActive(false);
                arrowIndicator.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// ��ת����׼�ǵ���귽��
    /// </summary>
    void RotateToMouse()
    {
        // ��ȡ����������꣨2D������Z����0��
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        // ���㷽����������ɫ �� ��꣩
        Vector3 direction = mouseWorldPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += rotationOffset;  // ����ƫ�ƣ�ʹ����ȷָ����꣨��������ƹ���ʼ���ң�

        // ��ת����׼��
        bowTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        arrowIndicator.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}