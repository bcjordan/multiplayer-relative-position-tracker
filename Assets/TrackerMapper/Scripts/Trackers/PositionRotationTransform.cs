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
