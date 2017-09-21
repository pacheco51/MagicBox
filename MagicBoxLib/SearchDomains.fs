[<AutoOpen>]
module SearchDomains

type SearchBaseUrl = string
type Keyword = string
type QueryParam = string
type Query = QueryParam * Keyword
type NextPageParamName = string 
type NextPageValue = int -> int 
type PageNum = int 
type AllOtherQueryParams = Map<string,string>
type CssSelectors = ResultCss of string | NextPageCss of string

type [<Measure>] DelayInMs

type HtmlSearchEngine =
  SearchBaseUrl * 
  Query * 
  NextPageParamName *
  NextPageValue *
  PageNum *
  AllOtherQueryParams * 
  CssSelectors *
  int<DelayInMs> *
  SearchProxy option