﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//This namespace is useful for processing the ATOM feed
using System.Xml.Linq;
//This namespace contains the XMLUrlResolver class used below
using System.Xml;
//This namespace contains stream classes
using System.IO;
//This namespace contains the CredentialCache class
using System.Net;

namespace REST_QueryExcelDataRange
{
    class Program
    {
        //Store namespaces to ease later code
        const string atomNameSpace = "http://www.w3.org/2005/Atom";
        const string xlsvcNameSpace = "http://schemas.microsoft.com/office/2008/07/excelservices/rest";

        //Alter these values to match your site, document library, spreadsheet, and data range name
        const string sharepointSite = "http://intranet.contoso.com";
        const string docLibrary = "shared%20documents";
        const string spreadsheetFileName = "cyclepartsales.xlsx";
        const string rangeName = "ProductAndSalesData";

        static void Main(string[] args)
        {
            //This example gets all the cells in the specified data range and lists them
            //Note: Check that the Excel Services service application is running before you run this example

            //Get a reference to the atom namespace
            XNamespace atomNS = atomNameSpace;
            //build the relative Url to the excel workbook model
            string relativeUrl = "/_vti_bin/ExcelRest.aspx/" + docLibrary + "/" + spreadsheetFileName + "/model/Ranges('" + rangeName + "')?$format=atom";
            //Use an XMLUrlResolver object to pass credentials to the REST service
            XmlUrlResolver resolver = new XmlUrlResolver();
            resolver.Credentials = CredentialCache.DefaultCredentials;
            //Build the URI to pass the the resolver
            Uri baseUri = new Uri(sharepointSite);
            Uri fullUri = resolver.ResolveUri(baseUri, relativeUrl);
            //Tell the user what the stream address is
            Console.WriteLine("Opening this stream: " + fullUri.ToString());
            //Call the resolver and receive the ATOM feed as a result
            Stream atomStream = (Stream)resolver.GetEntity(fullUri, null, typeof(Stream));
            //Load the stream into an XDocument
            XDocument atomResults = XDocument.Load(atomStream);
            //Query the XDocument for all title elements that are children of an entry element
            IEnumerable<XElement> cells =
                from c in atomResults.Elements()
                select c;
            Console.WriteLine("Cells: ");
            //Display each of the rows
            foreach (XElement cell in cells)
            {
                Console.WriteLine((string)cell);
            }
            //This line prevents the console disappearing before you can read the result
            //Alternatively, remove this line a run the project without debugging (CTRL-F5)
            Console.ReadKey();
        }
    }
}
