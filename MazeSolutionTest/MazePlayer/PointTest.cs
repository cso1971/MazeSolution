using FluentAssertions;
using MazeSolution.MazePlayer;
using MazeSolution.MazeServerConnection;
using Xunit;

namespace MazeSolutionTest.MazePlayer
{
    [Collection("Maze Collection")]
    public class PointTest
    {
        [Fact]
        public void should_create_a_way_point()
        {
            var point = new Point(new Position(3, 4), DirectionEnum.West,
                new AvailableDirections {East = true, West = true}, Status.OnTheWay);

            point.IsCross.Should().BeFalse();
            point.IsImpasse.Should().BeFalse();
            point.Directions.Should().HaveCount(2);
        }
        
        [Fact]
        public void should_create_a_cross_point()
        {
            var point = new Point(new Position(3, 4), DirectionEnum.East,
                new AvailableDirections {East = true, West = true, North = true}, Status.OnTheWay);

            point.IsCross.Should().BeTrue();
            point.IsImpasse.Should().BeFalse();
            point.Directions.Should().HaveCount(3);
        }
        
        [Fact]
        public void should_create_an_impasse_point()
        {
            var point = new Point(new Position(3, 4), DirectionEnum.East,
                new AvailableDirections {East = true}, Status.OnTheWay);

            point.IsCross.Should().BeFalse();
            point.IsImpasse.Should().BeTrue();
            point.Directions.Should().HaveCount(1);
        }
        
        [Fact]
        public void should_close_direction()
        {
            var point = new Point(new Position(3, 4), DirectionEnum.East,
                new AvailableDirections {East = true, North = true, South = true}, Status.OnTheWay);

            point.CloseDirection(DirectionEnum.North);
            point.Directions.Contains(DirectionEnum.North).Should().BeFalse();
        }
        
        [Fact]
        public void should_get_next_direction_with_all_available()
        {
            var point = new Point(new Position(3, 4), DirectionEnum.East,
                new AvailableDirections {East = true, North = true, South = true, West = true}, Status.OnTheWay);

            point.GetNextDirection().Should().Be(DirectionEnum.North);
        }
        
        [Fact]
        public void should_get_next_direction_with_few_available()
        {
            var point = new Point(new Position(3, 4), DirectionEnum.West,
                new AvailableDirections {West = true, South = true}, Status.OnTheWay);

            point.GetNextDirection().Should().Be(DirectionEnum.South);
        }
    }
}