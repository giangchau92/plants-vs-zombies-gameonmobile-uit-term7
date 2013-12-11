using Microsoft.Xna.Framework;
using System;
namespace SCSEngine.GestureHandling
{
    public interface IGestureHandlingFactory
    {
        IGestureDispatcher CreateDispatcher();
        IGestureManager CreateManager(Game game, ITouchController tc);
        ITouchController CreateTouchController();
    }
}