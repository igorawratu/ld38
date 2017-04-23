using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class Command : MonoBehaviour {
	public enum CommandType { GreaterThan, LessThan, EqualTo, Plus, Minus, Modulo, Multiply, Divide, SetColor }
	public CommandType command_type_;
	public InputField input_field_a_;
	public InputField input_field_b_;

	private delegate void CommandDelegate(int a, int b, int x, int y);
	private Dictionary<CommandType, CommandDelegate> command_delegates_;

	void Start()
	{
		command_delegates_ = new Dictionary<CommandType, CommandDelegate>();
		command_delegates_[CommandType.GreaterThan] = (int a, int b, int x, int y) => 
			SystemState.Instance.c[SystemState.Instance.grid_dimensions_ * y + x] = a > b;
		command_delegates_[CommandType.LessThan] = (int a, int b, int x, int y) => 
			SystemState.Instance.c[SystemState.Instance.grid_dimensions_ * y + x] = a < b;
		command_delegates_[CommandType.EqualTo] = (int a, int b, int x, int y) => 
			SystemState.Instance.c[SystemState.Instance.grid_dimensions_ * y + x] = a == b;
		command_delegates_[CommandType.Plus] = (int a, int b, int x, int y) => 
			SystemState.Instance.r0[SystemState.Instance.grid_dimensions_ * y + x] = a + b;
		command_delegates_[CommandType.Minus] = (int a, int b, int x, int y) => 
			SystemState.Instance.r0[SystemState.Instance.grid_dimensions_ * y + x] = a - b;
		command_delegates_[CommandType.Modulo] = (int a, int b, int x, int y) => 
			SystemState.Instance.r0[SystemState.Instance.grid_dimensions_ * y + x] = b == 0 ? 0 : a % b;
		command_delegates_[CommandType.Multiply] = (int a, int b, int x, int y) => 
			SystemState.Instance.r0[SystemState.Instance.grid_dimensions_ * y + x] = a * b;
		command_delegates_[CommandType.Divide] = (int a, int b, int x, int y) => 
			SystemState.Instance.r0[SystemState.Instance.grid_dimensions_ * y + x] = b == 0 ? 0 : a / b;
		command_delegates_[CommandType.SetColor] = (int a, int b, int x, int y) =>
			SystemState.Instance.Grid[SystemState.Instance.grid_dimensions_ * y + x] = true;
	}

	public void Execute(int x, int y)
	{
		int a, b;

		if (input_field_a_ != null)
		{
			var texta = input_field_a_.text;

			if (texta == "x")
			{
				a = x;
			}
			else if (texta == "y")
			{
				a = y;
			}
			else if (texta == "r")
			{
				a = SystemState.Instance.r0[SystemState.Instance.grid_dimensions_ * y + x];
			}
			else
			{
				int n;
				bool isNumeric = int.TryParse(texta, out n);

				a = isNumeric ? n : 0;
			}
		}
		else a = 0;

		if (input_field_b_ != null)
		{
			var textb = input_field_b_.text;

			if (textb == "x")
			{
				b = x;
			}
			else if (textb == "y")
			{
				b = y;
			}
			else if (textb == "r")
			{
				b = SystemState.Instance.r0[SystemState.Instance.grid_dimensions_ * y + x];
			}
			else
			{
				int n;
				bool isNumeric = int.TryParse(textb, out n);

				b = isNumeric ? n : 0;
			}
		}
		else b = 0;

		command_delegates_[command_type_](a, b, x, y);
	}
}
