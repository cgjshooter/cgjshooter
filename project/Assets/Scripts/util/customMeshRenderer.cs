using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customMeshRenderer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void createObstacleMesh(MeshFilter meshFilter, float height)
    {
        var mesh = meshFilter.mesh;
        int numberOfVertices = 8;

        Vector3[] vertices = new Vector3[numberOfVertices]; //8
        Vector3[] verticesDistances = new Vector3[numberOfVertices/2-1]; //3

        float width = UnityEngine.Random.value + 2f;

        for(int i=0; i<verticesDistances.Length; ++i)
        {
            verticesDistances[i] = new Vector3(UnityEngine.Random.value, UnityEngine.Random.value, 0);
        }
        vertices[0] = verticesDistances[0];
        vertices[1] = vertices[0] + verticesDistances[1].normalized * width;
        vertices[2] = vertices[0] + verticesDistances[2].normalized * width;
        vertices[3] = vertices[1] + verticesDistances[2].normalized * width;

        for(int i = vertices.Length / 2; i<vertices.Length; ++i)
        {
            vertices[i] = vertices[i-numberOfVertices/2] + new Vector3(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value + height);
        }
        
        mesh.vertices = vertices;
        int numberOfTriangles = (2 * numberOfVertices - 4);
        int[] tri = new int[3 * numberOfTriangles];

        for(int i = 0; i < tri.Length; i += 3)
        {
            tri[i] = 0;
            tri[i + 1] = 0;
            tri[i + 2] = 0;
        }

        mesh.triangles = tri;
    }
}
