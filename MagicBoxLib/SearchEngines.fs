module Google

open FSharp.Data
open System.Net
open FSharp.Data.HttpResponseHeaders

type SearchResult =
 {
   Url:string
   LinkText:string 
   ResultPosition:int 
   Snippet:string option
  }
type SearchEngine =
   {   SearchEngineName: string
       SearchBaseUrl: string
       QueryParamName: string option
       NextPageUrl:string->string option
       OtherQueryParams: Map<string,string> option
       Parser: string ->  Result<SearchResult list, string>       
       HttpRequestData: RequestData
   }

let Google =
    {
      SearchEngineName = "Google"  
      SearchBaseUrl = "https://www.google.com/search"  
      QueryParamName = Some "q"
      NextPageUrl = fun html -> let nodes = HtmlDocument.Parse(html).CssSelect("a[href]")|>List.filter(fun x-> x.AttributeValue("href").Contains "&start=")
                                if List.length nodes >= 0 then (nodes |>List.last).AttributeValue("href")|>Some else None
      OtherQueryParams = None
      Parser = (fun html->let nodes = HtmlDocument.Parse(html).CssSelect("div.g") |> List.filter(fun n-> n.CssSelect("h3.r")|>List.isEmpty|>not)
                          if(nodes.IsEmpty) then
                            //ToDo :: Check wheather Google blocked us
                            Error "Couldn't found results..."
                          else
                            Ok(nodes|>List.mapi(fun i n-> let a = List.head (n.CssSelect("h3 > a"))
                                                          let href = a.AttributeValue("href")
                                                          let index = href.IndexOf("&sa=")
                                                          let link = href.[7..index]
                                                          let position = i+1
                                                          let linkText = a.InnerText()
                                                          let span = List.head (n.CssSelect("span.st"))
                                                          let snippet = span.InnerText()
                                                          {
                                                            Url = link
                                                            LinkText = linkText
                                                            ResultPosition = position
                                                            Snippet=snippet|>Some
                                                          }
                                                )))
      HttpRequestData = DefaultRequestData
     }

let Bing = {
      SearchEngineName = "Bing"  
      SearchBaseUrl = "http://www.bing.com/search"  
      QueryParamName = Some "q"
      NextPageUrl = fun html -> let nodes = HtmlDocument.Parse(html).CssSelect("a.sb_pagN")
                                if List.length nodes >= 0 then (nodes |>List.head).AttributeValue("href")|>Some else None
      OtherQueryParams = None
      Parser = (fun html->let nodes = HtmlDocument.Parse(html).CssSelect("li.b_algo") 
                          if(nodes.IsEmpty) then
                            //ToDo :: Check wheather Bing blocked us
                            Error "Couldn't found results..."
                          else
                            Ok(nodes|>List.mapi(fun i n-> let a = List.head (n.CssSelect("h2 > a"))
                                                          let href = a.AttributeValue("href")
                                                          let position = i+1
                                                          let linkText = a.InnerText()
                                                          let span = List.head (n.CssSelect("div > p"))
                                                          let snippet = span.InnerText()
                                                          {
                                                            Url = href
                                                            LinkText = linkText
                                                            ResultPosition = position
                                                            Snippet=snippet|>Some
                                                          }
                                                )))
      HttpRequestData = DefaultRequestData
     }
     
let Yahoo = {
      SearchEngineName = "Yahoo"  
      SearchBaseUrl = "https://search.yahoo.com/search"  
      QueryParamName = Some "p"
      NextPageUrl = fun html -> let nodes = HtmlDocument.Parse(html).CssSelect("a.next")
                                if List.length nodes >= 0 then (nodes |>List.head).AttributeValue("href")|>Some else None
      OtherQueryParams = None
      Parser = (fun html->let nodes = HtmlDocument.Parse(html).CssSelect("div.dd.algo") 
                          if(nodes.IsEmpty) then
                            //ToDo :: Check wheather Bing blocked us
                            Error "Couldn't found results..."
                          else
                            Ok(nodes|>List.mapi(fun i n-> let a = List.head (n.CssSelect("a.ac-algo"))
                                                          let href = a.AttributeValue("href")
                                                          let link=
                                                            if(href.StartsWith("http")) then href
                                                            else
                                                                let index = href.IndexOf("http")
                                                                let lastIndex = href.IndexOf("/")
                                                                href.[index..lastIndex]
                                                          let position = i+1
                                                          let linkText = a.InnerText()
                                                          let p = List.head (n.CssSelect("div.compText.aAbs p"))
                                                          let snippet = p.InnerText()
                                                          {
                                                            Url = link
                                                            LinkText = linkText
                                                            ResultPosition = position
                                                            Snippet=snippet|>Some
                                                          }
                                                )))
      HttpRequestData = DefaultRequestData
     }
