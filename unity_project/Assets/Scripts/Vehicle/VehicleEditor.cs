using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class VehicleEditor : MonoBehaviour
{
    private VehicleRefreshManager vehicleRefreshManager;

    public enum VehicleLength
    {
        Length1 = 1,
        Length2 = 2,
        Length3 = 3
    }

    public int vehicleLength = 2;
    //[HideInInspector] public int selectedPrefabIndex = -1;
    public bool isHorizontal = false;
    public bool isFacingBackwards = false;
    public bool isMainVehicle = false;


    private void Awake()
    {
        vehicleRefreshManager = GameObject.FindWithTag("VehicleRefreshManager").GetComponent<VehicleRefreshManager>();
    }

    public void RefreshVehicle()
    {
        vehicleRefreshManager.RefreshVehiclesOnBoard(gameObject);
    }

    public VehicleData GetVehicleData()
    {
        VehicleData vehicleData = new VehicleData
        {
            positionX = transform.position.x,
            positionY = transform.position.y,
            positionZ = transform.position.z,

            vehicleLength = vehicleLength,
            //selectedPrefabIndex = selectedPrefabIndex,
            isHorizontal = isHorizontal,
            isFacingBackwards = isFacingBackwards,
            isMainVehicle = isMainVehicle
        };
        
        return vehicleData;
    }

    public void SetVehicleData(VehicleData vehicleData)
    {
        transform.position = new Vector3(vehicleData.positionX, vehicleData.positionY, vehicleData.positionZ);

        vehicleLength = vehicleData.vehicleLength;
        //selectedPrefabIndex = vehicleData.selectedPrefabIndex;
        isHorizontal = vehicleData.isHorizontal;
        isFacingBackwards= vehicleData.isFacingBackwards;
        isMainVehicle = vehicleData.isMainVehicle;

        RefreshVehicle();
    }
}




#if UNITY_EDITOR
[CustomEditor(typeof(VehicleEditor))]
public class VehicleEditorSettings : Editor
{
    VehicleRefreshManager vehicleRefreshManager;

    public override void OnInspectorGUI()
    {
        vehicleRefreshManager = GameObject.FindWithTag("VehicleRefreshManager").GetComponent<VehicleRefreshManager>();
        VehicleEditor editorMode = (VehicleEditor)target;

        EditorGUILayout.Space(10f);

        // Vehicle Length legördülõ menü
        editorMode.vehicleLength = (int)(VehicleEditor.VehicleLength)EditorGUILayout.EnumPopup("Vehicle Length", (VehicleEditor.VehicleLength)editorMode.vehicleLength);

        /*
        // Select Prefab legördülõ menü
        List<GameObject> currentList = vehicleRefreshManager.GetCurrentPrefabList((VehicleEditor.VehicleLength)editorMode.vehicleLength);
        if (currentList != null && currentList.Count > 0)
        {
            string[] prefabNames = new string[currentList.Count];
            for (int i = 0; i < currentList.Count; i++)
            {
                prefabNames[i] = currentList[i] != null ? currentList[i].name : "N/A";
            }
            editorMode.selectedPrefabIndex = EditorGUILayout.Popup("Select Prefab", editorMode.selectedPrefabIndex, prefabNames);
        }
        else
        {
            EditorGUILayout.HelpBox("Add prefabs to the list for the selected length.\nCheck: | VehicleRefreshManager |", MessageType.Warning);
        } */

        // Checkboxok
        editorMode.isHorizontal = EditorGUILayout.Toggle("Is Horizontal", editorMode.isHorizontal);
        editorMode.isFacingBackwards = EditorGUILayout.Toggle("Is Facing Backwards", editorMode.isFacingBackwards);
        editorMode.isMainVehicle = EditorGUILayout.Toggle("Is Main Vehicle", editorMode.isMainVehicle);

        EditorGUILayout.Space(20f);

        // Refresh gomb
        if (GUILayout.Button("Refresh Vehicle"))
        {
            editorMode.RefreshVehicle();
        }

        EditorGUILayout.Space(10f);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif