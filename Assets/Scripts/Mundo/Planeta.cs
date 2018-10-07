using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planeta : MonoBehaviour
{
    [Range(2,256)]
    public int resolution = 10;
    public bool autoUpdate = true;
    public enum FaceRenderMask {All, Top, Bottom, Left, Right, Front, Back };
    public FaceRenderMask faceRenderMask;

    public ShapeSettings shapeSettings;
    public ColourSettings colourSettings;

    [HideInInspector]
    public bool shapeSettingsFoldout;
    [HideInInspector]
    public bool colourSettingsFoldout;

    ShapeGenerator geraShape = new ShapeGenerator();
    ColourGenerator colourGenerator = new ColourGenerator();

    [SerializeField, HideInInspector]
    public MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;
    
    public delegate void PlanetaAction();
    public static event PlanetaAction OnPlanetaPronto;

    void Start()
    {
        UIManager.OnClickTile += PaintTile;
    }
    void Update()
    {
        AtualizaPlaneta();
    }

    void OnDestroy()
    {
        UIManager.OnClickTile -= PaintTile;
    }

    void Initialize()
    {
        geraShape.UpdateSettings(shapeSettings);
        colourGenerator.UpdateSettings(colourSettings);

        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.right, Vector3.left, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh " + i);
                meshObj.transform.parent = transform;
                
                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
                meshObj.AddComponent<MeshCollider>();
                meshObj.layer = 20;
            }
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colourSettings.planetMaterial;

            terrainFaces[i] = new TerrainFace(geraShape, meshFilters[i].sharedMesh, resolution, directions[i]);
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask -1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);
        }
    }

    public void GeraPlaneta()
    {
        Initialize();
        GeraMesh();
        GeraCores();
        if(OnPlanetaPronto != null)
            OnPlanetaPronto();

    }

    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GeraMesh();
        }
    }

    public void OnColourSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GeraCores();
        }
    }

    void GeraMesh()
    {
        /* foreach(TerrainFace face in terrainFaces)
        {
            face.ConstroiMesh();
        } */
        for(int i = 0; i < 6; i++)
        {
            if(meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].ConstroiMesh();
            }
        }

        colourGenerator.UpdateElevation(geraShape.elevationMinMax);
    }

    void GeraCores()
    {

        colourGenerator.UpdateColours();
    }

    //
    public TerrainFace[] GetTerrainFaces()
    {
        return terrainFaces;
    }

    public void AtualizaPlaneta()
    {
        Initialize();
        GeraCores();
    }

    void DeleteTri(Transform trans, int index)
    {
        Destroy(trans.gameObject.GetComponent<MeshCollider>());
        Mesh mesh = trans.GetComponent<MeshFilter>().mesh;
        int[] oldTriangles = mesh.triangles;
        int[] newTriangles = new int[mesh.triangles.Length - 3];

        int i = 0;
        int j = 0;
        while (j < mesh.triangles.Length)
        {
            if (j != index * 3)
            {
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
            }
            else
            {
                j += 3;
            }
        }
        trans.gameObject.GetComponent<MeshFilter>().mesh.triangles = newTriangles;
        trans.gameObject.AddComponent<MeshCollider>();
    }

    void PaintTile(Transform trans, int index)
    {
        Mesh mesh = trans.GetComponent<MeshFilter>().mesh;
 

        Debug.Log(mesh.colors.Length);

        mesh.colors[index * 3] = Color.blue;
        mesh.colors[index * 3 + 1] = Color.blue;
        mesh.colors[index * 3 + 2] = Color.blue;

    }

}
