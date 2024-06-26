using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


[CreateAssetMenu(menuName = "Camera Manager")]
public class CameraManagement : ScriptableObject
{
    [SerializeField]
    public enum Cameras { Indoor, Overhead, Window, KitchenWindow};
    public Cameras currentEnum;
    public bool changingCameras;
    public GameObject currentCamera;
}
