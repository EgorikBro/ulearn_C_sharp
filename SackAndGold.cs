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

		if (command.DeltaX != 0 || command.DeltaY != 0)
		{
			var targetCreature = Game.Map[x + command.DeltaX, y + command.DeltaY];
			if (targetCreature is Sack)
			{
				command.DeltaX = 0;
				command.DeltaY = 0;
			}
		}

		return command;
	}

	public bool DeadInConflict(ICreature conflictedObject)
	{
		if (conflictedObject is Sack) return true;
		if (conflictedObject is Gold) return false;
		return conflictedObject is not Terrain;
	}
}

public class Sack : ICreature
{
	private int fallCounter = 0;

	public string GetImageFileName()
	{
		return "Sack.png";
	}

	public int GetDrawingPriority()
	{
		return 0;
	}

	public CreatureCommand Act(int x, int y)
	{
		var command = new CreatureCommand();
		var canFall = y < Game.MapHeight - 1;
		
		if (canFall)
		{
			var creatureBelow = Game.Map[x, y + 1];
			if (creatureBelow == null || (creatureBelow is Player && fallCounter > 0))
			{
				fallCounter++;
				command.DeltaY = 1;
				return command;
			}
		}

		if (fallCounter > 1)
		{
			command.TransformTo = new Gold();
			fallCounter = 0;
		}
		else
		{
			fallCounter = 0;
		}

		return command;
	}

	public bool DeadInConflict(ICreature conflictedObject)
	{
		return false;
	}
}

public class Gold : ICreature
{
	public string GetImageFileName()
	{
		return "Gold.png";
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
		if (conflictedObject is Player)
		{
			Game.Scores += 10;
			return true;
		}
		return false;
	}
}
