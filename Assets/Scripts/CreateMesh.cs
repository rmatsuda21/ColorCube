using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMesh : MonoBehaviour
{
    private Mesh mesh;
    public Color[] cols;

    private int[] playerCols = null;

    public void CreateCube(int[] playerCols, int playerX, int playerY, int boardWidth, int boardHeight, float size, bool title = false)
    {
        this.playerCols = playerCols;

        if(cols != null)
            this.cols = Manager.instance.cols;
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf == null) return;

        mesh = MakeCube(1.0f);
        mf.mesh = mesh;

        MeshCollider mc = GetComponent<MeshCollider>();
        if (mc != null)
            mc.sharedMesh = mesh;

        if (!title)
            transform.localPosition = new Vector3((playerX - boardWidth / 2) * size, 0, (boardWidth / 2 - playerY) * size);
    }

    public void ChangeColor(int iTriangle)
    {
        Color colorT = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        Debug.Log(colorT);
        Color[] colors = mesh.colors;
        int iStart = mesh.triangles[iTriangle * 3];
        for (int i = iStart; i < iStart + 4; i++)
            colors[i] = colorT;
        mesh.colors = colors;
    }

    Mesh MakeCube(float fSize)
    {
        float fHS = fSize / 2.0f;
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[] {
             new Vector3(-fHS,  fHS, -fHS), new Vector3( fHS,  fHS, -fHS), new Vector3( fHS, -fHS, -fHS), new Vector3(-fHS, -fHS, -fHS),  // Front
             new Vector3( fHS,  fHS, -fHS), new Vector3( fHS,  fHS,  fHS), new Vector3( fHS, -fHS,  fHS), new Vector3( fHS, -fHS, -fHS),  // Right
             new Vector3(-fHS, -fHS,  fHS), new Vector3( fHS, -fHS,  fHS), new Vector3( fHS,  fHS,  fHS), new Vector3(-fHS,  fHS,  fHS),  // Back
             new Vector3(-fHS,  fHS,  fHS), new Vector3(-fHS,  fHS, -fHS), new Vector3(-fHS, -fHS, -fHS), new Vector3(-fHS, -fHS,  fHS),  // Left
             new Vector3(-fHS,  fHS,  fHS), new Vector3( fHS,  fHS,  fHS), new Vector3( fHS,  fHS, -fHS), new Vector3(-fHS,  fHS, -fHS),  // Top 
             new Vector3(-fHS, -fHS, -fHS), new Vector3( fHS, -fHS, -fHS), new Vector3( fHS, -fHS,  fHS), new Vector3(-fHS, -fHS,  fHS)}; // Bottom

        int[] triangles = new int[mesh.vertices.Length / 4 * 2 * 3];
        int iPos = 0;
        for (int i = 0; i < mesh.vertices.Length; i = i + 4)
        {
            triangles[iPos++] = i;
            triangles[iPos++] = i + 1;
            triangles[iPos++] = i + 2;
            triangles[iPos++] = i;
            triangles[iPos++] = i + 2;
            triangles[iPos++] = i + 3;
        }

        mesh.triangles = triangles;
        Color colorT = Color.red;
        Color[] colors = new Color[mesh.vertices.Length];
        int colIndex = 0;
        for (int i = 0; i < colors.Length; i++)
        {
            if ((i % 4) == 0)
                colorT = Manager.instance.getColor(playerCols[colIndex++]);
            colors[i] = colorT;
        }

        mesh.colors = colors;
        mesh.RecalculateNormals();
        return mesh;
    }

    public void setColors()
    {
        MeshFilter mf = GetComponent<MeshFilter>();

        Color colorT = Color.red;
        Color[] colors = new Color[mesh.vertices.Length];
        int colIndex = 0;
        for (int i = 0; i < colors.Length; i++)
        {
            if ((i % 4) == 0)
                colorT = Manager.instance.getColor(playerCols[colIndex++]);
            colors[i] = colorT;
        }

        mf.mesh.colors = colors;
    }
}
