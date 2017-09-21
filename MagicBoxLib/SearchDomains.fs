[<AutoOpen>]
module SearchDomains

type SearchResult =
    {
      SearchEngineName :string
      SearchKeyword:string
      Url:string
      ResultPostion:int
      HtmlSnippet:string
     }

type SearchEngine =
   {   SearchEngineName: string
       SearchBaseUrl: string
       Query: string * string
       NextPageParamName: string
       NextPageValue: int -> int  
       PageNum: int
       AllOtherQueryParams: Map<string,string> 
       Results: string -> SearchResult list
   }
      