using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    [SerializeField]
    private GameObject image;
    
    void Awake()
    {
        if (instance)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }

    public void Wasted()
    {
        image.SetActive(true);

        Time.timeScale = 0;
    }
}
