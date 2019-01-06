using FluentAssertions;
using MazeSolution.MazeServerConnection;
using Xunit;

namespace MazeSolutionTest.MazeServerConnection
{
    [Collection("Maze Collection")]
    public class MazeConnectionIntegrationTest
    {
        private readonly MazeConnection _mazeServerConnection;

        public MazeConnectionIntegrationTest()
        {
            _mazeServerConnection = new MazeConnection();
        }

        [Fact]
        public void should_get_position()
        {
            _mazeServerConnection.Reset();
            _mazeServerConnection.GetPosition().Should().BeEquivalentTo(new PositionResponse
            {
                Position = new Position(1, 1)
            });
        }
        
        [Fact]
        public void should_get_directions()
        {
            _mazeServerConnection.Reset();
            _mazeServerConnection.GetDirections().Should().BeEquivalentTo(new AvailableDirections
            {
                East = true,
                North = false,
                West = false,
                South = true
            });
        }
        
        [Fact]
        public void should_get_status()
        {
            _mazeServerConnection.Reset();
            _mazeServerConnection.GetStatus().Should().BeEquivalentTo(new StatusResponse()
            {
                State = Status.OnTheWay
            });
        }
        
        [Fact]
        public void should_move()
        {
            _mazeServerConnection.Reset();
            _mazeServerConnection.Move(DirectionEnum.East);
            _mazeServerConnection.GetPosition().Should().BeEquivalentTo(new PositionResponse
            {
                Position = new Position(2, 1)
            });
        }
    }
}