using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolrNet;
using Microsoft.Practices.ServiceLocation;
using SolrNet.Commands.Parameters;

namespace GQKN.Archive.Impl
{
    public class QuerySolrBase<T> where T : new()
    {
        private static void LoadSolr(string url)
        {           
            Startup.InitContainer();

            Startup.Init<T>(url);
        }

        public static SolrQueryResults<T> QuerySolr(string urlLoad, AbstractSolrQuery query, QueryOptions queryOptions)
        {
            LoadSolr(urlLoad);

            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<T>>();
                        
            var results = solr.Query(query, queryOptions);

            return results;
        }
    }
}
