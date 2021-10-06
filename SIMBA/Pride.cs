using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMBA
{
    class Pride
    {
        private List<Lion> lions;
        private List<Lion> lionesses;
        private List<Lion> maleCubs = new List<Lion>();
        private List<Lion> femaleCubs = new List<Lion>();

        public Pride() { }

        public Pride(List<Lion> lions, List<Lion> lionesses, List<Lion> maleCubs, List<Lion> femaleCubs)
        {
            this.lions = lions;
            this.lionesses = lionesses;
            this.maleCubs = maleCubs;
            this.femaleCubs = femaleCubs;
        }

        public List<Lion> getLions()
        { return lions; }

        public void setLions(List<Lion> lions)
        { this.lions = lions; }

        public List<Lion> getLionesses()
        { return lionesses; }

        public void setLionesses(List<Lion> lionesses)
        { this.lionesses = lionesses; }

        public List<Lion> getMaleCubs()
        { return maleCubs; }

        public void setMaleCubs(Lion maleCubs)
        { this.maleCubs.Add(maleCubs); }

        public List<Lion> getFemaleCubs()
        { return femaleCubs; }

        public void setFemaleCubs(Lion femaleCubs)
        { this.femaleCubs.Add(femaleCubs); }

        public List<int> hunting()
        {
            Random rand = new Random();
            List<string> newAlignment = new List<string>();
            List<int> huntersIndex = new List<int>(); // index list of selected lionesses
            List<int> stayingIndex = new List<int>(); // index list of lionesses which have stayed
            int i = 0, numberOfLionesses = getLionesses().Count;
            Operators op = new Operators();

            // SELECTING HUNTERS RANDOMLY
            // Always 7 hunters
            // 2 - LEFT WING; 3 - CENTER; 2 - RIGHT WING            
            do
            {
                int index = rand.Next(0, numberOfLionesses);
                if (!huntersIndex.Contains(index))
                {
                    huntersIndex.Add(index);
                    i++;
                }
            } while (i < 7);

            //storing the index of lionesses who don't go hunting
            for (int l = 0; l < numberOfLionesses; l++)
            {
                if(!huntersIndex.Contains(l))
                    stayingIndex.Add(l);
            }

            // DEALING WITH LEFT HUNTERS
            for (int j = 0; j < 2; j++)
            {
                int orientation = 0; //0 - attack left, 1 - attack right
                bool improved = false;

                do
                {
                    int alignmentSize = getLionesses()[huntersIndex[j]].getSequences()[0].Length;
                    int inferiorLimit = 0, superiorLimit = 0; // limit of action

                    if (orientation == 0) //attack originally left
                    {
                        inferiorLimit = 0;
                        superiorLimit = alignmentSize / 3;
                    }
                    else //attack opposite way
                    {
                        inferiorLimit = alignmentSize - (alignmentSize / 3);
                        superiorLimit = alignmentSize;
                    }

                    // execute operators
                    newAlignment = op.mutation(getLionesses()[huntersIndex[j]].getSequences(), inferiorLimit, superiorLimit);

                    Evaluation e = new Evaluation();
                    double[] newScore = e.evaluateAlignment(newAlignment);
                    double[] oldScore = getLionesses()[huntersIndex[j]].getFitness();

                    //if the new solution improved, update the lioness
                    if ((newScore[0] > oldScore[0] && newScore[1] >= oldScore[1]) || (newScore[0] >= oldScore[0] && newScore[1] > oldScore[1]))
                    {
                        //update the alignment
                        getLionesses()[huntersIndex[j]].setSequences(newAlignment);
                        getLionesses()[huntersIndex[j]].setFitness(newScore);

                        improved = true;

                        //control the orientation of attack
                        if (orientation == 0)
                            orientation = 1;
                        else
                            orientation = 0;
                    }
                    else
                        improved = false;

                } while (improved != false);
            }

            // DEALING WITH MIDDLE HUNTERS
            for (int j = 2; j < 5; j++)
            {
                bool improved = false;

                do
                {
                    int alignmentSize = getLionesses()[huntersIndex[j]].getSequences()[0].Length;
                    int inferiorLimit = alignmentSize / 3, superiorLimit = inferiorLimit * 2; // limit of action

                    // execute operators
                    newAlignment = op.mutation(getLionesses()[huntersIndex[j]].getSequences(), inferiorLimit, superiorLimit);

                    Evaluation e = new Evaluation();
                    double[] newScore = e.evaluateAlignment(newAlignment);
                    double[] oldScore = getLionesses()[huntersIndex[j]].getFitness();

                    //if the new solution improved, update the lioness
                    if ((newScore[0] > oldScore[0] && newScore[1] >= oldScore[1]) || (newScore[0] >= oldScore[0] && newScore[1] > oldScore[1]))
                    {
                        //update the alignment
                        getLionesses()[huntersIndex[j]].setSequences(newAlignment);
                        getLionesses()[huntersIndex[j]].setFitness(newScore);

                        improved = true;
                    }
                    else
                        improved = false;

                } while (improved != false);
            }

            // DEALING WITH RIGHT HUNTERS
            for (int j = 5; j < 7; j++)
            {
                
                bool improved = false;
                int orientation = 0; //0 - attack right, 1 - attack left

                do
                {
                    int alignmentSize = getLionesses()[huntersIndex[j]].getSequences()[0].Length;
                    int inferiorLimit = alignmentSize - (alignmentSize / 3), superiorLimit = alignmentSize; // limit of action

                    if (orientation == 0) //attack originally right
                    {
                        inferiorLimit = alignmentSize - (alignmentSize / 3);
                        superiorLimit = alignmentSize;
                        
                    }
                    else //attack opposite way
                    {
                        inferiorLimit = 0;
                        superiorLimit = alignmentSize / 3;
                    }

                    // execute operators
                    newAlignment = op.mutation(getLionesses()[huntersIndex[j]].getSequences(), inferiorLimit, superiorLimit);

                    Evaluation e = new Evaluation();
                    double[] newScore = e.evaluateAlignment(newAlignment);
                    double[] oldScore = getLionesses()[huntersIndex[j]].getFitness();

                    //if the new solution improved, update the lioness
                    if ((newScore[0] > oldScore[0] && newScore[1] >= oldScore[1]) || (newScore[0] >= oldScore[0] && newScore[1] > oldScore[1]))
                    {
                        //update the alignment
                        getLionesses()[huntersIndex[j]].setSequences(newAlignment);
                        getLionesses()[huntersIndex[j]].setFitness(newScore);

                        improved = true;

                        //control the orientation of attack
                        if (orientation == 0)
                            orientation = 1;
                        else
                            orientation = 0;
                    }
                    else
                        improved = false;

                } while (improved != false);
            }

            string oi = "";

            return stayingIndex;
        }

        public void roaming(int index)
        {
            int numberOfMales = getLions().Count;

            // for each male do
            for(int i = 0; i < numberOfMales; i++)
            {
                Random rand = new Random();
                int alignmentSize = getLions()[i].getSequences()[0].Length;
                int numberOfBases = (int) Math.Round(alignmentSize * Variables.roamingPercent); //max number of bases to roaming
                int initialPosition = rand.Next(0, alignmentSize - numberOfBases); //beginning of selected bases
                string newAlignment = "";
                bool isEmpty = false; //flag to control whether we have only gaps on the selected region
                List<string> before = new List<string>(); // list of bases before the region
                List<string> after = new List<string>(); // list of bases after the region
                string toRealign = "";

                for (int j = 0; j < getLions()[i].getSequences().Count; j++)
                {
                    before.Add(getLions()[i].getSequences()[j].Substring(0, initialPosition)); //gets the bases before selected region
                    after.Add(getLions()[i].getSequences()[j].Substring(initialPosition + numberOfBases, alignmentSize - (initialPosition + numberOfBases))); //gets the bases after selected region
                    newAlignment = newAlignment + ">" + j + "\n";
                    toRealign = getLions()[i].getSequences()[j].Substring(initialPosition, numberOfBases); //select the region to realign
                    newAlignment = newAlignment + toRealign.Replace("-","") + "\n"; //creating the string to the realigner file
                    
                    // verifies if any sequence has only gaps
                    if(toRealign.Replace("-","") == "")
                    {
                        isEmpty = true;
                        break;
                    }
                }

                //if the selected sequence don't contain only gaps, realign
                if (isEmpty != true)
                {
                    StreamWriter write = new StreamWriter("realigner" + index + ".fasta");
                    write.Write(newAlignment);
                    write.Close();
                    write.Dispose();

                    //executes the process to realign the selected region
                    Process p = new Process();
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.FileName = "cmd.exe";
                    if(rand.Next(2) == 1)
                        p.StartInfo.Arguments = "/c kalign -in realigner" + index + ".fasta -out outrealigner" + index + ".fasta";
                    else
                        p.StartInfo.Arguments = "/c clustalo -in realigner" + index + ".fasta -out outrealigner" + index + ".fasta";
                    p.Start();
                    p.WaitForExit();

                    List<string> realigned = new List<string>(); // list with te realigned sequences
                    int n_seq = 0;
                    string alignment = "";

                    if (File.Exists("outrealigner" + index + ".fasta"))
                    {
                        alignment = File.ReadAllText("outrealigner" + index + ".fasta");
                    }
                    else
                        continue;

                    //storing the realigned sequences on the list
                    for (int k = 0; k < alignment.Length; k++) 
                    {
                        if (String.Compare(alignment[k].ToString(), ">") == 0)
                        {
                            for (int j = k + 1; j < alignment.Length; j++)
                            {
                                if (String.Compare(alignment[j].ToString(), "\n") == 0)
                                {
                                    realigned.Add("");
                                    k = j + 1;
                                    break;
                                }
                            }
                            n_seq++;
                        }
                        if (Char.IsLetter(alignment, k) == true || alignment[k] == '-' || alignment[k] == '.')
                        {
                            if (alignment[k] == '.')
                                realigned[n_seq - 1] = realigned[n_seq - 1].ToString() + "-";
                            else
                                realigned[n_seq - 1] = realigned[n_seq - 1].ToString() + alignment[k].ToString().ToUpper();
                        }
                    }

                    // mounting the realigned region with before and after sequences
                    for (int n = 0; n < realigned.Count; n++)
                        realigned[n] = before[n] + realigned[n] + after[n];

                    //evaluating the new alignment
                    Evaluation e = new Evaluation();
                    double[] newScore = e.evaluateAlignment(realigned);
                    double[] oldScore = getLions()[i].getFitness();
                    //double[] oldScore = e.evaluateAlignment(getLions()[i].getSequences());

                    //if the new solution improved, update the lion
                    if((newScore[0] > oldScore[0] && newScore[1] >= oldScore[1]) || (newScore[0] >= oldScore[0] && newScore[1] > oldScore[1]))
                    {
                        getLions()[i].setSequences(realigned);
                        getLions()[i].setFitness(newScore);
                    }

                    File.Delete("outrealigner" + index + ".fasta");
                    string oi = "";
                }
            }
        }

        public void mating()
        {
            Random r = new Random();

            int lioness_mate = lionesses.Count;

            for (int i = 0; i < lioness_mate; i++)
            {
                double probability = r.NextDouble();
                if (probability <= Variables.matingProbability) //do mating
                {
                    List<List<string>> cubs;
                    int lionToMate = r.Next(lions.Count);
                    int cutPoint = 0;
                    Operators o = new Operators();

                    if (r.NextDouble() <= 1.0) //probability of executing horizontal crossover is 80%
                    {
                        cutPoint = r.Next(1, getLionesses()[i].getSequences().Count);
                        cubs = o.horizontalCrossover(getLionesses()[i].getSequences(), getLions()[lionToMate].getSequences(), cutPoint);
                    }
                    else
                    {
                        cutPoint = r.Next(3, getLionesses()[i].getSequences()[0].Length - 1);
                        cubs = o.verticalCrossover(getLionesses()[i].getSequences(), getLions()[lionToMate].getSequences(), cutPoint);
                    }

                    //generating the new lions
                    Evaluation e = new Evaluation();

                    Lion cub1 = new Lion(cubs[0], e.evaluateAlignment(cubs[0]));
                    Lion cub2 = new Lion(cubs[1], e.evaluateAlignment(cubs[1]));

                    if(r.Next(2) == 0) //randomize the cubs' sex
                    {
                        //setMaleCubs(cub1);
                        //setFemaleCubs(cub2);

                        lions.Add(cub1);
                        lionesses.Add(cub2);
                    }
                    else
                    {
                        //setMaleCubs(cub2);
                        //setFemaleCubs(cub1);

                        lions.Add(cub2);
                        lionesses.Add(cub1);
                    }
                }
            }

            string oi = "";
        }

        public List<Lion> defense()
        {
            //ordering resident males
            Operators o = new Operators();
            o.sorting(getLions());

            int nMalePride = Variables.nMaleResidents / Variables.numberOfPrides;
            List<Lion> toBecomeNomad = new List<Lion>(); //list to store the lions that is gonna become nomads

            //the weakest male will be driven out the pride to become nomad
            while (getLions().Count > nMalePride)
            {
                toBecomeNomad.Add(getLions()[getLions().Count - 1]); //add the lion to the list to become nomad
                getLions().RemoveAt(getLions().Count - 1);
            }

            return toBecomeNomad;
        }

        public List<string> prideGlobalBest() //find the pride global best individual
        {
            List<string> best = new List<string>();
            int bestLion = 0, bestLioness = 0;

            //finding the best lion
            bestLion = 0;
            for (int i = 0; i < getLions().Count; i++)
            {
                if ((getLions()[i].getFitness()[0] > getLions()[bestLion].getFitness()[0] && (getLions()[i].getFitness()[1] >= getLions()[bestLion].getFitness()[1])) || (getLions()[i].getFitness()[0] >= getLions()[bestLion].getFitness()[0] && (getLions()[i].getFitness()[1] > getLions()[bestLion].getFitness()[1])))
                    bestLion = i;
            }

            //finding the best lioness
            bestLioness = 0;
            for (int i = 0; i < getLionesses().Count; i++)
            {
                if ((getLionesses()[i].getFitness()[0] > getLionesses()[bestLioness].getFitness()[0] && (getLionesses()[i].getFitness()[1] >= getLionesses()[bestLioness].getFitness()[1])) || (getLionesses()[i].getFitness()[0] >= getLionesses()[bestLioness].getFitness()[0] && (getLionesses()[i].getFitness()[1] > getLionesses()[bestLioness].getFitness()[1])))
                    bestLioness = i;
            }


            //find the best global
            if ((getLions()[bestLion].getFitness()[0] > getLionesses()[bestLioness].getFitness()[0] && (getLions()[bestLion].getFitness()[1] >= getLionesses()[bestLioness].getFitness()[1])) || (getLions()[bestLion].getFitness()[0] >= getLionesses()[bestLioness].getFitness()[0] && (getLions()[bestLion].getFitness()[1] > getLionesses()[bestLioness].getFitness()[1])))
                best = getLions()[bestLion].getSequences();
            else
                best = getLionesses()[bestLioness].getSequences();


            return best;
        }

        public void moveToSafePlace(List<int> stayingLionesses, List<string> safestPlace)
        {
            Random r = new Random();
            Operators o = new Operators();
            Evaluation e = new Evaluation();
            int numberOfSequences = getLionesses()[0].getSequences().Count;

            for (int i = 0; i < stayingLionesses.Count; i++)
            {
                int cut = r.Next(1, numberOfSequences);
                List<string> currentLioness = getLionesses()[stayingLionesses[i]].getSequences();

                List<string> newLioness = o.learnSaferPlace(currentLioness, safestPlace, cut);

                double[] oldScore = e.evaluateAlignment(currentLioness);
                double[] newScore = e.evaluateAlignment(newLioness);

                //if the result improved, update the solution
                if ((newScore[0] > oldScore[0] && newScore[1] >= oldScore[1]) || (newScore[0] >= oldScore[0] && newScore[1] > oldScore[1]))
                {
                    getLionesses()[stayingLionesses[i]].setSequences(newLioness);
                    getLionesses()[stayingLionesses[i]].setFitness(newScore);
                }
            }
        }


        public void nomadRandomMove()
        {
            Random r = new Random();
            int nLions = getLions().Count;

            // DEALING WITH LIONS
            //for (int i = 0; i < nLions; i++)
            //{
            Parallel.For(0, nLions, i =>
            {
                int alignmentSize = getLions()[i].getSequences()[0].Length;
                int numberOfBases = (int)Math.Round(alignmentSize * (r.Next(5, 26) / 100.0)); //max number of bases to roaming
                int initialPosition = r.Next(0, alignmentSize - numberOfBases); //beginning of selected bases
                string newAlignment = "";
                bool isEmpty = false; //flag to control whether we have only gaps on the selected region
                List<string> before = new List<string>(); // list of bases before the region
                List<string> after = new List<string>(); // list of bases after the region

                for (int j = 0; j < getLions()[i].getSequences().Count; j++)
                {
                    before.Add(getLions()[i].getSequences()[j].Substring(0, initialPosition)); //gets the bases before selected region
                    after.Add(getLions()[i].getSequences()[j].Substring(initialPosition + numberOfBases, alignmentSize - (initialPosition + numberOfBases))); //gets the bases after selected region
                    newAlignment = newAlignment + ">" + j + "\n";
                    string toRealign = getLions()[i].getSequences()[j].Substring(initialPosition, numberOfBases); //select the region to realign
                    newAlignment = newAlignment + toRealign.Replace("-", "") + "\n"; //creating the string to the realigner file

                    // verifies if any sequence has only gaps
                    if (toRealign.Replace("-", "") == "")
                    {
                        isEmpty = true;
                        break;
                    }
                }

                //if the selected sequence don't contain only gaps, realign
                if (isEmpty != true)
                {
                    StreamWriter write = new StreamWriter("realigner" + i +".fasta");
                    write.Write(newAlignment);
                    write.Close();
                    write.Dispose();

                    //executes the process to realign the selected region
                    Process p = new Process();
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.Arguments = "/c kalign -in realigner" + i + ".fasta -out outrealigner" + i + ".fasta";
                    p.Start();
                    p.WaitForExit();

                    List<string> realigned = new List<string>(); // list with te realigned sequences
                    int n_seq = 0;
                    string alignment = "";

                    if (File.Exists("outrealigner" + i + ".fasta"))
                        alignment = File.ReadAllText("outrealigner" + i + ".fasta");
                    else
                        return;

                    //storing the realigned sequences on the list
                    for (int k = 0; k < alignment.Length; k++)
                    {
                        if (String.Compare(alignment[k].ToString(), ">") == 0)
                        {
                            for (int j = k + 1; j < alignment.Length; j++)
                            {
                                if (String.Compare(alignment[j].ToString(), "\n") == 0)
                                {
                                    realigned.Add("");
                                    k = j + 1;
                                    break;
                                }
                            }
                            n_seq++;
                        }
                        if (Char.IsLetter(alignment, k) == true || alignment[k] == '-' || alignment[k] == '.')
                        {
                            if (alignment[k] == '.')
                                realigned[n_seq - 1] = realigned[n_seq - 1].ToString() + "-";
                            else
                                realigned[n_seq - 1] = realigned[n_seq - 1].ToString() + alignment[k].ToString().ToUpper();
                        }
                    }

                    // mounting the realigned region with before and after sequences
                    for (int n = 0; n < realigned.Count; n++)
                        realigned[n] = before[n] + realigned[n] + after[n];

                    //evaluating the new alignment
                    Evaluation e = new Evaluation();
                    double[] newScore = e.evaluateAlignment(realigned);
                    double[] oldScore = getLions()[i].getFitness();
                    //double[] oldScore = e.evaluateAlignment(getLions()[i].getSequences());

                    //if the new solution improved, update the lion
                    if ((newScore[0] > oldScore[0] && newScore[1] >= oldScore[1]) || (newScore[0] >= oldScore[0] && newScore[1] > oldScore[1]))
                    {
                        getLions()[i].setSequences(realigned);
                        getLions()[i].setFitness(newScore);
                    }

                    File.Delete("outrealigner" + i + ".fasta");
                    File.Delete("realigner" + i + ".fasta");
                    string oi = "";

                }
            });
            //}



            // DEALING WITH LIONESSES
            int nLionesses = getLionesses().Count;
            for (int i = 0; i < nLionesses; i++)
            {
                int alignmentSize = getLionesses()[i].getSequences()[0].Length;
                int numberOfBases = (int)Math.Round(alignmentSize * (r.Next(5, 26) / 100.0)); //max number of bases to roaming
                int initialPosition = r.Next(0, alignmentSize - numberOfBases); //beginning of selected bases
                string newAlignment = "";
                bool isEmpty = false; //flag to control whether we have only gaps on the selected region
                List<string> before = new List<string>(); // list of bases before the region
                List<string> after = new List<string>(); // list of bases after the region

                for (int j = 0; j < getLionesses()[i].getSequences().Count; j++)
                {
                    before.Add(getLionesses()[i].getSequences()[j].Substring(0, initialPosition)); //gets the bases before selected region
                    after.Add(getLionesses()[i].getSequences()[j].Substring(initialPosition + numberOfBases, alignmentSize - (initialPosition + numberOfBases))); //gets the bases after selected region
                    newAlignment = newAlignment + ">" + j + "\n";
                    string toRealign = getLionesses()[i].getSequences()[j].Substring(initialPosition, numberOfBases); //select the region to realign
                    newAlignment = newAlignment + toRealign.Replace("-", "") + "\n"; //creating the string to the realigner file

                    // verifies if any sequence has only gaps
                    if (toRealign.Replace("-", "") == "")
                    {
                        isEmpty = true;
                        break;
                    }
                }

                //if the selected sequence don't contain only gaps, realign
                if (isEmpty != true)
                {
                    StreamWriter write = new StreamWriter("realigner.fasta");
                    write.Write(newAlignment);
                    write.Close();
                    write.Dispose();

                    //executes the process to realign the selected region
                    Process p = new Process();
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.Arguments = "/c kalign -in realigner.fasta -out outrealigner.fasta";
                    p.Start();
                    p.WaitForExit();

                    List<string> realigned = new List<string>(); // list with te realigned sequences
                    int n_seq = 0;
                    string alignment = "";

                    if (File.Exists("outrealigner.fasta"))
                        alignment = File.ReadAllText("outrealigner.fasta");
                    else
                        continue;

                    //storing the realigned sequences on the list
                    for (int k = 0; k < alignment.Length; k++)
                    {
                        if (String.Compare(alignment[k].ToString(), ">") == 0)
                        {
                            for (int j = k + 1; j < alignment.Length; j++)
                            {
                                if (String.Compare(alignment[j].ToString(), "\n") == 0)
                                {
                                    realigned.Add("");
                                    k = j + 1;
                                    break;
                                }
                            }
                            n_seq++;
                        }
                        if (Char.IsLetter(alignment, k) == true || alignment[k] == '-' || alignment[k] == '.')
                        {
                            if (alignment[k] == '.')
                                realigned[n_seq - 1] = realigned[n_seq - 1].ToString() + "-";
                            else
                                realigned[n_seq - 1] = realigned[n_seq - 1].ToString() + alignment[k].ToString().ToUpper();
                        }
                    }

                    // mounting the realigned region with before and after sequences
                    for (int n = 0; n < realigned.Count; n++)
                        realigned[n] = before[n] + realigned[n] + after[n];

                    //evaluating the new alignment
                    Evaluation e = new Evaluation();
                    double[] newScore = e.evaluateAlignment(realigned);
                    double[] oldScore = getLionesses()[i].getFitness();
                    //double[] oldScore = e.evaluateAlignment(getLions()[i].getSequences());

                    //if the new solution improved, update the lion
                    if ((newScore[0] > oldScore[0] && newScore[1] >= oldScore[1]) || (newScore[0] >= oldScore[0] && newScore[1] > oldScore[1]))
                    {
                        getLionesses()[i].setSequences(realigned);
                        getLionesses()[i].setFitness(newScore);
                    }

                    File.Delete("outrealigner.fasta");
                    string oi = "";

                }
            }
        }

        
    }
}
