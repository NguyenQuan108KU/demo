using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskPosition : MonoBehaviour
{
    public static DiskPosition instance;
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(instance);
    }
}
