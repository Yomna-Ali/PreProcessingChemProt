using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using java.io;
using java.util;
using edu.stanford.nlp.ling;
using Cloudmersive.APIClient.NET.NLP.Api;
using Cloudmersive.APIClient.NET.NLP.Client;
using Cloudmersive.APIClient.NET.NLP.Model;
using edu.stanford.nlp.tagger.maxent;
using edu.stanford.nlp.pipeline;

namespace PreProcessingChemProt
{
    public static class ChemProtAbstractsReader
    {
        public static List<LabelledAbstractTokens> ReadAbstractsFile(Dictionary<string, List<EntityObject>> tokenizedEntities)
        {
            String st = System.IO.File.ReadAllText(@"D:\yomna\ChemProt_Corpus\ChemProt_Corpus\chemprot_test_gs\chemprot_test_abstracts_gs.tsv");
            
            var abstracts_ = st.Split('\n');

            // ParseEachRecordInEntities(separated);
            System.Console.WriteLine("no of records= " + abstracts_.Length);
            return ParseEachAbstract(abstracts_, tokenizedEntities);
        }

        public static List<LabelledAbstractTokens> ParseEachAbstract(String[] abstractsList, Dictionary<string, List<EntityObject>> entities_to_be_mapped)
        {
            string[] assignedLabelsForAbtstractTokens;
            List<LabelledAbstractTokens> labelledAbstracts = new List<LabelledAbstractTokens>();
            List<LabelledAbstractTokens> labelledAbstractsSentenced = new List<LabelledAbstractTokens>();
            //var jarRoot = @"../../../data/paket-files/nlp.stanford.edu/stanford-tagger-4.2.0";
            //var modelsDirectory = jarRoot + @"/models";
            //var model = @"D:\yomna\ChemProt_Corpus\TaggerModels\stanford-postagger-full\models" + @"/english-bidirectional-distsim.tagger";
            // Part-of-speech tag a string
            //var tagger = new MaxentTagger(model);
            List<string[]> parsedAbstracts = new List<string[]>();
            foreach (var paperAbstractRow in abstractsList)
            {
                List<string[]> words_perSentence = new List<string[]>();
                List<string[]> labels_perSentence = new List<string[]>();
                var parsedRecords = paperAbstractRow.Split('\t');

                var thisAbstractId = parsedRecords[0];
                var entitiesInAbstract = entities_to_be_mapped[parsedRecords[0]];

                /// test test
                /// 
                string[] sentences = Regex.Split(parsedRecords[2], @"(?<=[\.])\s+");
                int currentSentenceLength;
                int current_startOffSet_sentence = parsedRecords[1].Length + 1;

                foreach (var sentence in sentences)
                {
                    //try POS Tagger
                    //try
                    //{
                    //    var sentencess = MaxentTagger.tokenizeText(new java.io.StringReader(sentence)).toArray();
                    //    foreach (ArrayList sentencee in sentencess)
                    //    {
                    //        var taggedSentence = tagger.tagSentence(sentencee,true);
                    //        System.Console.WriteLine(SentenceUtils.listToString(taggedSentence, false));
                    //    }
                    //    //Console.WriteLine(result);
                    //}
                    //catch (Exception e)
                    //{
                    //    System.Console.Write("Exception when calling PosTaggerStringApi.PosTaggerStringPost: " + e.Message);
                    //}


                    ///
                    currentSentenceLength = sentence.Length;

                    //select entities belonging to this specific sentence in the abstarct by using the startOffset-endOffset values
                    try
                    {
                        var associatedEntitiesToThatSentence = entitiesInAbstract.Where(ent => ent.endCharOffset >= current_startOffSet_sentence
                        && ent.startCharOffset <= currentSentenceLength - 1 + current_startOffSet_sentence).ToList();
                        //now we have a sentence string and entites associated with it, time to map them and split sentence to words
                        var sentenceTokens = sentence.Split(' ');
                        var sentenceLabels = new string[sentenceTokens.Length];
                        // initialize all labels as O
                        for (int i = 0; i < sentenceTokens.Length; i++)
                        {
                            sentenceLabels[i] = "O";
                        }

                        // start filling the sentence labels with the entites
                        associatedEntitiesToThatSentence.ForEach(ent =>
                        {
                            for (int i = 0; i < sentenceTokens.Length; i++)
                            {
                                if (i == sentenceTokens.Length - 1 && sentenceTokens.Length>0)
                                {
                                    try
                                    {
                                        if (sentenceTokens[i][sentenceTokens[i].Length - 1] == '.')
                                            sentenceTokens[i] = sentenceTokens[i].Substring(0, sentenceTokens[i].Length - 1);
                                    }
                                    catch(Exception ee)
                                    {
                                        System.Console.WriteLine(ee.Message);
                                    }
                                }

                                if (sentenceTokens[i].Contains(ent.textOfEntity))
                                {
                                    sentenceLabels[i] = ent.entityType;
                                }
                            }
                        });
                        var count = sentenceLabels.Where(s => s != "O").Count();

                        current_startOffSet_sentence = current_startOffSet_sentence + currentSentenceLength + 1;
                        words_perSentence.Add(sentenceTokens);
                        labels_perSentence.Add(sentenceLabels);
                    }
                    catch(Exception ex)
                    {
                        System.Console.WriteLine(ex.Message);
                    }
                }

                labelledAbstractsSentenced.Add(new(parsedRecords[0], words_perSentence, labels_perSentence));


                //var tokensAbstract = parsedRecords[2].Split(' ');
                //parsedAbstracts.Add(parsedRecords);
                //assignedLabelsForAbtstractTokens = new string[tokensAbstract.Length];
                //for (int i = 0; i < tokensAbstract.Length; i++)
                //{
                //    assignedLabelsForAbtstractTokens[i] = "O";
                //}

                //entitiesInAbstract.ForEach(ent => {
                //    for (int i = 0; i < tokensAbstract.Length; i++)
                //    {
                //        if (tokensAbstract[i] == ent.textOfEntity)
                //        {
                //            assignedLabelsForAbtstractTokens[i] = ent.entityType;
                //        }
                //    }
                //});
                //labelledAbstracts.Add(new(parsedRecords[0], tokensAbstract, assignedLabelsForAbtstractTokens));
            }

            return labelledAbstractsSentenced;
        }
    }
}  
