using UnityEngine;
using System.Collections;
using System;

public class PositionRotationTransform
{
    public Vector3 position;
    public Vector3 rotation;

    public PositionRotationTransform AddTo(PositionRotationTransform transform)
    {
        PositionRotationTransform newTransform = new PositionRotationTransform();
        newTransform.position = position + transform.position;
        newTransform.rotation = rotation + transform.rotation;
        return newTransform;
    }
}
