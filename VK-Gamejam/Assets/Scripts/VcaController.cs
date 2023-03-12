using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class VcaController : MonoBehaviour
{
    private FMOD.Studio.VCA _vcaController;
    public string VcaName;

    private void Awake()
    {
        _vcaController = FMODUnity.RuntimeManager.GetVCA("vca:/" + VcaName);
    }
}
