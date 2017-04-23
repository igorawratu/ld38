using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {
	public GameObject grid_parent_;
	public GameObject reference_grid_parent_;

	public GameObject cell_prefab_;

    public GameObject execute_;
    public GameObject finish_;
    public GameObject level_complete_;
    public Text last_best_;
    public Text loading_next_;

    public GameObject clicksound_;
    public GameObject executesound_;

	private CommandExecutor[] executors_;
	private int current_level_ = 0;
	private Image[] ui_grid_;
	private Image[] reference_ui_grid_;
	private HashSet<Command> commands_;
    private CommandPlacer placer_;

    public Text levelLabel;

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

		float half_grid_width = grid_parent_.GetComponent<RectTransform>().rect.width / 2;
        float half_grid_height = grid_parent_.GetComponent<RectTransform>().rect.height / 2;
        float half_reference_grid = reference_grid_parent_.GetComponent<RectTransform>().rect.width / 2;

		for (int i = 0; i < grid_dim * grid_dim; ++i)
		{
			var gridcell = Instantiate(cell_prefab_);
			var referencegridcell = Instantiate(cell_prefab_);

			int x = i % grid_dim;
			int y = i / grid_dim;

			var gridcell_rt = gridcell.GetComponent<RectTransform>();
			gridcell.GetComponent<RectTransform>().position = new Vector3(x * grid_cell_width - half_grid_width + grid_cell_width / 2,
				y * grid_cell_height - half_grid_height + grid_cell_width / 2, 0);
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
        placer_ = FindObjectOfType<CommandPlacer>();

        Restart();
    }
	
	private void UpdateCellColours()
	{
		for(int i = 0; i < ui_grid_.Length; ++i)
		{
			ui_grid_[i].color = SystemState.Instance.Grid[i] ? new Color(0, 0.75f, 0, 1) : new Color(0, 0, 0.75f, 1);
            reference_ui_grid_[i].color = Levels.Instance.ReferenceGrids[current_level_][i] ? new Color(0, 0.75f, 0, 1) : new Color(0, 0, 0.75f, 1);
		}
	}

	// Update is called once per frame
	void Update () {
		UpdateCellColours();

        levelLabel.text = "Level " + (current_level_ + 1);
        last_best_.text = Levels.Instance.scores_[current_level_] == 0 ? "" : "Fewest Moves: " + Levels.Instance.scores_[current_level_];
    }

	public void Execute()
	{
        Instantiate(executesound_);
		if (!Run())
		{
			return;
		}

		if (Compare())
		{
            execute_.SetActive(false);
            finish_.SetActive(true);
            level_complete_.SetActive(true);

            int score = 0;

            for(int i = 0; i < SystemState.Instance.CommandCounter.Length; ++i)
            {
                score = Math.Max(score, SystemState.Instance.CommandCounter[i]);
            }

            Levels.Instance.scores_[current_level_] = Levels.Instance.scores_[current_level_] == 0 ? score : Math.Min(score, Levels.Instance.scores_[current_level_]);
        }
	}

    public void NextLevel()
    {
        StartCoroutine(LoadNextLevel());

        Instantiate(clicksound_);
    }

    public void Restart()
    {
        foreach (var executor in executors_)
        {
            executor.RemoveCommands();
        }

        foreach (var command in commands_)
        {
            Destroy(command.gameObject);
        }
        commands_.Clear();

        SystemState.Instance.Reset();

        execute_.SetActive(true);
        finish_.SetActive(false);
        level_complete_.SetActive(false);

        Instantiate(clicksound_);
    }

	private IEnumerator LoadNextLevel()
	{
        //display success message and check if game is over
        level_complete_.SetActive(false);
        loading_next_.text = "Loading next level in 3";
        for (int i = 0; i < 3; ++i)
		{
			yield return new WaitForSeconds(1f);
            loading_next_.text = "Loading next level in " + (2 - i);
            //display waiting message
        }

		current_level_++;

        if(current_level_ >= Levels.Instance.ReferenceGrids.Count)
        {
            SceneManager.LoadScene("End");
        }

		foreach (var executor in executors_)
		{
			executor.RemoveCommands();
		}

        placer_.RemoveAllCommands();

        loading_next_.text = "";
        SystemState.Instance.Reset();
        execute_.SetActive(true);
        finish_.SetActive(false);
        
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
        Instantiate(clicksound_);
        foreach (var executor in executors_)
		{
			executor.AddCommand(command);
		}
		commands_.Add(command);
	}

	public void RemoveCommand(Command command)
	{
        Instantiate(clicksound_);
        foreach (var executor in executors_)
		{
			executor.RemoveCommand(command);
		}
		commands_.Remove(command);
	}
}
