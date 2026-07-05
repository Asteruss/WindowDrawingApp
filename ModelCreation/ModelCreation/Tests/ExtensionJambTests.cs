using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelCreation.Tests;

public class ExtensionJambTests
{
    [Fact]
    public void Picture_AddJamb_StoredCorrectlyWithReferences()
    {
        // Arrange
        var picture = new Picture(1000, 1000);
        var jamb = new ExtensionJamb(50, 1000, picture, AttachSide.Left);

        // Act
        picture.ExtensionJambs.Add(jamb);

        // Assert
        Assert.Single(picture.ExtensionJambs);
        Assert.Same(picture, jamb.AttachedTo); // Проверяем, что ссылка указывает именно на это окно
        Assert.Equal(AttachSide.Left, jamb.Side);
    }

    [Fact]
    public void Picture_AddJambAttachedToAnotherJamb_GraphLinksAreCorrect()
    {
        // Arrange
        var picture = new Picture(1000, 1000);
        var jamb1 = new ExtensionJamb(50, 1000, picture, AttachSide.Left);
        var jamb2 = new ExtensionJamb(30, 1000, jamb1, AttachSide.Left); // Второй крепится к первому

        // Act
        picture.ExtensionJambs.Add(jamb1);
        picture.ExtensionJambs.Add(jamb2);

        // Assert
        Assert.Equal(2, picture.ExtensionJambs.Count); // Оба лежат в плоском списке окна
        Assert.Same(jamb1, jamb2.AttachedTo); // Но ссылка второго ведет на первый
    }

    [Fact]
    public void Picture_Resize_DoesNotAffectAttachedJambDimensions()
    {
        // Arrange - КРИТИЧЕСКИЙ ТЕСТ НА ИЗОЛЯЦИЮ
        var picture = new Picture(1000, 1000);
        var jamb = new ExtensionJamb(50, 1000, picture, AttachSide.Left);
        picture.ExtensionJambs.Add(jamb);

        // Act - Пользователь меняет размер окна
        picture.Width = 2000;
        picture.Height = 2000;

        // Assert - Доборник не должен никак реагировать на изменение окна!
        Assert.Equal(50, jamb.Width);   // Толщина осталась 50
        Assert.Equal(1000, jamb.Length); // Длина осталась 1000 (не стала 2000!)
    }

    [Fact]
    public void ExtensionJamb_CanBeCreatedLongerThanWindow()
    {
        // Arrange & Act
        // Часто доборник уходит в откос стены и может быть длиннее самого окна
        var picture = new Picture(1000, 1000);
        var jamb = new ExtensionJamb(50, 1200, picture, AttachSide.Left);

        // Assert
        Assert.Equal(1200, jamb.Length);
        Assert.Equal(1000, picture.Height); // Окно осталось 1000, доборник 1200 - конфликтов нет
    }

    [Fact]
    public void Picture_WindowFrameResize_DoesNotAffectJamb()
    {
        // Arrange
        var picture = new Picture(1000, 1000);
        var jamb = new ExtensionJamb(50, 1000, picture, AttachSide.Right);
        picture.ExtensionJambs.Add(jamb);

        // Act - Представим, что меняется размер именно внутренней рамы окна
        // (В твоем коде WindowFrame имеет публичный сеттер, так что мы можем его заменить для теста)
        picture.WindowFrame = new WindowFrame(2000, 2000);

        // Assert - Доборник все равно ни при чем
        Assert.Equal(1000, jamb.Length);
        Assert.Equal(50, jamb.Width);
    }
}