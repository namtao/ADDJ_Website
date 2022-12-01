using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Documents;

namespace ADDJ.Core.AILucene
{
    public class AIFieldUnikeyAttribute : AIFieldAttribute 
    {
        // Methods
        public AIFieldUnikeyAttribute()
        {
            base.FieldStoreLucene = Field.Store.YES;
            base.FieldIndexLucene = Field.Index.ANALYZED;
            base.IsKoDau = false;
        }

        public AIFieldUnikeyAttribute(string fieldName)
            : base(fieldName)
        {
            base.FieldStoreLucene = Field.Store.YES;
            base.FieldIndexLucene = Field.Index.NOT_ANALYZED;
            base.IsKoDau = false;
        }

    }
}
