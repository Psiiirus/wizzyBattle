using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngameTextPopup : MonoBehaviour {
	
	public GameObject _TextPrefab;
	private List<GameObject>  _currentPopups = new List<GameObject>();
	private List<float>  		_currentPopupsY = new List<float>();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		for(int i=0;i<_currentPopups.Count;i++)
		{	
			if(_currentPopups[i].transform.position.y < _currentPopupsY[i])
			{	
				_currentPopups[i].transform.Translate(Vector3.up * Time.deltaTime * 2);
			}
			else
			{
				Destroy(_currentPopups[i]);
				_currentPopups.Remove(_currentPopups[i]);
				_currentPopupsY.Remove(_currentPopupsY[i]);
			}
		}
	}
	
	public void destroyAll()
	{
		for(int i=0;i<_currentPopups.Count;i++)
		{	
			Destroy(_currentPopups[i]);	
		}
	}
	
	public void popupText(string aText,Vector3 aPos,float aPosYAnimated)
	{
		GameObject popupText = Instantiate(_TextPrefab) as GameObject;
		
		popupText.transform.position = aPos;
		popupText.GetComponent<TextMesh>().text = aText;
		
		_currentPopups.Add(popupText);
		_currentPopupsY.Add(aPosYAnimated);
	}
}
