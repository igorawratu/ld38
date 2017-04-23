using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPlacer : MonoBehaviour {
    public GameObject greater_prefab_;
    public GameObject less_prefab_;
    public GameObject equal_prefab_;
    public GameObject plus_prefab_;
    public GameObject minus_prefab_;
    public GameObject mod_prefab_;
    public GameObject mult_prefab_;
    public GameObject div_prefab_;
    public GameObject setcolor_prefab_;

    public GameObject commands_parent_;

    private List<GameObject> current_commands_;

    private Game game_;

    // Use this for initialization
    void Start () {
        game_ = FindObjectOfType<Game>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OrganizeCommands()
    {
        var parent_rt = commands_parent_.GetComponent<RectTransform>();

        for(int i = 0; i < current_commands_.Count; ++i)
        {
            var rt = current_commands_[i].GetComponent<RectTransform>();
			float command_height_ = rt.rect.height;

			rt.SetParent(null);
            rt.position = new Vector3(0, i * (command_height_ + 5f), 0);
            rt.SetParent(parent_rt, false);
        }
    }

    private void AddCommand(Command.CommandType commandType)
    {
        GameObject command;

        switch (commandType)
        {
            case Command.CommandType.GreaterThan:
                command = Instantiate(greater_prefab_);
                break;
            case Command.CommandType.LessThan:
                command = Instantiate(less_prefab_);
                break;
            case Command.CommandType.EqualTo:
                command = Instantiate(equal_prefab_);
                break;
            case Command.CommandType.Plus:
                command = Instantiate(plus_prefab_);
                break;
            case Command.CommandType.Minus:
                command = Instantiate(minus_prefab_);
                break;
            case Command.CommandType.Modulo:
                command = Instantiate(mod_prefab_);
                break;
            case Command.CommandType.Multiply:
                command = Instantiate(mult_prefab_);
                break;
            case Command.CommandType.Divide:
                command = Instantiate(div_prefab_);
                break;
            case Command.CommandType.SetColor:
            default:
                command = Instantiate(setcolor_prefab_);
                break;
        }

        current_commands_.Add(command);
        game_.AddCommand(command.GetComponent<Command>());
		command.GetComponent<RectTransform>().SetParent(commands_parent_.GetComponent<RectTransform>());


		OrganizeCommands();
    }

    public void RemoveCommand(GameObject command)
    {
        current_commands_.Remove(command);
        game_.RemoveCommand(command.GetComponent<Command>());
        OrganizeCommands();
    }
}
