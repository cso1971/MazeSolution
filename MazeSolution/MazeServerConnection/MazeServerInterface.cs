using System;

namespace MazeSolution.MazeServerConnection
{
    public interface IMazeServerConnection
    {
        void Move(DirectionEnum direction);
        void Reset();
        
        AvailableDirections GetDirections();
        PositionResponse GetPosition();
        StatusResponse GetStatus();
    }

    public class StatusResponse
    {
        public Status State { get; set; }
    }

    public enum Status
    {
        OnTheWay,
        TargetReached,
        Failed
    }

    public class PositionResponse
    {
        public Position Position { get; set; }
    }

    public class Position : IEquatable<Position>
    {
        public int X { get; }
        public int Y { get; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Position other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Position) obj);
        }
        
        public static bool operator== (Position obj1, Position obj2) => 
            (obj1?.X == obj2?.X && obj1?.Y == obj2?.Y);

        public static bool operator!= (Position obj1, Position obj2)
        {
            return !(obj1?.X == obj2?.X 
                     && obj1?.Y == obj2?.Y);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public string Print() => $"({X.ToString()}, {Y.ToString()}) ";
    }

    public class AvailableDirections
    {
        public bool North { get; set; }
        public bool East { get; set; }
        public bool South { get; set; }
        public bool West { get; set; }
    }

    public enum DirectionEnum
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }
}