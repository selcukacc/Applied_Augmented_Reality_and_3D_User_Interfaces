using MathNet.Numerics.LinearAlgebra.Double;
using System;

namespace AugmentedRealityHW2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Test 1.1
            Console.WriteLine(">>>>>>>>>>>>>> Part 1.1 Tests <<<<<<<<<<<<<<");
            double[,] S = { { 3, 5 }, { 25, 53 }, { 45, 170 }, { 13, 64 } };
            double[,] I = { { 6, 10 }, { 28, 58 }, { 48, 175 }, { 16, 69 } };

            double[] result = HomographyCalculation(S, I);
            double[,] homographyMatrix = ArrayToMatrix(result);
            Console.WriteLine("Homography Matrix:");
            PrintMatrix(homographyMatrix);


            // Test 1.3
            Console.WriteLine(">>>>>>>>>>>>>> Part 1.3 Tests <<<<<<<<<<<<<<");
            double[,] point = { { 3 },
                                { 5 },
                                { 1 } };

            Console.WriteLine("\nProjection for Scene point\n");
            double[,] projection = ProjectionCalculation(homographyMatrix, point);
            PrintMatrix(projection);

            // Test 1.4
            Console.WriteLine(">>>>>>>>>>>>>> Part 1.4 Tests <<<<<<<<<<<<<<");
            Console.WriteLine("\nProjection for Image point\n");
            PrintMatrix(ProjectionCalculationForImagePoint(homographyMatrix, projection));

            GivenImagesHomographyCalculation();
        }

        // Problem 1.1
        public static double[] HomographyCalculation(double[,] S, double[,] I)
        {
            // 4 corresponding points have 8 equations.
            int rowLen = S.GetLength(0) * 2;
            int columnLen = 9;

            double[,] equationMatrix = new double[rowLen, columnLen];

            for (int i = 0; i < S.GetLength(0); i++)
            {
                double[,] correspondingCoordinates = { { -S[i, 0], -S[i, 1], -1, 0, 0, 0, S[i, 0] * I[i, 0], S[i, 1] * I[i, 0], I[i, 0] },
                                                  { 0, 0, 0, -S[i, 0], -S[i, 1], -1, S[i, 0] * I[i, 1], S[i, 1] * I[i, 1], I[i, 1] } };
                for (int k = 0; k < 2; k++)
                {
                    for (int j = 0; j < columnLen; j++)
                    {
                        equationMatrix[i * 2, j] = correspondingCoordinates[0, j];
                        equationMatrix[i * 2 + 1, j] = correspondingCoordinates[1, j];
                    }
                }
            }

            // We use single-value-decomposition for solving equations.
            var svd = DenseMatrix.OfArray(equationMatrix).Svd(true);

            // A = U * S * VT(V's transpose)
            // Homography array is VT's last column.
            var result = svd.VT.Row(svd.VT.RowCount - 1);
            return result.AsArray();
        }

        // Problem 1.3
        public static double[,] ProjectionCalculation(double[,] homographyMatrix, double[,] point)
        {
            double[,] multiplication = MultiplyTwoMatrices(homographyMatrix, point);

            var projectionResult = new double[,] { { multiplication[0, 0] / multiplication[2,0] },
                                         { multiplication[1, 0] / multiplication[2,0] },
                                         { 1 } };

            return projectionResult;
        }

        // Problem 1.4
        public static double[,] ProjectionCalculationForImagePoint(double[,] homographyMatrix, double[,] point)
        {
            var inverseOfHomographyMatrix = DenseMatrix.OfArray(homographyMatrix).Inverse();

            var projectionResult = ProjectionCalculation(inverseOfHomographyMatrix.ToArray(), point);
            return projectionResult;
        }

        // Problem 1.5, Problem 1.6 and Problem 1.7
        public static void GivenImagesHomographyCalculation()
        {
            double[,] pdfCoordinates = { { 100, 100 },
                                         { 200, 100 },
                                         { 100, 200 },
                                         { 200, 200 },
                                         { 100, 300 } };

            double[,] image1 = { { 758, 1745 },
                                 { 987, 1743 },
                                 { 759, 1512 },
                                 { 987, 1514 },
                                 { 764, 1277 } };

            double[,] image2 = { { 738, 1939 },
                                 { 971, 1939 },
                                 { 733, 1711 },
                                 { 969, 1714 },
                                 { 729, 1480 } };

            double[,] image3 = { { 878, 1702 },
                                 { 1125, 1705 },
                                 { 894, 1457 },
                                 { 1139, 1467 },
                                 { 910, 1219 } };

            var calculation1 = HomographyCalculation(pdfCoordinates, image1);
            var homography1 = ArrayToMatrix(calculation1);

            var calculation2 = HomographyCalculation(pdfCoordinates, image2);
            var homography2 = ArrayToMatrix(calculation2);

            var calculation3 = HomographyCalculation(pdfCoordinates, image3);
            var homography3 = ArrayToMatrix(calculation3);


            Console.WriteLine(">>>>>>>>>>>>>> Part 1.5 Tests <<<<<<<<<<<<<<");
            Console.WriteLine("------- Image 1 Results -------");
            // Image 1 analysis
            // It should be (300, 300) => (1225, 1287) 
            Console.WriteLine("Result of point: (300, 300): ");
            PrintMatrix(ProjectionCalculation(homography1, new double[,] { { 300 }, { 300 }, { 1 } }));

            // It should be (400, 300) => (1457, 1287)
            Console.WriteLine("Result of point: (400, 300): ");
            PrintMatrix(ProjectionCalculation(homography1, new double[,] { { 400 }, { 300 }, { 1 } }));
            
            // It should be (500, 100) => (1685, 1753)
            Console.WriteLine("Result of point: (500, 100): ");
            PrintMatrix(ProjectionCalculation(homography1, new double[,] { { 500 }, { 100 }, { 1 } }));

            Console.WriteLine("------- Image 2 Results -------");
            // It should be (300, 300) => () 
            Console.WriteLine("Result of point: (300, 300): ");
            PrintMatrix(ProjectionCalculation(homography2, new double[,] { { 300 }, { 300 }, { 1 } }));

            // It should be (400, 300) => ()
            Console.WriteLine("Result of point: (400, 300): ");
            PrintMatrix(ProjectionCalculation(homography2, new double[,] { { 400 }, { 300 }, { 1 } }));

            // It should be (500, 100) => ()
            Console.WriteLine("Result of point: (500, 100): ");
            PrintMatrix(ProjectionCalculation(homography2, new double[,] { { 500 }, { 100 }, { 1 } }));

            Console.WriteLine("------- Image 3 Results -------");
            // It should be (300, 300) => () 
            Console.WriteLine("Result of point: (300, 300): ");
            PrintMatrix(ProjectionCalculation(homography3, new double[,] { { 300 }, { 300 }, { 1 } }));

            // It should be (400, 300) => ()
            Console.WriteLine("Result of point: (400, 300): ");
            PrintMatrix(ProjectionCalculation(homography3, new double[,] { { 400 }, { 300 }, { 1 } }));

            // It should be (500, 100) => ()
            Console.WriteLine("Result of point: (500, 100): ");
            PrintMatrix(ProjectionCalculation(homography3, new double[,] { { 500 }, { 100 }, { 1 } }));

            Console.WriteLine(">>>>>>>>>>>>>> Part 1.6 Tests <<<<<<<<<<<<<<");
            // Problem 1.6
            double[,] S1 = new double[,] { { 7.5f }, { 5.5f }, { 1 } };
            double[,] S2 = new double[3, 1] { { 6.3f }, { 3.3f }, { 1 } };
            double[,] S3 = new double[3, 1] { { 0.1f }, { 0.1f }, { 1 } };

            // problem 1.6 - image 1
            Console.WriteLine("S1 results for image 1: ");
            PrintMatrix(ProjectionCalculation(homography1, new double[,] { { 7.5f }, { 5.5f }, { 1 } }));
            Console.WriteLine("S2 results for image 1: ");
            PrintMatrix(ProjectionCalculation(homography1, new double[,] { { 7.5f }, { 5.5f }, { 1 } }));
            Console.WriteLine("S3 results for image 1: ");
            PrintMatrix(ProjectionCalculation(homography1, new double[,] { { 7.5f }, { 5.5f }, { 1 } }));

            // problem 1.6 - image 2
            Console.WriteLine("S1 results for image 2: ");
            PrintMatrix(ProjectionCalculation(homography2, new double[,] { { 7.5f }, { 5.5f }, { 1 } }));
            Console.WriteLine("S2 results for image 2: ");
            PrintMatrix(ProjectionCalculation(homography2, new double[,] { { 7.5f }, { 5.5f }, { 1 } }));
            Console.WriteLine("S3 results for image 2: ");
            PrintMatrix(ProjectionCalculation(homography2, new double[,] { { 7.5f }, { 5.5f }, { 1 } }));

            // problem 1.6 - image 3
            Console.WriteLine("S1 results for image 3: ");
            PrintMatrix(ProjectionCalculation(homography3, new double[,] { { 7.5f }, { 5.5f }, { 1 } }));
            Console.WriteLine("S2 results for image 3: ");
            PrintMatrix(ProjectionCalculation(homography3, new double[,] { { 7.5f }, { 5.5f }, { 1 } }));
            Console.WriteLine("S3 results for image 3: ");
            PrintMatrix(ProjectionCalculation(homography3, new double[,] { { 7.5f }, { 5.5f }, { 1 } }));

            // Problem 1.7
            Console.WriteLine(">>>>>>>>>>>>>> Part 1.7 Tests <<<<<<<<<<<<<<");
            double[,] I1 = new double[,] { { 500f }, { 400f }, { 1 } };
            double[,] I2 = new double[3, 1] { { 86f }, { 167f }, { 1 } };
            double[,] I3 = new double[3, 1] { { 10f }, { 10f }, { 1 } };

            // problem 1.7 - image 1 
            Console.WriteLine("I1 results for image 1: ");
            PrintMatrix(ProjectionCalculationForImagePoint(homography1, I1));
            Console.WriteLine("I2 results for image 1: ");
            PrintMatrix(ProjectionCalculationForImagePoint(homography1, I2));
            Console.WriteLine("I3 results for image 1: ");
            PrintMatrix(ProjectionCalculationForImagePoint(homography1, I3));

            // problem 1.7 - image 2 
            Console.WriteLine("I1 results for image 2: ");
            PrintMatrix(ProjectionCalculationForImagePoint(homography2, I1));
            Console.WriteLine("I2 results for image 2: ");
            PrintMatrix(ProjectionCalculationForImagePoint(homography2, I2));
            Console.WriteLine("I3 results for image 2: ");
            PrintMatrix(ProjectionCalculationForImagePoint(homography2, I3));

            // problem 1.7 - image 3 
            Console.WriteLine("I1 results for image 3: ");
            PrintMatrix(ProjectionCalculationForImagePoint(homography3, I1));
            Console.WriteLine("I2 results for image 3: ");
            PrintMatrix(ProjectionCalculationForImagePoint(homography3, I2));
            Console.WriteLine("I3 results for image 3: ");
            PrintMatrix(ProjectionCalculationForImagePoint(homography3, I3));

        }

        private static void PrintMatrix(double[,] m)
        {
            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    Console.Write(m[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        private static double[,] MultiplyTwoMatrices(double[,] m1, double[,] m2)
        {
            double[,] multiplicationResult = new double[m1.GetLength(0), m1.GetLength(1)];

            for (int i = 0; i < m2.GetLength(0); i++)
            {
                for (int j = 0; j < m2.GetLength(1); j++)
                {
                    multiplicationResult[i, j] = 0;
                    for (int k = 0; k < m1.GetLength(1); k++)
                    {
                        multiplicationResult[i, j] += m1[i, k] * m2[k, j];
                    }
                }
            }
            return multiplicationResult;
        }

        private static double[,] ArrayToMatrix(double[] homographyArray)
        {
            double[,] result = new double[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    result[i, j] = homographyArray[i * 3 + j];
                }
            }

            return result;
        }

    }
}
