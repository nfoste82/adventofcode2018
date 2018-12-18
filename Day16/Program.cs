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
        // Input without test program data
        string[] lines = File.ReadAllLines("../../../input.txt");

        List<string> codes = Codes().ToList();

        int numThreeMatchesOrMore = 0;
        for (int i = 0; i < lines.Length; i += 4)
        {
            int matches = 0;
            var beforeRegisters = lines[i].GetStringBetweenChars('[', ']').Replace(" ", "").Split(',').ToVector4i();
            var instruction = lines[i + 1].Split(' ').ToVector4i();
            var afterRegisters = lines[i + 2].GetStringBetweenChars('[', ']').Replace(" ", "").Split(',').ToVector4i();

            foreach (var code in codes)
            {
                var result = OpCode(code, beforeRegisters, instruction);
                if (afterRegisters.Equals(result))
                {
                    matches++;

                    var key = (code, instruction.OpCode);
                    if (_codesToInstructionCounts.TryGetValue(key, out var count))
                    {
                        ++count;
                        _codesToInstructionCounts[key] = count;
                    }
                    else
                    {
                        _codesToInstructionCounts.Add(key, count);
                    }
                }
            }

            if (matches >= 3)
            {
                ++numThreeMatchesOrMore;
            }
        }
        
        Console.WriteLine("Part 1 answer: " + numThreeMatchesOrMore);

        bool removed = true;
        while (removed)
        {
            removed = false;

            for (int i = 0; i < 16; ++i)
            {
                // Found a lone code for an instruction, remove all other instances of the code
                if (_codesToInstructionCounts.Count(kvp => kvp.Key.instructionNum == i) == 1)
                {
                    var code = _codesToInstructionCounts.Single(kvp => kvp.Key.instructionNum == i).Key.code;
                    
                    var others = _codesToInstructionCounts.Where(pair => pair.Key.code == code || pair.Key.instructionNum == i).ToList();
                    if (others.Count > 1)
                    {
                        removed = true;

                        foreach (var kvp in others)
                        {
                            _codesToInstructionCounts.Remove(kvp.Key);
                        }
                        _codesToInstructionCounts.Add((code, i), 1);
                    }

                    if (removed)
                    {
                        break;
                    }
                }
            }
        }

        string[] testProgLines = File.ReadAllLines("../../../input_alt.txt");

        Vector4i registers = new Vector4i(0,0,0,0);
        foreach (var line in testProgLines)
        {
            var instruction = line.Split(' ').ToVector4i();
            registers = OpCode(instruction.OpCode, registers, instruction);
        }
        
        Console.WriteLine("Part 2 answer: " + registers.OpCode);
    }

    static Dictionary<(string code, int instructionNum), int> _codesToInstructionCounts = new Dictionary<(string code, int instructionNum), int>();    

    public static Vector4i OpCode(int code, Vector4i registers, Vector4i instructions)
    {
        return OpCode(_codesToInstructionCounts.Single(kvp => kvp.Key.instructionNum == code).Key.code, registers, instructions);
    }

    public static Vector4i OpCode(string code, Vector4i registers, Vector4i instructions)
    {
        switch (code)
        {
            case "addr": return ADDOpCode(false, registers, instructions);
            case "addi": return ADDOpCode(true, registers, instructions);
            case "mulr": return MULOpCode(false, registers, instructions);
            case "muli": return MULOpCode(true, registers, instructions);
            case "banr": return BitwiseANDOpCode(false, registers, instructions);
            case "bani": return BitwiseANDOpCode(true, registers, instructions);
            case "borr": return BitwiseOROpCode(false, registers, instructions);
            case "bori": return BitwiseOROpCode(true, registers, instructions);
            case "setr": return AssignmentOpCode(false, registers, instructions);
            case "seti": return AssignmentOpCode(true, registers, instructions);
            case "gtir": return GreaterThanOpCode(true, false, registers, instructions);
            case "gtri": return GreaterThanOpCode(false, true, registers, instructions);
            case "gtrr": return GreaterThanOpCode(false, false, registers, instructions);
            case "eqir": return EqualityOpCode(true, false, registers, instructions);
            case "eqri": return EqualityOpCode(false, true, registers, instructions);
            case "eqrr": return EqualityOpCode(false, false, registers, instructions);
            default:
                throw new ArgumentOutOfRangeException(nameof(code));
        }
    }

    public static IEnumerable<string> Codes()
    {
        yield return "addr";
        yield return "addi";
        yield return "mulr";
        yield return "muli";
        yield return "banr";
        yield return "bani";
        yield return "borr";
        yield return "bori";
        yield return "setr";
        yield return "seti";
        yield return "gtir";
        yield return "gtri";
        yield return "gtrr";
        yield return "eqir";
        yield return "eqri";
        yield return "eqrr";
    }

    private static Vector4i ADDOpCode(bool immediate, Vector4i registers, Vector4i instructions)
    {   
        // Addition:
        //
        // addi (add immediate) stores into register C the result of adding register A and value B.
        // addr (add register) stores into register C the result of adding register A and register B.
        
        int result = registers.Get(instructions.A);
        result += immediate ? instructions.B : registers.Get(instructions.B);
        
        Vector4i registersOut = registers;
        registersOut.Set(instructions.C, result);
        
        //Console.WriteLine(registersOut);
        return registersOut;
    }

    private static Vector4i MULOpCode(bool immediate, Vector4i registers, Vector4i instructions)
    {
        // Multiplication:

        // muli (multiply immediate) stores into register C the result of multiplying register A and value B.
        // mulr (multiply register) stores into register C the result of multiplying register A and register B.
        
        int result = registers.Get(instructions.A);
        result *= immediate ? instructions.B : registers.Get(instructions.B);
        
        Vector4i registersOut = registers;
        registersOut.Set(instructions.C, result);
        return registersOut;
    }

    private static Vector4i BitwiseANDOpCode(bool immediate, Vector4i registers, Vector4i instructions)
    {
        // bani (bitwise AND immediate) stores into register C the result of the bitwise AND of register A and value B.
        // banr (bitwise AND register) stores into register C the result of the bitwise AND of register A and register B.

        int result = registers.Get(instructions.A);
        result &= immediate ? instructions.B : registers.Get(instructions.B);
        
        Vector4i registersOut = registers;
        registersOut.Set(instructions.C, result);
        return registersOut;
    }
    
    private static Vector4i BitwiseOROpCode(bool immediate, Vector4i registers, Vector4i instructions)
    {
        // bani (bitwise AND immediate) stores into register C the result of the bitwise AND of register A and value B.
        // banr (bitwise AND register) stores into register C the result of the bitwise AND of register A and register B.

        int result = registers.Get(instructions.A);
        result |= immediate ? instructions.B : registers.Get(instructions.B);
        
        Vector4i registersOut = registers;
        registersOut.Set(instructions.C, result);
        return registersOut;
    }
    
    private static Vector4i AssignmentOpCode(bool immediate, Vector4i registers, Vector4i instructions)
    {
        // seti (set immediate) stores value A into register C. (Input B is ignored.)
        // setr (set register) copies the contents of register A into register C. (Input B is ignored.)

        int result = immediate ? instructions.A : registers.Get(instructions.A);
        
        Vector4i registersOut = registers;
        registersOut.Set(instructions.C, result);
        return registersOut;
    }

    private static Vector4i GreaterThanOpCode(bool immediateA, bool immediateB, Vector4i registers, Vector4i instructions)
    {
        //gtir (greater-than immediate/register) sets register C to 1 if value A is greater than register B. Otherwise, register C is set to 0.
        //gtri (greater-than register/immediate) sets register C to 1 if register A is greater than value B. Otherwise, register C is set to 0.
        //gtrr (greater-than register/register) sets register C to 1 if register A is greater than register B. Otherwise, register C is set to 0.
        
        int a = immediateA ? instructions.A : registers.Get(instructions.A);
        int b = immediateB ? instructions.B : registers.Get(instructions.B);
        int result = (a > b) ? 1 : 0; 
        
        Vector4i registersOut = registers;
        registersOut.Set(instructions.C, result);
        return registersOut;
    }
    
    private static Vector4i EqualityOpCode(bool immediateA, bool immediateB, Vector4i registers, Vector4i instructions)
    {
        // eqir (equal immediate/register) sets register C to 1 if value A is equal to register B. Otherwise, register C is set to 0.
        // eqri (equal register/immediate) sets register C to 1 if register A is equal to value B. Otherwise, register C is set to 0.
        // eqrr (equal register/register) sets register C to 1 if register A is equal to register B. Otherwise, register C is set to 0.
        
        int a = immediateA ? instructions.A : registers.Get(instructions.A);
        int b = immediateB ? instructions.B : registers.Get(instructions.B);
        int result = (a == b) ? 1 : 0; 
        
        Vector4i registersOut = registers;
        registersOut.Set(instructions.C, result);
        return registersOut;
    }
}
    
    public struct Vector4i
    {
        public Vector4i(string[] values)
        {
            OpCode = int.Parse(values[0]);
            A = int.Parse(values[1]);
            B = int.Parse(values[2]);
            C = int.Parse(values[3]);
        }
        
        public Vector4i(int opCode, int a, int b, int c)
        {
            OpCode = opCode;
            A = a;
            B = b;
            C = c;
        }

        public void Set(int register, int value)
        {
            if (register == 0) OpCode = value;
            if (register == 1) A = value;
            if (register == 2) B = value;
            if (register == 3) C = value;
        }

        public int Get(int register)
        {
            if (register == 0) return OpCode;
            if (register == 1) return A;
            if (register == 2) return B;
            if (register == 3) return C;

            throw new ArgumentOutOfRangeException(nameof(register));
        }

        public bool Equals(Vector4i other)
        {
            return OpCode == other.OpCode && A == other.A && B == other.B && C == other.C;
        }

        public override string ToString()
        {
            return $"{OpCode},{A},{B},{C}";
        }

        public int OpCode, A, B, C;
    }
    
    public static class StringExtensions
    {
        public static string GetStringBetweenChars(this string input, char begin, char end)
        {
            return input.Split(begin, end)[1];
        }
        
        public static Vector4i ToVector4i(this string[] str)
        {
            return new Vector4i(str);
        }
    }
}