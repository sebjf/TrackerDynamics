using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RecorderNode : MonoBehaviour
{
    public string directory;
    public string filename;

    private FileStream stream;
    private BinaryWriter Writer;

    private TimeManager time;

    private void Awake()
    {
        time = GetComponentInParent<TimeManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        stream = new FileStream(Path.Combine(directory,filename), FileMode.Create);
        Writer = new BinaryWriter(stream);
    }

    private void OnApplicationQuit()
    {
        Writer.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WriteTransform(Transform transform, int id, int word)
    {
        var timestamp = TimeManager.ToSeconds(time.gameTime);
        Writer.Write((float)id);
        Writer.Write(timestamp);
        Writer.Write(transform.localPosition.x);
        Writer.Write(transform.localPosition.y);
        Writer.Write(transform.localPosition.z);
        Writer.Write(transform.localRotation.x);
        Writer.Write(transform.localRotation.y);
        Writer.Write(transform.localRotation.z);
        Writer.Write(transform.localRotation.w);
        Writer.Write((float)word); // note this is not a reinterpret cast yet. keep the values simple!
    }
}
