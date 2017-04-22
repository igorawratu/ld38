using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
	public GameObject grid_parent_;
	public GameObject reference_grid_parent_;

	public GameObject cell_prefab_;

	private CommandExecutor[] executors_;
	private int current_level_;
	private Image[] ui_grid_;
	private Image[] reference_ui_grid_;
	private HashSet<Command> commands_;

	private void InitGrids()
	{
		int grid_dim = SystemState.Instance.grid_dimensions_;
		var grid_parent_rt = grid_parent_.GetComponent<RectTransform>();
		var reference_parent_rt = reference_grid_parent_.GetComponent<RectTransform>();

		float grid_cell_width = grid_parent_rt.rect.width / grid_dim;
		float grid_cell_height = grid_parent_rt.rect.height / grid_dim;
		float reference_grid_cell_width = reference_parent_rt.rect.width / grid_dim;
		float reference_grid_cell_height = reference_parent_rt.rect.height / grid_dim;

		ui_grid_ = new Image[grid_dim * grid_dim];
		reference_ui_grid_ = new Image[grid_dim * grid_dim];

		float half_grid = grid_parent_.GetComponent<RectTransform>().rect.width / 2;
		float half_reference_grid = reference_grid_parent_.GetComponent<RectTransform>().rect.width / 2;

		for (int i = 0; i < grid_dim * grid_dim; ++i)
		{
			var gridcell = Instantiate(cell_prefab_);
			var referencegridcell = Instantiate(cell_prefab_);

			int x = i % grid_dim;
			int y = i / grid_dim;

			var gridcell_rt = gridcell.GetComponent<RectTransform>();
			gridcell.GetComponent<RectTransform>().position = new Vector3(x * grid_cell_width - half_grid + grid_cell_width / 2,
				y * grid_cell_height - half_grid + grid_cell_width / 2, 0);
			gridcell.GetComponent<RectTransform>().sizeDelta = new Vector2(grid_cell_width, grid_cell_height);
			gridcell_rt.SetParent(grid_parent_rt, false);

			var reference_gridcell_rt = referencegridcell.GetComponent<RectTransform>();
			
			reference_gridcell_rt.position = new Vector3(x * reference_grid_cell_width - half_reference_grid + reference_grid_cell_width / 2,
				y * reference_grid_cell_height - half_reference_grid + reference_grid_cell_width / 2, 0);
			reference_gridcell_rt.sizeDelta = new Vector2(reference_grid_cell_width, reference_grid_cell_height);
			reference_gridcell_rt.SetParent(reference_parent_rt, false);

			ui_grid_[i] = gridcell.GetComponent<Image>();
			reference_ui_grid_[i] = referencegridcell.GetComponent<Image>();
		}
	}

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
		SystemState.Instance.grid_dimensions_ = 5;

		InitPokemans();
		InitGrids();

		commands_ = new HashSet<Command>();
	}
	
	private void UpdateCellColours()
	{
		for(int i = 0; i < ui_grid_.Length; ++i)
		{
			ui_grid_[i].color = SystemState.Instance.Grid[i] ? Color.black : Color.white;
			reference_ui_grid_[i].color = Levels.Instance.ReferenceGrids[current_level_][i] ? Color.black : Color.white;
		}
	}

	// Update is called once per frame
	void Update () {
		UpdateCellColours();
	}

	public void Execute()
	{
		if (!Run())
		{
			return;
		}

		if (Compare())
		{
			StartCoroutine(LoadNextLevel());
		}
		else
		{
			//failed, try agian
		}
	}

	private IEnumerator LoadNextLevel()
	{
		//display success message and check if game is over

		for(int i = 0; i < 3; ++i)
		{
			yield return new WaitForSeconds(1f);
			//display waiting message
		}

		current_level_++;
		foreach (var executor in executors_)
		{
			executor.RemoveCommands();
		}

		foreach(var command in commands_)
		{
			Destroy(command.gameObject);
		}
		commands_.Clear();

		SystemState.Instance.Reset();
	}

	private bool Run()
	{
		if(executors_ == null)
		{
			return false;
		}

		SystemState.Instance.Reset();
		for(int i = 0; i < executors_.Length; ++i)
		{
			int x = i % SystemState.Instance.grid_dimensions_;
			int y = i / SystemState.Instance.grid_dimensions_;
			executors_[i].Execute(x, y);
		}

		return true;
	}

	private bool Compare()
	{
		for(int i = 0; i < SystemState.Instance.Grid.Length; ++i)
		{
			if (SystemState.Instance.Grid[i] != Levels.Instance.ReferenceGrids[current_level_][i])
			{
				return false;
			}
		}

		return true;
	}

	public void AddCommand(Command command)
	{
		foreach(var executor in executors_)
		{
			executor.AddCommand(command);
		}
		commands_.Add(command);
	}

	public void RemoveCommand(Command command)
	{
		foreach(var executor in executors_)
		{
			executor.RemoveCommand(command);
		}
		commands_.Remove(command);
	}
}
