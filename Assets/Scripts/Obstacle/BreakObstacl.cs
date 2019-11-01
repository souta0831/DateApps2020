using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player_State;
public class BreakObstacl : MonoBehaviour
{
    [SerializeField] List<GameObject> _break_efect;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ListInstance();
            if (collision.gameObject.GetComponent<Player>().GetState().GetType() == typeof(PlayerStateSliding))
            {
                Destroy(this.gameObject);
            }
        }

    }
    private void ListInstance()
    {
        for (int i = 0; i < _break_efect.Count; i++)
        {
            if (_break_efect[i] != null)
            {
                Instantiate(_break_efect[i], transform.position, transform.rotation);
            }

        }
    }
}
