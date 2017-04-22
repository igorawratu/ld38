using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class CommandExecutor{
	public List<Command> commands_;
	public int current_command_;
	public bool Completed { get; set; }

	// Use this for initialization
	public CommandExecutor() {
		commands_ = new List<Command>();
		Completed = true;
	}

	public void AddCommand(Command command, int position)
	{
		position = Math.Max(0, Math.Min(commands_.Count, position));
		commands_.Insert(position, command);
	}

	public void RemoveCommand(Command command)
	{
		commands_.Remove(command);
	}

	public void Execute(int x, int y)
	{
		int coord = x + y * SystemState.Instance.grid_dimensions_;
		for (int i = 0; i < commands_.Count; ++i)
		{
			if (!SystemState.Instance.c[coord])
			{
				continue;
			}

			commands_[i].Execute(x, y);
			SystemState.Instance.CommandCounter[coord]++;
		}
	}
}
