using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMeshCollider : MonoBehaviour
{
    // Instance variables
    private WeightList[]   nodeWeights;    // array of node weights (one per node)
    private Vector3[]       newVert;        // array for the regular update of the collision mesh
   
    private Mesh            mesh;       // the dynamically-updated collision mesh
    private MeshCollider    collide;    // quick pointer to the mesh collider that we're updating

    private bool updating;
    private Coroutine updateFunc;
    public bool UpdateOnAwake;
    public float updateTime;
    private WaitForSeconds updateWaitTime;
   
    void Start()
    {
        SkinnedMeshRenderer rend = GetComponent(typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer;
        collide = GetComponent(typeof(MeshCollider)) as MeshCollider;
       
        if (collide!=null  && rend!=null)
        {
            Mesh baseMesh = rend.sharedMesh;
            mesh = new Mesh();
            mesh.vertices = baseMesh.vertices;
            mesh.uv = baseMesh.uv;
            mesh.triangles = baseMesh.triangles;
            newVert = new Vector3[baseMesh.vertices.Length];
           
            short i;
            // Make a CWeightList for each bone in the skinned mesh        
            nodeWeights = new WeightList[rend.bones.Length];
            for ( i=0 ; i<rend.bones.Length ; i++ )
            {
                nodeWeights[i] = new WeightList();
                nodeWeights[i].transform = rend.bones[i];
            }
 
            // Create a bone weight list for each bone, ready for quick calculation during an update...
            Vector3 localPt;
            for ( i=0 ; i<baseMesh.vertices.Length ; i++ )
            {
                BoneWeight bw = baseMesh.boneWeights[i];
                if (bw.weight0!=0.0f)
                {
                    localPt = baseMesh.bindposes[bw.boneIndex0].MultiplyPoint3x4( baseMesh.vertices[i] );
                    nodeWeights[bw.boneIndex0].weights.Add( new VertexWeight( i, localPt, bw.weight0 ) );
                }
                if (bw.weight1!=0.0f)
                {
                    localPt = baseMesh.bindposes[bw.boneIndex1].MultiplyPoint3x4( baseMesh.vertices[i] );
                    nodeWeights[bw.boneIndex1].weights.Add( new VertexWeight( i, localPt, bw.weight1 ) );
                }
                if (bw.weight2!=0.0f)
                {
                    localPt = baseMesh.bindposes[bw.boneIndex2].MultiplyPoint3x4( baseMesh.vertices[i] );
                    nodeWeights[bw.boneIndex2].weights.Add( new VertexWeight( i, localPt, bw.weight2 ) );
                }
                if (bw.weight3!=0.0f)
                {
                    localPt = baseMesh.bindposes[bw.boneIndex3].MultiplyPoint3x4( baseMesh.vertices[i] );
                    nodeWeights[bw.boneIndex3].weights.Add( new VertexWeight( i, localPt, bw.weight3 ) );
                }
            }

            updating = false;
            updateWaitTime = new WaitForSeconds(updateTime);
            UpdateCollisionMesh();
            if(UpdateOnAwake)
                StartUpdate();
        }
        
 
    }

    public void StartUpdate()
    {
        if (!updating)
        {
            updating = true;
            updateFunc = StartCoroutine(Updating());
        }
    }

    public void StopUpdate()
    {
        if(updateFunc!= null)
            StopCoroutine(updateFunc);
        updating = false;
    }

    private IEnumerator Updating()
    {
        while (updating)
        {
            UpdateCollisionMesh();
            yield return updateWaitTime;
        }
    }

    void UpdateCollisionMesh()
    {

        if (mesh != null)
        {
            // Start by initializing all vertices to 'empty'
            for (int i = 0; i < newVert.Length; i++)
            {
                newVert[i] = new Vector3(0, 0, 0);
            }

            // Now get the local positions of all weighted indices...
            foreach (WeightList wList in nodeWeights)
            {
                foreach (VertexWeight vw in wList.weights)
                {
                    newVert[vw.index] += wList.transform.localToWorldMatrix.MultiplyPoint3x4(vw.localPosition) *
                                         vw.weight;
                }
            }

            // Now convert each point into local coordinates of this object.
            for (int i = 0; i < newVert.Length; i++)
            {
                newVert[i] = transform.InverseTransformPoint(newVert[i]);
            }

            // Update the mesh ( collider) with the updated vertices
            mesh.vertices = newVert;
            mesh.RecalculateBounds();
            collide.sharedMesh = mesh;
        }
    }
}
