using System;
using System.Linq;
using Point = System.Tuple<int, int>;

namespace MarsRover
{
    /// <summary>
    /// Cardinal compass directions, first letter of North, East, South, West.
    /// </summary>
    public enum Direction
    {
        N = 0,
        E = 1,
        S = 2,
        W = 3
    }

    /// <summary>
    /// Rotation to the Left or Right by 90°.
    /// </summary>
    public enum Rotate
    {
        L,
        R
    }

    public interface IParsePlateau
    {
        Point MaxPosition(string coords);
    }

    public interface IParseRover
    {
        IState Start(string coordsAndDirection);

        // REVIEW:  If we cared about not going out of bounds of the grid then bounds checking could
        //          be added to the return function possibly adding an input parameter for the grid.
        Func<Rover, Rover> Movements(string movements);
    }

    public class Parser : IParsePlateau, IParseRover
    {
        public Point MaxPosition(string coords)
        {
            return ParsePoint(coords);
        }

        private Point ParsePoint(string coords)
        {
            var parts = coords.Split(' ');
            if (parts.Length != 2)
            {
                throw new ArgumentException(string.Format("Expected two integer numbers but got '{0}'", coords));
            }

            int x = Parse(parts[0]);
            int y = Parse(parts[1]);

            return new Point(x, y);
        }

        private static int Parse(string numberAsString)
        {
            int n = 0;
            if (!int.TryParse(numberAsString, out n))
            {
                throw new ArgumentException(string.Format("Expected an integer number but got '{0}'", numberAsString));
            }

            return n;
        }

        public IState Start(string coordsAndDirection)
        {
            var parts = coordsAndDirection.Split(' ');
            if (parts.Length != 3)
            {
                throw new ArgumentException(string.Format("Expected two integer numbers and a direction but got '{0}'", coordsAndDirection));
            }

            var directionAsString = parts[2];
            if (directionAsString.Length != 1)
            {
                throw new ArgumentException(string.Format("Expected a single letter for the direction, but got '{0}'", directionAsString));
            }

            MarsRover.Direction direction;
            if (!Enum.TryParse(directionAsString, out direction))
            {
                throw new ArgumentException(string.Format("Expected a single compass point letter for the direction, ie. one of NESW, but got '{0}'", directionAsString));
            }

            var position = ParsePoint(string.Format("{0} {1}", parts[0], parts[1]));

            return new Rover(position, direction);
        }

        public Func<Rover, Rover> Movements(string movements)
        {
            var result = movements.ToCharArray().Aggregate<Char, Func<Rover, Rover>>(
                r => r, // Identity function as initial value.
                (f, c) =>
                {
                    // NOTE: Can also be written without temporaries as ...
                    // return r => Movement(c)(f(r));
                    return r =>
                        {
                            // Apply movement to the result of past movements.
                            var nth = f(r);
                            var nthPlus1 = Movement(c)(nth);
                            return nthPlus1;
                        };
                });

            return result;
        }

        private Func<Rover, Rover> Movement(char movement)
        {
            switch(movement)
            {
                case 'L':
                    return r => { r.Rotate(Rotate.L); return r; };
                case 'M':
                    return r => { r.Move(); return r; };
                case 'R':
                    return r => { r.Rotate(Rotate.R); return r; };
                default:
                    return r => r;
            }
        }
    }
}
