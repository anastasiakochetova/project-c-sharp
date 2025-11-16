# Text Border Formatter

Простая утилита для красивого вывода текста в рамке из символов `+`, `-` и `|`.

## Описание

Функция `WriteTextWithBorder` принимает строку текста и выводит ее на экран в эстетичной рамке, с пробелами между текстом и границами для лучшей читаемости.

## Примеры использования

```csharp
public static void Main()
{
    WriteTextWithBorder("Menu:");
    WriteTextWithBorder("");
    WriteTextWithBorder(" ");
    WriteTextWithBorder("Game Over!");
    WriteTextWithBorder("Select level:");
}
```
