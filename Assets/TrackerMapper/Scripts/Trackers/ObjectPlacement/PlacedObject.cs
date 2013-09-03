using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlacedObject
{
    public PositionRotationTransform positionRotationTransform;
    public GameObject prefabObject;
    public GameObject instanceObject;

    public PlacedObject(PositionRotationTransform positionRotationTransform, GameObject prefabObject)
    {
        this.positionRotationTransform = positionRotationTransform;
        this.prefabObject = prefabObject;
    }
}

