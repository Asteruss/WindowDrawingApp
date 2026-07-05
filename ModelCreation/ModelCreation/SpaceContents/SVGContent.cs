using ModelCreation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ModelCreation.SpaceContents;

public class SVGContent : ISpaceContent
{
    public string RawSvg { get; }

    // из svg
    public double BaseWidth { get; private set;}
    public double BaseHeight { get; private set; }

    // Текушие размеры
    public double CurrentWidth { get; private set; }
    public double CurrentHeight { get; private set; }
    // искажение пропорций
    public bool AllowDistortion { get; set; } = false;

    public SVGContent(string rawSvg)
    {
        var clearSvg = SVGHelper.SanitizeSvgForCad(rawSvg);
        RawSvg = clearSvg ?? throw new ArgumentNullException("SVG не может быть пустым");
        ParseViewBox();
    }
    public void Resize(double width, double height)
    {
        CurrentWidth = width;
        CurrentHeight = height;
    }

    // Проверяем пропорции
    public bool CanFit(double targetWidth, double targetHeight)
    {
        if (targetWidth <= 0 || targetHeight <= 0) return false;

        // Если искажение разрешено, влезет куда угодно (если размер больше нуля)
        if (AllowDistortion) return true;

        // Проверка пропорций
        double svgAspect = BaseWidth / BaseHeight;       // Пропорция SVG 
        double targetAspect = targetWidth / targetHeight; // Пропорция пространства

        if (targetAspect > svgAspect) return false;

        return true;
    }

    // Вспомогательный метод парсинга viewBox из XML
    private void ParseViewBox()
    {
        try
        {
            var doc = XDocument.Parse(RawSvg);
            var svgElement = doc.Root;
            var viewBox = svgElement.Attribute("viewBox")?.Value;

            if (string.IsNullOrEmpty(viewBox))
                throw new Exception("SVG не содержит атрибута viewBox. Вставка невозможна.");

            var parts = viewBox.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 4) throw new Exception("Неверный формат viewBox.");

            // viewBox выглядит как "minX minY width height" (например "0 0 60 40")
            var width = double.Parse(parts[2]);
            var height = double.Parse(parts[3]);

            if (width <= 0 || height <= 0) throw new Exception("Размеры viewBox меньше или равны нулю.");
            BaseWidth = width;
            BaseHeight = height;
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка парсинга SVG: {ex.Message}");
        }
    }
}
