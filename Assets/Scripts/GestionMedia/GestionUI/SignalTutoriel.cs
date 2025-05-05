using UnityEngine;
using UnityEngine.Playables;

public class SignalTutoriel : MonoBehaviour
{
    public PlayableDirector director;

    public void PauseTutotiel(){
        director.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }
}
