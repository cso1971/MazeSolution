using System.Collections.Generic;
using MazeSolution.MazeServerConnection;

namespace MazeSolution.MazePlayer
{
    public static class Extensions
    {
        public static IList<DirectionEnum> ToDirections(this AvailableDirections availables)
        {
            var list = new List<DirectionEnum>();
            
            if (availables.East) list.Add(DirectionEnum.East);
            if (availables.South) list.Add(DirectionEnum.South);
            if (availables.West) list.Add(DirectionEnum.West);
            if (availables.North) list.Add(DirectionEnum.North);
            
            return list;
        }

        public static DirectionEnum Opposite(this DirectionEnum direction)
        {
            var result = (int) direction + 2;
            if (result >= 4) result -= 4;
            return (DirectionEnum)result;
        }
        
        public static DirectionEnum NextDirection(this DirectionEnum origin)
        {
            var result = (int) origin + 1;
            if (result >= 4) result -= 4;
            return (DirectionEnum)result;
        }
    }
}