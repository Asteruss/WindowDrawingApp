using ModelCreation.SpaceContents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelCreation;

public class LightSpace : WindowNode
{
    public double Width { get; private set; }
    public double Height { get; private set; }

    private ISpaceContent _content;
    public ISpaceContent Content
    {
        get => _content;
        set
        {
            _content = value;
            if (Width > 0 && Height > 0)
            {
                _content?.Resize(Width, Height);
            }
        }
    }

    public void Resize(double width, double height)
    {
        Width = width;
        Height = height;
        Content?.Resize(width, height);
    }
    public LightSpace(ISpaceContent? content = default)
    {
        Content = content ?? new EmptySpaceContent();
    }
}
