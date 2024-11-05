using W9temp.Data;
using W9temp.Services;

namespace W9temp;

class Program
{
    static void Main(string[] args)
    {
        using (var context = new GameContext())
        {
            // Seed the data if necessary
            context.Seed();

            // Initialize GameEngine and Menu
            var gameEngine = new GameEngine(context);
            var menu = new Menu(gameEngine);

            // Show the menu
            menu.Show();
        }
    }
}