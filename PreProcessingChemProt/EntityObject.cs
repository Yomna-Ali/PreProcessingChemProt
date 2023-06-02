using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreProcessingChemProt
{
    public class EntityObject
    {
        String entityTermNumber { get; set; }
       
        public String entityType { get; set; } //B- or I-
        
        public int startCharOffset { get; set; }
        
        public int endCharOffset { get; set; }

        public String textOfEntity { get; set; }

        public EntityObject(String entityTermNumber, String entityType, int start, int end, String tex)
        {
            this.entityTermNumber = entityTermNumber;
            this.entityType = entityType;
            this.startCharOffset = start;
            this.endCharOffset = end;
            this.textOfEntity = tex;
        }
    }
}
