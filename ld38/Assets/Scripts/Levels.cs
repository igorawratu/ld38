using UnityEngine;
using System.Collections.Generic;

public class Levels : Singleton<Levels> {

	List<bool[]> reference_grids_;
	public List<bool[]> ReferenceGrids
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

	private bool[] CreateLevel1()
	{
		bool[] grid = new bool[SystemState.Instance.grid_dimensions_ * SystemState.Instance.grid_dimensions_];
		for(int i = 0; i < grid.Length; ++i)
		{
			int x = i % SystemState.Instance.grid_dimensions_;
			int y = i / SystemState.Instance.grid_dimensions_;

			grid[i] = x == y;
		}

		return grid;
	}

	private void InitReferenceGrids()
	{
		reference_grids_ = new List<bool[]>();
		reference_grids_.Add(CreateLevel1());
	}

	// Use this for initialization
	void Start () {
		InitReferenceGrids();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
