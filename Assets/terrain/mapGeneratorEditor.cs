using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using EditorGUITable;
using System.IO;

[CustomEditor(typeof(CustomTerrain))]
[CanEditMultipleObjects]
public class mapGeneratorEditor : Editor
{
    //proprietes
    SerializedProperty randomHeightRange;
    SerializedProperty heightMapImage;
    SerializedProperty heightMapScale;
    SerializedProperty perlinXScale;
    SerializedProperty perlinYScale;
    SerializedProperty perlinOffsetX;
    SerializedProperty perlinOffsetY;
    SerializedProperty perlinOctaves;
    SerializedProperty perlinPersistance;
    SerializedProperty perlingHeightScale;
    SerializedProperty restartData;
    SerializedProperty count;
    SerializedProperty MinHeight;
    SerializedProperty maxHeight;
    SerializedProperty fallOff;
    SerializedProperty DropOff;
    SerializedProperty height;
    SerializedProperty roughness;
    SerializedProperty smoothAmmount;

    GUITableState perlinParameterTable;
    SerializedProperty perlinParameters;

    SerializedProperty noiseStrenght;

    GUITableState SplatHeightsTable;
    SerializedProperty terrainHeight;

    GUITableState treesTable;
    SerializedProperty MaxTrees;
    SerializedProperty treesSpacing;
    SerializedProperty stuff;

    GUITableState DetailsTable;
    SerializedProperty MaxDetails;
    SerializedProperty detailsSpacing;

    SerializedProperty WaterHeight;
    SerializedProperty WaterGo;
    SerializedProperty erosionType;
    SerializedProperty ecrosionStrngth;
    SerializedProperty springsPerRiver;
    SerializedProperty solubility;
    SerializedProperty droplets;
    SerializedProperty ecrosionSmooth;
    SerializedProperty rain;
    GUITableState treeTable;
    SerializedProperty Layer;
    SerializedProperty terrainLayer;
    SerializedProperty groundLayer;
    //fold outs
    bool showRandom = false;
    bool showLoadHeights = false;
    bool loadPerlin = false;
    bool showPerlinTable = false;
    bool showVor = false;
    bool showMidPoint = false;
    bool showSmooth = false;
    bool showSplatMaps = false;
    bool showHeightMapTexture = false;
    bool showTrees = false;
    bool showDetails = false;
    bool showWater = false;
    bool showEcrosion = false;
    bool stuffShow = false;
    void OnEnable()
    {
        groundLayer = serializedObject.FindProperty("groundLayer");
        terrainLayer = serializedObject.FindProperty("terrainLayere");
        Layer = serializedObject.FindProperty("layer");
        treeTable = new GUITableState("treeTable");
        rain = serializedObject.FindProperty("rain");
        ecrosionSmooth = serializedObject.FindProperty("ecrosionSmooth");
        droplets = serializedObject.FindProperty("droplets");
        solubility = serializedObject.FindProperty("solubility");
        springsPerRiver = serializedObject.FindProperty("springsPerRiver");
        ecrosionStrngth = serializedObject.FindProperty("ecrosionStrngth");
        erosionType = serializedObject.FindProperty("erosionType");
        WaterHeight = serializedObject.FindProperty("WaterHeight");
        WaterGo = serializedObject.FindProperty("WaterGo");
        detailsSpacing = serializedObject.FindProperty("detailsSpacing");
        MaxDetails = serializedObject.FindProperty("MaxDetails");
        stuff = serializedObject.FindProperty("stuff");
        treesSpacing = serializedObject.FindProperty("treesSpacing");
        MaxTrees = serializedObject.FindProperty("MaxTrees");
        restartData = serializedObject.FindProperty("restartData");
        heightMapImage = serializedObject.FindProperty("heightMapImage");
        heightMapScale = serializedObject.FindProperty("heightMapScale");
        randomHeightRange = serializedObject.FindProperty("randomHeightRange");
        perlinXScale = serializedObject.FindProperty("perlinXScale");
        perlinYScale = serializedObject.FindProperty("perlinYScale");
        perlinOffsetX = serializedObject.FindProperty("perlinOffsetX");
        perlinOffsetY = serializedObject.FindProperty("perlinOffsetY");
        perlinOctaves = serializedObject.FindProperty("perlinOctaves");
        perlinPersistance = serializedObject.FindProperty("perlinPersistance");
        perlingHeightScale = serializedObject.FindProperty("perlingHeightScale");
        perlinParameterTable = new GUITableState("perlinParameterTable");
        SplatHeightsTable = new GUITableState("SplatHeightsTable");
        treesTable = new GUITableState("treesTable");
        DetailsTable = new GUITableState("DetailsTable");
        perlinParameters = serializedObject.FindProperty("perlinParameters");
        count = serializedObject.FindProperty("count");
        MinHeight = serializedObject.FindProperty("MinHeight");
        maxHeight = serializedObject.FindProperty("maxHeight");
        fallOff = serializedObject.FindProperty("fallOff");
        DropOff = serializedObject.FindProperty("DropOff");
        roughness = serializedObject.FindProperty("roughness");
        height = serializedObject.FindProperty("height");
        smoothAmmount = serializedObject.FindProperty("smoothAmmount");
        noiseStrenght = serializedObject.FindProperty("noiseStrenght");
        terrainHeight = serializedObject.FindProperty("terrainHeight");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        CustomTerrain terrain = (CustomTerrain)target;
        EditorGUILayout.PropertyField(restartData);
        EditorGUILayout.Space(10);
        //treeSpawning
        stuffShow = EditorGUILayout.Foldout(stuffShow, "AddDetails");
        if (stuffShow)
        {

            EditorGUILayout.PropertyField(terrainLayer, new GUIContent("Terrain Layer"));
            EditorGUILayout.PropertyField(Layer, new GUIContent("Layer"));
            treeTable = GUITableLayout.DrawTable(treeTable, serializedObject.FindProperty("TreeList"));
            EditorGUILayout.Space(20);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
                terrain.AddTree();
            if (GUILayout.Button("-"))
                terrain.RemoveTree();
            EditorGUILayout.EndHorizontal();
        }
        //randome terrain
        showRandom = EditorGUILayout.Foldout(showRandom, "Random");
        if (showRandom)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Set heights between Random values", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(randomHeightRange);
            if (GUILayout.Button("Random heights"))
            {
                terrain.RandomTerrain();
            }
        }

