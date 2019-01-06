using System.Collections.Generic;
using MazeSolution.MazeServerConnection;

namespace MazeSolution.MazePlayer
{
    public class Point
    {
        public Position Position { get; }
        public IList<DirectionEnum> Directions { get; }
        public bool CurrentAttempt { get; set; } = true;
        private readonly DirectionEnum _entryDirection;

        public bool IsCross => Directions.Count > 2;
        public bool IsImpasse => Directions.Count == 1;
        public Status Status { get; }

        public Point(Position position, DirectionEnum entryDirection, AvailableDirections avails, Status status)
        {
            Position = position;
            _entryDirection = entryDirection;
            Directions = avails.ToDirections();
            Status = status;
        }

        public void CloseDirection(DirectionEnum direction)
        {
            Directions.Remove(direction);
        }

        public DirectionEnum GetNextDirection()
        {
            var result = _entryDirection.Opposite();

            do result = result.NextDirection();
            while (!Directions.Contains(result));

            return result;
        }
    }
}