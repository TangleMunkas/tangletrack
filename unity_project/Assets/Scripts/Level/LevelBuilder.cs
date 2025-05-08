using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

public class LevelBuilder : MonoBehaviour
{
    private string levelsFolderPath = "C:/Users/sebes/Desktop/tangle_track/_levels/";

    public VehicleDataManager vehicleDataManager;
    public GameObject vehiclePrefab;

    public int selectedIndex = 0; // A leg�rd�l� men�ben kiv�lasztott szint
    [HideInInspector] public int lastLoadedLevelIndex = 0;

    private void Start()
    {
        selectedIndex = 0;

        Debug.Log($"Ment�si hely: {levelsFolderPath}");
    }

    [Obsolete]
    public void SaveLevelButtonPressed()
    {
        if (vehicleDataManager.CreateSavingData().Count < 1)
        {
            SaveJsonHandler.SaveList(vehicleDataManager.CreateSavingData(), $"level_{selectedIndex + 1}_default.json", $"{levelsFolderPath}", false);
            lastLoadedLevelIndex = selectedIndex;

            #if UNITY_EDITOR
                EditorUtility.DisplayDialog("Data saved", "Level data was succesfully saved.", "Cool");
            #endif

            Debug.Log($"Level {selectedIndex + 1} SAVED!");
        }
        else
        {
            if (!AreListsEqualIgnoreOrder(vehicleDataManager.GetLevelData(lastLoadedLevelIndex, $"{levelsFolderPath}", false), vehicleDataManager.CreateSavingData()))
            {
                #if UNITY_EDITOR
                    if (ShowBoolPopup("Warning!", $"Level {lastLoadedLevelIndex + 1} contains different data. Do you want to overwrite it?"))
                    {
                        SaveJsonHandler.SaveList(vehicleDataManager.CreateSavingData(), $"level_{selectedIndex + 1}_default.json", $"{levelsFolderPath}", false);
                        lastLoadedLevelIndex = selectedIndex;
                        EditorUtility.DisplayDialog("Data saved", "Level data was succesfully overwritten.", "Cool");
                        Debug.Log($"Level {lastLoadedLevelIndex + 1} OVERWRITTEN!");
                    }
                    else
                    {
                        Debug.Log($"Level {lastLoadedLevelIndex + 1} overwrite cancelled.");
                    }
                #endif
            }
        }
    }

    [Obsolete]
    public bool LoadLevelButtonPressed()
    {
        if (vehicleDataManager.CreateSavingData().Count < 1)
        {
            RemoveVehiclesFromScene();
            vehicleDataManager.LoadData(vehicleDataManager.GetLevelData(selectedIndex, $"{levelsFolderPath}", false));
        }
        else
        {
            if (!AreListsEqualIgnoreOrder(vehicleDataManager.GetLevelData(lastLoadedLevelIndex, "C:/Users/sebes/Desktop/tangle_track/_levels/", false), vehicleDataManager.CreateSavingData()))
            {
                #if UNITY_EDITOR
                    if (ShowBoolPopup("Warning!", $"Level {lastLoadedLevelIndex + 1} has unsaved changes. Do you want to continue?"))
                    {
                        RemoveVehiclesFromScene();
                        vehicleDataManager.LoadData(vehicleDataManager.GetLevelData(selectedIndex, $"{levelsFolderPath}", false));
                    }
                    else
                    {
                        return false;
                    }
                #endif
            }
            else
            {
                RemoveVehiclesFromScene();
                vehicleDataManager.LoadData(vehicleDataManager.GetLevelData(selectedIndex, $"{levelsFolderPath}", false));
            }
        }
        return true;
    }

    public void ClearLevelButtonPressed()
    {
        #if UNITY_EDITOR
            if (ShowBoolPopup("Warning!", $"Are you sure you want to clear Level {selectedIndex + 1}?"))
            {
                SaveJsonHandler.SaveList(new List<VehicleData>(), $"level_{selectedIndex + 1}_default.json", $"{levelsFolderPath}", false);
                EditorUtility.DisplayDialog("Data cleared", "Level data was succesfully cleared.", "Cool");
                Debug.Log($"Level {selectedIndex + 1} CLEARED!");
            }
            else
            {
                Debug.Log($"Clear Level {selectedIndex + 1} cancelled.");
            }
        #endif
    }

