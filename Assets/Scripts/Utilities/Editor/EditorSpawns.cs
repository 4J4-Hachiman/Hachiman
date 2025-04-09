using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/*
    Custom Editor pour gérer les placements des points
    de patrouille et des spawns des ennemis.
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 26/03/2025;
*/

public class EditorToolSpawns : EditorWindow
{
    /* ======================= VARIABLES ======================= */
    private GameObject terrain;
    private GameObject spawnMain;
    private List<GameObject> spawnGroups;
    private Vector3[] pointsEdit = new Vector3[0];
    private int selectedGroup;

    [MenuItem("Window/Custom Editor/Spawn", priority = 0)]
    public static void ShowWindow()
    {
        EditorToolSpawns window = GetWindow(typeof(EditorToolSpawns)) as EditorToolSpawns;
        window.Show();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        spawnMain = GameObject.FindWithTag("SpawnData");
        if (!spawnMain)
        {
            spawnMain = new("SpawnData") { tag = "SpawnData", isStatic = true };
        }
        spawnMain = GameObject.FindWithTag("SpawnData");
        terrain = GameObject.FindWithTag("Terrain");
        spawnGroups = new List<GameObject>();
        for (int i = 0; i < spawnMain.transform.childCount; i++)
        {
            spawnGroups.Add(spawnMain.transform.GetChild(i).gameObject);
        }
        selectedGroup = -1;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnGUI()
    {
        GUILayout.Label("Terrain Point Tool", EditorStyles.boldLabel, GUILayout.Height(20));
        terrain = EditorGUILayout.ObjectField("Terrain", terrain, typeof(GameObject), true) as GameObject;
        spawnMain = EditorGUILayout.ObjectField("Level SpawnData", spawnMain, typeof(GameObject), true) as GameObject;
        GUILayout.Label("Options", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Creer un groupe"))
        {
            GameObject newGroup = new($"Groupe.{spawnGroups.Count:D3}", typeof(SpawnGroup)) { isStatic = true };
            newGroup.transform.parent = spawnMain.transform;
            spawnGroups.Add(newGroup);
            for (int i = 0; i < spawnGroups.Count; i++)
            {
                spawnGroups[i].name = $"Groupe.{i:D3}";
            }
            Repaint();
        }

        if (GUILayout.Button("Ajouter route au groupe"))
        {
            if (selectedGroup <= -1)
            {
                Debug.LogWarning("Selectionner un groupe a qui assigner les points");
            }
            spawnGroups[selectedGroup].GetComponent<SpawnGroup>().AddRoute(pointsEdit);
            pointsEdit = new Vector3[0];
            Repaint();
        }

        if (GUILayout.Button("Effacer groupe"))
        {
            if (selectedGroup <= -1)
            {
                Debug.LogWarning("No group selected");
            }

            DestroyImmediate(spawnGroups[selectedGroup]);
            spawnGroups.RemoveAt(selectedGroup);
            selectedGroup = -1;
            for (int i = 0; i < spawnGroups.Count; i++)
            {
                spawnGroups[i].name = $"Groupe.{i:D3}";
            }
            Repaint();
        }

        if (GUILayout.Button("Effacer les points"))
        {
            pointsEdit = new Vector3[0];
        }
        
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("Groups", EditorStyles.boldLabel);
        for (int i = 0; i < spawnGroups.Count; i++)
        {
            string btnText = $"Groupe {i + 1:D3}";
            if (selectedGroup == i)
            {
                btnText += " ===> SELECTED <===";
                GUI.color = Color.green;
            }

            if (GUILayout.Button(btnText))
            {
                Selection.activeGameObject = spawnGroups[i];
                selectedGroup = i;
            }
            GUI.color = Color.white;
        }

        EditorGUILayout.LabelField("Points: ", EditorStyles.boldLabel);

        for (int i = 0; i < pointsEdit.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Vector3Field($"Position {i}", pointsEdit[i]);
            EditorGUILayout.EndHorizontal();
        }
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (terrain != null && hit.collider.gameObject.CompareTag("Terrain"))
                {
                    ArrayUtility.Add(ref pointsEdit, hit.point);
                    Repaint();
                }
            }
        }

        for (int i = 0; i < pointsEdit.Length - 1; i++)
        {
            Handles.color = Color.red;
            Handles.DrawLine(pointsEdit[i], pointsEdit[i + 1], 2f);
        }

        for (int i = 0; i < pointsEdit.Length; i++)
        {
            Handles.Label(pointsEdit[i], $"Point {i}", new GUIStyle()
            {
                normal = new GUIStyleState { textColor = Color.red },
                fontSize = 12,
                fontStyle = FontStyle.Bold,
            });

            Handles.color = Color.red;
            Handles.SphereHandleCap(0, pointsEdit[i], Quaternion.identity, 0.3f, EventType.Repaint);
        }
        SetSelectionDisplay();
    }

    private void SetSelectionDisplay()
    {
        if (selectedGroup < 0)
        {
            return;
        }

        SpawnGroup group = spawnGroups[selectedGroup].GetComponent<SpawnGroup>();        
        PatrolRoute[] routes = group.patrolRoutes;

        Handles.color = Color.green;
        Handles.SphereHandleCap(0, group.gameObject.transform.position, Quaternion.identity, 10f, EventType.Repaint);
        for (int i = 0; i < routes.Length; i++)
        {
            Vector3[] pts = routes[i].GetPatrolPoints();

            for (int j = 0; j < pts.Length; j++)
            {
                GUIStyle style = new()
                {
                    normal = new GUIStyleState { textColor = Color.green },
                    fontSize = 12,
                    fontStyle = FontStyle.Bold
                };
                Handles.color = Color.green;
                Handles.DrawLine(pts[j], pts[j == pts.Length - 1 ? 0 : j + 1], 3f);
                Handles.Label(pts[j], $"Point: {j + 1:D3}", style);
            }
        }
    }
}