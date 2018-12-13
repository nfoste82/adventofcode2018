using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
internal class Program
{
    public static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("../../../input.txt");

        List<List<char>> grid = new List<List<char>>();
        for (int i = 0; i < lines.Length; ++i)
        {
            var gridLine = new List<char>();
            
            var line = lines[i];

            foreach (var character in line)
            {
                gridLine.Add(character);
            }

            grid.Add(gridLine);
        }

        var cartNum = 0;
        var carts = new List<Cart>();

        for (int i = 0; i < lines.Length; ++i)
        {
            var line = lines[i];
            
            int cartIndex = line.IndexOf('<');
            if (cartIndex != -1)
            {
                carts.Add(new Cart(new Vector2i(cartIndex, i), new Vector2i(-1, 0), '-'));
            }
            
            cartIndex = line.IndexOf('>');
            if (cartIndex != -1)
            {
                carts.Add(new Cart(new Vector2i(cartIndex, i), new Vector2i(1, 0), '-'));
            }
            
            cartIndex = line.IndexOf('^');
            if (cartIndex != -1)
            {
                carts.Add(new Cart(new Vector2i(cartIndex, i), new Vector2i(0, -1), '|'));
            }
            
            cartIndex = line.IndexOf('v');
            if (cartIndex != -1)
            {
                carts.Add(new Cart(new Vector2i(cartIndex, i), new Vector2i(0, 1), '|'));
            }
        }
        
        Simulate(new List<Cart>(carts), new List<List<char>>(grid), 1);
        Simulate(new List<Cart>(carts), new List<List<char>>(grid), 2);
    }

    private static void Simulate(List<Cart> carts, List<List<char>> grid, int part)
    {
        while (true)
        {
            var crashedCarts = new List<Cart>();
            
            foreach (var cart in carts)
            {
                if (crashedCarts.Contains(cart))
                {
                    continue;
                }
                
                // Move carts
                MoveCart(cart, grid);
                
                // Two carts with the same position crash
                var crashes = carts.GroupBy(x => x.position)
                    .Where(group => group.Count() > 1)
                    .Select(group => group.Key).ToList();

                if (crashes.Any())
                {
                    if (part == 1)
                    {
                        var pos = crashes.First();
                        Console.WriteLine("Part1: " + pos.X+","+pos.Y);
                        return;
                    }
                    crashedCarts.AddRange(carts.Where(c => crashes.Contains(c.position)).ToList());
                }
            }

            carts.RemoveAll(c => crashedCarts.Contains(c));

            if (carts.Count == 1)
            {
                Console.WriteLine("Part2: " + carts[0].position.X+","+carts[0].position.Y);
                return;
            }

            // Sort carts by position
            carts.Sort((a, b) =>
            {
                if (a.position.Y == b.position.Y)
                {
                    return a.position.X - b.position.X;
                }

                return a.position.Y - b.position.Y;
            });
        }
    }

    private static void MoveCart(Cart cart, List<List<char>> grid)
    {
        // Find track in front of cart
        Vector2i nextPosition = Vector2i.Add(cart.position, cart.forward);
        
        // Shouldn't need to check bounds if we're doing things correctly

        char track = grid[nextPosition.Y][nextPosition.X];

        grid[cart.position.Y][cart.position.X] = cart.trackUnderCart;
        cart.trackUnderCart = track;

        // Cart moves
        cart.position = nextPosition;

        // Handle turning cart
        if (cart.forward.X != 0)   // Facing left or right
        {
            // Track can be \, or /, or - or +
            switch (track)
            {
                case '\\':    // Right turn
                    cart.forward.Turn(Direction.Right);
                    break;
                case '/':    // Left turn
                    cart.forward.Turn(Direction.Left);
                    break;
                case '-': break;
                case '+':
                    cart.TurnForIntersection();
                    break;
            }
        }
        else // Facing up or down
        {
            switch (track)
            {
                case '\\':    // Right turn
                    cart.forward.Turn(Direction.Left);
                    break;
                case '/':    // Left turn
                    cart.forward.Turn(Direction.Right);
                    break;
                case '-': break;
                case '+':
                    cart.TurnForIntersection();
                    break;
            }
        }
    }
    
    public enum Direction
    {
        Left,
        Straight,
        Right,
    }

    public class Cart
    {
        public Cart(Vector2i position, Vector2i forward, char track)
        {
            this.position = position;
            this.forward = forward;
            lastTurnDirection = Direction.Right;
            this.trackUnderCart = track;
        }
        
        public void TurnForIntersection()
        {
            switch (lastTurnDirection)
            {
                case Direction.Right: 
                    lastTurnDirection = Direction.Left;
                    break;
                case Direction.Left:
                    lastTurnDirection = Direction.Straight;
                    break;
                case Direction.Straight: 
                    lastTurnDirection = Direction.Right;
                    break;
            }
            
            if (lastTurnDirection != Direction.Straight)
            {
                forward.Turn(lastTurnDirection);
            }
        }
        
        public Vector2i position;
        public Vector2i forward;
        public Direction lastTurnDirection;
        public char trackUnderCart;
    }
    
    public struct Vector2i
    {
        public Vector2i(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector2i Add(Vector2i a, Vector2i b)
        {
            return new Vector2i(a.X + b.X, a.Y + b.Y);
        }

        public void Turn(Direction direction)
        {
            // Probably could've used trig here. I was tired so I hacked it.
            switch (direction)
            {
                case Direction.Left:
                    if (X == 1 && Y == 0)
                    {
                        X = 0;
                        Y = -1;
                    }
                    else if (X == 0 && Y == -1)
                    {
                        X = -1;
                        Y = 0;
                    }
                    else if (X == -1 && Y == 0)
                    {
                        X = 0;
                        Y = 1;
                    }
                    else if (X == 0 && Y == 1)
                    {
                        X = 1;
                        Y = 0;
                    }

                    break;
                case Direction.Right:
                    if (X == 1 && Y == 0)
                    {
                        X = 0;
                        Y = 1;
                    }
                    else if (X == 0 && Y == 1)
                    {
                        X = -1;
                        Y = 0;
                    }
                    else if (X == -1 && Y == 0)
                    {
                        X = 0;
                        Y = -1;
                    }
                    else if (X == 0 && Y == -1)
                    {
                        X = 1;
                        Y = 0;
                    }

                    break;
            }
        }
        
        public int X;
        public int Y;
    }
}
}