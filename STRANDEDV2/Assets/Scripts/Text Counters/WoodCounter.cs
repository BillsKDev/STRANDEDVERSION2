using UnityEngine;
using TMPro;

public class WoodCounter : MonoBehaviour
{
    public static int wood = 0;

    void Update()
    {
        GetComponent<TMP_Text>().SetText("Wood Collected: "+ wood.ToString() + "/2");

        if (wood == 3)
        {
            GetComponent<TMP_Text>().SetText("Wood Collected!");
        }
    }

    public void AddWood() => wood++;
}
