using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	private CommandExecutor[] executors_;

	private bool executing_;

	private void InitPokemans()
	{
		int gridsize = SystemState.Instance.grid_dimensions_ * SystemState.Instance.grid_dimensions_;
		executors_ = new CommandExecutor[gridsize];

		for(int i = 0; i < gridsize; ++i)
		{
			executors_[i] = new CommandExecutor();
		}
	}

	// Use this for initialization
	void Start () {
		InitPokemans();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void Run()
	{
		if(executors_ == null || executing_)
		{
			return;
		}

		executing_ = true;
		SystemState.Instance.Reset();
		for(int i = 0; i < executors_.Length; ++i)
		{
			int x = i % SystemState.Instance.grid_dimensions_;
			int y = i / SystemState.Instance.grid_dimensions_;
			executors_[i].Execute(x, y);
		}

	}
}
