using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WriteLargeFile : MonoBehaviour {
    IEnumerator Start () {
        Debug.Log("Start Write small(< 2gb) file");

        var filePath = Application.temporaryCachePath + "/test1.file";
        Debug.Log(filePath);
        yield return WriteFile(filePath, (long)1024 * 1024 * 1024);

        Debug.Log("    Done");

        yield return new WaitForSeconds(1.0f);

        Debug.Log("Start Write large(> 3gb) file");

        filePath = Application.temporaryCachePath + "/test2.file";
        Debug.Log(filePath);
        yield return WriteFile(filePath, (long)1024 * 1024 * 1024 * 3);

        Debug.Log("    Done");
    }

    IEnumerator WriteFile(string filePath, long size)
    {
        Debug.Log("    size: " + size);

        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("    Failed... " + e.ToString());
            yield break;
        }

        const int FRAGMENT_SIZE = 1024 * 1024;
        var loopCount = size / FRAGMENT_SIZE;

        using (FileStream fs = new FileStream(filePath, FileMode.Append))
        { 
            for (var i = 0; i < loopCount; i++)
            {
                if (i % 100 == 0)
                    Debug.Log("    Write data: " + (loopCount - i) * FRAGMENT_SIZE + " bytes remaining....");

                try
                {
                    var data = new byte[FRAGMENT_SIZE];
                    fs.Write(data, 0, data.Length);
                }
                catch(System.Exception e)
                {
                    Debug.LogError("    Failed... " + e.ToString());
                    yield break;
                }
                yield return null;
            }
        }
    }
}
