using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCSEngine.SkeletalTransform
{
    public interface ITransformModel
    {
        Matrix TransformMatrix { get; }
    }
}