namespace ModelCreation.Tests;

using ModelCreation.SpaceContents;
using Xunit;

// Предполагается, что классы EmptySpaceContent, MockContent и BaseSizes доступны
// (Для SVG используем реальный класс SvgContent с валидным минимальным XML)

public class ComplexWindowIntegrationTests
{
    // Валидный минимальный SVG для тестов (viewBox 1x1 влезет куда угодно)
    private const string ValidSvg = "<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 1 1'></svg>";

    [Fact]
    public void ComplexWindow_DeepNesting_AllDimensionsCalculatedCorrectly()
    {
        // ==========================================
        // 1. АРХИТЕКТУРА (Сборка дерева снизу вверх)
        // ==========================================

        // Импост 2-го уровня (горизонтальный, внутри правой части)
        var impostL2 = new ImpostContent() { Width = 20 };
        impostL2.Content = new SVGContent(ValidSvg); // SVG внутри импоста
        var splitL2 = new SplitContent(impostL2, offset: 300, SplitOrientation.Horizontal);

        // Импост 1-го уровня (вертикальный, делит окно пополам)
        var impostL1 = new ImpostContent() { Width = 10 };
        var splitL1 = new SplitContent(impostL1, offset: 400, SplitOrientation.Vertical);

        // Наполняем пространства 1-го уровня
        splitL1.Space1.Content = new SVGContent(ValidSvg); // SVG в левом стекле
        splitL1.Space2.Content = splitL2;                 // Правое стекло делится 2-м импостом

        // Создаем окно и кладем корневое дерево
        var picture = new Picture(1000, 1000);
        picture.WindowFrame.Left.Width = 50;
        picture.WindowFrame.Right.Width = 50;

        // Кладем дерево в корень. Умный сеттер LightSpace должен протолкнуть размеры вниз
        picture.WindowFrame.RootSpace.Content = splitL1;

        // ==========================================
        // 2. ПРОВЕРКА РАМЫ И КОРНЕВОГО ПРОСТРАНСТВА
        // ==========================================

        double expectedTopLength = 1000 - 50 - 50; // 900
        Assert.Equal(expectedTopLength, picture.WindowFrame.Top.Length);
        Assert.Equal(expectedTopLength, picture.WindowFrame.Bottom.Length);

        // Высота света зависит от ширины Top и Bottom (предполагаем BaseSizes.BeamWidth = 50 по умолчанию)
        double expectedLightHeight = 1000 - picture.WindowFrame.Top.Width - picture.WindowFrame.Bottom.Width;
        Assert.Equal(expectedTopLength, picture.WindowFrame.RootSpace.Width);
        Assert.Equal(expectedLightHeight, picture.WindowFrame.RootSpace.Height);

        // ==========================================
        // 3. ПРОВЕРКА 1-ГО УРОВНЯ (Вертикальный сплит)
        // ==========================================

        Assert.Equal(expectedLightHeight, impostL1.Length); // Длина импоста = высоте света
        Assert.Equal(10, impostL1.Width);

        // Левое пространство
        Assert.Equal(400, splitL1.Space1.Width);  // Равно offset
        Assert.Equal(expectedLightHeight, splitL1.Space1.Height);

        // Правое пространство
        double expectedSpace2Width = expectedTopLength - 400 - 10; // 900 - 400 - 10 = 490
        Assert.Equal(expectedSpace2Width, splitL1.Space2.Width);
        Assert.Equal(expectedLightHeight, splitL1.Space2.Height);

        // ==========================================
        // 4. ПРОВЕРКА 2-ГО УРОВНЯ (Горизонтальный сплит внутри Space2)
        // ==========================================

        Assert.Equal(expectedSpace2Width, impostL2.Length); // Длина равна ширине родителя (490)
        Assert.Equal(20, impostL2.Width);

        // Верхняя часть правого стекла
        Assert.Equal(expectedSpace2Width, splitL2.Space1.Width);
        Assert.Equal(300, splitL2.Space1.Height); // Равно offset

        // Нижняя часть правого стекла
        double expectedSpace2_2Height = expectedLightHeight - 300 - 20; // (Высота света) - 300 - 20
        Assert.Equal(expectedSpace2Width, splitL2.Space2.Width);
        Assert.Equal(expectedSpace2_2Height, splitL2.Space2.Height);

        // ==========================================
        // 5. ПРОВЕРКА ГЛУБОКО ЗАСЫПАННЫХ SVG
        // ==========================================

        // SVG в левом стекле
        var svgInGlass = splitL1.Space1.Content as SVGContent;
        Assert.NotNull(svgInGlass);
        Assert.Equal(400, svgInGlass.CurrentWidth);
        Assert.Equal(expectedLightHeight, svgInGlass.CurrentHeight);

        // SVG внутри импоста 2-го уровня
        var svgInImpost = impostL2.Content as SVGContent;
        Assert.NotNull(svgInImpost);
        // В ImpostContent.Resize() мы передаем (Length, Width) -> (490, 20)
        Assert.Equal(expectedSpace2Width, svgInImpost.CurrentWidth);
        Assert.Equal(20, svgInImpost.CurrentHeight);
    }

    [Fact]
    public void ComplexWindow_ChangeFrameBeamWidth_RipplesToDeepElements()
    {
        // Arrange
        var impostL2 = new ImpostContent() { Width = 20 };
        impostL2.Content = new SVGContent(ValidSvg);
        var splitL2 = new SplitContent(impostL2, 300, SplitOrientation.Horizontal);

        var impostL1 = new ImpostContent() { Width = 10 };
        var splitL1 = new SplitContent(impostL1, 400, SplitOrientation.Vertical);

        splitL1.Space2.Content = splitL2;

        var picture = new Picture(1000, 1000);
        picture.WindowFrame.Left.Width = 50;
        picture.WindowFrame.Right.Width = 50;
        picture.WindowFrame.RootSpace.Content = splitL1;

        // Сохраняем начальные размеры самого глубокого элемента
        var deepSvg = impostL2.Content as SVGContent;
        double initialDeepWidth = deepSvg.CurrentWidth; // Должно быть 490

        // Act: Утолщаем левый брус рамы на 40 единиц!
        // Это должно сработать событие WidthChanged в WindowFrame -> Recalculate()
        picture.WindowFrame.Left.Width = 90;

        // Assert: Проверяем волну изменений
        // 1. Горизонтальные брусы рамы стали короче
        Assert.Equal(860, picture.WindowFrame.Top.Length); // 1000 - 90 - 50 = 860 (если Top.Width не менялся)

        // 2. Корневое пространство сузилось
        Assert.Equal(860, picture.WindowFrame.RootSpace.Width);

        // 3. Импост 1 уровня стал короче
        Assert.Equal(900, impostL1.Length);

        // 4. Пространство 2 уровня сузилось
        double newSpace2Width = 860 - 400 - 10; // 450
        Assert.Equal(newSpace2Width, splitL1.Space2.Width);

        // 5. САМОЕ ГЛАВНОЕ: Импост 2 уровня и его внутренний SVG получили новую ширину!
        Assert.Equal(newSpace2Width, impostL2.Length);
        Assert.Equal(newSpace2Width, deepSvg.CurrentWidth); // Было 490, стало 450

        // Убеждаемся, что оно точно изменилось
        Assert.NotEqual(initialDeepWidth, deepSvg.CurrentWidth);
    }
}
