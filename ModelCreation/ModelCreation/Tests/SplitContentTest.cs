using ModelCreation.SpaceContents;

namespace ModelCreation.Tests;

public class MockContent : ISpaceContent
{
    public double LastWidth { get; private set; }
    public double LastHeight { get; private set; }
    public int ResizeCallCount { get; private set; }

    public void Resize(double width, double height)
    {
        LastWidth = width;
        LastHeight = height;
        ResizeCallCount++;
    }
}

public class SplitContentTest
{
    [Fact]
    public void SplitContent_Vertical_CalculatesSpacesCorrectly()
    {
        // Arrange
        var impost = new ImpostContent() { Width = 10 }; // Толщина 10
        var split = new SplitContent(impost, offset: 50, SplitOrientation.Vertical);

        // Act
        split.Resize(200, 100); // Окно 200х100

        // Assert
        Assert.Equal(100, impost.Length); // Длина импоста равна высоте окна
        Assert.Equal(50, split.Space1.Width); // Левое пространство = offset
        Assert.Equal(100, split.Space1.Height);
        Assert.Equal(140, split.Space2.Width); // Правое = 200 - 50(offset) - 10(импост)
        Assert.Equal(100, split.Space2.Height);
    }

    [Fact]
    public void SplitContent_Horizontal_CalculatesSpacesCorrectly()
    {
        // Arrange
        var impost = new ImpostContent() { Width = 20 };
        var split = new SplitContent(impost, offset: 30, SplitOrientation.Horizontal);

        // Act
        split.Resize(150, 200);

        // Assert
        Assert.Equal(150, impost.Length); // Длина импоста равна ширине окна
        Assert.Equal(150, split.Space1.Width);
        Assert.Equal(30, split.Space1.Height); // Верхнее = offset
        Assert.Equal(150, split.Space2.Width);
        Assert.Equal(150, split.Space2.Height); // Нижнее = 200 - 30 - 20
    }

    [Fact]
    public void SplitContent_ChangeOffset_RecalculatesSpaces()
    {
        // Arrange
        var impost = new ImpostContent() { Width = 10 };
        var split = new SplitContent(impost, offset: 0, SplitOrientation.Vertical);
        split.Resize(100, 100);

        // Act - двигаем импост вправо
        split.Offset = 40;

        // Assert
        Assert.Equal(40, split.Space1.Width);
        Assert.Equal(50, split.Space2.Width); // 100 - 40 - 10
    }

    [Fact]
    public void SplitContent_ChangeImpostWidth_RecalculatesSpaces()
    {
        // Arrange
        var impost = new ImpostContent() { Width = 10 };
        var split = new SplitContent(impost, offset: 50, SplitOrientation.Vertical);
        split.Resize(200, 100);

        // Act - утолщаем импост
        impost.Width = 50;

        // Assert
        // Offset не меняется, но правое пространство сжимается
        Assert.Equal(50, split.Space1.Width);
        Assert.Equal(100, split.Space2.Width); // 200 - 50(offset) - 50(новый импост)
    }

    [Fact]
    public void SplitContent_OffsetClamping_DoesNotAllowNegativeSizes()
    {
        // Arrange
        var impost = new ImpostContent() { Width = 10 };
        var split = new SplitContent(impost, offset: 50, SplitOrientation.Vertical);
        split.Resize(200, 100);

        // Act - Пытаемся сдвинуть импост за правый край
        split.Offset = 999;

        // Assert - Offset должен быть ограничен
        Assert.Equal(190, split.Offset); // 200 - 10(импост) = 190 (максимум)
        Assert.Equal(190, split.Space1.Width);
        Assert.Equal(0, split.Space2.Width); // Пространство не уходит в минус
    }

    [Fact]
    public void SplitContent_ParentResize_UpdatesImpostInternalContent()
    {
        // Arrange
        var innerMock = new MockContent();
        var impost = new ImpostContent();
        impost.Content = innerMock; // Кладем мок внутрь импоста

        var split = new SplitContent(impost, 0, SplitOrientation.Vertical);

        // Act
        split.Resize(100, 200); // Даем размер сплиту

        // Assert - Внутренний контент импоста должен получить размер (Длина, Ширина)
        Assert.Equal(1, innerMock.ResizeCallCount); // Вызвался 1 раз (вручную из SplitContent.Resize)
        Assert.Equal(200, innerMock.LastWidth);  // Длина импоста стала шириной для внутренностей
        Assert.Equal(impost.Width, innerMock.LastHeight); // Ширина импоста стала высотой
    }

    [Fact]
    public void LightSpace_SetContent_IfAlreadySized_UpdatesContent()
    {
        // Arrange
        var space = new LightSpace();
        space.Resize(100, 50); // Задали размер пространству

        var mock = new MockContent();

        // Act - Кладем контент В УЖЕ РАЗМЕРЕННОЕ пространство
        space.Content = mock;

        // Assert
        Assert.Equal(1, mock.ResizeCallCount);
        Assert.Equal(100, mock.LastWidth);
        Assert.Equal(50, mock.LastHeight);
    }
}
