using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreProcessingChemProt
{
    //Read the chemprot_sample_entities.tsv file
    // Parse the data  and change into BIO form
    //- for each row:
    //    Tokenize enttity text
    //    if onr token set as B-entity, otherwise B-entity for first token and I-entities for the rest
    // 
    public static class ChemProtEntityReader
    {
        static List<String> EntitiesRows = new List<string>();

        static Dictionary<string, List<EntityObject>> tokenizedEntities = new Dictionary<string, List<EntityObject>>();

        public static Dictionary<string, List<EntityObject>> ReadEntitiesFile()
        {
            String st = File.ReadAllText(@"D:\yomna\ChemProt_Corpus\ChemProt_Corpus\chemprot_test_gs\chemprot_test_entities_gs.tsv");
            var separated = st.Split('\n');

            var x = ParseEachRecordInEntities(separated);
            Console.WriteLine("no of records= " + separated.Length);
            return x;
        }

        public static Dictionary<string, List<EntityObject>> ParseEachRecordInEntities(String[] records)
        {
            foreach (var record in records)
            {
                var parsedRecords = record.Split('\t');
                //split each text on spaces only?
                var splitEntityText = parsedRecords[5].Split(' ');
                List<EntityObject> splitEntitiesInBIOForm = new List<EntityObject>();
                if (splitEntityText.Length >1)
                {
                    int startOffset = Int32.Parse(parsedRecords[3]);
                    int endOffset = startOffset + splitEntityText[0].Length - 1; // endoffset is the end index of this tokenized part
                    //Add start of this entity and use B- prefix
                    splitEntitiesInBIOForm.Add(new EntityObject(parsedRecords[1], "B-" + parsedRecords[2], startOffset,endOffset, splitEntityText[0]));

                    //add rest of entity with prefix I-
                    for (int i = 1; i < splitEntityText.Length; i++)
                    {
                        //new token start and end indices: start is end of last word + 2 (remeber we split on space) but this is wrong because what if there was > 2 spaces?
                        startOffset = endOffset + 2;
                        endOffset = startOffset + splitEntityText[i].Length - 1;
                        splitEntitiesInBIOForm.Add(new EntityObject(parsedRecords[1], "I-" + parsedRecords[2], startOffset,
                       endOffset, splitEntityText[i]));
                    }
                }
                else
                {
                    splitEntitiesInBIOForm.Add(new EntityObject(parsedRecords[1], "B-" + parsedRecords[2], Int32.Parse(parsedRecords[3]),
                    Int32.Parse(parsedRecords[4])-1, parsedRecords[5]));
                }

               // var newEntityObject = new EntityObject(parsedRecords[1], parsedRecords[2], Int32.Parse(parsedRecords[3]),
                    //Int32.Parse(parsedRecords[4]), parsedRecords[5]);
                if (tokenizedEntities.ContainsKey(parsedRecords[0]))
                {
                    tokenizedEntities[parsedRecords[0]].AddRange(splitEntitiesInBIOForm);
                }
                else
                {
                    List<EntityObject> newList = new List<EntityObject>();
                    newList.AddRange(splitEntitiesInBIOForm);
                    tokenizedEntities.Add(parsedRecords[0], newList);
                }

                //Console.WriteLine(parsedRecords.Length);
            }

            return tokenizedEntities;
         }
    }
} 