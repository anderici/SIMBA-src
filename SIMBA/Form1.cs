using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIMBA
{
    public partial class Form1 : Form
    {
        public static Form1 F = null;

        public Form1()
        {
            F = this;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

            
        }

        private void tbActions_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            switch (tbActions.Buttons.IndexOf(e.Button))
            {
                case 0:
                    newAlignment();
                    break;
                case 1:
                    openFile();
                    break;
                case 2:
                    LionOptimizationAlgorithm loa = new LionOptimizationAlgorithm();
                    loa.run(F);
                    break;
            }
        }

        public void newAlignment()
        {
            txtInput.Text = "";
            txtOutput.Text = "";
        }

        public void openFile()
        {
            openFileDialog1.Title = "Open Fasta File";
            openFileDialog1.InitialDirectory = Application.StartupPath + "\\Input";
            openFileDialog1.ShowDialog();

            
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            StreamReader SR = new StreamReader(openFileDialog1.OpenFile());
            string Data = SR.ReadToEnd();
            SR.Close();
            string Title = "";
            string Path = openFileDialog1.FileName;
            Title = Path.Remove(0, Path.LastIndexOf("\\") + 1);
            Variables.fileName = Title;
            Title = Title.Remove(Title.LastIndexOf("."), Title.Length - Title.LastIndexOf("."));
            openFileDialog1.Dispose();
            F.Text = "SIMBA: " + Title;
            txtInput.Text = Data;
            Variables.filePath = Path;
            
            string name = "";
            int n_seq = 0;
            List<string> seqs = new List<string>();
            //string alignment = File.ReadAllText("BB12006.fasta");
            string alignment = Data;

            //read the sequences to calculate the weights
            for (int i = 0; i < alignment.Length; i++) //here we store the seq name
            {
                if (String.Compare(alignment[i].ToString(), ">") == 0)
                {
                    for (int j = i + 1; j < alignment.Length; j++)
                    {
                        if (String.Compare(alignment[j].ToString(), "\n") != 0)
                        {
                            name += alignment[j];
                        }
                        else
                        {
                            seqs.Add("");
                            i = j + 1;
                            break;
                        }
                    }
                    Variables.seqNames.Add(name);
                    name = "";
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

            // Calculate the weights between the sequences
            // Based on LevenshteinDistance
            StreamWriter write = new StreamWriter("weights.txt");

            for (int i = 0; i < n_seq - 1; i++)
            {
                for (int j = i + 1; j < n_seq; j++)
                {
                    double w = 1 - (LevenshteinDistance.Calculate(seqs[i], seqs[j]) / (double)Math.Max(seqs[i].Length, seqs[j].Length));
                    write.WriteLine(w);
                }
            }

            write.Close();
            write.Dispose();

            
        }
    }
}
