using UnityEngine;
using UnityEngine.Playables;

public class SignalTutoriel : MonoBehaviour
{
    public PlayableDirector director;
    public bool tutorielComplete;

    void Start()
    {
        tutorielComplete = false;
    }

    public void PauseTutoriel()
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }

    public void JouerTutoriel()
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }

    public void CompleteTuto()
    {
        tutorielComplete = true;
    }
}
