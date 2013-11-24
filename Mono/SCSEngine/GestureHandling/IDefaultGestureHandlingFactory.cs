using System;
namespace SCSEngine.GestureHandling
{
    public interface IDefaultGestureHandlingFactory
    {
        IGestureDispatcher CreateDispatcher();
        IGestureManager CreateManager(Microsoft.Xna.Framework.Game game);
    }
}
