using UnityEngine;
using TMPro;

public class StoneCounter : MonoBehaviour
{
    public static int stone = 0;

    void Update()
    {
        GetComponent<TMP_Text>().SetText("Stone Collected: " + stone.ToString() + "/1");

        if (stone == 3)
        {
            GetComponent<TMP_Text>().SetText("Stone Collected!");
        }
    }

    public void AddStone() => stone++;
}
