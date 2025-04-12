using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rabah.Utils
{
    public class SceneNavigation : BaseSingleton<SceneNavigation>
    {
        [SerializeField]
        private List<SceneData> appScenesData;

        private Dictionary<AppScenesNames, SceneReference> scenes = new();


        protected override void Awake()
        {
            base.Awake();
            foreach (var sceneData in appScenesData)
            {
                scenes.Add(sceneData.appSceneName, sceneData.scene);
            }
        }

        private void OnValidate()
        {
            foreach (var sceneData in appScenesData)
            {
                sceneData.Name = sceneData.appSceneName.ToString();
            }
        }
        public void LoadScene(int sceneIndex)
        {
            string scenePath = appScenesData[sceneIndex].scene.ScenePath;
            SceneManager.LoadSceneAsync(scenePath);
        }

        public void LoadScene(AppScenesNames name)
        {
            SceneManager.LoadSceneAsync(scenes[name].ScenePath);
        }
    }

    public enum AppScenesNames
    {
        Splash,
        Definitions,
        ApplySubItemsOn3D,
        DefinitionsDrawingAndJobOrders,
        Departments,
        ApplyItemsOnActivities3D
    }

    [Serializable]
    public class SceneData
    {
        public string Name;
        public AppScenesNames appSceneName;
        public SceneReference scene;
    }
}