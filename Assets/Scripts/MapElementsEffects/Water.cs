using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Water : MonoBehaviour
{
    private Vector3 waveSource1 = new(2.0f, 0.0f, 2.0f);
    public float waveFrequency = 0.53f;
    public float waveHeight = 0.48f;

    public float waveLength = 0.71f;
    //public bool edgeBlend = true;
    //public bool forceFlatShading = true;

    private Mesh mesh;
    private Vector3[] originalVerts;
    private Vector3[] displacedVerts;

    private void Start()
    {
        GeneralSetup();
    }

    private void GeneralSetup()
    {
        if (Camera.main != null)
        {
            Camera.main.depthTextureMode |= DepthTextureMode.Depth;
        }

        MeshFilter mf = GetComponent<MeshFilter>();
        MakeMeshLowPoly(mf);
    }

    private void FixedUpdate()
    {
        CalcWave();
    }

    private void MakeMeshLowPoly(MeshFilter mf)
    {
        mesh = mf.sharedMesh;
        Vector3[] oldVerts = mesh.vertices;
        int[] triangles = mesh.triangles;

        Vector3[] newVerts = new Vector3[triangles.Length];
        for (int i = 0; i < triangles.Length; i++)
        {
            newVerts[i] = oldVerts[triangles[i]];
            triangles[i] = i;
        }

        mesh.vertices = newVerts;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        originalVerts = mesh.vertices;
        displacedVerts = new Vector3[originalVerts.Length];
    }


    private void CalcWave()
    {
        for (int i = 0; i < originalVerts.Length; i++)
        {
            Vector3 v = originalVerts[i];
            v.y = 0.0f;

            float dist = Vector3.Distance(v, waveSource1);
            dist = dist % waveLength / waveLength;
            v.y = waveHeight * Mathf.Sin((Time.time * Mathf.PI * 2.0f * waveFrequency) + (Mathf.PI * 2.0f * dist));

            displacedVerts[i] = v;
        }

        mesh.vertices = displacedVerts;
        mesh.RecalculateNormals();
        mesh.MarkDynamic();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}