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
            | Success(_, u, p) -> failure.Invoke ((String.Format ("Did not consume entire stream. Remaining: '{0}'", input.Substring ((int)p.Index))), u);
            | Failure(err, _, u) -> failure.Invoke (err, u)

module Whitespace =
    let surroundedby outer inner = between outer outer inner;

    let skip_surroundedby outer inner a = ((surroundedby outer inner) >> ignore) a;

    let significantWs a = ((skipString "  ") <|> (skipString "\t")) a;
    let ws = spaces;
    let ws_not_newline a = (skipManySatisfy (function ' '|'\t' -> true | _ -> false)) a;
    let skip_str_ws str = parse { do! ws
                                  do! skipString str
                                  do! ws
                                  return () };
    let skip_newline_ws a = (parse { do! ws_not_newline
                                     do! CharParsers.skipUnicodeNewline
                                     do! ws_not_newline
                                     return () }) a;

module Identifier =
    let private identifierStart c = isAsciiLetter c || c = '_'
    let private identifierInner c = isAsciiLetter c || isDigit c || c = '_'
    let private identifierOptions = IdentifierOptions(isAsciiIdStart = identifierStart, isAsciiIdContinue = identifierInner)
    let parser a = (identifier identifierOptions) a
        