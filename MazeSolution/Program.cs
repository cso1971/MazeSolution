using System;
using System.Linq;
using MazeSolution.MazePlayer;
using MazeSolution.MazeServerConnection;

namespace MazeSolution
{
    public static class Program
    {
        private static IMazeServerConnection _connection;
        private static INavigator _navigator;
        private static Spiner _spin;

        public static void Main()
        {
            Console.WriteLine("Starting Maze Path Finder.");

            _connection = new MazeConnection();
            _navigator = new Navigator(new MazeConnection());
            _spin = new Spiner();
            
            var currentPoint = new Point(_connection.GetPosition().Position, DirectionEnum.East, 
                _connection.GetDirections(), _connection.GetStatus().State);
            
            Console.Write("Finding path to the target: ");

            do
            {
                currentPoint = _navigator.Move(currentPoint.Position);
                _spin.Turn();
            } while (currentPoint.Status != Status.TargetReached);

            var target = _navigator.VisitedPoints.Last().Position;
            Console.WriteLine($"Found target at position: ({target.X}, {target.Y})");
            Console.WriteLine($"Points visited in the last attempt: {_navigator.VisitedPoints.Count(x => x.CurrentAttempt)}");
            Console.WriteLine("Path to target:");
            Console.WriteLine("");
            
            PrintVisitedPoints();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static void PrintVisitedPoints()
        {
            for (var i = 1; i < 45; i++)
            {
                for (var j = 1; j < 45; j++)
                {
                    var point = _navigator.VisitedPoints.FirstOrDefault(x => x.Position == new Position(j, i));

                    if (point != null)
                        Console.Write(
                            point.CurrentAttempt
                                ? "+"
                                : "-");
                    else
                        Console.Write(" ");
                }

                Console.WriteLine("");
            }
        }
    }
}