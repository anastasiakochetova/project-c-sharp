using Avalonia.Input;

namespace Digger.Architecture;

public class Terrain : ICreature
{
	public string GetImageFileName()
	{
		return "Terrain.png";
	}

	//Приоритет отрисовки определяет порядок наложения спрайтов.
	//Terrain имеет приоритет 0 (низкий), поэтому рисуется 1-м и находится под др. объектами.
	public int GetDrawingPriority()
	{
		return 0;
	}

	//Terrain статичен и не перемещается, поэтому всегда возвращает команду без движения.
	public CreatureCommand Act(int x, int y)
	{
		return new CreatureCommand { DeltaX = 0, DeltaY = 0 };
	}

	public bool DeadInConflict(ICreature conflictedObject)
	{
		return conflictedObject is Player;
	}
}

public class Player : ICreature
{
	public string GetImageFileName()
	{
		return "Digger.png";
	}

	//Приоритет 1 выше, чем у Terrain (0), поэтому игрок рисуется поверх земли.
	public int GetDrawingPriority()
	{
		return 1;
	}

	//Определяем направление движения на основе последней нажатой клавиши (Game.KeyPressed)
	//Вычисляем целевую позицию с учетом текущих координат (x, y)
	//Валидируем целевую позицию: проверяем, не выходит ли она за границы карты
	//Если валидация пройдена - возвращаем команду движения, иначе - команду остаться на месте
	public CreatureCommand Act(int x, int y)
	{
		var (deltaX, deltaY) = GetMovementDirection();
		int newX = x + deltaX;
		int newY = y + deltaY;

		// Проверка границ: если целевая позиция выходит за пределы массива Game.Map,
		// игрок не может туда переместиться. Возвращаем команду без движения,
		if (!IsWithinBounds(newX, newY))
		{
			return new CreatureCommand { DeltaX = 0, DeltaY = 0 };
		}

		return new CreatureCommand { DeltaX = deltaX, DeltaY = deltaY };
	}

	private (int deltaX, int deltaY) GetMovementDirection()
	{
		int deltaX = 0;
		int deltaY = 0;

		switch (Game.KeyPressed)
		{
			case Key.Left:
				deltaX = -1;
				break;
			case Key.Right:
				deltaX = 1;
				break;
			case Key.Up:
				deltaY = -1;
				break;
			case Key.Down:
				deltaY = 1;
				break;
		}

		return (deltaX, deltaY);
	}

	private bool IsWithinBounds(int x, int y)
	{
		return x >= 0 && x < Game.MapWidth && y >= 0 && y < Game.MapHeight;
	}

	/// система разрешения конфликтов вызывает:
	/// - Terrain.DeadInConflict(Player) -> true (террейн умирает)
	/// - Player.DeadInConflict(Terrain) -> false (игрок выживает)
	public bool DeadInConflict(ICreature conflictedObject)
	{
		return false;
	}
}
