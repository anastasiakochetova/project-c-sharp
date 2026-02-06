using Avalonia.Input;

namespace Digger.Architecture;

public class Terrain : ICreature
{
    public string GetImageFileName()
    {
        return "Terrain.png";
    }
    
    public int GetDrawingPriority()
    {
        return 0;
    }
    
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
    
    public int GetDrawingPriority()
    {
        return 1;
    }
    
    public CreatureCommand Act(int x, int y)
    {
        var (deltaX, deltaY) = GetMovementDirection();
        int newX = x + deltaX;
        int newY = y + deltaY;

        if (!IsWithinBounds(newX, newY) || !CanMoveTo(newX, newY))
        {
            return new CreatureCommand { DeltaX = 0, DeltaY = 0 };
        }

        return new CreatureCommand { DeltaX = deltaX, DeltaY = deltaY };
    }

    private (int deltaX, int deltaY) GetMovementDirection()
    {
        switch (Game.KeyPressed)
        {
            case Key.Left:
                return (-1, 0);
            case Key.Right:
                return (1, 0);
            case Key.Up:
                return (0, -1);
            case Key.Down:
                return (0, 1);
            default:
                return (0, 0);
        }
    }

    private bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < Game.MapWidth && y >= 0 && y < Game.MapHeight;
    }
    
    private bool CanMoveTo(int x, int y)
    {
        var creature = Game.Map[x, y];
        return !(creature is Sack);
    }

    public bool DeadInConflict(ICreature conflictedObject)
    {
        return conflictedObject is Sack;
    }
}

public class Sack : ICreature
{
    private int fallingDistance = 0;
    private bool isFalling = false;

    public string GetImageFileName()
    {
        return "Sack.png";
    }
    
    public int GetDrawingPriority()
    {
        return 2;
    }

    public CreatureCommand Act(int x, int y)
    {
        TryStartFalling(x, y);
        
        if (isFalling)
        {
            return HandleFallingState(x, y);
        }
        
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
        {
            return ContinueFalling();
        }
        else
        {
            return LandSafely(x, y);
        }
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
        {
            return CreateTransformToGoldCommand();
        }
        
        return CreateStationaryCommand();
    }

    private CreatureCommand CreateMoveDownCommand()
    {
        return new CreatureCommand { DeltaX = 0, DeltaY = 1 };
    }

    private CreatureCommand CreateStationaryCommand()
    {
        return new CreatureCommand { DeltaX = 0, DeltaY = 0 };
    }

    private CreatureCommand CreateTransformToGoldCommand()
    {
        return new CreatureCommand 
        { 
            DeltaX = 0, 
            DeltaY = 0,
            TransformTo = new Gold()
        };
    }

    private bool CanStartFalling(int x, int y)
    {
        if (y + 1 >= Game.MapHeight)
        {
            return false;
        }
        
        var creatureBelow = Game.Map[x, y + 1];
        return creatureBelow == null;
    }

    private bool CanContinueFalling(int x, int y)
    {
        if (y + 1 >= Game.MapHeight)
        {
            return false;
        }
        
        var creatureBelow = Game.Map[x, y + 1];
        return creatureBelow == null || creatureBelow is Player;
    }

    private bool ShouldTransformToGold(int x, int y)
    {
        if (fallingDistance <= 1)
        {
            return false;
        }
        
        int landingY = y + 1;
        
        if (landingY >= Game.MapHeight)
        {
            return true;
        }
        
        var creatureBelow = Game.Map[x, landingY];
        
        return creatureBelow is Terrain || 
               creatureBelow is Sack || 
               creatureBelow is Gold;
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
        return 3;
    }
    
    public CreatureCommand Act(int x, int y)
    {
        return new CreatureCommand { DeltaX = 0, DeltaY = 0 };
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
