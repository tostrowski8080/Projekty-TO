namespace TO_Lab2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Vector2D v2d_base_1 = new Vector2D(3.0, 4.0);
            Vector3DDecorator v1 = new Vector3DDecorator(v2d_base_1, 1.0);

            Vector3DInheritance v2 = new Vector3DInheritance(-1.0, 2.0, 5.0);
            Vector2D v2d_base_2 = (Vector2D)v2.getSrcV();

            Vector2D v2d_base_3 = new Vector2D(0.0, -2.0);
            Vector3DDecorator v3 = new Vector3DDecorator(v2d_base_3, 0.0);

            IVector[] vectors = { v1, v2, v3 };
            Vector2D[] baseVectors2D = { v2d_base_1, v2d_base_2, v2d_base_3 };
            string[] names = { "v1", "v2", "v3" };
            Console.WriteLine("Wektory");
            for (int i = 0; i < vectors.Length; i++)
            {
                Console.WriteLine($"\n{names[i]} kartezjanskie: [{string.Join(", ", vectors[i].getComponents())}]");
                IPolar2D polarVec = new Polar2DAdapter(baseVectors2D[i]);
                Console.WriteLine($"\t{names[i]} biegunowe 2D: [r: {polarVec.abs():F2}, angle: {polarVec.getAngle():F2} rad]");
                Console.WriteLine($"\t{names[i]} abs 3D: {vectors[i].abs():F2}");
            }
            Console.WriteLine("\ncdot");
            for (int i = 0; i < vectors.Length; i++)
            {
                for (int j = 0; j < vectors.Length; j++)
                {
                    double dotProduct = vectors[i].cdot(vectors[j]);
                    Console.WriteLine($"\t{names[i]} * {names[j]} = {dotProduct:F2}");
                }
            }
            Console.WriteLine("\ncross");
            for (int i = 0; i < vectors.Length; i++)
            {
                for (int j = 0; j < vectors.Length; j++)
                {
                    IVector result;
                    if (vectors[i] is Vector3DDecorator vA)
                    {
                        result = vA.cross(vectors[j]);
                    }
                    else if (vectors[i] is Vector3DInheritance vB)
                    {
                        result = vB.cross(vectors[j]);
                    }
                    else
                    {
                        continue;
                    }
                    Console.WriteLine($"\t{names[i]} × {names[j]} = [{string.Join(", ", result.getComponents())}]");
                }
            }
        }
    }
}
