using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace ActinUranium.Web.Helpers
{
    public class Collage : List<Rectangle>
    {
        private const int ColumnCount = 3;
        private const int RowCount = 100;

        private static readonly WeightedLottery<Rectangle> RectangleVariants = new WeightedLottery<Rectangle>();

        static Collage()
        {
            RectangleVariants.Add(new Rectangle(0, 0, 1, 1), 4);
            RectangleVariants.Add(new Rectangle(0, 0, 1, 2), 4);
            RectangleVariants.Add(new Rectangle(0, 0, 2, 2), 1);
        }

        public static Collage Create(int elementCount)
        {
            var collage = new Collage();

            bool[,] bitmap = new bool[RowCount, ColumnCount];

            for (int elementIndex = 0; elementIndex < elementCount; elementIndex++)
            {
                Rectangle rectangle = RectangleVariants.Next();
                rectangle = Align(bitmap, rectangle);
                collage.Add(rectangle);
            }

            return collage;
        }

        private static Rectangle Align(bool[,] bitmap, Rectangle rectangle)
        {
            for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
                {
                    if (!bitmap[rowIndex, columnIndex])
                    {
                        if ((rectangle.Width == 1) && (rectangle.Height == 1))
                        {
                            bitmap[rowIndex, columnIndex] = true;

                            rectangle.X = columnIndex;
                            rectangle.Y = rowIndex;

                            return rectangle;
                        }
                        else if ((rectangle.Width == 1) && (rectangle.Height == 2))
                        {
                            if (((rowIndex + 1) < RowCount) && !bitmap[rowIndex + 1, columnIndex])
                            {
                                bitmap[rowIndex, columnIndex] = true;
                                bitmap[rowIndex + 1, columnIndex] = true;

                                rectangle.X = columnIndex;
                                rectangle.Y = rowIndex;

                                return rectangle;
                            }
                        }
                        else if ((rectangle.Width == 2) && (rectangle.Height == 2))
                        {
                            if ((((columnIndex + 1) < ColumnCount) && !bitmap[rowIndex, columnIndex + 1]) &&
                                (((rowIndex + 1) < RowCount) && !bitmap[rowIndex + 1, columnIndex]) &&
                                (!bitmap[rowIndex + 1, columnIndex + 1]))
                            {
                                bitmap[rowIndex, columnIndex] = true;
                                bitmap[rowIndex, columnIndex + 1] = true;
                                bitmap[rowIndex + 1, columnIndex] = true;
                                bitmap[rowIndex + 1, columnIndex + 1] = true;

                                rectangle.X = columnIndex;
                                rectangle.Y = rowIndex;

                                return rectangle;
                            }
                        }
                        else
                        {
                            Debug.Fail("Invalid size.");
                        }
                    }
                }
            }

            return rectangle;
        }
    }
}
