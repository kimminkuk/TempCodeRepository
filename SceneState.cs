using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum E_SceneState
{
    idle,
    pass,
    fail
}
public class SceneState : MonoBehaviour {
    public SceneState SceneState_current;

    public SceneIdle()
    {
        SceneState_current = SceneState.idle;
        return;
    }

    public ScenePass()
    {
        SceneState_current = SceneState.pass;
        return;
    }

    public SceneFail()
    {
        SceneState_current = SceneState.fail;
        return;
    }

}