namespace ZSharp.Parser
open System;
open FParsec;

module Core =
    
    let identifier = identifier;
    let significantWs a = ((skipString "  ") <|> (skipString "\t")) a;
    let ws = spaces;    
    
    let ps = pstring "(" >>. pfloat .>> pstring ")"

    let testfunc () : unit =
        let f = run ps "(3.141)"
        match f with
        | Success(number, _, _) -> printfn "%A" number
        | Failure(err, _, _)    -> printfn "%s" err
