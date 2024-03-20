using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSelet : MonoBehaviour
{

    private void Start()
    {
        onSeletEnd();
    }


    public void onSeletStart()
    {
        gameObject.SetActive(true);
    }

    public void onSeletEnd()
    {
        gameObject.SetActive(false);
    }


}
