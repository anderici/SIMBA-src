using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace SIMBA
{
    class LionOptimizationAlgorithm
    {
        private List<Pride> prides = new List<Pride>();

        // GENERATES THE INITAL POPULATION AND DIVIDE IT AMONG THE PRIDES
        public void run(Form1 F)
        {
            /*int nNomad = (int) Math.Ceiling(Variables.popSize * Variables.nomadRatio); //number of nomads
            int nResident = Variables.popSize - nNomad; //total number of residents
            int nFemaleNomads = (int)Math.Ceiling(nNomad * (1 - Variables.femaleRatio)); //number of female nomads
            int nMaleNomads = nNomad - nFemaleNomads; //number of male nomads
            int nFemaleResidents = (int)Math.Ceiling(nResident * Variables.femaleRatio); //number of female residents
            int nMaleResidents = nResident - nFemaleResidents; //number of male residents*/
            Random r = new Random();
            List<Lion> initialPopulation = new List<Lion>(); //list with generated initial population

            var watch = System.Diagnostics.Stopwatch.StartNew();

            //for (int i = 0; i < Variables.popSize; i++)
            //{
            //    initialPopulation.Add(generateInitialAlignment(i)); // fill the general initial population list
            //}

            Parallel.For(0, Variables.popSize, i =>
            {
               initialPopulation.Add(generateInitialAlignment(i)); // fill the general initial population list
            });




            //********** FILLING THE NOMAD PRIDE **********//

            Pride nomad = new Pride();
            List<Lion> mN = new List<Lion>(); // list of nomad males selected from initial population
            List<Lion> fN = new List<Lion>(); // list of nomad females selected from initial population

            //generating the male nomads list
            for (int i = 0; i < Variables.nMaleNomads; i++)
            {
                int index = r.Next(0, initialPopulation.Count);
                mN.Add(initialPopulation[index]);
                initialPopulation.RemoveAt(index);
            }

            //generating the female nomads list
            for (int i = 0; i < Variables.nFemaleNomads; i++)
            {
                int index = r.Next(0, initialPopulation.Count);
                fN.Add(initialPopulation[index]);
                initialPopulation.RemoveAt(index);
            }

            nomad.setLions(mN);
            nomad.setLionesses(fN);

            // add the new pride to the prides' list
            prides.Add(nomad);



            //********** FILLING THE RESIDENT PRIDES **********//

            int nMalePride = Variables.nMaleResidents / Variables.numberOfPrides;
            int nFemalePride = Variables.nFemaleResidents / Variables.numberOfPrides;

            for (int i = 0; i < Variables.numberOfPrides; i++)
            {
                Pride resident = new Pride();
                List<Lion> mR = new List<Lion>(); // list of redisent males selected from initial population
                List<Lion> fR = new List<Lion>(); // list of resident females selected from initial population

                //generating the male residents list
                for (int j = 0; j < nMalePride; j++)
                {
                    int index = r.Next(0, initialPopulation.Count);
                    mR.Add(initialPopulation[index]);
                    initialPopulation.RemoveAt(index);
                }

                //generating the female residents list
                for (int j = 0; j < nFemalePride; j++)
                {
                    int index = r.Next(0, initialPopulation.Count);
                    fR.Add(initialPopulation[index]);
                    initialPopulation.RemoveAt(index);
                }

                resident.setLions(mR);
                resident.setLionesses(fR);

                // add the new pride to the prides' list
                prides.Add(resident);
            }


            int nGeneration = 0;
            
            do
            {
                //********** FOR EACH RESIDENT PRIDE, PROCESS **********//
                //for (int i = 1; i < prides.Count; i++)
                //{
                Parallel.For(1, prides.Count, i =>
                {
                    System.Windows.Forms.Application.DoEvents();
                    //********** HUNTING **********//
                    List<int> stayingLionesses = prides[i].hunting();

                    //********** MOVING TO SAFER PLACE **********//
                    List<string> prideGlobalBest = prides[i].prideGlobalBest();
                    prides[i].moveToSafePlace(stayingLionesses, prideGlobalBest);

                    //********** ROAMING **********//
                    prides[i].roaming(i);

                    //********** MATING **********//
                    prides[i].mating();

                    //DEFENSE AGAINST NEW MATURE//
                    List<Lion> toBecomeNomads = prides[i].defense();

                    //adding the driven out resident males to nomad pride
                    for (int j = 0; j < toBecomeNomads.Count; j++)
                        prides[0].getLions().Add(toBecomeNomads[j]);

                    string olá = "";
                });
                //}

                string oi = "";


                //********** FOR NOMADS, PROCESS **********//

                //********** MOVING RANDOMLY **********//
                prides[0].nomadRandomMove();

                //********** MATING **********//
                prides[0].mating();

                //********** NOMAD ATTACK **********//
                nomadAttack();


                //********** FOR EACH RESIDENT PRIDE, IMMIGRATE **********//
                for (int i = 1; i < prides.Count; i++)
                {
                    Random rand = new Random();
                    int excedent = prides[i].getLionesses().Count - nFemalePride;
                    int nImmigrate = excedent + (int)Math.Round(Variables.immigrateRate * nFemalePride);

                    // randomly choose the lionesses to become nomad
                    for (int j = 0; j < nImmigrate; j++)
                    {
                        int index = rand.Next(0, prides[i].getLionesses().Count);
                        prides[0].getLionesses().Add(prides[i].getLionesses()[index]);
                        prides[i].getLionesses().RemoveAt(index);
                    }
                    string hello = "";
                }


                //********** FOR EACH PRIDE, equlibrate the population **********//
                populationEquilibrium(nMalePride, nFemalePride);

                nGeneration++;
                string ola = "";

            } while (nGeneration < Variables.generations);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            //concatenating all lions
            List<Lion> wholePop = new List<Lion>();

            for(int i = 0; i < Variables.numberOfPrides + 1; i++)
            {
                wholePop.AddRange(prides[i].getLions());
                wholePop.AddRange(prides[i].getLionesses());
            }

            Operators o = new Operators();
            o.sorting(wholePop);

            string fileToWrite = "";
            if (File.Exists("Output\\" + Variables.fileName) == false)
                fileToWrite = "Output\\" + Variables.fileName;
            else
                fileToWrite = "Output\\" + Variables.fileName.Split('.')[0] + "_.fasta";

            string writeEt = "";
            if (File.Exists("Runtime\\" + Variables.fileName) == false)
                writeEt = "Runtime\\" + Variables.fileName;
            else
                writeEt = "Runtime\\" + Variables.fileName.Split('.')[0] + "_.fasta";

            StreamWriter sw = new StreamWriter(fileToWrite);
            StreamWriter et = new StreamWriter(writeEt);

            string alinhamento = "", toTxt = "";

            if (Variables.clustal_final.getFitness()[0] >= wholePop[0].getFitness()[0])
                wholePop[0] = Variables.clustal_final;
            else if (Variables.kalign_final.getFitness()[0] >= wholePop[0].getFitness()[0])
                wholePop[0] = Variables.kalign_final;

            for (int i = 0; i < Variables.seqNames.Count; i++)
            {
                alinhamento = alinhamento + ">" + Variables.seqNames[i] + "\n" + wholePop[0].getSequences()[i] + "\n";
                toTxt = toTxt + ">" + Variables.seqNames[i] + "\r\n" + wholePop[0].getSequences()[i] + "\r\n\r\n";
            }
            sw.Write(alinhamento);
            et.Write(elapsedMs);
            sw.Close();
            sw.Dispose();
            et.Close();
            et.Dispose();

            F.txtOutput.Text = alinhamento + "\r\n\r\nElapsed time: " + elapsedMs;
        }

        public Lion generateInitialAlignment(int aligned_index) //SE PARALELO, USAR UM INT INDEX COMO PARAMETRO E COLOCAR ALIGNED_INDEX
        {
            List<string> seqs = new List<string>();
            string Name = "";
            int n_seq = 0;
            Random r = new Random();
            int rInt = r.Next(0, 2);
            int kind = 0;
            System.Windows.Forms.Application.DoEvents();

            //executes the process to generate initial population alignments
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "cmd.exe";
            switch (rInt)
            {
                case 0: //use default kalign
                    //p.StartInfo.Arguments = "/c kalign -in BB12006.fasta -out aligned.fasta";
                    p.StartInfo.Arguments = "/c kalign -in Input\\" + Variables.fileName + " -out aligned" + aligned_index + ".fasta";
                    kind = 0;
                    break;

                case 1: //use default Clustal Omega
                    p.StartInfo.Arguments = "/c clustalo -i Input\\" + Variables.fileName + " -o aligned" + aligned_index + ".fasta --force";
                    kind = 1;
                    break;

                    //MUSCLE MUDA A ORDEM DAS SEQUÊNCIAS!! POR ISSO TIREI, POR ENQUANTO

                    //case 1: //use default muscle
                    //p.StartInfo.Arguments = "/c muscle -in 1abb.fa -out aligned.fasta";
                    //break;
            }
            p.Start();
            p.WaitForExit();

            string alignment = File.ReadAllText("aligned" + aligned_index + ".fasta");
            File.Delete("aligned" + aligned_index + ".fasta");

            for (int i = 0; i < alignment.Length; i++) //here we store the seq name
            {
                if (String.Compare(alignment[i].ToString(), ">") == 0)
                {
                    for (int j = i + 1; j < alignment.Length; j++)
                    {
                        if (String.Compare(alignment[j].ToString(), "\n") != 0)
                        {
                            Name += alignment[j];
                        }
                        else
                        {
                            seqs.Add("");
                            i = j+1;
                            break;
                        }
                    }
                    n_seq++;
                }
                if (Char.IsLetter(alignment, i) == true || alignment[i] == '-' || alignment[i] == '.')
                {
                    if (alignment[i] == '.')
                        seqs[n_seq - 1] = seqs[n_seq - 1].ToString() + "-";
                    else
                        seqs[n_seq - 1] = seqs[n_seq - 1].ToString() + alignment[i].ToString().ToUpper();
                }
            }

            //creating the lion object to fill the pride
            Lion lion = new Lion();
            Evaluation eval = new Evaluation();
            double[] fitness = eval.evaluateAlignment(seqs);
            lion.setSequences(seqs);
            lion.setFitness(fitness);

            if (kind == 0)
                Variables.kalign_final = lion;
            else
                Variables.clustal_final = lion;

            return lion;
        }


        // nomad attack operator
        public void nomadAttack()
        {
            Random r = new Random();

            for (int i = 0; i < prides[0].getLions().Count; i++) //for each lion
            {
                for (int j = 1; j < Variables.numberOfPrides; j++) //for each pride
                {
                    bool won = false;

                    if (r.Next(2) == 1) // attacks the pride
                    {
                        for(int z = 0; z < prides[j].getLions().Count; z++) //fights against each lion of the pride
                        {
                            //checks if the nomad lion is stronger than the resident
                            if ((prides[0].getLions()[i].getFitness()[0] > prides[j].getLions()[z].getFitness()[0] && prides[0].getLions()[i].getFitness()[1] >= prides[j].getLions()[z].getFitness()[1]) || (prides[0].getLions()[i].getFitness()[0] >= prides[j].getLions()[z].getFitness()[0] && prides[0].getLions()[i].getFitness()[1] > prides[j].getLions()[z].getFitness()[1]))
                            {
                                //the nomad becomes resident and the resident becomes nomad
                                Lion aux = new Lion();
                                aux = prides[j].getLions()[z];
                                prides[j].getLions()[z] = prides[0].getLions()[i];
                                prides[0].getLions()[i] = aux;

                                won = true;
                                break;
                            }
                        }
                    }

                    if (won == true)
                        break;
                }
            }
        }

        public void populationEquilibrium(int nMalePride, int nFemalePride)
        { 
            int nNomad = (int) Math.Ceiling(Variables.popSize * Variables.nomadRatio); //number of nomads
            int nResident = Variables.popSize - nNomad; //total number of residents
            int nFemaleNomads = (int)Math.Ceiling(nNomad * (1 - Variables.femaleRatio)); //number of female nomads
            int nMaleNomads = nNomad - nFemaleNomads; //number of male nomads

            Operators o = new Operators();
            o.sorting(prides[0].getLionesses()); //sorting the nomad lionesses
            o.sorting(prides[0].getLions()); //sorting the nomad lions
            
            for (int i = 1; i < prides.Count; i++) //filling the resident females
            {
                while(prides[i].getLionesses().Count < nFemalePride)
                {
                    prides[i].getLionesses().Add(prides[0].getLionesses()[0]);
                    prides[0].getLionesses().RemoveAt(0);
                }
                string ola = "";
            }

            while(prides[0].getLionesses().Count > nFemaleNomads) // the weakest female nomads die
            {
                int toDie = prides[0].getLionesses().Count - 1;
                prides[0].getLionesses().RemoveAt(toDie);
            }
            string hello = "";

            while (prides[0].getLions().Count > nMaleNomads) // the weakest male nomads die
            {
                int toDie = prides[0].getLions().Count - 1;
                prides[0].getLions().RemoveAt(toDie);
            }
            hello = "";
        }
    }
}
