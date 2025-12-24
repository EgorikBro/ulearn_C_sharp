using System;
using Avalonia.Input;
using Digger.Architecture;

namespace Digger;

public class Terrain : ICreature
{
	public string GetImageFileName()
	{
		return "Terrain.png";
	}

	public int GetDrawingPriority()
	{
		return 2;
	}

	public CreatureCommand Act(int x, int y)
	{
		return new CreatureCommand();
	}

	public bool DeadInConflict(ICreature conflictedObject)
	{
		return true;
	}
}

public class Player : ICreature
{
	public string GetImageFileName()
	{
		return "Digger.png";
	}

	public int GetDrawingPriority()
	{
		return 1;
	}

	public CreatureCommand Act(int x, int y)
	{
		var command = new CreatureCommand();

		if (Game.KeyPressed == Key.Left && x > 0)
			command.DeltaX = -1;
		else if (Game.KeyPressed == Key.Right && x < Game.MapWidth - 1)
			command.DeltaX = 1;
		else if (Game.KeyPressed == Key.Up && y > 0)
			command.DeltaY = -1;
		else if (Game.KeyPressed == Key.Down && y < Game.MapHeight - 1)
			command.DeltaY = 1;
      
		return command;
	}

	public bool DeadInConflict(ICreature conflictedObject)
	{
		return conflictedObject is not Terrain;
	}
}
