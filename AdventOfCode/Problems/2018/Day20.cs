using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode.Problems._2018
{
    internal class Day20 : BaseProblem
    {
        Dictionary<Point, int> rooms = new Dictionary<Point, int>();
        Stack<Point> roomStack = new Stack<Point>();

        public Day20() : base(2018, 20){}

        internal override string SolvePart1()
        {

            var startingRoom = new Point(0, 0);
            rooms.Add(startingRoom, 0);

            Walk(startingRoom);

            var farthestRoom = rooms.Keys.OrderBy(k => rooms[k]).Last();

            var distance = rooms[farthestRoom];

            return distance.ToString();
        }

        internal override string SolvePart2()
        {
            /* Assumes Part1 already ran and rooms are populated */
            var roomsAtLeast1000 = rooms.Keys.Where(k => rooms[k] >= 1000).Count();
            return roomsAtLeast1000.ToString();
        }

        void Walk(Point start)
        {
            var currentRoom = start;
            var charIndex = 0;
            Point nextRoom;
            while (charIndex < InputLines[0].Length)
            {
                var nextChar = InputLines[0][charIndex++];
                switch (nextChar)
                {
                    case 'N':
                        nextRoom = new Point(currentRoom.X, currentRoom.Y + 1);
                        if (rooms.ContainsKey(nextRoom) == false)
                        {
                            rooms.Add(nextRoom, rooms[currentRoom] + 1);
                        }
                        currentRoom = nextRoom;
                        break;
                    case 'E':
                        nextRoom = new Point(currentRoom.X + 1, currentRoom.Y);
                        if (rooms.ContainsKey(nextRoom) == false)
                        {
                            rooms.Add(nextRoom, rooms[currentRoom] + 1);
                        }
                        currentRoom = nextRoom;
                        break;
                    case 'S':
                        nextRoom = new Point(currentRoom.X, currentRoom.Y - 1);
                        if (rooms.ContainsKey(nextRoom) == false)
                        {
                            rooms.Add(nextRoom, rooms[currentRoom] + 1);
                        }
                        currentRoom = nextRoom;
                        break;
                    case 'W':
                        nextRoom = new Point(currentRoom.X - 1, currentRoom.Y);
                        if (rooms.ContainsKey(nextRoom) == false)
                        {
                            rooms.Add(nextRoom, rooms[currentRoom] + 1);
                        }
                        currentRoom = nextRoom;
                        break;
                    case '(':
                        roomStack.Push(currentRoom);
                        break;
                    case '|':
                        currentRoom = roomStack.Peek();
                        break;
                    case ')':
                        currentRoom = roomStack.Pop();
                        break;
                }
            }
        }
    }
}
