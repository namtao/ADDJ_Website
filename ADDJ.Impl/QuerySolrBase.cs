using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolrNet;
using Microsoft.Practices.ServiceLocation;
using SolrNet.Commands.Parameters;
using SolrNet.Impl;
using ADDJ.Entity;
using SolrNet.DSL;
using ADDJ.Core;

namespace ADDJ.Impl
{
    public class QuerySolrBase<T> where T : new()
    {
        private static void LoadSolr(string url)
        {
            Startup.Container.Clear();
            Startup.InitContainer();
            Startup.Init<T>(url);
        }

        public static SolrQueryResults<T> QuerySolr(string urlLoad, AbstractSolrQuery query, QueryOptions queryOptions)
        {
            LoadSolr(urlLoad);

            ISolrOperations<T> solr = ServiceLocator.Current.GetInstance<ISolrOperations<T>>();
            SolrQueryResults<T> results = solr.Query(query, queryOptions);
            return results;
        }

        ///// <summary>
        ///// Author : Phi Hoang Hai
        ///// Created date : 19/04/2015
        ///// Todo : Thực hiện truy vấn Solr theo phương thức Post
        ///// </summary>
        ///// <param name="urlLoad"></param>
        ///// <param name="query"></param>
        ///// <param name="queryOptions"></param>
        ///// <returns></returns>
        //public static SolrQueryResults<T> QuerySolrPostMethod(string urlLoad, AbstractSolrQuery query, QueryOptions queryOptions)
        //{
        //    try
        //    {                
        //        Startup.Init<T>(new PostSolrConnection(new SolrConnection(urlLoad), urlLoad));
        //        var solr = Startup.Container.GetInstance<ISolrOperations<T>>();
        //        var results = solr.Query(query, queryOptions);

        //        return results;
        //    }
        //    catch(Exception ex)
        //    {
        //        Utility.LogEvent("QuerySolrPostMethod : " + ex.Message);
        //        throw ex;
        //    }            
        //}

        public static int UpdateSolr(string urlLoad, T item)
        {
            SolrConnection conn = new SolrConnection(urlLoad);
            //Startup.Init<T>(urlLoad);
            SolrNet.Commands.CommitCommand Commit = new SolrNet.Commands.CommitCommand();
            Commit.WaitFlush = null;
            Commit.WaitSearcher = true;
            ISolrOperations<T> solrWorker = ServiceLocator.Current.GetInstance<ISolrOperations<T>>();

            solrWorker.Add(item);
            string result = Commit.Execute(conn);
            return 1;
        }
    }
}
