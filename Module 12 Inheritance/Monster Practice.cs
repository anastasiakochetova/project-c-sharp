using System;
using Avalonia.Input;

namespace Digger.Architecture;

public class Terrain : ICreature
{
	public string GetImageFileName() => "Terrain.png";

	public int GetDrawingPriority() => 0;

	public CreatureCommand Act(int x, int y) =>
		new() { DeltaX = 0, DeltaY = 0 };

	public bool DeadInConflict(ICreature conflictedObject)
	{
		// Террейн разрушается игроком и монстром
		return conflictedObject is Player || conflictedObject is Monster;
	}
}

public class Player : ICreature
{
	public string GetImageFileName() => "Digger.png";

	public int GetDrawingPriority() => 1;

	public CreatureCommand Act(int x, int y)
	{
		var (deltaX, deltaY) = GetMovementDirection();
		var newX = x + deltaX;
		var newY = y + deltaY;

		if (!IsWithinBounds(newX, newY) || !CanMoveTo(newX, newY))
			return new CreatureCommand { DeltaX = 0, DeltaY = 0 };

		return new CreatureCommand { DeltaX = deltaX, DeltaY = deltaY };
	}

	private (int deltaX, int deltaY) GetMovementDirection()
	{
		return Game.KeyPressed switch
		{
			Key.Left => (-1, 0),
			Key.Right => (1, 0),
			Key.Up => (0, -1),
			Key.Down => (0, 1),
			_ => (0, 0)
		};
	}

	private static bool IsWithinBounds(int x, int y) =>
		x >= 0 && x < Game.MapWidth && y >= 0 && y < Game.MapHeight;

	private static bool CanMoveTo(int x, int y)
	{
		var creature = Game.Map[x, y];
		// Игрок не может войти в мешок
		return creature is not Sack;
	}

	public bool DeadInConflict(ICreature conflictedObject)
	{
		// Игрок погибает от падающего мешка и монстра
		return conflictedObject is Sack || conflictedObject is Monster;
	}
}

public class Sack : ICreature
{
	private int fallingDistance;
	private bool isFalling;

	public bool IsFalling => isFalling && fallingDistance > 0;

	public string GetImageFileName() => "Sack.png";

	public int GetDrawingPriority() => 2;

	public CreatureCommand Act(int x, int y)
	{
		TryStartFalling(x, y);

		if (isFalling)
			return HandleFallingState(x, y);

		return CreateStationaryCommand();
	}

	private void TryStartFalling(int x, int y)
	{
		if (!isFalling && CanStartFalling(x, y))
		{
			isFalling = true;
			fallingDistance = 0;
		}
	}

	private CreatureCommand HandleFallingState(int x, int y)
	{
		if (CanContinueFalling(x, y))
			return ContinueFalling();

		return LandSafely(x, y);
	}

	private CreatureCommand ContinueFalling()
	{
		fallingDistance++;
		return CreateMoveDownCommand();
	}

	private CreatureCommand LandSafely(int x, int y)
	{
		isFalling = false;

		if (ShouldTransformToGold(x, y))
			return CreateTransformToGoldCommand();

		return CreateStationaryCommand();
	}

	private static CreatureCommand CreateMoveDownCommand() =>
		new() { DeltaX = 0, DeltaY = 1 };

	private static CreatureCommand CreateStationaryCommand() =>
		new() { DeltaX = 0, DeltaY = 0 };

	private static CreatureCommand CreateTransformToGoldCommand() =>
		new()
		{
			DeltaX = 0,
			DeltaY = 0,
			TransformTo = new Gold()
		};

	private static bool CanStartFalling(int x, int y)
	{
		if (y + 1 >= Game.MapHeight)
			return false;

		var creatureBelow = Game.Map[x, y + 1];
		return creatureBelow == null;
	}

	private static bool CanContinueFalling(int x, int y)
	{
		if (y + 1 >= Game.MapHeight)
			return false;

		var creatureBelow = Game.Map[x, y + 1];

		// Мешок может падать через пустоту, игрока и монстра
		return creatureBelow == null || creatureBelow is Player || creatureBelow is Monster;
	}

