[<AutoOpen>]
module SearchDomains

type CssSelectors = 
    |ResultCss of string 
    |NextPageCss of string

type SearchEngine =
  {   SearchEngineName: string
      SearchBaseUrl : string
      Query : string * string
      NextPageParamName : string
      NextPageValue:int -> int  
      PageNum : int
      AllOtherQueryParams: Map<string,string> 
      CssSelectors : CssSelectors
  }