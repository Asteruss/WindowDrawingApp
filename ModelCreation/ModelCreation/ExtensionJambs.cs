using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelCreation;

public class ExtensionJamb : Beam, IAttachable
{
    public IAttachable AttachedTo { get; set; }
    public AttachSide Side { get; set; }
    public ExtensionJamb(double width, double length, IAttachable attachedTo, AttachSide side) : base(width, length)
    {
        AttachedTo = attachedTo;
        Side = side;
    }

}

public enum AttachSide
{
    Left, Top, Right, Bottom    
}
