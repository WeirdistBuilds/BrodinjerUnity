﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaling_VFX : MonoBehaviour
{
    public ParticleSystem Indicator, Highlighter;
    public GameObject ObjectToHighlight;
    public float StartingSizeIndicatorMin, StartingSizeIndicatorMax, StartingSizeHighlightMin, StartingSizeHighlightMax;
    public bool UseMesh;

    private MeshFilter filter;
    private MeshRenderer Rend;
    private ParticleSystem.ShapeModule indicatorShape, highlightShape;
    private ParticleSystem.MainModule indicatorMain, highlightMain;

    private void Start()
    {
        indicatorShape = Indicator.shape;
        indicatorShape.enabled = true;
        indicatorMain = Indicator.main;
        highlightMain = Highlighter.main;
        highlightShape = Highlighter.shape;
        highlightShape.enabled = true;
        if (UseMesh)
        {
            if (ObjectToHighlight != null && (filter = ObjectToHighlight.GetComponent<MeshFilter>()) != null)
            {
                indicatorShape.shapeType = ParticleSystemShapeType.Mesh;
                indicatorShape.mesh = filter.mesh;
                highlightShape.shapeType = ParticleSystemShapeType.Mesh;
                highlightShape.mesh = filter.mesh;
            }
        }
        else
        {
            if (ObjectToHighlight != null && (Rend = ObjectToHighlight.GetComponent<MeshRenderer>()) != null)
            {
                indicatorShape.shapeType = ParticleSystemShapeType.MeshRenderer;
                indicatorShape.meshRenderer = Rend;
                highlightShape.shapeType = ParticleSystemShapeType.MeshRenderer;
                highlightShape.meshRenderer = Rend;
            }
        }
        Indicator.Play();
    }

    public void Highlight()
    {
        Debug.Log("Highlight");
        //Indicator.Stop();
        Highlighter.Play();
    }

    public void UnHighlight()
    {
        Debug.Log("UnHighlight");
        //Indicator.Play();
        Highlighter.Stop();
    }

    public void Scale(float current)
    {
        indicatorMain.startSize = GeneralFunctions.ConvertRange(0, 1, StartingSizeIndicatorMin,
            StartingSizeIndicatorMax, current);
        highlightMain.startSizeX = GeneralFunctions.ConvertRange(0, 1, StartingSizeHighlightMin,
            StartingSizeHighlightMax, current);
    }
}
