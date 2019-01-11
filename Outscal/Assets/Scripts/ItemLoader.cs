using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CubeData
{
	public List<Cues> cubeInfo = new List<Cues> ();
}

public class Cues
{
	public string cubeName;
	public string scorePoints;
}

public class ItemLoader : MonoBehaviour
{
    public const string path = "items";
    public GameObject _spawnObjParent;
    public int numberOfCube;
    public GameObject cubePrefab;
    public Material blue;
    public Material red;
    public Vector3 center;
    public Vector3 size;

    private int _scoreUpdateRed;
    private int _scoreUpdateBlue;
    private GameObject _tempGameObject;
	
	public CubeData _cueData
	{
		get;
		set;
	}

    private void Start()
    {
        Debug.Log("Application.persistentDataPath = " + Application.persistentDataPath);
		CubeData cube = new CubeData();
		cube.cubeInfo.Insert (0, new Cues ());
		cube.cubeInfo [0].cubeName = "blue";
		cube.cubeInfo[0].scorePoints = "20";
		cube.cubeInfo.Insert (0, new Cues ());
		cube.cubeInfo [0].cubeName = "red";
		cube.cubeInfo[0].scorePoints = "15";
		XMLSerializer.Save<CubeData> (Application.persistentDataPath + "/CubeData.txt", cube);

		_cueData = XMLSerializer.Load<CubeData> (Application.persistentDataPath + "/CubeData.txt");
		Debug.Log ("CueData = " + _cueData.cubeInfo[0].cubeName);

		Generate ();
    }


    void AddProperties()
    {
		for (int i = 0; i < _cueData.cubeInfo.Count; i++)
		{
			Debug.Log ("AddProperties called = " + _cueData.cubeInfo [i].cubeName);
			if (_cueData.cubeInfo [i].cubeName == "red")
			{
				_scoreUpdateRed = int.Parse (_cueData.cubeInfo [i].scorePoints);
			}
			else if (_cueData.cubeInfo [i].cubeName == "blue")
			{
				_scoreUpdateBlue = int.Parse (_cueData.cubeInfo [i].scorePoints);
			}	
			Debug.Log ("Sco = " + _cueData.cubeInfo [i].cubeName);
		}
    }

    public int ScoreUpdateRed
    {
        get
        {
            return _scoreUpdateRed;
        }
        set
        {
            _scoreUpdateRed = value;
        }
    }

    public int ScoreUpdateBlue
    {
        get
        {
            return _scoreUpdateBlue;
        }
        set
        {
            _scoreUpdateBlue = value;
        }
    }

    void Generate()
    {
		AddProperties ();
		for (int i = 0; i < numberOfCube; i++) 
		{

			Vector3 pos = center + new Vector3 (Random.Range (-size.x / 2, size.x / 2), 0, Random.Range (-size.z / 2, size.z / 2));
			_tempGameObject = Instantiate (cubePrefab, pos, Quaternion.identity);

			int m = Random.Range (1, 3);
           
			switch (m)
			{
				case 1:
						_tempGameObject.GetComponent<Renderer> ().material = red;
						_tempGameObject.name = "RedCube";
						break;
				case 2:
						_tempGameObject.GetComponent<Renderer> ().material = blue;
						_tempGameObject.name = "BlueCube";
						break;
				default:
						break;
			}
			_tempGameObject.transform.SetParent (_spawnObjParent.transform);
		}
    }
}
