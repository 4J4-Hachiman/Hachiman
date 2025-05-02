using UnityEngine;
using UnityEngine.Playables;

public class testTimelineTuto : MonoBehaviour
{
    public PlayableDirector director;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y)){
            director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }
    }
}
