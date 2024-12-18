using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITween : MonoBehaviour
{

    [SerializeField] GameObject uiSetting;
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.scale(uiSetting, new Vector3(1, 1, 1), 1f).setEase(LeanTweenType.easeOutElastic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
