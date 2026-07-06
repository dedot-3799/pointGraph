namespace PointGraph.Math
{
    public class CoordinateSystem
    {
        public double Width { get; }
        public double Height { get; }
        public double Scale { get; }

        public CoordinateSystem(double width = 500, double height = 500, double scale = 20)
        {
            Width = width;
            Height = height;
            Scale = scale;
        }

        // Screen Coordinates (top-left is 0,0) -> Cartesian Coordinates (center is 0,0)
        public (double X, double Y) ScreenToCartesian(double screenX, double screenY)
        {
            double cartesianX = (screenX - Width / 2) / Scale;
            double cartesianY = (Height / 2 - screenY) / Scale;
            return (cartesianX, cartesianY);
        }

        // Cartesian Coordinates (center is 0,0) -> Screen Coordinates (top-left is 0,0)
        public (double X, double Y) CartesianToScreen(double cartesianX, double cartesianY)
        {
            double screenX = cartesianX * Scale + Width / 2;
            double screenY = Height / 2 - cartesianY * Scale;
            return (screenX, screenY);
        }
    }
}
