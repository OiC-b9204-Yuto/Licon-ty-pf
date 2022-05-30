using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Licon.Damaged;

public class Arrow : MonoBehaviour
{
    private float m_Speed = 0.0f;
    private float m_Direction = 0;
    private float m_StartTime = 0.0f;
    private float m_LifeSpan = 0.0f;

    public void Initialize(float speed, float dir, float life)
    {
        m_Speed = speed;
        m_Direction = dir;
        m_LifeSpan = life;

        float rx;
        float rz;
        rx = 270;
        rz = -dir * Mathf.Rad2Deg + 90;
        transform.Rotate(rx, 0, rz);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_StartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - m_StartTime > m_LifeSpan)
        {
            Destroy(gameObject);
        }
        Vector3 pos = transform.position;
        pos.x = pos.x + Mathf.Cos(m_Direction) * m_Speed;
        pos.z = pos.z + Mathf.Sin(m_Direction) * m_Speed;
        transform.position = pos;
    }

    public void SetSpeed()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "GimmickCollider")
        {
            return;
        }

        if (collider.gameObject.tag == "Player")
        {
            var player = collider.gameObject.GetComponent<PlayerMove>();
            if (!player)
            {
                return;
            }
            player.UpdateDamaged();
        }
        Destroy(this.gameObject);
    }
}