    public void AddVehiclesButtonPressed()
    {
        #if UNITY_EDITOR
            AddVehiclesPopup.ShowPopup(this);
        #endif
    }

    [Obsolete]
    public void RemoveVehiclesButtonPressed()
    {
        if (vehicleDataManager.CreateSavingData().Count > 0 && !AreListsEqualIgnoreOrder(vehicleDataManager.GetLevelData(lastLoadedLevelIndex, "C:/Users/sebes/Desktop/tangle_track/_levels/", false), vehicleDataManager.CreateSavingData()))
        {
            #if UNITY_EDITOR
                if (ShowBoolPopup("Warning!", $"Level {selectedIndex + 1} has unsaved changes. Do you want to continue?"))
                {
                    RemoveVehiclesFromScene();
                }
                else
                {
                    Debug.Log($"Removing vehicles from scene cancelled.");
                }
            #endif
        }
        else
        {
            RemoveVehiclesFromScene();
        }
    }

    private bool ShowBoolPopup(string title, string message)
    {
        #if UNITY_EDITOR
            return EditorUtility.DisplayDialog(title, message, "Yes", "No");
        #else
            Debug.LogWarning("ShowBoolPopup is only available in Unity Editor.");
            return false;
        #endif
    }

    [Obsolete]
    private void RemoveVehiclesFromScene()
    {
        GameObject vehicleContainer = GameObject.Find("VehicleContainer");

        if (vehicleContainer == null)
        {
            Debug.LogError("VehicleContainer not found in the scene!");
            return;
        }

        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            if ((obj.CompareTag("Vehicle") || obj.CompareTag("MainVehicle")) &&
                obj.GetComponent<VehicleController>() != null &&
                obj.transform.parent == vehicleContainer.transform)
            {
                if (Application.isPlaying)
                {
                    Destroy(obj);
                }
                else
                {
                    #if UNITY_EDITOR
                        DestroyImmediate(obj);
                    #endif
                }
            }
        }
    }

    private bool AreListsEqualIgnoreOrder(List<VehicleData> list1, List<VehicleData> list2)
    {
        if (list1.Count != list2.Count) return false;

        var set1 = new HashSet<VehicleData>(list1, new VehicleDataEqualityComparer());
        var set2 = new HashSet<VehicleData>(list2, new VehicleDataEqualityComparer());

        return set1.SetEquals(set2);
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(LevelBuilder))]
public class MyComponentEditor : Editor
{
    [Obsolete]
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelBuilder myComponent = (LevelBuilder)target;
        bool isPlaying = Application.isPlaying;

        // Sz�nek l�trehoz�sa az RGB �rt�kek alapj�n
        Color saveButtonColor = new Color(3f / 255f, 145f / 255f, 46f / 255f); // 3, 145, 46 (z�ld)
        Color loadButtonColor = new Color(153f / 255f, 151f / 255f, 6f / 255f); // 153, 151, 6 (aranys�rga)
        Color clearButtonColor = new Color(191f / 255f, 10f / 255f, 10f / 255f); // 191, 10, 10 (piros)
        Color addButtonColor = new Color(189f / 255f, 32f / 255f, 176f / 255f); // 189, 32, 176 (s�t�t r�zsasz�n)
        Color removeButtonColor = new Color(10f / 255f, 91f / 255f, 138f / 255f); // 10, 91, 138 (k�k)


        // A leg�rd�l� men� a levels lista elemeivel
        //

        GUILayout.Space(15);

        // Piros felirat st�lus
        GUIStyle redLabelStyle = new GUIStyle(EditorStyles.label)
        {
            normal = { textColor = Color.red },
            fontStyle = FontStyle.Bold,
            fontSize = 22
        };

