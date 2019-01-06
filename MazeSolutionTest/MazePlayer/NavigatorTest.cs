using FluentAssertions;
using MazeSolution.MazePlayer;
using MazeSolution.MazeServerConnection;
using NSubstitute;
using Xunit;

namespace MazeSolutionTest.MazePlayer
{
    [Collection("Maze Collection")]
    public class NavigatorTest
    {
        private readonly IMazeServerConnection _connection;
        private readonly Navigator _navigator;

        public NavigatorTest()
        {
            _connection = Substitute.For<IMazeServerConnection>();
            _connection.GetDirections().Returns(new AvailableDirections {East = true, South = true, West = true});
            _connection.GetStatus().Returns(new StatusResponse {State = Status.OnTheWay});
            _navigator = new Navigator(_connection);
        }

        [Fact]
        public void should_move_and_store_new_visited_point()
        {
            var position = new Position(1, 1);
            var availableDirections = new AvailableDirections {East = true, South = true};
            
            _connection.GetPosition()
                .Returns(new PositionResponse{Position = position}, 
                    new PositionResponse{Position = new Position(2, 1)});
            
            _connection.GetDirections().Returns(availableDirections);
            
            var point = new Point(position, DirectionEnum.East, availableDirections, Status.OnTheWay);
            
            _navigator.Move(position);
            
            _connection.Received().Move(point.GetNextDirection());
            _connection.Received().GetPosition();
        }
        
        [Fact]
        public void should_move_and_store_last_visited_cross()
        {
            var position = new Position(1, 1);
            var availableDirections = new AvailableDirections {East = true, South = true, North = true};
            
            _connection.GetPosition()
                .Returns(new PositionResponse{Position = new Position(2, 1)});
            
            _connection.GetDirections().Returns(availableDirections);
            
            _navigator.Move(position);

            _navigator.LastVisitedCross.Position.Should().Be(new Position(2, 1));
        }
        
        [Fact]
        public void should_move_and_remove_direction_for_impasse()
        {
            var position = new Position(1, 1);
            var availableDirections = new AvailableDirections {East = true};
            
            _navigator.LastVisitedCross = new Point(position, DirectionEnum.East, new AvailableDirections
            {
                East = true, 
                South = true, 
                North = true
            }, Status.OnTheWay);
            
            _connection.GetPosition()
                .Returns(new PositionResponse{Position = position}, 
                    new PositionResponse{Position = new Position(2, 1)});
            
            _connection.GetDirections().Returns(availableDirections);
            
            _navigator.Move(position);

            _navigator.LastVisitedCross.Directions.Should().HaveCount(2);
        }
    }
}