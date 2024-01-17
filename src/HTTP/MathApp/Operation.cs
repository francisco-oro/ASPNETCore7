namespace MathApp
{
    public class Operation
    {
        public static int? Parse(int num1, int num2, string op)
        {
            switch (op)
            {
                case "add":
                    return num1 + num2;
                case "subtract":
                    return num1 - num2;                
                case "multiply":
                    return num1 * num2;
                case "divide":
                    return num1 / num2;
                case "modulus":
                    return num1 % num2;
                default:
                    throw new ArgumentException($"invalid input for '{op}'");

            }
        }
    }
}
