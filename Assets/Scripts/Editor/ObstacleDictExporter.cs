using UnityEngine;
using SimpleJSON;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ObstacleDictExporter : EditorWindow {

    [SerializeField]
    ObstacleDictionary dict;

    [MenuItem("Window/Obstacle JSON Exporter")]
    public static void ShowWindow()
    {
        GetWindow<ObstacleDictExporter>("Obstacle JSON Exporter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Export JSON file for our server", EditorStyles.boldLabel);

        if (GUILayout.Button("Export"))
        {
            if (Selection.gameObjects[0].GetComponent<ObstacleDictionary>() != null)
            {
                Debug.Log("We have a dictionary!");

                ObstacleDictionary dict = Selection.gameObjects[0].GetComponent<ObstacleDictionary>();
                List<JSONObstacle> obst = new List<JSONObstacle>();

                foreach(ObstacleDictionary.Obstacle o in dict.obstacles)
                {
                    obst.Add(new JSONObstacle(o.name, o.impenetrable, o.interactive, o.banano));
                }

                string json = "{\"obstacles\":[\n"; // Initialize our json beginning

                for (int i = 0; i < obst.Count; i++)
                {
                    json += JsonUtility.ToJson(obst[i]); // Serialize our obstacle into JSON format

                    if (i != obst.Count - 1)
                    {
                        json += ",";
                    }

                    json += "\n";
                }

                json += "]}";

                WriteJSON(json);

            }
        }
    }

    [System.Serializable]
    public class JSONObstacle
    {
        public string name;
        public bool impenetrable;
        public bool interactive;
        public bool banano;

        public JSONObstacle(string name, bool imp, bool interact, bool ban)
        {
            this.name = name;
            this.impenetrable = imp;
            this.interactive = interact;
            this.banano = ban;
        }

    }

    private void WriteJSON(string input)
    {

        string filePath = Application.dataPath + "/" + "dictionary.txt";

        var sr = File.CreateText(filePath);

        sr.WriteLine(input);
        sr.Close();

        return;


    }

}
