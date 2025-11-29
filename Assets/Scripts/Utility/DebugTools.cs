using UnityEngine;
using System.Collections.Generic;
public class DebugTools : MonoBehaviour
{
    [Header("Hotkeys")]
    public KeyCode toggleWireframeKey = KeyCode.F1;
    public KeyCode toggleCollidersKey = KeyCode.F2;
    public KeyCode toggleFPSKey = KeyCode.F3;

    private bool wireframeEnabled = false;
    private bool colliderDebugEnabled = false;
    private bool showFPS = false;

    private float fps;
    private float fpsTimer;

    void Update()
    {
        // Wireframe toggle
        if (Input.GetKeyDown(toggleWireframeKey))
            wireframeEnabled = !wireframeEnabled;

        // Collider gizmo toggle
        if (Input.GetKeyDown(toggleCollidersKey))
            colliderDebugEnabled = !colliderDebugEnabled;

        // FPS toggle
        if (Input.GetKeyDown(toggleFPSKey))
            showFPS = !showFPS;

        // FPS counter
        if (showFPS)
        {
            fpsTimer += Time.deltaTime;
            if (fpsTimer >= 0.5f)
            {
                fps = 1f / Time.deltaTime;
                fpsTimer = 0f;
            }
        }
    }

    // Wireframe render hook
    void OnPreRender()
    {
        if (wireframeEnabled)
            GL.wireframe = true;
    }

    void OnPostRender()
    {
        if (wireframeEnabled)
            GL.wireframe = false;
    }

    // Collider wireframes
    void OnDrawGizmos()
    {
        if (!colliderDebugEnabled) return;

        Gizmos.color = Color.green;

        foreach (var col in FindObjectsByType<Collider>(sortMode:default))
        {
            if (col is BoxCollider b)
                Gizmos.DrawWireCube(b.bounds.center, b.bounds.size);

            else if (col is SphereCollider s)
                Gizmos.DrawWireSphere(s.bounds.center, s.radius);

            else if (col is CapsuleCollider c)
            {
                // Simple capsule representation
                Gizmos.DrawWireSphere(c.bounds.center + Vector3.up * (c.height / 2 - c.radius), c.radius);
                Gizmos.DrawWireSphere(c.bounds.center - Vector3.up * (c.height / 2 - c.radius), c.radius);
                Gizmos.DrawWireCube(c.bounds.center, new Vector3(c.radius * 2, c.height - c.radius * 2, c.radius * 2));
            }
        }
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 18;
        style.normal.textColor = Color.white;

        GUILayout.BeginVertical("box");

        GUILayout.Label("Debug Tools Active", style);

        GUILayout.Label($"[F1] Wireframe: {(wireframeEnabled ? "ON" : "OFF")}");
        GUILayout.Label($"[F2] Collider Gizmos: {(colliderDebugEnabled ? "ON" : "OFF")}");
        GUILayout.Label($"[F3] FPS Counter: {(showFPS ? "ON" : "OFF")}");

        if (showFPS)
            GUILayout.Label($"FPS: {fps:0.0}");

        GUILayout.EndVertical();
    }
}
