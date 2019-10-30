using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode.Problems._2018
{
    internal struct Day20Coord
    {
        internal int X;
        internal int Y;

        internal Day20Coord(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    internal class Day20 : BaseProblem
    {
        Dictionary<Day20Coord, int> rooms = new Dictionary<Day20Coord, int>();
        Stack<Day20Coord> roomStack = new Stack<Day20Coord>();

        Day20Coord currentRoom;
        public Day20() : base(2018, 20){}

        internal override string SolvePart1()
        {

            var startingRoom = new Day20Coord(0, 0);
            rooms.Add(startingRoom, 0);

            Walk(startingRoom, 1);

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

        void Walk(Day20Coord start, int charIndex)
        {
            var currentRoom = start;
            Day20Coord nextRoom;
            while (charIndex < InputLines[0].Length)
            {
                var nextChar = InputLines[0][charIndex++];
                switch (nextChar)
                {
                    case 'N':
                        nextRoom = new Day20Coord(currentRoom.X, currentRoom.Y + 1);
                        if (rooms.ContainsKey(nextRoom) == false)
                        {
                            rooms.Add(nextRoom, rooms[currentRoom] + 1);
                        }
                        currentRoom = nextRoom;
                        break;
                    case 'E':
                        nextRoom = new Day20Coord(currentRoom.X + 1, currentRoom.Y);
                        if (rooms.ContainsKey(nextRoom) == false)
                        {
                            rooms.Add(nextRoom, rooms[currentRoom] + 1);
                        }
                        currentRoom = nextRoom;
                        break;
                    case 'S':
                        nextRoom = new Day20Coord(currentRoom.X, currentRoom.Y - 1);
                        if (rooms.ContainsKey(nextRoom) == false)
                        {
                            rooms.Add(nextRoom, rooms[currentRoom] + 1);
                        }
                        currentRoom = nextRoom;
                        break;
                    case 'W':
                        nextRoom = new Day20Coord(currentRoom.X - 1, currentRoom.Y);
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
