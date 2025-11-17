# 🧠 Задача: Упростить логику робота до одного оператора

В воскресенье Вася пришёл в кружок робототехники и увидел код управления боевым роботом:

```csharp
public static bool ShouldFire(bool enemyInFront, string enemyName, int robotHealth)
{
    bool shouldFire = true;
    if (enemyInFront == true)
    {
        if (enemyName == "boss")
        {
            if (robotHealth < 50) shouldFire = false;
            if (robotHealth > 100) shouldFire = true;
        }
    }
    else
    {
        return false;
    }
    return shouldFire;
}
```
