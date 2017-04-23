using UnityEngine;
using System.Collections.Generic;

public class Levels : Singleton<Levels> {

	List<bool[]> reference_grids_;
    public int[] scores_;
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
        for (int i = 0; i < grid.Length; ++i)
        {
            int x = i % SystemState.Instance.grid_dimensions_;
            int y = i / SystemState.Instance.grid_dimensions_;

            grid[i] = x == 3;
            Debug.Log(grid[i]);
        }

        return grid;
    }

    private bool[] CreateLevel2()
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

    private bool[] CreateLevel3()
    {
        bool[] grid = new bool[SystemState.Instance.grid_dimensions_ * SystemState.Instance.grid_dimensions_];
        for (int i = 0; i < grid.Length; ++i)
        {
            int x = i % SystemState.Instance.grid_dimensions_;
            int y = i / SystemState.Instance.grid_dimensions_;

            grid[i] = x == 0 || y == 0 || x == SystemState.Instance.grid_dimensions_ - 1 || y == SystemState.Instance.grid_dimensions_ - 1;
        }

        return grid;
    }

    private bool[] CreateLevel4()
    {
        bool[] grid = new bool[SystemState.Instance.grid_dimensions_ * SystemState.Instance.grid_dimensions_];
        for (int i = 0; i < grid.Length; ++i)
        {
            int x = i % SystemState.Instance.grid_dimensions_;
            int y = i / SystemState.Instance.grid_dimensions_;

            grid[i] = (x + y) > 2 && (x + y < 5);
        }

        return grid;
    }

    private bool[] CreateLevel5()
    {
        bool[] grid = new bool[SystemState.Instance.grid_dimensions_ * SystemState.Instance.grid_dimensions_];
        for (int i = 0; i < grid.Length; ++i)
        {
            int x = i % SystemState.Instance.grid_dimensions_;
            int y = i / SystemState.Instance.grid_dimensions_;

            grid[i] = (x + y) > 2 || (x - y) < 2;
        }

        return grid;
    }

    private void InitReferenceGrids()
	{
		reference_grids_ = new List<bool[]>();
		reference_grids_.Add(CreateLevel1());
        reference_grids_.Add(CreateLevel2());
        reference_grids_.Add(CreateLevel3());
        reference_grids_.Add(CreateLevel4());
        reference_grids_.Add(CreateLevel5());

        scores_ = new int[reference_grids_.Count];
    }

	// Use this for initialization
	void Awake () {
		InitReferenceGrids();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
