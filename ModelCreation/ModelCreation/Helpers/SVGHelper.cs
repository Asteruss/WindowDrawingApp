using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ModelCreation.Helpers;

public class SVGHelper
{
    public static string SanitizeSvgForCad(string rawSvg)
    {
        var doc = XDocument.Parse(rawSvg);
        var svg = doc.Root;
        if (svg == null)
            return doc.ToString();

        svg.Attribute("content")?.Remove();

        svg.Attribute("width")?.Remove();
        svg.Attribute("height")?.Remove();

        svg.Attribute("style")?.Remove();



        return doc.ToString();
    }
}
