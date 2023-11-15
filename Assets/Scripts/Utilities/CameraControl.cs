using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using System.IO.Compression;
public class CameraControl : MonoBehaviour
{
    private CinemachineConfiner2D confiner2D;
    public CinemachineCollisionImpulseSource cameraImpulse;
    public VoidEventSo cameraShakeEvent;

    void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }

    //方法註冊與註銷
    void OnEnable()
    {
        cameraShakeEvent.OnEventRaised += OnCameraShakeEvent;
    }
    void OnDisable()
    {
        cameraShakeEvent.OnEventRaised -= OnCameraShakeEvent;
    }


    void Start()
    {
        GetNewCameraBound();
    }

    //需要執行的方法
    private void OnCameraShakeEvent()
    {
        cameraImpulse.GenerateImpulse();
    }

    private void GetNewCameraBound()
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null)
        {
            return;
        }
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        confiner2D.InvalidateCache();
    }
}
