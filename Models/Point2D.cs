namespace PointGraph.Models
{
    public class Point2D
    {
        public string Id { get; set; } = string.Empty;
        public double X { get; set; } // Screen X coordinate
        public double Y { get; set; } // Screen Y coordinate
        public string Color { get; set; } = "#333333";
        public string Label { get; set; } = string.Empty;

        // Dragging state
        public bool IsDragging { get; set; }
        public double DragStartX { get; set; }
        public double DragStartY { get; set; }
        public double PointStartX { get; set; }
        public double PointStartY { get; set; }

        public Point2D(string id, double x, double y, string color, string label)
        {
            Id = id;
            X = x;
            Y = y;
            Color = color;
            Label = label;
        }
    }
}
