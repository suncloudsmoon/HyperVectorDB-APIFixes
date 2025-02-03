//
// Copyright © César Souza, 2009-2017
// cesarsouza at gmail.com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperVectorDB {
    static class Math {
        public static double CosineSimilarity(double[] x, double[] y) {
            double num = 0.0;
            double num2 = 0.0;
            double num3 = 0.0;
            for (int i = 0; i < x.Length; i++) {
                num += x[i] * y[i];
                num2 += x[i] * x[i];
                num3 += y[i] * y[i];
            }
            double num4 = System.Math.Sqrt(num2) * System.Math.Sqrt(num3);
            if (num != 0.0) {
                return 1.0 - num / num4;
            }
            return 1.0;
        }
        public static double JaccardDissimilarity(double[] x, double[] y) {
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < x.Length; i++) {
                if (x[i] != 0.0 || y[i] != 0.0) {
                    if (x[i] == y[i]) {
                        num++;
                    }
                    num2++;
                }
            }
            if (num2 != 0) {
                return 1.0 - (double)num / (double)num2;
            }
            return 0.0;
        }
        public static double EuclideanDistance(double[] x, double[] y) {
            double num = 0.0;
            for (int i = 0; i < x.Length; i++) {
                double num2 = x[i] - y[i];
                num += num2 * num2;
            }
            return System.Math.Sqrt(num);
        }
        public static double ManhattanDistance(double[] x, double[] y) {
            double num = 0.0;
            for (int i = 0; i < x.Length; i++) {
                num += System.Math.Abs(x[i] - y[i]);
            }
            return num;
        }
        public static double ChebyshevDistance(double[] x, double[] y) {
            double num = System.Math.Abs(x[0] - y[0]);
            for (int i = 1; i < x.Length; i++) {
                double num2 = System.Math.Abs(x[i] - y[i]);
                if (num2 > num) {
                    num = num2;
                }
            }
            return num;
        }
        public static double CanberraDistance(double[] x, double[] y) {
            double num = 0.0;
            for (int i = 0; i < x.Length; i++) {
                num += System.Math.Abs(x[i] - y[i]) / (System.Math.Abs(x[i]) + System.Math.Abs(y[i]));
            }
            return num;
        }
    }
}