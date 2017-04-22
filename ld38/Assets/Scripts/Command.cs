using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Command : MonoBehaviour{
	public enum CommandType { GreaterThan, LessThan, EqualTo, Plus, Minus, Modulo, Multiply, Divide, SetColor }
	public CommandType command_type_;
	public int A { get; set; }
	public int B { get; set; }

	public enum ValueType { Constant, X, Y, R0 }
	public ValueType aValueType;
	public ValueType bValueType;

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
			SystemState.Instance.r0[SystemState.Instance.grid_dimensions_ * y + x] = a % b;
		command_delegates_[CommandType.Multiply] = (int a, int b, int x, int y) => 
			SystemState.Instance.r0[SystemState.Instance.grid_dimensions_ * y + x] = a * b;
		command_delegates_[CommandType.Divide] = (int a, int b, int x, int y) => 
			SystemState.Instance.r0[SystemState.Instance.grid_dimensions_ * y + x] = a % b;
		command_delegates_[CommandType.SetColor] = (int a, int b, int x, int y) =>
			SystemState.Instance.Grid[SystemState.Instance.grid_dimensions_ * y + x] = Color.black;

		A = 0;
		B = 0;

		aValueType = ValueType.Constant;
		bValueType = ValueType.Constant;
	}

	public void Execute(int x, int y)
	{
		int a, b;

		switch (aValueType)
		{
			case ValueType.Constant:
				a = A;
				break;
			case ValueType.R0:
				a = SystemState.Instance.r0[SystemState.Instance.grid_dimensions_ * y + x];
				break;
			case ValueType.X:
				a = x;
				break;
			case ValueType.Y:
				a = y;
				break;
			default:
				a = A;
				break;
		}

		switch (bValueType)
		{
			case ValueType.Constant:
				b = B;
				break;
			case ValueType.R0:
				b = SystemState.Instance.r0[SystemState.Instance.grid_dimensions_ * y + x];
				break;
			case ValueType.X:
				b = x;
				break;
			case ValueType.Y:
				b = y;
				break;
			default:
				b = B;
				break;
		}

		command_delegates_[command_type_](a, b, x, y);
	}
}
