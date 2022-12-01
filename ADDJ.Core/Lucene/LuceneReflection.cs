using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Documents;
using System.Reflection;

namespace ADDJ.Core.AILucene
{
    public class LuceneReflection
    {
        public static List<Field> GetLuceneFields<T>(T obj, bool isSearch)
        {
            List<Field> fields = new List<Field>();
            Field field = null;
            // get all properties of the object type
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                //If property is not null add it as field and it is not a search
                if (obj.GetType().GetProperty(propertyInfo.Name).GetValue(obj, null) != null && !isSearch)
                {
                    field = GetLuceneFieldsForInsertUpdate(obj, propertyInfo, false);
                }
                else
                {
                    field = GetLuceneFieldsForInsertUpdate(obj, propertyInfo, true);

                }
                fields.Add(field);
            }

            return fields;
        }

        private static Field GetLuceneFieldsForInsertUpdate<T>(T obj, PropertyInfo propertyInfo, bool isSearch)
        {
            Field field = null;

            object[] dbFieldAtts = propertyInfo.GetCustomAttributes(typeof(LuceneAttribute), isSearch);

            if (dbFieldAtts.Length > 0 && propertyInfo.PropertyType == typeof(System.String))
            {
                var luceneWrapAttribute = ((LuceneAttribute)dbFieldAtts[0]);
                field = GetLuceneField(obj, luceneWrapAttribute, propertyInfo, isSearch);
            }
            else if (propertyInfo.PropertyType != typeof(System.String))
            {
                throw new InvalidCastException(string.Format("{0} must be a string in order to get indexed", propertyInfo.Name));
            }

            return field;
        }


        private static Field GetLuceneField<T>(T obj, LuceneAttribute luceneWrapAttribute, PropertyInfo propertyInfo, bool isSearch)
        {
            Field.Store store = luceneWrapAttribute.IsStored ? Field.Store.YES : Field.Store.NO;
            Field.Index index = luceneWrapAttribute.IsSearchable ? Field.Index.TOKENIZED : Field.Index.UN_TOKENIZED;
            //if it is not a search assign the object value to the field
            string propertyValue = isSearch ? string.Empty : obj.GetType().GetProperty(propertyInfo.Name).GetValue(obj, null).ToString();
            Field field = new Field(propertyInfo.Name, propertyValue, store, index);
            return field;
        }

        public static T GetObjFromDocument<T>(Document document) where T : new()
        {
            T obj = new T();
            var fields = GetLuceneFields(obj, true);
            foreach (var field in fields)
            {
                //setting values to properties of the object via reflection
                obj.GetType().GetProperty(field.Name()).SetValue(obj, document.Get(field.Name()), null);
            }

            return (T)obj;
        }
    }
}
