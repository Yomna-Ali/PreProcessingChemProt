using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreProcessingChemProt
{
   public class LabelledAbstractTokens
    {
        public string PMID { get; set; } //the abstract unique identifier

        //we can turn it into list of sentence tokens List<string[]>absttractTokens
        //so that means each list element is a sentence, that sentcence has an array of tokens text constructing it.

        public List<string[]> AbstractTokens_perSentence = new List<string[]>();

        public List<string[]> AbstractLabels_perSentence = new List<string[]>();

        public string[] AbstractTokens { get; set; } //The abstract text content is tokenized here, also need to add sentence separator?

        //same here, labelled at sentence level then list<string[]>tokens
        public string[]TokensLabels { get; set; }

        public LabelledAbstractTokens(string pmid, string[] tokens, string[] labels)
        {
            this.PMID = pmid;
            this.AbstractTokens = tokens;
            this.TokensLabels = labels;
        }

        public LabelledAbstractTokens(string pmid, List<string[]> tokens, List<string[]> labels)
        {
            this.PMID = pmid;
            this.AbstractTokens_perSentence = tokens;
            this.AbstractLabels_perSentence = labels;
        }
    }
}
