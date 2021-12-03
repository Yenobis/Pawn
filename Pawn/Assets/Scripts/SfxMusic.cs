using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class SfxMusic : MonoBehaviour
{
    private static SfxMusic _i;

    public static SfxMusic i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load<SfxMusic>("SfxMusic"));
            return _i;
        }
    }

    public AudioClip moverEspada;
}
