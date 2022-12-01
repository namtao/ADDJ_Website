using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ADDJ.Entity;
using ADDJ.Core;
using ADDJ.Core.Provider;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.Standard;
using System.IO;

namespace ADDJ.Impl
{
	/// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của DauSoCP
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>14/10/2013</date>
	
	public class DauSoCPImpl : BaseImpl<DauSoCPInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "DauSoCP";            
            IndexLocation = Path.Combine(Config.PathIndexLucene, TableName) + @"\";
            MaxFieldLength = 10;
            IsUseLucene = true;
            IsUpdateLucene = true;
        }
        public List<DauSoCPInfo> Suggestion(string query)
        {
            string[] fields = new string[] { "ID", "DauSo" };

            var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, fields, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
            parser.SetAllowLeadingWildcard(true);
            parser.SetDefaultOperator(QueryParser.Operator.OR);

            query = query + "*";

            var q = parser.Parse(query);

            return this.Search(q, null, null, 10, false);
        }
		#region Function 
		
		#endregion
    }
}
