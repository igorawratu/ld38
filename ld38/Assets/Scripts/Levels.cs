using UnityEngine;
using System.Collections.Generic;

public class Levels : MonoBehaviour {

	List<Color[]> reference_grids_;
	public List<Color[]> ReferenceGrids
	{
		get
		{
			if(reference_grids_ == null)
			{
				InitReferenceGrids();
			}

			return reference_grids_;
		}
	}

	private void InitReferenceGrids()
	{

	}

	// Use this for initialization
	void Start () {
		InitReferenceGrids();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
