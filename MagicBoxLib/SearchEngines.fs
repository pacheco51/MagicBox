module Google

open FSharp.Data
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
                            //ToDo 
                            //Check wheather Google blocked us
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

