using System.Collections;
using System.Collections.Generic;
using N.Package.GameSystems.Utility.SceneTools;
using UnityEngine;

public class Demo2TestRunner : MonoBehaviour
{
    void Start()
    {
        var stream = new NSceneEventStream<DemoTask>();
        foreach (var message in stream.Consume())
        {
            Debug.Log($"Received: {message.Message}");
        }

        NSceneEventStream.ClearAllStreams();
    }
}