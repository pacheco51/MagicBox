[<AutoOpen>]
module SearchDomains

type SearchResult =
    {
      SearchEngineName :string
      SearchKeyword:string
      Url:string
      LinkText:string 
      ResultPostion:int option
      HtmlSnippet:string option
     }

type IntOrString =
    |PageString of string
    |PageInt of int

type SearchEngine =
   {   SearchEngineName: string
       SearchBaseUrl: string
       QueryParamName: string 
       NextPageParamName: string
       NextPageValue: IntOrString -> IntOrString option
       PagesToScrape: int
       AllOtherQueryParams: (string*string) list option
       Parser: string -> SearchResult list option
   }
