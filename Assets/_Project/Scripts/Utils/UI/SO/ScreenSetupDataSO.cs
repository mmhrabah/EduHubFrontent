using Rabah.Utils.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "ScreenSetupData", menuName = "ScriptableObjects/ScreenSetupData", order = 1)]
public class ScreenSetupDataSO : ScriptableObject
{
    [Header("Screen Layout")]
    public string title;
    public bool isFullScreen = true;
    public int screenWidth = 1920;
    public int screenHeight = 1080;
    public float screenScale = 1.0f;
    public ScreenOrientation orientation = ScreenOrientation.LandscapeLeft;
    public bool isAutoRotate = false;
    public bool hasBackButton;
    public bool hasResetButton;
    public bool hasNextButton;
    public bool hasBottomPanel;
    public bool hasTopPanel;
    public RenderMode renderMode;
    public ScreenHandle screenHandle;
    public string mainEndpoint;
}
