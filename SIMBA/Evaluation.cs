using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMBA
{
    class Evaluation
    {
        // calculates the multiobjective fitness [WSP,TC]
        public double[] evaluateAlignment(List<string> alignment)
        {
            double[] fitness = new double[2];
            fitness[0] = weightedSumOfPairs(alignment);
            fitness[1] = totalColumnScore(alignment);
            return fitness;
        }

        public double weightedSumOfPairs(List<string> alignment) // calculates WSP score
        {
            double wspScore = 0.0;

            // calculates the sum-of-pairs
            for (int L = 0; L < alignment[0].Length; L++)
            {
                StreamReader read = new StreamReader("weights.txt");

                for (int i = 0; i < alignment.Count - 1; i++)
                {
                    for (int j = i + 1; j < alignment.Count; j++)
                    {
                        int resA = charToIndex(alignment[i][L]);
                        int resB = charToIndex(alignment[j][L]);
                        double w = Convert.ToDouble(read.ReadLine());
                        if (resA != 24 && resB != 24)
                            wspScore += (w * Variables.BLOSUM62[resA, resB]);
                    }
                }
                read.Close();
                read.Dispose();
            }

            double agp = 0.0;

            // calculates the affine gap
            for (int i = 0; i < alignment.Count; i++)
            {
                int j = 0;
                while ((j = alignment[i].IndexOf('-', j)) != -1)
                {
                    agp += Variables.gOpen;
                    j++;

                    while (j < alignment[0].Length && alignment[i][j] == '-')
                    {
                        agp += Variables.gExtended;
                        j++;
                    }
                }
            }

            wspScore -= agp;

            return wspScore;
        }

        public int charToIndex(char residue) // convert the residue to index to search on the PAM250 matrix
        {
            switch(residue)
            {
                case 'A':
                    return 0;
                case 'R':
                    return 1;
                case 'N':
                    return 2;
                case 'D':
                    return 3;
                case 'C':
                    return 4;
                case 'Q':
                    return 5;
                case 'E':
                    return 6;
                case 'G':
                    return 7;
                case 'H':
                    return 8;
                case 'I':
                    return 9;
                case 'L':
                    return 10;
                case 'K':
                    return 11;
                case 'M':
                    return 12;
                case 'F':
                    return 13;
                case 'P':
                    return 14;
                case 'S':
                    return 15;
                case 'T':
                    return 16;
                case 'W':
                    return 17;
                case 'Y':
                    return 18;
                case 'V':
                    return 19;
                case 'B':
                    return 20;
                case 'J':
                    return 21;
                case 'Z':
                    return 22;
                case 'X':
                    return 23;
                case '-':
                    return 24;
                default:
                    return 25;
            }
        }

        //public int charToIndex(char residue) // convert the residue to index to search on the BLOSUM matrix
        //{
        //    switch (residue)
        //    {
        //        case 'A':
        //            return 0;
        //        case 'R':
        //            return 1;
        //        case 'N':
        //            return 2;
        //        case 'D':
        //            return 3;
        //        case 'C':
        //            return 4;
        //        case 'Q':
        //            return 5;
        //        case 'E':
        //            return 6;
        //        case 'G':
        //            return 7;
        //        case 'H':
        //            return 8;
        //        case 'I':
        //            return 9;
        //        case 'L':
        //            return 10;
        //        case 'K':
        //            return 11;
        //        case 'M':
        //            return 12;
        //        case 'F':
        //            return 13;
        //        case 'P':
        //            return 14;
        //        case 'S':
        //            return 15;
        //        case 'T':
        //            return 16;
        //        case 'W':
        //            return 17;
        //        case 'Y':
        //            return 18;
        //        case 'V':
        //            return 19;
        //        case 'B':
        //            return 20;
        //        case 'Z':
        //            return 21;
        //        case 'X':
        //            return 22;
        //        case '-':
        //            return 23;
        //        default:
        //            return 24;
        //    }
        //}

        public double totalColumnScore(List<string> alignment) // calculates TC Score
        {
            int tcScore = 0;
            for(int j = 0; j < alignment[0].Length; j++)
            {
                for(int i = 1; i < alignment.Count; i++)
                {
                    if ((alignment[0][j] != alignment[i][j]) || (alignment[0][j] == '-' || alignment[i][j] == '-'))
                        break;
                    if (i == alignment.Count - 1)
                        tcScore++;
                }
            }
            return tcScore;
        }
    }
}
