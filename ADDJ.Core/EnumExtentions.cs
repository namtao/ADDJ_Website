using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;

namespace ADDJ.Core
{
    #region Extended Attributes
    public class Name : Attribute
    {
        private string _value;

        public Name(string value)
        {
            _value = value;
        }
        public string Value => _value;
    }
    public class ThuTu : Attribute
    {
        private int _value;

        public ThuTu(int value)
        {
            _value = value;
        }

        public int Value => _value;
    }
    public class KeySolr4Query : Attribute
    {
        private string _value;

        public KeySolr4Query(string value)
        {
            _value = value;
        }
        public string Value => _value;
    }

    public class KeySolrBefore4Query : Attribute
    {
        private string _value;
        public KeySolrBefore4Query(string value)
        {
            _value = value;
        }
        public string Value => _value;
    }
    #endregion
    public static class EnumExtentions
    {
        public static string Name(this Enum enumValue)
        {
            Type type = enumValue.GetType();

            FieldInfo fi = type.GetField(enumValue.ToString());
            Name[] attrs = fi.GetCustomAttributes(typeof(Name), false) as Name[];

            if (attrs != null && attrs.Length > 0) return attrs[0].Value;
            return null;
        }
        public static int ThuTu(this Enum enumValue)
        {
            Type type = enumValue.GetType();
            FieldInfo fi = type.GetField(enumValue.ToString());
            ThuTu[] attrs = fi.GetCustomAttributes(typeof(ThuTu), false) as ThuTu[];

            if (attrs != null && attrs.Length > 0) return attrs[0].Value;
            return 0;
        }
        public static string KeySolr4Query(this Enum enumValue)
        {
            Type type = enumValue.GetType();

            FieldInfo fi = type.GetField(enumValue.ToString());
            KeySolr4Query[] attrs = fi.GetCustomAttributes(typeof(KeySolr4Query), false) as KeySolr4Query[];

            if (attrs != null && attrs.Length > 0) return attrs[0].Value;
            return null;
        }
        public static string KeySolrBefore4Query(this Enum enumValue)
        {
            Type type = enumValue.GetType();

            FieldInfo fi = type.GetField(enumValue.ToString());
            KeySolrBefore4Query[] attrs = fi.GetCustomAttributes(typeof(KeySolrBefore4Query), false) as KeySolrBefore4Query[];

            if (attrs != null && attrs.Length > 0) return attrs[0].Value;
            return null;
        }
    }
}