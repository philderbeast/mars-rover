using System;
using System.Collections.Generic;
using Point = System.Tuple<int, int>;

namespace MarsRover
{
    public interface IState
    {
        Point Position { get; }
        Direction Direction { get; }
    }

    public interface IAction
    {
        Point Move(int count=1);
        Direction Rotate(IEnumerable<Rotate> steps);
    }

    public class Rover : IState, IAction
    {
        public Point Position { get; private set; }
        public Direction Direction { get; private set; }

        public Rover(Point position, Direction initialDirection)
        {
            Position = position;
            Direction = initialDirection;
        }

        public Rover(int x, int y, Direction initialDirection)
            : this(new Point(x, y), initialDirection)
        {
        }

        public Point Move(int count = 1)
        {
            for (int ii = 0; ii < count; ii++)
            {
                MoveOnce();
            }

            return Position;
        }

        public Direction Rotate(IEnumerable<Rotate> steps)
        {
            foreach (var step in steps)
            {
                Direction = Rotate(step);
            }

            return Direction;
        }

        public Direction Rotate(Rotate step)
        {
            int directionAsOrdinal = CompassPointOrdinal(Direction);
            int change = CompassPointOrdinalChange(step);

            // NOTE:    Add 4 so that the remainder is always in range.
            int newDirection = (directionAsOrdinal + change + 4) % 4;

            if (!Enum.IsDefined(typeof(MarsRover.Direction), newDirection))
            {
                throw new InvalidProgramException(string.Format("A compass bearing of '{0}' is not one of 'NESW'", newDirection));
            }

            Direction = (MarsRover.Direction)Enum.ToObject(typeof(MarsRover.Direction), newDirection);

            return Direction;
        }

        private Point MoveOnce()
        {
            switch (Direction)
            {
                case MarsRover.Direction.N:
                    Position = new Point(Position.Item1, Position.Item2 + 1);
                    break;
                case MarsRover.Direction.S:
                    Position = new Point(Position.Item1, Position.Item2 - 1);
                    break;
                case MarsRover.Direction.E:
                    Position = new Point(Position.Item1 + 1, Position.Item2);
                    break;
                case MarsRover.Direction.W:
                    Position = new Point(Position.Item1 - 1, Position.Item2);
                    break;
            }

            return Position;
        }

        private static int CompassPointOrdinal(Direction direction)
        {
            return (int)direction;
        }

        private static int CompassPointOrdinalChange(Rotate step)
        {
            switch (step)
            {
                case MarsRover.Rotate.L:
                    return -1;
                case MarsRover.Rotate.R:
                    return 1;
                default:
                    return 0;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Position.Item1, Position.Item2, Direction);
        }
    }
}
