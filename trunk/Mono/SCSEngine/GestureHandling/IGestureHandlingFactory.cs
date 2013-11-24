using System;
namespace SCSEngine.GestureHandling
{
    public interface IGestureHandlingFactory
    {
        IGestureDispatcher CreateDispatcher();
        IGestureManager CreateManager(Microsoft.Xna.Framework.Game game);
    }
}