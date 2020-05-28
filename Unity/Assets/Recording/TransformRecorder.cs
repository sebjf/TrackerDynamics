using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformRecorder : MonoBehaviour
{
    private RecorderNode recorder;

    public int id;
    public int word;

    private void Awake()
    {
        recorder = GetComponentInParent<RecorderNode>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (recorder)
        {
            recorder.WriteTransform(transform, id, word);
        }
    }
}
