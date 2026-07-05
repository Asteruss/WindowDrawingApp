using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelCreation;

public class WindowNode
{
    public WindowNode Parent { get; set; }
    public virtual string DisplayName { get; protected set; } = "Неизвестный элемент";
}
