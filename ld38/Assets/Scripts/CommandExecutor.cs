using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class CommandExecutor{
	public List<Command> commands_;

	// Use this for initialization
	public CommandExecutor() {
		commands_ = new List<Command>();
	}

	public void AddCommand(Command command)
	{
		commands_.Add(command);
	}

	public void RemoveCommand(Command command)
	{
		commands_.Remove(command);
	}

	public void RemoveCommands()
	{
		commands_.Clear();
	}

	public void Execute(int x, int y)
	{
		int coord = x + y * SystemState.Instance.grid_dimensions_;
		for (int i = 0; i < commands_.Count; ++i)
		{
			if (!SystemState.Instance.c[coord] && 
                commands_[i].command_type_ != Command.CommandType.EqualTo &&
                commands_[i].command_type_ != Command.CommandType.GreaterThan &&
                commands_[i].command_type_ != Command.CommandType.LessThan)
			{
                if (commands_[i].command_type_ != Command.CommandType.EqualTo &&
                commands_[i].command_type_ != Command.CommandType.GreaterThan &&
                commands_[i].command_type_ != Command.CommandType.LessThan)
                {
                    SystemState.Instance.c[coord] = true;
                }

                continue;
			}

			commands_[i].Execute(x, y);
            if(commands_[i].command_type_ != Command.CommandType.EqualTo &&
                commands_[i].command_type_ != Command.CommandType.GreaterThan &&
                commands_[i].command_type_ != Command.CommandType.LessThan)
            {
                SystemState.Instance.c[coord] = true;
            }
			SystemState.Instance.CommandCounter[coord]++;
		}
	}
}
