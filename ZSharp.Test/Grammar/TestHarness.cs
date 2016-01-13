using System;
using System.Collections.Generic;
using FParsec;
using Microsoft.FSharp.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZSharp.Parser;

namespace ZSharp.Test.Grammar
{
    static class TestHarness
    {
        public static KeyValuePair<UserState, Result> TestPositive<UserState, Result>(UserState initialState, Func<CharStream<UserState>, Reply<Result>> parser, string input)
        {
            var state = default(UserState);
            var result = default(Result);

            TestHelper.test<Result, UserState>(input, initialState, parser,
                success: (s, r) => {
                    state = s;
                    result = r;
                },
                failure: (err, _) => Assert.Fail(err)
            );

            return new KeyValuePair<UserState, Result>(state, result);
        }

        public static Result TestPositive<Result>(Func<CharStream<Unit>, Reply<Result>> parser, string input)
        {
            return TestPositive<Unit, Result>(null, parser, input).Value;
        }

        public static KeyValuePair<UserState, string> TestNegative<UserState, Result>(UserState initialState, Func<CharStream<UserState>, Reply<Result>> parser, string input)
        {
            var state = default(UserState);
            var result = default(string);

            TestHelper.test<Result, UserState>(input, initialState, parser,
                success: (_, r) => Assert.Fail("Unexpected success parsing '{0}' into '{1}'", input, r),
                failure: (e, s) => {
                    state = s;
                    result = e;
                });

            return new KeyValuePair<UserState, string>(state, result);
        }

        public static string TestNegative<Result>(Func<CharStream<Unit>, Reply<Result>> parser, string input)
        {
            return TestNegative<Unit, Result>(null, parser, input).Value;
        }
    }
}
