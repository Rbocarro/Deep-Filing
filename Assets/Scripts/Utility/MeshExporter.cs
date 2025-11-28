using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using System.Globalization;
public class MeshExporter
{
    [MenuItem("Tools/Mesh/Export/OBJ")]
    static void ExportSelectedToOBJ()
    {
        GameObject[] selection = Selection.gameObjects;

        if (selection.Length == 0)
        {
            Debug.LogWarning("No GameObjects selected.");
            return;
        }

        string path = EditorUtility.SaveFilePanel(
            "Export OBJ",
            "",
            "ExportedSelection.obj",
            "obj");

        if (string.IsNullOrEmpty(path))
            return;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("# Exported from Unity");

        int vertexOffset = 0;

        foreach (GameObject go in selection)
        {
            MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>();

            foreach (MeshFilter mf in meshFilters)
            {
                Mesh mesh = mf.sharedMesh;
                if (!mesh) continue;

                Transform t = mf.transform;

                sb.AppendLine("g " + mf.name);

                //FIX MIRRORING from unity to OBJ handedness
                Matrix4x4 m = t.localToWorldMatrix;
                m = FlipHandedness(m); 


                // Write vertices
                foreach (Vector3 v in mesh.vertices)
                {
                    Vector3 wv = m.MultiplyPoint3x4(v);
                    sb.AppendFormat(CultureInfo.InvariantCulture,
                        "v {0} {1} {2}\n", wv.x, wv.y, wv.z);
                }

                // Write normals
                foreach (Vector3 n in mesh.normals)
                {
                    Vector3 wn = m.MultiplyVector(n).normalized;
                    sb.AppendFormat(CultureInfo.InvariantCulture,
                        "vn {0} {1} {2}\n", wn.x, wn.y, wn.z);
                }

                // Write UVs
                foreach (Vector2 uv in mesh.uv)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture,
                        "vt {0} {1}\n", uv.x, uv.y);
                }

                // Write faces (reverse winding order for right-handed)
                int[] tris = mesh.triangles;
                for (int i = 0; i < tris.Length; i += 3)
                {
                    int a = tris[i + 0] + 1 + vertexOffset;
                    int b = tris[i + 2] + 1 + vertexOffset; // swapped
                    int c = tris[i + 1] + 1 + vertexOffset; // swapped

                    sb.AppendLine($"f {a}/{a}/{a} {b}/{b}/{b} {c}/{c}/{c}");
                }

                vertexOffset += mesh.vertices.Length;
            }
        }

        File.WriteAllText(path, sb.ToString());
        Debug.Log("Exported OBJ to: " + path);
    }
    static Matrix4x4 FlipHandedness(Matrix4x4 m)
    {
        Matrix4x4 flip = Matrix4x4.Scale(new Vector3(1, 1, -1));
        return flip * m;
    }
}
