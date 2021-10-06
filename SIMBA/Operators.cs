using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMBA
{
    class Operators
    {
        public List<string> mutation(List<string> alignment, int inferiorLimit, int superiorLimit)
        {
            List<string> newAlignment = new List<string>(alignment);

            newAlignment = moveBlocks(newAlignment, inferiorLimit, superiorLimit);

            newAlignment = mergeBlocks(newAlignment, inferiorLimit, superiorLimit);

            newAlignment = divideBlock(newAlignment, inferiorLimit, superiorLimit);

            newAlignment = deleteGapColumns(newAlignment);

            return newAlignment;
        }

        //moves a random block of gaps
        private List<string> moveBlocks(List<string> alignment, int inferiorLimit, int superiorLimit)
        {
            Random rand = new Random();

            for(int i = 0; i < alignment.Count; i++)
            {
                int initialPosition = rand.Next(inferiorLimit, superiorLimit); //select a position
                
                initialPosition = alignment[i].IndexOf("-", initialPosition); //find the first gap from the selected position
                int finalPosition = initialPosition;

                if (initialPosition != -1)
                {
                    //find the final of the gap block
                    while (alignment[i][finalPosition] == '-' && finalPosition < alignment[i].Length - 1)
                        finalPosition++;

                    char[] vetRes = alignment[i].ToCharArray();

                    if (alignment[i][finalPosition] != '-' && initialPosition != finalPosition) //if it is not the end of alignment, move to the right
                    {
                        vetRes[initialPosition] = alignment[i][finalPosition];
                        initialPosition++;

                        for (int z = initialPosition; z <= finalPosition; z++)
                            vetRes[z] = '-';

                        alignment[i] = new string(vetRes);
                    }
                    else //if it is the end of alignment, move left
                    {
                        vetRes[finalPosition] = vetRes[initialPosition - 1];
                        vetRes[initialPosition - 1] = '-';

                        alignment[i] = new string(vetRes);
                    }
                }
            }

            return alignment;
        }

        //merge two gap blocks
        private List<string> mergeBlocks(List<string> alignment, int inferiorLimit, int superiorLimit)
        {
            //ESTÁ COM BUG !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! REVER
            //USAR CASE AA.FA PARA VER

            Random rand = new Random();
            

            for (int i = 0; i < alignment.Count; i++)
            {
                int trials = 0;
                bool flag = true;

                if (rand.Next(0, 2) == 1) //randomize if the sequence will be processed
                {
                    //randomize until find a base which is not a gap
                    int initialPosition = rand.Next(inferiorLimit, superiorLimit);
                    while (alignment[i][initialPosition] == '-')
                    {
                        initialPosition = rand.Next(inferiorLimit, superiorLimit);

                        if(trials > 3)
                        {
                            flag = false;
                            break;
                        }

                        trials++;
                    }

                    if (flag == false)
                        continue;
                    else
                        trials = 0;


                    //find the first block of bases
                    int finalPosition = initialPosition;                                    
                    while (alignment[i][finalPosition] != '-' && finalPosition < alignment[i].Length - 1)
                        finalPosition++;

                    //find the begin of the second block of bases
                    int initialPosition2 = finalPosition;
                    while (alignment[i][initialPosition2] == '-' && initialPosition2 < alignment[i].Length - 1)
                        initialPosition2++;

                    //find the final of the second block of bases
                    int finalPosition2 = initialPosition2;
                    while (alignment[i][finalPosition2] != '-' && finalPosition2 < alignment[i].Length - 1)
                        finalPosition2++;

                    char[] vetRes = alignment[i].ToCharArray(); //aux array

                    int control = finalPosition;

                    //merge the two blocks
                    if (control < alignment[i].Length)
                    {
                        for (int z = initialPosition2; z < finalPosition2; z++)
                        {
                            vetRes[control] = alignment[i][z];
                            vetRes[z] = '-';
                            control++;
                        }

                        alignment[i] = new string(vetRes);
                    }
                }
            }

            return alignment;
        }

        //divide a random block into two blocks gapped
        private List<string> divideBlock(List<string> alignment, int inferiorLimit, int superiorLimit)
        {
            Random rand = new Random();
            bool valid = false;
            int sequence = rand.Next(0, alignment.Count); //randomize a sequence to deal
            int initialPosition = 0, finalPosition = 0;
            string before = "", after = "";
            int ctrl = 0;

            while (valid == false)
            {
                if (ctrl == 5) //avoid running forever
                    return alignment;

                initialPosition = rand.Next(inferiorLimit, superiorLimit);
                if(alignment[sequence][initialPosition] != '-')
                {
                    finalPosition = initialPosition;
                    while(alignment[sequence][finalPosition] != '-' && finalPosition < alignment[sequence].Length - 1)
                    {
                        ctrl = 0;
                        finalPosition++;
                        valid = true;
                    }
                }

                ctrl++;
            }

            int selectedPosition = rand.Next(initialPosition, finalPosition);

            before = alignment[sequence].Substring(0, selectedPosition); //gets the bases before selected region
            after = alignment[sequence].Substring(selectedPosition, alignment[0].Length - selectedPosition); //gets the bases after selected region

            //divide the block into two
            alignment[sequence] = before + '-' + after;

            //add a gap at the final of each other sequence of the alignment
            for(int i = 0; i < alignment.Count; i++)
            {
                if (i != sequence)
                {
                    if (rand.Next(0, 2) == 0)
                        alignment[i] = alignment[i] + '-';
                    else
                        alignment[i] = '-' + alignment[i];
                }
            }

            return alignment;
        }

        //delete columns of gaps
        private List<string> deleteGapColumns(List<string> alignment)
        {
            for(int j = 0; j < alignment[0].Length; j++)
            {
                for(int i = 0; i < alignment.Count; i++)
                {
                    if (alignment[i][j] != '-')
                        break;
                    
                    if(i == alignment.Count - 1)
                    {
                        for (int z = 0; z < alignment.Count; z++)
                            alignment[z] = alignment[z].Remove(j, 1);
                    }

                }
            }

            return alignment;
        }

        //vertical crossover
        public List<List<string>> verticalCrossover(List<string> female, List<string> male, int cutPoint)
        {
            //lists to represent the alignments of each cub
            List<string> cub1 = new List<string>();
            List<string> cub2 = new List<string>();

            for(int i = 0; i < female.Count; i++)
            {
                int count = 0;

                for (int j = 0; j < cutPoint; j++)
                {
                    if (female[i][j] != '-') //count how many bases until cutpoint
                        count++;

                    //before += female[i][j];
                }

                cub1.Add(female[i].Substring(0, cutPoint)); //stores the first part, until the cutpoint, for cub1

                int x = 0;
                int index = 0;
                string seq = "";

                while(x < count) //searching for the cutpoint, based on the number of bases counted above
                {
                    seq += male[i][index]; //stores the first part of the alignment for cub2

                    if (male[i][index] != '-')                                          
                        x++;

                    index++; //controls where is the variable cutpoint for parent2
                }

                cub2.Add(seq); //adds the first part of the alignment for cub2
                cub2[i] += female[i].Substring(cutPoint, (female[i].Length - cutPoint)); //
                
                cub1[i] += male[i].Substring(index, (male[i].Length - index));

                //falta verificar se tem que por gap no fim para deixar tamanhos das sequÊncias iguais
            }

            List<List<string>> cubs = new List<List<string>>();
            cubs.Add(cub1);
            cubs.Add(cub2);

            //adding gaps to equal the seq sizes
            for (int i = 0; i < 2; i++)
            {
                //finding the longest seq for each cub
                int max = cubs[i][0].Length;

                for (int j = 0; j < cubs[i].Count; j++)
                {
                    if (cubs[i][j].Length > max)
                        max = cubs[i][j].Length;
                }

                //fill with gaps at the end of the alignment
                for (int j = 0; j < cubs[i].Count; j++)
                {
                    while (cubs[i][j].Length != max)
                        cubs[i][j] += '-';
                }

                //cubs[i] = deleteGapColumns(cubs[i]);
            }

            return cubs;
        }

        //horizontal crossover
        public List<List<string>> horizontalCrossover(List<string> female, List<string> male, int cutPoint)
        {
            List<List<string>> cubs = new List<List<string>>();

            //lists to represent the alignments of each cub
            List<string> cub1 = new List<string>();
            List<string> cub2 = new List<string>();

            //taking the first part for each cubs
            for(int i = 0; i < cutPoint; i++)
            {
                cub1.Add(female[i]);
                cub2.Add(male[i]);
            }

            //taking the second part for each cubs
            for (int j = cutPoint; j < female.Count; j++)
            {
                cub1.Add(male[j]);
                cub2.Add(female[j]);
            }

            cubs.Add(cub1);
            cubs.Add(cub2);

            //adding gaps to equal the seq sizes
            for(int i = 0; i < 2; i++)
            {
                //finding the longest seq for each cub
                int max = cubs[i][0].Length;

                for(int j = 0; j < cubs[i].Count; j++)
                {
                    if (cubs[i][j].Length > max)
                        max = cubs[i][j].Length;
                }

                //fill with gaps at the end of the alignment
                for(int j = 0; j < cubs[i].Count; j++)
                {
                    while (cubs[i][j].Length != max)
                        cubs[i][j] += '-';
                }
            }

            return cubs;
        }

        public void sorting(List<Lion> individuals)
        {
            Lion aux = new Lion();

            //multiobjective sorting
            for(int i = 0; i < individuals.Count - 1; i++)
            {
                for(int j = i + 1; j < individuals.Count; j++)
                {
                    if( (individuals[j].getFitness()[0] > individuals[i].getFitness()[0] && (individuals[j].getFitness()[1] >= individuals[i].getFitness()[1])) || (individuals[j].getFitness()[0] >= individuals[i].getFitness()[0] && (individuals[j].getFitness()[1] > individuals[i].getFitness()[1])) )
                    {
                        aux = individuals[i];
                        individuals[i] = individuals[j];
                        individuals[j] = aux;
                    }
                }
            }

            string oi = "";
        }

        public List<string> learnSaferPlace(List<string> stayingLioness, List<string> safestPlace, int cutPoint)
        {
            List<string> newPlace = new List<string>();

            //taking the first part
            for (int i = 0; i < cutPoint; i++)
                newPlace.Add(stayingLioness[i]);

            //taking the second part for each cubs
            for (int j = cutPoint; j < safestPlace.Count; j++)
                newPlace.Add(safestPlace[j]);

            
             //finding the longest seq for each cub
             int max = newPlace[0].Length;

             for (int j = 0; j < newPlace.Count; j++)
             {
                 if (newPlace[j].Length > max)
                     max = newPlace[j].Length;
             }

             //fill with gaps at the end of the alignment
             for (int j = 0; j < newPlace.Count; j++)
             {
                 while (newPlace[j].Length != max)
                    newPlace[j] += '-';
             }

            return newPlace;
        }
    }
}