using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] blockers = new GameObject[4]; // 0 = top, 1 = right, 2 = bottom, 3 = left
    bool connected;

    public void SetBlockers(bool[] status)
    {
        connected = false;
        for (int i = 0; i < status.Length; i++)
        {
            blockers[i].SetActive(!status[i]);
            if (status[i])
            {
                connected = true;
            }
        }

        if (!connected)
        {
            Destroy(gameObject);
        }
    }
}
