# straight implementation of qsort with no total functional considerations
fun qsort : [T] -> [T]
match []        => []
	  [x]       => [x]
	  [x, ..xs] => qsort(lessthan(x), xs)) ++ [x] ++ qsort(greaterthan(x), xs))

# Implementation of qsort with total functional property (recurse towards 0)
fun qsort2 : (N, [T]) -> [T]
	where N : Integer(positive, zero)
		  T : Comparable
	match (0, x)               => x
		  (_, [])              => []
		  (_, [x])             => [x]
		  (c, [x, ..xs])       => qsort2(c - 1, filter((a => a < x), xs)) ++ [x] ++ qsort2(c - 1, filter((a => a > x), xs))

# Simple identity function
fun id : A -> A
	match (a) => a

# Very sketchy demo of type constraints for operators on types
fun mul : (A, B) -> C
	where A : Op(* : (A, B) -> C)
	match (a, b) => a * b

# Sketchy demo of searching for a trait implemented on a type (trait identified by name). A trait supplies a list of functions on the type callable with dot operator
fun blargh : (A, B) -> C
	where A : Trait(TraitName)
	match (a, b) => a.dostuff(b)

# Monoidal chaining?
# do : name of monoid
# let : modify the monoid
# ; : Separates operations on the monoid
# bash(a, b) : implicitly accessing properties bound in the monoid, since it's the final expression
fun monoidal : () -> ()
	do { let a = foo(); let b = bar(a); bash(a, b) }