﻿// Copyright (C) 2016 Singapore ETH Centre, Future Cities Laboratory
// All rights reserved.
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//
// Author:  Michael Joos  (joos@arch.ethz.ch)
// Summary: base class for all traffic elements (pedestrians, cyclists, vehicles, etc.)

using UnityEngine;

public class SimulationElement : MonoBehaviour
{
    [System.Serializable]
    public class MaterialReplacement
    {
        public Renderer mesh = null;
        public bool includeCurrentMeshMaterial = true;
        public Material[] materials = new Material[0];
    }

    [HideInInspector] public Vector3 targetPosition;
    [HideInInspector] public Quaternion targetRotation;
    [HideInInspector] public int fromKeyframe;
    [HideInInspector] public int toKeyframe;

    [Header("Customization")]
    public MaterialReplacement[] materialReplacements = new MaterialReplacement[0];

    protected static readonly System.Random random = new System.Random(13);

    protected float targetSpeed = 0;
    protected float previousSpeed = 0;

    public float TargetSpeed
    {
        get { return targetSpeed; }
    }

    void Awake()
    {
        if (materialReplacements.Length > 0)
        {
            foreach (var replacement in materialReplacements)
            {
                if (replacement.mesh != null && replacement.materials.Length > 0)
                {
                    int index = random.Next(replacement.includeCurrentMeshMaterial ? -1 : 0, replacement.materials.Length);
                    if (index >= 0)
                    {
                        replacement.mesh.material = replacement.materials[index];
                    }
                }
            }
        }
    }

    public virtual void SetSpeed(float speed)
    {
        previousSpeed = targetSpeed;
        targetSpeed = speed;
    }

    public void MoveToTarget()
    {
        UpdateMovement();
        TeleportToTarget();
    }

    public void TeleportToTarget()
    {
        transform.localPosition = targetPosition;
        transform.localRotation = targetRotation;
    }

    protected virtual void UpdateMovement() { }

    public virtual void SoundSignal() { }

}
