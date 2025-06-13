using UnityEngine;

public class AudioPauseOnTabSwitch : MonoBehaviour
{
    void OnApplicationFocus(bool hasFocus)
    {

        AudioListener.pause = !hasFocus;
    }
}