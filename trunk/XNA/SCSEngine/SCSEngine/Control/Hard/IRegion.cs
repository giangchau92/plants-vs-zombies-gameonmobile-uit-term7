using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCSEngine.Control
{
    public interface IRegion
    {
        bool Contains(Vector2 value);
    }
}
