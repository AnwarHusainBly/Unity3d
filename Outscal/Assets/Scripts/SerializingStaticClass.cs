using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Threading;

public static class XMLSerializer
{
	//public static string tar;
	public static void Save<T>(string FileName, T targetObject)
    {
		//tar = targetObject.ToString ();
        using (var writer = new System.IO.StreamWriter(FileName))
        {
			//Debug.Log ("Xml = "+targetObject +" Path = "+FileName);
            var serializer = new XmlSerializer(targetObject.GetType());
            serializer.Serialize(writer, targetObject);
            writer.Flush();
        }
    }

    public static T Load<T>(string FileName)
    {
        using (var stream = System.IO.File.OpenRead(FileName))
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream);
        }
        
    }
}