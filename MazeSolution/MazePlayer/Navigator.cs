using System.Collections.Generic;
using System.Linq;
using MazeSolution.MazeServerConnection;

namespace MazeSolution.MazePlayer
{
    public interface INavigator
    {
        IList<Point> VisitedPoints { get; }
        Point Move(Position origin);
    }

    public class Navigator : INavigator
    {
        public IList<Point> VisitedPoints { get; private set; } = new List<Point>();
        public Point LastVisitedCross { get; set; }

        private readonly IMazeServerConnection _connection;

        public Navigator(IMazeServerConnection connection)
        {
            _connection = connection;
            _connection.Reset();
            VisitedPoints.Add(new Point(new Position(1, 1), 
                DirectionEnum.East, 
                _connection.GetDirections(),
                GetStatus()));
        }

        public Point Move(Position origin)
        {
            var direction = NextDirection(origin);
            var position = NextPosition(direction);
            
            if (IsACircle(position)) 
                return Reset();
            
            StoreVisited(position, direction);

            var point = VisitedPoints.First(x => x.Position == position);

            StoreLastCross(point);

            point.CurrentAttempt = true;

            if (point.IsImpasse) 
                return Reset();
            
            return point;
         }

        private Position NextPosition(DirectionEnum direction)
        {
            _connection.Move(direction);
            return _connection.GetPosition().Position;
        }

        private void StoreLastCross(Point point)
        {
            if (point.IsCross)
                LastVisitedCross = point;
        }

        private void StoreVisited(Position position, DirectionEnum direction)
        {
            if (VisitedPoints.All(x => x.Position != position))
                VisitedPoints.Add(new Point(position,
                    direction, _connection.GetDirections(),
                    GetStatus()));
        }

        private bool IsACircle(Position position) => VisitedPoints.Any(x => x.CurrentAttempt && x.Position == position);

        private DirectionEnum NextDirection(Position origin) => VisitedPoints
            .First(x => x.Position == origin)
            .GetNextDirection();

        private Point Reset()
        {
            LastVisitedCross?.Directions.Remove(LastVisitedCross.GetNextDirection());
            _connection.Reset();
            
            VisitedPoints = VisitedPoints.Select(x =>
            {
                x.CurrentAttempt = false;
                return x;
            }).ToList();
            
            return VisitedPoints.First();
        }

        private Status GetStatus() => _connection.GetStatus().State;
    }
 }