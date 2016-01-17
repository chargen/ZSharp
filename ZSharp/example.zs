fun qsort : [T] -> [T]
    []        => []
    [x]       => [x]
    [x, ..xs] => qsort(lessthan(x), xs)) ++ [x] ++ qsort(greaterthan(x), xs))

fun qsort2 : (N, [T]) -> [T]
	where N : Integer(positive, zero)
		| T : Comparable
	match (0, x)               => x
		| (_, [])              => []
		| (_, [x])             => [x]
		| (c, [x, ..xs])       => qsortRecursive(c - 1, filter((a => a < x), xs)) ++ [x] ++ qsortRecursive(c - 1, filter((a => a > x), xs))

fun id : A -> A
	match (a) => a

fun mul : (A, B) -> C
	where A : Op(*, (A, B) -> C)
	match (a, b) => a * b

fun add : (A, B) -> C
	where A : Addable(B -> C)
	match (a, b) => a + b

	where B : Addable(A -> C)
	match (a, b) => b + a