        // Piros felirat megjelen�t�se
        EditorGUILayout.LabelField($"Loaded level: {myComponent.lastLoadedLevelIndex + 1}", redLabelStyle);

        GUILayout.Space(10);

        // Gombst�lus a Save gombhoz
        GUIStyle saveButtonStyle = new GUIStyle(GUI.skin.button);
        saveButtonStyle.fontSize = 14;
        saveButtonStyle.fontStyle = FontStyle.Bold;
        saveButtonStyle.normal.textColor = Color.white;
        saveButtonStyle.normal.background = MakeTex(2, 2, saveButtonColor);

        GUILayout.Space(15);

        // Save Level gomb
        EditorGUI.BeginDisabledGroup(myComponent.selectedIndex < 0 || isPlaying);
        if (GUILayout.Button($"Save Level {myComponent.selectedIndex + 1}", saveButtonStyle))
        {
            myComponent.SaveLevelButtonPressed();
        }
        EditorGUI.EndDisabledGroup();

        GUILayout.Space(10); // Hely elv�laszt�sa

        // Gombst�lus a Load gombhoz
        GUIStyle loadButtonStyle = new GUIStyle(GUI.skin.button);
        loadButtonStyle.fontSize = 14;
        loadButtonStyle.fontStyle = FontStyle.Bold;
        loadButtonStyle.normal.textColor = Color.white;
        loadButtonStyle.normal.background = MakeTex(2, 2, loadButtonColor);

        // Load Level gomb
        EditorGUI.BeginDisabledGroup(myComponent.selectedIndex < 0 || isPlaying);
        string loadButtonText = "Load Level";
        if (myComponent.selectedIndex == myComponent.lastLoadedLevelIndex)
        {
            loadButtonText = "Reload Level";
        }

        if (GUILayout.Button($"{loadButtonText} {myComponent.selectedIndex + 1}", loadButtonStyle))
        {
            if (myComponent.LoadLevelButtonPressed())
            {
                myComponent.lastLoadedLevelIndex = myComponent.selectedIndex;
            }
            else
            {
                myComponent.selectedIndex = myComponent.lastLoadedLevelIndex;
            }
        }
        EditorGUI.EndDisabledGroup();

        GUILayout.Space(10); // Hely elv�laszt�sa

        // Gombst�lus a Clear gombhoz
        GUIStyle clearButtonStyle = new GUIStyle(GUI.skin.button);
        clearButtonStyle.fontSize = 14;
        clearButtonStyle.fontStyle = FontStyle.Bold;
        clearButtonStyle.normal.textColor = Color.white;
        clearButtonStyle.normal.background = MakeTex(2, 2, clearButtonColor);

        // Clear Level gomb
        EditorGUI.BeginDisabledGroup(myComponent.selectedIndex < 0 || isPlaying);
        if (GUILayout.Button($"Clear Level {myComponent.selectedIndex + 1}", clearButtonStyle))
        {
            myComponent.ClearLevelButtonPressed();
        }
        EditorGUI.EndDisabledGroup();

        GUILayout.Space(30); // Hely elv�laszt�sa

        // Gombst�lus az Add gombhoz
        GUIStyle addButtonStyle = new GUIStyle(GUI.skin.button);
        addButtonStyle.fontSize = 14;
        addButtonStyle.fontStyle = FontStyle.Bold;
        addButtonStyle.normal.textColor = Color.white;
        addButtonStyle.normal.background = MakeTex(2, 2, addButtonColor);

        // Add Vehicles gomb
        if (GUILayout.Button($"Add Vehicles to Scene", addButtonStyle))
        {
            myComponent.AddVehiclesButtonPressed();
        }
        EditorGUI.EndDisabledGroup();

        GUILayout.Space(10);

        // Gombst�lus a Remove gombhoz
        GUIStyle removeButtonStyle = new GUIStyle(GUI.skin.button);
        removeButtonStyle.fontSize = 14;
        removeButtonStyle.fontStyle = FontStyle.Bold;
        removeButtonStyle.normal.textColor = Color.white;
        removeButtonStyle.normal.background = MakeTex(2, 2, removeButtonColor);

