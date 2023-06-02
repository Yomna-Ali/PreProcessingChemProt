using System;
using System.IO;

namespace PreProcessingChemProt
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            try
            {
                var entitesToPAssToAbstracts = ChemProtEntityReader.ReadEntitiesFile();
                // need tp modify the matching of entities with abstract tokens. maybe use the start offsets to help
                var anotatedAbstracts = ChemProtAbstractsReader.ReadAbstractsFile(entitesToPAssToAbstracts);
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(@"C:\Users\hosam\Desktop\", "chemprot_Test.txt")))
                {
                    int count = 0;
                    foreach (var annotatedAbstrct in anotatedAbstracts)
                    {
                        for (int sentenceNo = 0; sentenceNo < annotatedAbstrct.AbstractTokens_perSentence.Count; sentenceNo++)
                        {
                            for (int tokenNo = 0; tokenNo < annotatedAbstrct.AbstractTokens_perSentence[sentenceNo].Length; tokenNo++)
                            {
                                if (count==93)
                                { }
                                outputFile.WriteLine(annotatedAbstrct.AbstractTokens_perSentence[sentenceNo][tokenNo] + "\t" + annotatedAbstrct.AbstractLabels_perSentence[sentenceNo][tokenNo]);
                                count++;
                            }

                            outputFile.WriteLine();
                        }
                    }
                    // String st = File.ReadAllText(@"D:\yomna\ChemProt_Corpus\ChemProt_Corpus\chemprot_sample\chemprot_sample_entities.tsv");
                    // Console.WriteLine(st);
                }



                //using (StreamWriter outputFile = new StreamWriter(Path.Combine(@"C:\Users\hosam\Desktop\", "SampleDataYomna.txt")))
                //{
                //    foreach (var abstrct in TokensAndLabels)
                //    {
                //        for (int i = 0; i < abstrct.AbstractTokens.Length; i++)
                //        {
                //            outputFile.WriteLine(abstrct.AbstractTokens[i] + " " + abstrct.TokensLabels[i]);
                //        }
                //    }
                //    // String st = File.ReadAllText(@"D:\yomna\ChemProt_Corpus\ChemProt_Corpus\chemprot_sample\chemprot_sample_entities.tsv");
                //    // Console.WriteLine(st);
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine("The bl bla");
                Console.WriteLine(e.Message);
            }
        }
    }
}
