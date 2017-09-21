[<AutoOpen>]
module  HtmlProcessor
open System

type Ip = string
type Port = int 
type UserName = string
type Password = string

type SearchProxy =
    |PrivateProxy of Ip * Port * UserName * Password
    |PublicProxy of Ip * Port

let GetRandomHeaders =
    let headers = [|
        "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.86 Safari/537.36";
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_1) AppleWebKit/601.2.7 (KHTML, like Gecko) Version/9.0.1 Safari/601.2.7";
        "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.86 Safari/537.36";
        "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36";
        "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
        "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36";
        |]
    let rnd = new Random()
    let len = headers.Length
    fun()-> headers.[rnd.Next(len)]

let Fetch url (proxy:SearchProxy) =
     

