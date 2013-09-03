using UnityEngine;
using System.Collections;
using System;

public class PositionRotationTransform
{
    public Vector3 position;
    public Vector3 rotation;

    public PositionRotationTransform()
    {
    }
    
    public static PositionRotationTransform FromTo(GameObject fromObject, GameObject toObject)
    {
        PositionRotationTransform relativePositionRotationTransform = new PositionRotationTransform();
        relativePositionRotationTransform.position = toObject.transform.position - fromObject.transform.position;
        relativePositionRotationTransform.rotation = toObject.transform.rotation.eulerAngles - fromObject.transform.rotation.eulerAngles;
        return relativePositionRotationTransform;
    }

    public PositionRotationTransform(Transform transform)
    {
        position = transform.position;
        rotation = transform.rotation.eulerAngles;
    }

    public PositionRotationTransform(Vector3 position, Vector3 rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }

    public PositionRotationTransform AddTo(PositionRotationTransform transform)
    {
        PositionRotationTransform newTransform = new PositionRotationTransform();
        newTransform.position = position + transform.position;
        newTransform.rotation = rotation + transform.rotation;
        return newTransform;
    }
}
