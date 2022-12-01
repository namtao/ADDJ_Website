using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Documents;

namespace ADDJ.Core.AILucene
{
    public class AIFieldAttribute : Attribute
    {

        public AIFieldAttribute()
        {
            this.FieldStoreLucene = Field.Store.YES;
            this.FieldIndexLucene = Field.Index.ANALYZED;
            this.IsKoDau = false;
        }

        public AIFieldAttribute(string fieldName)
        {
            this.FieldName = fieldName;

            this.FieldStoreLucene = Field.Store.YES;
            this.FieldIndexLucene = Field.Index.ANALYZED;
            this.IsKoDau = false;
        }

        public AIFieldAttribute(string fieldName, bool _isKoDau)
        {
            this.FieldName = fieldName;

            this.FieldStoreLucene = Field.Store.YES;
            this.FieldIndexLucene = Field.Index.ANALYZED;

            this.IsKoDau = _isKoDau;
        }

        public AIFieldAttribute(string _fieldName, FieldStore _fieldStore)
        {
            this.FieldName = _fieldName;
            this.FieldIndexLucene = Field.Index.ANALYZED;
            this.IsKoDau = false;

            if (_fieldStore == FieldStore.Yes)
                this.FieldStoreLucene = Field.Store.YES;
            else
                this.FieldStoreLucene = Field.Store.NO;
        }

        public AIFieldAttribute(string _fieldName, FieldIndex _fieldIndex)
        {
            this.FieldName = _fieldName;
            this.FieldStoreLucene = Field.Store.YES;
            this.IsKoDau = false;

            if (_fieldIndex == FieldIndex.ANALYZED)
                this.FieldIndexLucene = Field.Index.ANALYZED;
            else
                this.FieldIndexLucene = Field.Index.NOT_ANALYZED;
        }

        public AIFieldAttribute(string _fieldName, FieldStore _fieldStore, FieldIndex _fieldIndex)
        {
            this.FieldName = _fieldName;
            this.IsKoDau = false;

            if (_fieldStore == FieldStore.Yes)
                this.FieldStoreLucene = Field.Store.YES;
            else
                this.FieldStoreLucene = Field.Store.NO;

            if (_fieldIndex == FieldIndex.ANALYZED)
                this.FieldIndexLucene = Field.Index.ANALYZED;
            else
                this.FieldIndexLucene = Field.Index.NOT_ANALYZED;
        }

        public Field.Store FieldStoreLucene { get; set; }

        public Field.Index FieldIndexLucene { get; set; }

        public bool IsKoDau { get; set; }

        // Properties
        public float Boost { get; set; }

        public string FieldName { get; set; }

    }

    public enum FieldStore
    {
        Yes = 1,
        No = 2,
        Compare = 3,
    }

    public enum FieldIndex
    {
        ANALYZED = 1,
        NOT_ANALYZED = 2,
    }
}