	private bool ShouldTransformToGold(int x, int y)
	{
		if (fallingDistance <= 1)
			return false;

		var landingY = y + 1;

		if (landingY >= Game.MapHeight)
			return true;

		var creatureBelow = Game.Map[x, landingY];

		return creatureBelow is Terrain
		       || creatureBelow is Sack
		       || creatureBelow is Gold
		       || creatureBelow is Monster;
	}

	public bool DeadInConflict(ICreature conflictedObject)
	{
		// Сам мешок не разрушается при конфликтах
		return false;
	}
}

public class Gold : ICreature
{
	public string GetImageFileName() => "Gold.png";

	public int GetDrawingPriority() => 3;

	public CreatureCommand Act(int x, int y) =>
		new() { DeltaX = 0, DeltaY = 0 };

	public bool DeadInConflict(ICreature conflictedObject)
	{
		if (conflictedObject is Player)
		{
			Game.Scores += 10;
			return true;
		}

		// Золото исчезает, если в клетку приходит монстр
		return conflictedObject is Monster;
	}
}

public class Monster : ICreature
{
	public string GetImageFileName() => "Monster.png";

	// Рисуем монстра поверх земли и игроков, но под золотом
	public int GetDrawingPriority() => 2;

	public CreatureCommand Act(int x, int y)
	{
		// Если на карте нет диггера — стоим на месте
		if (!TryFindPlayer(out var playerX, out var playerY))
			return new CreatureCommand { DeltaX = 0, DeltaY = 0 };

		// Двигаемся к игроку по горизонтали или вертикали
		var (dx, dy) = ChooseDirectionTowardsPlayer(x, y, playerX, playerY);

		if (dx == 0 && dy == 0)
			return new CreatureCommand { DeltaX = 0, DeltaY = 0 };

		var targetX = x + dx;
		var targetY = y + dy;

		if (!IsWithinBounds(targetX, targetY))
			return new CreatureCommand { DeltaX = 0, DeltaY = 0 };

		var target = Game.Map[targetX, targetY];

		// Монстр не может ходить сквозь землю, мешки и других монстров
		if (target is Terrain || target is Sack || target is Monster)
			return new CreatureCommand { DeltaX = 0, DeltaY = 0 };

		// В клетку с золотом или игроком монстр может войти
		return new CreatureCommand { DeltaX = dx, DeltaY = dy };
	}

	private static bool TryFindPlayer(out int playerX, out int playerY)
	{
		for (var x = 0; x < Game.MapWidth; x++)
		for (var y = 0; y < Game.MapHeight; y++)
			if (Game.Map[x, y] is Player)
			{
				playerX = x;
				playerY = y;
				return true;
			}

		playerX = -1;
		playerY = -1;
		return false;
	}

	private static (int dx, int dy) ChooseDirectionTowardsPlayer(int x, int y, int playerX, int playerY)
	{
		var dx = Math.Sign(playerX - x);
		var dy = Math.Sign(playerY - y);

		// Сначала пробуем горизонталь, потом вертикаль
		if (dx != 0 && CanMoveTo(x + dx, y))
			return (dx, 0);

		if (dy != 0 && CanMoveTo(x, y + dy))
			return (0, dy);

		return (0, 0);
	}

	private static bool CanMoveTo(int x, int y)
	{
		if (!IsWithinBounds(x, y))
			return false;

		var target = Game.Map[x, y];
		return target is not Terrain
		       && target is not Sack
		       && target is not Monster;
	}

	private static bool IsWithinBounds(int x, int y) =>
		x >= 0 && x < Game.MapWidth && y >= 0 && y < Game.MapHeight;

	public bool DeadInConflict(ICreature conflictedObject)
	{
		// Монстры, пришедшие в одну клетку, все умирают
		// Падающий мешок убивает монстра
		return conflictedObject is Monster
		       || (conflictedObject is Sack sack && sack.IsFalling);
	}
}
