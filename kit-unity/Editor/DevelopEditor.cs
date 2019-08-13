// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

namespace Kit.Editor
{
	public class DevelopEditor 
	{		
		#region Kit/Model/Create Model Prefab
		[MenuItem("Kit/Create Model Prefab")]
		public static void CreateModelPrefab ()
		{
			string path = AssetDatabase.GetAssetOrScenePath(Selection.activeObject);
			
			if (path.Contains("@"))
			{
				path = path.Substring(0, path.LastIndexOf("@"));
			}
			else
			{
				path = path.Substring(0, path.LastIndexOf("."));
			}
			
			Object obj = AssetDatabase.LoadAssetAtPath(path + ".fbx", typeof(Object));
			if (obj == null) return;
			
			GameObject go = PrefabUtility.InstantiatePrefab(obj) as GameObject;
			if (go == null) return;		
			
			string directory = path.Substring(0, path.LastIndexOf("/"));
			directory = directory.Replace("Assets", "") + "/" + go.name;
			
			GameObject prefab = AssetDatabase.LoadAssetAtPath("Assets" + directory + ".prefab", typeof(Object)) as GameObject;
			if (prefab == null)
			{
				prefab = PrefabUtility.CreatePrefab("Assets" + directory + ".prefab", go);
				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(prefab));
			}
			
			Object.DestroyImmediate(go);
		}
		
		[MenuItem("Kit/Create Model Prefab", true)]
		public static bool CheckCreateModelPrefab ()
		{
			if (Selection.activeObject != null)
			{
				string path = AssetDatabase.GetAssetOrScenePath(Selection.activeObject).ToLower();
				
				if (path.EndsWith(".fbx"))
					return true;
			}
			
			return false;
		}
        #endregion

        #region Kit/Print/Print PrefabType
        [MenuItem("Kit/Print/Print PrefabType")]
        public static void PrintPrefabType()
        {
            Debug.Log(PrefabUtility.GetPrefabType(Selection.activeObject), Selection.activeObject);
        }

        [MenuItem("Kit/Print/Print PrefabType", true)]
        public static bool CheckPrintPrefabType()
        {
            if (Selection.activeObject != null)
            {
                PrefabType type = PrefabUtility.GetPrefabType(Selection.activeObject);
                if (type == PrefabType.ModelPrefab ||
                    type == PrefabType.ModelPrefabInstance ||
                    type == PrefabType.Prefab ||
                    type == PrefabType.PrefabInstance)
                    return true;
            }

            return false;
        }
        #endregion

        #region "Kit/Collect Resource Dependencies"
        [MenuItem("Kit/Collect Resource Dependencies")]
        public static void CollectResourceDependencies()
        {
            //CollectResourceDependencies(Selection.objects);
            CollectResourceDependencies(AssetDatabase.GetAssetPath(Selection.activeObject));
        }

        public static void CollectResourceDependencies(string path)
        {
            string[] pathNames = new string[] { path };
            string[] depends = AssetDatabase.GetDependencies(pathNames);
            for (int i = 0; i < depends.Length; ++i)
            {
                Debug.Log(depends[i]);
            }
        }

        public static void CollectResourceDependencies(Object[] roots)
        {
            Object[] objs = UnityEditor.EditorUtility.CollectDependencies(roots);
            Debug.Log(objs.Length);
            for (int i = 0; i < objs.Length; ++i)
            {
                if (AssetDatabase.IsMainAsset(objs[i]))
                    Debug.Log(objs[i], objs[i]);
            }
        }

        [MenuItem("Kit/Collect Resource Dependencies", true)]
        public static bool CheckCollectResourceDependencies()
        {
            return Selection.objects.Length == 1 && !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(Selection.activeObject));
        }
        #endregion

        #region "Kit/Project/Create Project Framework"
        [MenuItem("Kit/Project/Create Project Framework")]
		public static void CreateProjectFramework ()
		{
			string dataPath = Application.dataPath; 
			string[] projectPaths = new string[] {
                "/Art",
                "/Editor Default Resources",
                "/StreamingAssets",	
				"/Scripts",
                "/Scene" 
            };
			
			DirectoryInfo dirInfo;
			for (int i = 0; i < projectPaths.Length; ++i)
			{
				dirInfo = new DirectoryInfo(dataPath + projectPaths[i]);
				if (!dirInfo.Exists) dirInfo.Create();
				
				AssetDatabase.ImportAsset("Assets" + projectPaths[i]);
			}
		}
		#endregion
		
		#region "Kit/Get Resource Path"
		[MenuItem("Kit/Get Resource Path")]
		public static void GetResourcePath ()
		{
			Object obj = Selection.activeObject;
			if (obj && AssetDatabase.IsMainAsset(obj))
			{
				Debug.Log(EditorUtility.GetResourcePath(obj));
			}
		}
		
		[MenuItem("Assets/Get Resource Path")]
		public static void GetAssetsPath ()
		{
			GetResourcePath();
		}
		#endregion
	}
}
