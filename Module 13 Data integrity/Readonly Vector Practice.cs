namespace ReadOnlyVector
{
    public class ReadOnlyVector
    {
        public readonly double X;
        public readonly double Y;

        public ReadOnlyVector(double xComponent, double yComponent)
        {
            X = xComponent;
            Y = yComponent;
        }

        public ReadOnlyVector Add(ReadOnlyVector another)
        {
            double resultingX = X + another.X;
            double resultingY = Y + another.Y;
            return new ReadOnlyVector(resultingX, resultingY);
        }

        public ReadOnlyVector WithX(double replacementX)
        {
            return new ReadOnlyVector(replacementX, Y);
        }

        public ReadOnlyVector WithY(double replacementY)
        {
            return new ReadOnlyVector(X, replacementY);
        }
    }
}
