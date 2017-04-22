using UnityEngine;
using System.Collections.Generic;

public class SystemState : Singleton<SystemState> {
	public int grid_dimensions_ = 1;

	private int[] r0_;
	public int[] r0
	{
		get
		{
			if(r0_ == null)
			{
				InitR0();
			}

			return r0_;
		}
	}

	private bool[] c_;
	public bool[] c
	{
		get
		{
			if(c_ == null)
			{
				InitConditionals();
			}

			return c_;
		}
	}

	private bool[] grid_;
	public bool[] Grid {
		get
		{
			if (grid_ == null)
			{
				InitGrid();
			}

			return grid_;
		}
	}

	private int[] command_counter_;
	public int[] CommandCounter
	{
		get
		{
			if(command_counter_ == null)
			{
				InitCommandCounters();
			}

			return command_counter_;
		}
	}

	private void InitCommandCounters()
	{
		command_counter_ = new int[grid_dimensions_ * grid_dimensions_];
		for (int i = 0; i < grid_dimensions_ * grid_dimensions_; ++i)
		{
			command_counter_[i] = 0;
		}
	}

	private void InitGrid()
	{
		grid_ = new bool[grid_dimensions_ * grid_dimensions_];
		for(int i = 0; i < grid_dimensions_ * grid_dimensions_; ++i)
		{
			grid_[i] = false;
		}
	}

	private void InitR0()
	{
		r0_ = new int[grid_dimensions_ * grid_dimensions_];
		for (int i = 0; i < grid_dimensions_ * grid_dimensions_; ++i)
		{
			r0_[i] = 0;
		}
	}

	private void InitConditionals()
	{
		c_ = new bool[grid_dimensions_ * grid_dimensions_];
		for (int i = 0; i < grid_dimensions_ * grid_dimensions_; ++i)
		{
			c_[i] = true;
		}
	}

	// Use this for initialization
	void Start () {
		Reset();
	}

	public void Reset()
	{
		InitGrid();
		InitR0();
		InitConditionals();
		InitCommandCounters();
	}
}