        // Remove Vehicles gomb
        if (GUILayout.Button($"Remove all Vehicles from Scene", removeButtonStyle))
        {
            myComponent.RemoveVehiclesButtonPressed();
        }
        EditorGUI.EndDisabledGroup();

        GUILayout.Space(10);
    }

    private Texture2D MakeTex(int width, int height, Color color)
    {
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = color;
        }
        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }
}
#endif


#if UNITY_EDITOR
public class AddVehiclesPopup : EditorWindow
{
    private int vehicleCount = 0; // Az eg�sz sz�m beviteli mez� �rt�ke
    private LevelBuilder levelHandler;

    public static void ShowPopup(LevelBuilder handler)
    {
        // L�trehoz egy �j EditorWindow ablakot
        AddVehiclesPopup window = GetWindow<AddVehiclesPopup>("Add Vehicles");

        // Sz�l� objektum be�ll�t�sa
        window.levelHandler = handler;

        // Ablak m�rete
        window.minSize = new Vector2(300, 100);

        // K�z�pre helyez�s
        Rect mainWindow = EditorGUIUtility.GetMainWindowPosition(); // Editor f�ablak m�rete
        Vector2 centerPosition = new Vector2(
            mainWindow.x + mainWindow.width / 2f - window.minSize.x / 2f,
            mainWindow.y + mainWindow.height / 2f - window.minSize.y / 2f
        );

        window.position = new Rect(centerPosition, window.minSize);
    }


    private void OnGUI()
    {
        // Felirat
        GUILayout.Label("Add Vehicles", EditorStyles.boldLabel);

        // Eg�sz sz�m beviteli mez�
        vehicleCount = EditorGUILayout.IntField("Number of Vehicles:", vehicleCount);

        GUILayout.Space(10);

        // Gomb
        if (GUILayout.Button("Add Vehicles"))
        {
            if (vehicleCount > 0)
            {
                AddVehiclesToScene(vehicleCount);
                Close(); // Az ablak bez�r�sa
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Please enter a number greater than 0.", "OK");
            }
        }
    }

    private void AddVehiclesToScene(int count)
    {
        GameObject vehicleContainer = GameObject.Find("VehicleContainer");

        if (vehicleContainer == null)
        {
            Debug.LogError("VehicleContainer not found in the scene!");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            GameObject newVehicle = Instantiate(levelHandler.vehiclePrefab, vehicleContainer.transform);
            newVehicle.name = $"Vehicle_{i + 1}";
            newVehicle.transform.position = new Vector3(-3, 1, 3);
        }

        //Debug.Log($"{count} vehicles added to the scene.");
    }
}
#endif



class VehicleDataEqualityComparer : IEqualityComparer<VehicleData>
{
    public bool Equals(VehicleData x, VehicleData y)
    {
        return x.positionX == y.positionX &&
               x.positionY == y.positionY &&
               x.positionZ == y.positionZ &&
               x.vehicleLength == y.vehicleLength &&
               //x.selectedPrefabIndex == y.selectedPrefabIndex &&
               x.isHorizontal == y.isHorizontal &&
               x.isFacingBackwards == y.isFacingBackwards &&
               x.isMainVehicle == y.isMainVehicle;
    }

    public int GetHashCode(VehicleData obj)
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + obj.positionX.GetHashCode();
            hash = hash * 23 + obj.positionY.GetHashCode();
            hash = hash * 23 + obj.positionZ.GetHashCode();
            hash = hash * 23 + obj.vehicleLength.GetHashCode();
            //hash = hash * 23 + obj.selectedPrefabIndex.GetHashCode();
            hash = hash * 23 + obj.isHorizontal.GetHashCode();
            hash = hash * 23 + obj.isFacingBackwards.GetHashCode();
            hash = hash * 23 + obj.isMainVehicle.GetHashCode();
            return hash;
        }
    }
}