using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Focus : MonoBehaviour
{
    [SerializeField] private float focusSpeed;
    private Volume _volume;
    public DepthOfField DOF;
    private RaycastHit hit;
    public LayerMask LayerMask;

    // Start is called before the first frame update
    void Start()
    {
        _volume = GetComponent<Volume>();
        DepthOfField tmp;
        if (_volume.profile.TryGet<DepthOfField>(out tmp))
        {
            DOF = tmp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(transform.position, transform.forward, out hit,Mathf.Infinity, LayerMask);
        print(hit.transform.name);
        
            DOF.focusDistance.value = Mathf.Lerp(DOF.focusDistance.value, hit.distance, Time.deltaTime * focusSpeed);
        
    }
}
