# Maze Solution

## To Run The Application
---

1. Ensure you have in your PC Visual Studio 2017 or JetBrains Rider.
2. Ensure you have .Net Core 2.2 Framework SDK.
3. Open the MazeGame solution MazeGame.sln with an IDE (Visual Studio 2017 or JetBrains Rider).
4. Restore Nuget Packages.
5. Run MazeServer.exe.
6. Run the program from IDE (Ctrl+F5) or compilte it and run via command line with `dotnet MazeSolution.dll` from the Debug/donetapp2.2 folder

## Technology Stack:
---
- .Net Core 2.2.
- Console Application.
- xUnit for Unit Test.
- NewtonSoft.Json for json serialization.
- HttpWebRequest for REST interface.
- IDE: Jetbrains Rider.

## HISTORY: What I've done, step by step.
---

1. Issues reading specification doc: ___Missing port___.

2. Encountered problems trying to launch MazeServer.exe. 
Application stop after some seconds.
Tried to run as administrator, and it works.
Found port using NETSTAT in command line with MazeServer.exe running. PortNumber: 3000
Our REST API Server Address: http://localhost:3000

3. Tried to explore REST API, using POSTMAN Chrome extension. Every call works as I expect, but /move End point. 
Tried to pass direction parameter as Key-Value pair, using some possible name, 
or as simple string, but it looks not working.
I don't want to spend a lot of time creating infrastructure, 
I just need to get responses of the REST APIs.
I rapidly create an interface to perform Http invocations, with one method per REST API (MazeConnection).

4. Tested all API methods and everything works (ensure that MazeServer is running), 
even /move endpoint works with direction as simple string parameter in the payload.

5. The application consists in three layers:
    - __Infrastructure__ (_MazeConnection_ class): Connection to MazeServer and API invocation.
    - __Domain__ (_Navigator_ class): Tools to play in a correct way with the maze.
    - __Application__ (_Program_ class): it explores the maze until you reach the target and prints the result.

6. I wrote some tests to create instruments to play with the Maze using a Navigator class. I didn't covered all my code. It's not the goal. Just some tests to start playing.

7. We can consider the maze as a set of connected nodes (crosses), and the solution is a path connecting the starting point (Position: 1, 1) to the center (Position: 19, 21). I have defined four different types of position:
    - __Way point__: A position with only two avalable directions (one entry and one exit).
    - __Cross point__: A position with three or more available directions. When my cursor is in a cross point, I need to make a decision.
    - __Target__: The position where the Status is _TargetReached_.
    - __Starting position__: The position after a _Reset_.

8. The requirement say: "find your way to the center", not the shortest or the cheapest way. Just a way to the center of this certain maze. I just need to find a way to explore the possible paths until moving to the center, avoiding impasses. A reset invocation everytime I encounter an impasse (closing the direction in the last cross) allows to explore all the paths. Looking the Maze, I can detect two possible impasse scenarios:

   - A point with only one available direction to go back (I will reset my position to start again everytime I encounter an impasse).
   - A set of points in circle (I will reset position everytime the cursor enter in an already visited point, considering it as an impasse).

I can move my cursor choosing at every cross the first direction starting from my left, closing the direction that brings the cursor to an impasse.
At every impasse A reset will be performed and the cursor starts again a new exploration (avoiding the direction to the impasses).