        //load terrain
        showLoadHeights = EditorGUILayout.Foldout(showLoadHeights, "Load Heights");
        if (showLoadHeights)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Load Heights from Texture", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(heightMapImage);
            EditorGUILayout.PropertyField(heightMapScale);
            if (GUILayout.Button("Load heights"))
            {
                terrain.LoadTexture();
            }
        }

        //singleperlin noise terrain
        loadPerlin = EditorGUILayout.Foldout(loadPerlin, "Single Perlin Noise");
        if (loadPerlin)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Load Heights from Perlin Noise", EditorStyles.boldLabel);
            EditorGUILayout.Slider(perlinXScale, 0, .1f, new GUIContent("X Scale"));
            EditorGUILayout.Slider(perlinYScale, 0, .1f, new GUIContent("Y Scale"));
            EditorGUILayout.IntSlider(perlinOffsetX, 0, 10000, new GUIContent("Offset X"));
            EditorGUILayout.IntSlider(perlinOffsetY, 0, 10000, new GUIContent("Offset Y"));
            EditorGUILayout.IntSlider(perlinOctaves, 1, 10, new GUIContent("Octaves"));
            EditorGUILayout.Slider(perlinPersistance, 0.1f, 10, new GUIContent("Persistance"));
            EditorGUILayout.Slider(perlingHeightScale, 0, 1, new GUIContent("Height Scale"));
            if (GUILayout.Button("Load Perlin Noise"))
            {
                terrain.Perlin();
            }
        }

        //multiple perlin noise
        showPerlinTable = EditorGUILayout.Foldout(showPerlinTable, "Multiple Perlin Noise");
        if (showPerlinTable)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Multiple Perlin Noise", EditorStyles.boldLabel);
            perlinParameterTable = GUITableLayout.DrawTable(perlinParameterTable, perlinParameters);
            EditorGUILayout.Space(20);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
                terrain.AddNewPerlin();
            if (GUILayout.Button("-"))
                terrain.RemovePerlin();
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Apply Multiple Perlin"))
                terrain.MultiplePerlinTerrain();
        }

        //Vornoi
        showVor = EditorGUILayout.Foldout(showVor, "Voronoi");
        if (showVor)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.IntSlider(count, 0, 10, new GUIContent("Peak Count"));
            EditorGUILayout.Slider(fallOff, 0, 5, new GUIContent("Fall Off"));
            EditorGUILayout.Slider(DropOff, 0, 1, new GUIContent("Drop Off"));
            EditorGUILayout.Slider(MinHeight, 0, 1, new GUIContent("Min Height"));
            EditorGUILayout.Slider(maxHeight, 0, 1, new GUIContent("Max Height"));
            if (GUILayout.Button("Create Voronoi"))
            {
                terrain.Voronnoi();
            }
        }

        //MidPoint
        showMidPoint = EditorGUILayout.Foldout(showMidPoint, "Midpoint Displacement");
        if (showMidPoint)
        {
            EditorGUILayout.Slider(height, 0, .1f, new GUIContent("Height"));
            EditorGUILayout.Slider(roughness, 0, 10f, new GUIContent("Roughness"));
            if (GUILayout.Button("MDP"))
            {
                terrain.MidPointDiplacement();
            }
        }

        //splatHeights
        showSplatMaps = EditorGUILayout.Foldout(showSplatMaps, "Splat Maps");
        if (showSplatMaps)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Splat Maps", EditorStyles.boldLabel);
            SplatHeightsTable = GUITableLayout.DrawTable(SplatHeightsTable, serializedObject.FindProperty("splatHeights"));
            EditorGUILayout.Space(20);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
                terrain.AddSplat();
            if (GUILayout.Button("-"))
                terrain.RemoveSplat();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Slider(noiseStrenght, 0, .1f, new GUIContent("Noise"));
            if (GUILayout.Button("Apply SplatMaps"))
                terrain.SplatMaps();
        }

        //vegetation
        showTrees = EditorGUILayout.Foldout(showTrees, "Vegetation");
        if (showTrees)
        {
            EditorGUILayout.PropertyField(groundLayer, new GUIContent("Layer"));
            EditorGUILayout.IntSlider(MaxTrees, 0, 1000000, new GUIContent("Max Trees"));
            EditorGUILayout.IntSlider(treesSpacing, 1, 50, new GUIContent("Trees Spacing"));
            treesTable = GUITableLayout.DrawTable(treesTable, stuff);

            EditorGUILayout.Space(20);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
                terrain.AddTrees();
            if (GUILayout.Button("-"))
                terrain.RemoveTrees();
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Apply Vegetation"))
                terrain.PlantVegetation();
        }

        //showDetails
        showDetails = EditorGUILayout.Foldout(showDetails, "Detail");
        if (showDetails)
        {
            EditorGUILayout.IntSlider(MaxDetails, 0, 10000, new GUIContent("Max Details"));
            EditorGUILayout.IntSlider(detailsSpacing, 1, 50, new GUIContent("Detail Spacing"));
            DetailsTable = GUITableLayout.DrawTable(DetailsTable, serializedObject.FindProperty("details"));
            terrain.GetComponent<Terrain>().detailObjectDistance = MaxDetails.intValue;
            EditorGUILayout.Space(20);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
                terrain.AddDetails();
            if (GUILayout.Button("-"))
                terrain.RemoveDetails();
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Apply Details"))
                terrain.CreateDetails();
        }

        //showWater
        showWater = EditorGUILayout.Foldout(showWater, "Water");
        if (showWater)
        {
            EditorGUILayout.Slider(WaterHeight, 0, 1f, new GUIContent("Water Height"));
            EditorGUILayout.PropertyField(WaterGo);
            if (GUILayout.Button("Add Water"))
            {
                terrain.AddWater();
            }
        }

        //Ecrosion
        showEcrosion = EditorGUILayout.Foldout(showEcrosion, "Erosion");
        if (showEcrosion)
        {
            EditorGUILayout.PropertyField(rain, new GUIContent("Rain Prefab"));
            EditorGUILayout.PropertyField(erosionType, new GUIContent("Erosion Type"));
            EditorGUILayout.Slider(ecrosionStrngth, 0, 1, new GUIContent("Erosion Strength"));
            EditorGUILayout.IntSlider(droplets, 0, 1000, new GUIContent("Droplets"));
            EditorGUILayout.Slider(solubility, 0, 10, new GUIContent("Solubility"));
            EditorGUILayout.IntSlider(springsPerRiver, 0, 20, new GUIContent("Springs Per River"));
            EditorGUILayout.IntSlider(ecrosionSmooth, 0, 10, new GUIContent("Smooth Amount"));
            if (GUILayout.Button("Erode"))
            {
                terrain.Erode();
            }

        }

        //smooth
        showSmooth = EditorGUILayout.Foldout(showSmooth, "Smooth");
        if (showSmooth)
        {
            EditorGUILayout.IntSlider(smoothAmmount, 0, 10, new GUIContent("Smooth Ammount"));
            if (GUILayout.Button("Smooth"))
            {
                terrain.smooth();
            }
        }

        //show texture
        showHeightMapTexture = EditorGUILayout.Foldout(showHeightMapTexture, "Height map");
        if (showHeightMapTexture)
        {

            int wSize = (int)(EditorGUIUtility.currentViewWidth - 100);
            EditorGUILayout.Slider(terrainHeight, 0, 1000, new GUIContent("Max height"));
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(terrain.Htexture, GUILayout.Width(wSize), GUILayout.Height(wSize));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Refresh"))
            {
                terrain.HeightMap();
            }
            string fileName = "fileName";
            fileName = EditorGUILayout.TextField("Texture Name", fileName);
            if (GUILayout.Button("Save"))
            {
                byte[] bytes = terrain.Htexture.EncodeToPNG();
                System.IO.Directory.CreateDirectory(Application.dataPath + "/SavedTextures");
                File.WriteAllBytes(Application.dataPath + "/SavedTextures/" + fileName + ".png", bytes);
            }

        }


        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        if (GUILayout.Button("Reset"))
        {
            terrain.resetTerrain();
        }
        serializedObject.ApplyModifiedProperties();
    }
}
