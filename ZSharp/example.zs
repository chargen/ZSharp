# identity function
let id : T -> T = fun a => a;

# basic qsort (no totality)
let qsort : [T] -> [T] = fun
	[]		  => []
	[x]		  => [x]
	[x, ..xz] => qsort(lessthan(x, xs)) ++ x ++ qsort(greaterthan(x, xs));

# qsort with totality (recursion converges towards zero)
# Also a very vague sketch of typeclasses (N and T are some undefined types, we define typeclasses for them immediately below)
let qsort2 : (N, [T]) -> [T]
	where N : Integer(positive, zero)
	where T : Comparable
= fun
	(0, x)		=> x
	(_, [])		=> []
	(_, [x])	=> [x]
	(c, [x, ..xs]) => qsort2(c - 1, lessthan(x, xs)) ++ x ++ qsort2(c - 1, greaterthan(x, xs));

# Monoidal chaining?
# do : name of monoid
# let : modify the monoid
# ; : Separates operations on the monoid
# bash(a, b) : implicitly accessing values bound in the monoid, since it's the final expression
let monoidal : () -> () = fun
	do { let a = foo(); let b = bar(); bash(a, b) };