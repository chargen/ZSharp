namespace ZSharp.Parser
open System;
open FParsec;

module TestHelper =
    let test<'Result, 'UserState>
        (input:string)
        (state:'UserState)
        (parser:Func<CharStream<'UserState>, Reply<'Result>>)
        (success:Action<'UserState, 'Result>)
        (failure:Action<string, 'UserState>)
        : unit =
            let p a = parser.Invoke a
            let result = runParserOnString p state "" input
            match result with
            | Success(r, u, p) when p.Index = (int64 input.Length) -> success.Invoke (u, r);
            | Success(_, u, _) -> failure.Invoke ("Did not consume entire stream", u);
            | Failure(err, _, u) -> failure.Invoke (err, u)

module Whitespace =
    let significantWs a = ((skipString "  ") <|> (skipString "\t")) a;
    let ws = spaces;    

module Identifier =
    let private identifierStart c = isAsciiLetter c || c = '_'
    let private identifierInner c = isAsciiLetter c || isDigit c || c = '_'
    let private identifierOptions = IdentifierOptions(isAsciiIdStart = identifierStart, isAsciiIdContinue = identifierInner)
    let identifier a = (identifier identifierOptions) a
        