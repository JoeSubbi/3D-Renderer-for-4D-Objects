```cpp
struct Rotor {
	s: f32,
	e01: f32,
	e02: f32,
	e03: f32,
	e12: f32,
	e13: f32,
	e23: f32,
	e0123: f32,
}

impl std::ops::Mul<Rotor> for Rotor {
	type Output = Rotor;
	fn mul(self, r: Rotor) -> Rotor {
		// First I layout the elements of each rotor
		let a0 = self.s;
		let a1 = self.e01; // e01
		let a2 = self.e02; // e02
		let a3 = self.e03; // e03
		let a4 = self.e12; // e12
		let a5 = self.e13; // e13
		let a6 = self.e23; // e23
		let a7 = self.e0123; // e0123

		let b0 = r.s;
		let b1 = r.e01; // e01
		let b2 = r.e02; // e02
		let b3 = r.e03; // e03
		let b4 = r.e12; // e12
		let b5 = r.e13; // e13
		let b6 = r.e23; // e23
		let b7 = r.e0123; // e0123

		// Then I multiply each element of the 'self' with each element of 'r'
		/*

		+ a0	* b0 + a0 	 * b1e01 + a0 	 * b2e02 + a0	 * b3e03 + a0	 * b4e12 + a0	 * b5e13 + a0	 * b6e23 + a0	 * b7e0123
		+ a1e01 * b0 + a1e01 * b1e01 + a1e01 * b2e02 + a1e01 * b3e03 + a1e01 * b4e12 + a1e01 * b5e13 + a1e01 * b6e23 + a1e01 * b7e0123
		+ a2e02 * b0 + a2e02 * b1e01 + a2e02 * b2e02 + a2e02 * b3e03 + a2e02 * b4e12 + a2e02 * b5e13 + a2e02 * b6e23 + a2e02 * b7e0123
		+ a3e03 * b0 + a3e03 * b1e01 + a3e03 * b2e02 + a3e03 * b3e03 + a3e03 * b4e12 + a3e03 * b5e13 + a3e03 * b6e23 + a3e03 * b7e0123
		+ a4e12 * b0 + a4e12 * b1e01 + a4e12 * b2e02 + a4e12 * b3e03 + a4e12 * b4e12 + a4e12 * b5e13 + a4e12 * b6e23 + a4e12 * b7e0123
		+ a5e13 * b0 + a5e13 * b1e01 + a5e13 * b2e02 + a5e13 * b3e03 + a5e13 * b4e12 + a5e13 * b5e13 + a5e13 * b6e23 + a5e13 * b7e0123
		+ a6e23 * b0 + a6e23 * b1e01 + a6e23 * b2e02 + a6e23 * b3e03 + a6e23 * b4e12 + a6e23 * b5e13 + a6e23 * b6e23 + a6e23 * b7e0123
		+ a7e0123 * b0 + a7e0123 * b1e01 + a7e0123 * b2e02 + a7e0123 * b3e03 + a7e0123 * b4e12 + a7e0123 * b5e13 + a7e0123 * b6e23 + a7e0123 * b7e0123

		*/

		// Now I reduce each multiplication
		/*

		+ a0 * b0 -> (a0 * b0) 1
		+ a0 * b1e01 -> (a0 * b1) e01
		+ a0 * b2e02 -> (a0 * b2) e20
		+ a0 * b3e03 -> (a0 * b3) e03
		+ a0 * b4e12 -> (a0 * b4) e12
		+ a0 * b5e13 -> (a0 * b5) e13
		+ a0 * b6e23 -> (a0 * b6) e23
		+ a0 * b7e0123 -> (a0 * b7) e0123
		+ a1e01 * b0 -> (a1 * b0) e01
		+ a1e01 * b1e01 -> 0
		+ a1e01 * b2e02 -> 0
		+ a1e01 * b3e03 -> 0
		+ a1e01 * b4e12 -> (a1 * b4) e02
		+ a1e01 * b5e13 -> (a1 * b5) e03
		+ a1e01 * b6e23 -> (a1 * b6) e0123
		+ a1e01 * b7e0123 -> 0
		+ a2e02 * b0 -> (a2 * b0) e02
		+ a2e02 * b1e01 -> 0
		+ a2e02 * b2e02 -> 0
		+ a2e02 * b3e03 -> 0
		+ a2e02 * b4e12 -> -(a2 * b4) e01
		+ a2e02 * b5e13 -> -(a2 * b5) e0123
		+ a2e02 * b6e23 -> (a2 * b6) e03
		+ a2e02 * b7e0123 -> 0
		+ a3e03 * b0 -> (a3 * b0) e03
		+ a3e03 * b1e01 -> 0
		+ a3e03 * b2e02 -> 0
		+ a3e03 * b3e03 -> 0
		+ a3e03 * b4e12 -> (a3 * b4) e0123
		+ a3e03 * b5e13 -> -(a3 * b5) e01
		+ a3e03 * b6e23 -> -(a3 * b6) e02
		+ a3e03 * b7e0123 -> 0
		+ a4e12 * b0 -> (a4 * b0) e12
		+ a4e12 * b1e01 -> -(a4 * b1) e02
		+ a4e12 * b2e02 -> (a4 * b2) e01
		+ a4e12 * b3e03 -> (a4 * b3) e0123
		+ a4e12 * b4e12 -> -(a4 * b4)
		+ a4e12 * b5e13 -> -(a4 * b5) e23
		+ a4e12 * b6e23 -> (a4 * b6) e13
		+ a4e12 * b7e0123 -> -(a4 * b7) e03
		+ a5e13 * b0 -> (a5 * b0) e13
		+ a5e13 * b1e01 -> -(a5 * b1) e03
		+ a5e13 * b2e02 -> -(a5 * b2) e0123
		+ a5e13 * b3e03 -> (a5 * b3) e01
		+ a5e13 * b4e12 -> (a5 * b4) e23
		+ a5e13 * b5e13 -> -(a5 * b5)
		+ a5e13 * b6e23 -> -(a5 * b6) e12
		+ a5e13 * b7e0123 -> (a5 * b7) e02
		+ a6e23 * b0 -> (a6 * b0) e23
		+ a6e23 * b1e01 -> (a6 * b1) e0123
		+ a6e23 * b2e02 -> -(a6 * b2) e03
		+ a6e23 * b3e03 -> (a6 * b3) e02
		+ a6e23 * b4e12 -> -(a6 * b4) e13
		+ a6e23 * b5e13 -> (a6 * b5) e12
		+ a6e23 * b6e23 -> -(a6 * b6)
		+ a6e23 * b7e0123 -> -(a6 * b7) e01
		+ a7e0123 * b0 -> (a7 * b0) e0123
		+ a7e0123 * b1e01 -> 0
		+ a7e0123 * b2e02 -> 0
		+ a7e0123 * b3e03 -> 0
		+ a7e0123 * b4e12 -> -(a7 * b4) e03
		+ a7e0123 * b5e13 -> (a7 * b5) e02
		+ a7e0123 * b6e23 -> -(a7 * b6) e01
		+ a7e0123 * b7e0123 -> 0

		*/

		// Now I collect all the elements into their correct places
		// and implicitly return the new Rotor
		Rotor {
			s: (a0 * b0) - (a4 * b4) - (a5 * b5) - (a6 * b6),
			e01: (a0 * b1) + (a1 * b0) + (a4 * b2) + (a5 * b3) - (a2 * b4) - (a3 * b5) - (a6 * b7) - (a7 * b6),
			e02: (a0 * b2) + (a1 * b4) + (a2 * b0) + (a5 * b7) + (a6 * b3) + (a7 * b5) - (a3 * b6) - (a4 * b1),
			e03: (a0 * b3) + (a1 * b5) + (a2 * b6) + (a3 * b0) - (a4 * b7) - (a5 * b1) - (a6 * b2) - (a7 * b4),
			e12: (a0 * b4) + (a4 * b0) + (a6 * b5) - (a5 * b6),
			e13: (a0 * b5) + (a4 * b6) + (a5 * b0) - (a6 * b4),
			e23: (a0 * b6) + (a5 * b4) + (a6 * b0) - (a4 * b5),
			e0123: (a0 * b7) + (a1 * b6) + (a3 * b4) + (a4 * b3) + (a6 * b1) + (a7 * b0) - (a2 * b5) - (a5 * b2),
		}

		// Now I reduce each multiplication
		/*

		+ a0 * b0 			-> e
		+ a0 * b1e01 		-> e01
		+ a0 * b2e20 		-> e20
		+ a0 * b4e12 		-> e12
		+ a0 * b3e30 		-> e30
		+ a0 * b5e31 		-> e31
		+ a0 * b6e32 		-> e32
		+ a0 * b7e0123 		-> e0123

		+ a1e01 * b0 		-> e01
		+ a1e01 * b1e01 	-> -1
		+ a1e01 * b2e20 	-> e12
		+ a1e01 * b4e12 	-> -e20
		+ a1e01 * b3e30 	-> e31
		+ a1e01 * b5e31 	-> e30
		+ a1e01 * b6e32 	-> e0123
		+ a1e01 * b7e0123 	-> e32

		+ a2e20 * b0 		-> e20
		+ a2e20 * b1e01 	-> -e12
		+ a2e20 * b2e20 	-> -1
		+ a2e20 * b4e12 	-> e01
		+ a2e20 * b3e30 	-> -e32
		+ a2e20 * b5e31 	-> -e0123
		+ a2e20 * b6e32 	-> e30
		+ a2e20 * b7e0123 	-> e31

		+ a4e12 * b0 		-> e12
		+ a4e12 * b1e01 	-> e20
		+ a4e12 * b2e20 	-> -e01
		+ a4e12 * b4e12 	-> -1
		+ a4e12 * b3e30 	-> e0123
		+ a4e12 * b5e31 	-> -e32
		+ a4e12 * b6e32 	-> -e31
		+ a4e12 * b7e0123 	-> e30

		+ a3e30 * b0 		-> e30
		+ a3e30 * b1e01 	-> -e31
		+ a3e30 * b2e20 	-> e32
		+ a3e30 * b4e12 	-> e0123
		+ a3e30 * b3e30 	-> -1
		+ a3e30 * b5e31 	-> e01
		+ a3e30 * b6e32 	-> e20
		+ a3e30 * b7e0123 	-> -e12

		+ a5e31 * b0 		-> e31
		+ a5e31 * b1e01 	-> -e30
		+ a5e31 * b2e20 	-> -e0123
		+ a5e31 * b4e12 	-> e32
		+ a5e31 * b3e30 	-> -e01
		+ a5e31 * b5e31 	-> -1
		+ a5e31 * b6e32 	-> e12
		+ a5e31 * b7e0123 	-> e20

		+ a6e32 * b0 		-> e32
		+ a6e32 * b1e01 	-> e0123
		+ a6e32 * b2e20 	-> -e30
		+ a6e32 * b4e12 	-> e31
		+ a6e32 * b3e30 	-> -e20
		+ a6e32 * b5e31 	-> -e12
		+ a6e32 * b6e32 	-> -1
		+ a6e32 * b7e0123 	-> -e01

		+ a7e0123 * b0 		-> e0123
		+ a7e0123 * b1e01 	-> e32
		+ a7e0123 * b2e20 	-> e31
		+ a7e0123 * b4e12 	-> e30
		+ a7e0123 * b3e30 	-> -e12
		+ a7e0123 * b5e31 	-> e20
		+ a7e0123 * b6e32 	-> -e01
		+ a7e0123 * b7e0123 -> 1

		*/

		// Collect elements
		e     = a0 * b0      - a1e01 * b1e01    - a2e20 * b2e20   - a4e12 * b4e12   - a3e30 * b3e30   - a5e31 * b5e31   - a6e32 * b6e32	  + a7e0123 * b7e0123
		e01   = a0 * b1e01   + a1e01 * b0       + a2e20 * b4e12   - a4e12 * b2e20   + a3e30 * b5e31   - a5e31 * b3e30   - a6e32 * b7e0123 - a7e0123 * b6e32
		e20   = a0 * b2e20   - a1e01 * b4e12    + a2e20 * b0      + a4e12 * b1e01   + a3e30 * b6e32   + a5e31 * b7e0123	- a6e32 * b3e30	  + a7e0123 * b5e31
		e12   = a0 * b4e12   + a1e01 * b2e20    - a2e20 * b1e01   + a4e12 * b0      - a3e30 * b7e0123 + a5e31 * b6e32   - a6e32 * b5e31	  - a7e0123 * b3e30
		e30   = a0 * b3e30   + a1e01 * b5e31    + a2e20 * b6e32   + a4e12 * b7e0123 + a3e30 * b0      - a5e31 * b1e01   - a6e32 * b2e20	  + a7e0123 * b4e12
		e31   = a0 * b5e31   + a1e01 * b3e30    + a2e20 * b7e0123 - a4e12 * b6e32   - a3e30 * b1e01   + a5e31 * b0      + a6e32 * b4e12	  + a7e0123 * b2e20
		e32   = a0 * b6e32   + a1e01 * b7e0123  - a2e20 * b3e30   - a4e12 * b5e31   + a3e30 * b2e20   + a5e31 * b4e12   + a6e32 * b0      + a7e0123 * b1e01
		e0123 = a0 * b7e0123 + a1e01 * b6e32    - a2e20 * b5e31   + a4e12 * b3e30   + a3e30 * b4e12   - a5e31 * b2e20   + a6e32 * b1e01   + a7e0123 * b0
	}
}
```

```cs
// Rotate a Vector with a Rotor
public Vector4 Rotate(Vector4 u)
{
	Rotor4 p = this;

	/* Rotor3 version

	q = Px
	a.e1 * b.e   -> e1
	a.e1 * b.e12 -> e2
	a.e1 * b.e31 -> -e3
	a.e1 * b.e23 -> e123
	
	a.e2 * b.e   -> e2
	a.e2 * b.e12 -> -e1
	a.e2 * b.e31 -> e123
	a.e2 * b.e23 -> e3
	
	a.e3 * b.e   -> e3
	a.e3 * b.e12 -> e123
	a.e3 * b.e31 -> e1
	a.e3 * b.e23 -> -e2

	Therefore
	e1 =    a.e1 * b.e   - a.e2 * b.e12 + a.e3 * b.e31
	e2 =    a.e1 * b.e12 + a.e2 * b.e   - a.e3 * b.e23
	e3 =   -a.e1 * b.e31 + a.e2 * b.e23 + a.e3 * b.e 
	e123 =  a.e1 * b.e23 + a.e2 * b.e31 + a.e3 * b.e12
	
	Ordered ==
	e1 =   a.e1 * b.e - a.e2 * b.e12 + a.e3 * b.e31
	e2 =   a.e2 * b.e + a.e1 * b.e12 - a.e3 * b.e23
	e3 =   a.e3 * b.e - a.e1 * b.e31 + a.e2 * b.e23
	WHICH IS CORRECT!

	STAGE 2...
	r = qP* 
	a.e   * be1   -> e1
	a.e   * be2   -> e2
	a.e   * be3   -> e3
	a.e	  * be123 -> e123

	a.e12 * be1   -> -e2
	a.e12 * be2   -> e1
	a.e12 * be3   -> e123
	a.e12 * be123 -> -e3

	a.e31 * be1   -> e3
	a.e31 * be2   -> e123
	a.e31 * be3   -> -e1
	a.e31 * be123 -> -e2

	a.e23 * be1   -> e123
	a.e23 * be2   -> -e3
	a.e23 * be3   -> e2
	a.e23 * be123 -> -e1

	e1 = a.e * be1 + a.e12 * be2   - a.e31 * be3   - a.e23 * be123
	e2 = a.e * be2 - a.e12 * be1   - a.e31 * be123 + a.e23 * be3
	e3 = a.e * be3 - a.e12 * be123 + a.e31 * be1   - a.e23 * be2

	Ordered ==
	e1 = a.e * be1 + be2   * a.e12 - be3   * a.e31 - be123 * a.e23
	e2 = a.e * be2 - be1   * a.e12 - be123 * a.e31 + be3   * a.e23
	e3 = a.e * be3 - be123 * a.e12 + be1   * a.e31 - be2   * a.e23
	WHICH IS CORRECT!

	YESSSSSSSSSSSSSSSSSSSS!!!!!!!!!!!!!!!!!!!

	*/

	/* Rotor4 Version

	q = Px
	  a.e1 * b.e     -> e1
	+ a.e1 * b.e12   -> e2
	+ a.e1 * b.e31   -> -e3
	+ a.e1 * b.e23   -> e123
	+ a.e1 * b.e41   -> e4
	+ a.e1 * b.e42   -> e142
	+ a.e1 * b.e43   -> -e134
	+ a.e1 * b.e1234 -> e324

	q.y
	  a.e2 * b.e     -> e2
	+ a.e2 * b.e12   -> -e1
	+ a.e2 * b.e31   -> e123
	+ a.e2 * b.e23   -> e3
	+ a.e2 * b.e41   -> e142
	+ a.e2 * b.e42   -> -e4
	+ a.e2 * b.e43   -> e324
	+ a.e2 * b.e1234 -> e134

	q.z
	  a.e3 * b.e     -> e3
	+ a.e3 * b.e12   -> e123
	+ a.e3 * b.e31   -> e1
	+ a.e3 * b.e23   -> -e2
	+ a.e3 * b.e41   -> e134
	+ a.e3 * b.e42   -> e324
	+ a.e3 * b.e43   -> e4
	+ a.e3 * b.e1234 -> -e142

	q.w
	  a.e4 * b.e     -> e4
	+ a.e4 * b.e12   -> -e142
	+ a.e4 * b.e31   -> e134
	+ a.e4 * b.e23   -> e324
	+ a.e4 * b.e41   -> -e4
	+ a.e4 * b.e42   -> e2
	+ a.e4 * b.e43   -> -e3
	+ a.e4 * b.e1234 -> -e123

	Therefore
	e1 =  a.e1 * b.e   - a.e2 * b.e12 + a.e3 * b.e31 
	e2 =  a.e1 * b.e12 + a.e2 * b.e   - a.e3 * b.e23 + a.e4 * b.e42
	e3 = -a.e1 * b.e31 + a.e2 * b.e23 + a.e3 * b.e   - a.e4 * b.e43
	e4 =  a.e1 * b.e41 - a.e2 * b.e42 + a.e3 * b.e43 + a.e4 * b.e   - a.e4 * b.e41

	e123 =  a.e1 * b.e23   + a.e2 * b.e31   + a.e3 * b.e12   - a.e4 * b.e1234
	e142 =  a.e1 * b.e42   + a.e2 * b.e41   - a.e3 * b.e1234 - a.e4 * b.e12
	e134 = -a.e1 * b.e43   + a.e2 * b.e1234 + a.e3 * b.e41   + a.e4 * b.e31
	e324 =  a.e1 * b.e1234 + a.e2 * b.e43   + a.e3 * b.e42   + a.e4 * b.e23 
	
	Ordered ==
	e1 = a.e1 * b.e - a.e2 * b.e12 + a.e3 * b.e31 
	e2 = a.e2 * b.e + a.e1 * b.e12 - a.e3 * b.e23 + a.e4 * b.e42
	e3 = a.e3 * b.e - a.e1 * b.e31 + a.e2 * b.e23 - a.e4 * b.e43
	e4 = a.e4 * b.e + a.e1 * b.e41 - a.e4 * b.e41 + a.e3 * b.e43
	FINGERS CROSSED

	r = qP*
	a.e   * b.e1   -> e1
	a.e   * b.e2   -> e2
	a.e   * b.e3   -> e3
	a.e   * b.e4   -> e4
	a.e   * b.e123 -> e123
	a.e   * b.e134 -> e134
	a.e   * b.e142 -> e142
	a.e   * b.e324 -> e324

	a.e12 * b.e1   -> -e2
	a.e12 * b.e2   -> e1
	a.e12 * b.e3   -> e123
	a.e12 * b.e4   -> -e142
	a.e12 * b.e123 -> e3
	a.e12 * b.e134 -> e324
	a.e12 * b.e142 -> -e4
	a.e12 * b.e324 -> -e134

	a.e31 * b.e1   -> e3
	a.e31 * b.e2   -> e123
	a.e31 * b.e3   -> -e1
	a.e31 * b.e4   -> e134
	a.e31 * b.e123 -> e2
	a.e31 * b.e134 -> -e4
	a.e31 * b.e142 -> -e324
	a.e31 * b.e324 -> -e142

	a.e23 * b.e1   -> e123
	a.e23 * b.e2   -> -e3
	a.e23 * b.e3   -> e2
	a.e23 * b.e4   -> e324
	a.e23 * b.e123 -> e1
	a.e23 * b.e134 -> e142
	a.e23 * b.e142 -> e134
	a.e23 * b.e324 -> -e4

	a.e41 * b.e1   -> -e4
	a.e41 * b.e2   -> e142
	a.e41 * b.e3   -> e134
	a.e41 * b.e4   -> e1
	a.e41 * b.e123 -> e324 
	a.e41 * b.e134 -> e2
	a.e41 * b.e142 -> -e3
	a.e41 * b.e324 -> e123

	a.e42 * b.e1   -> e142
	a.e42 * b.e2   -> e4
	a.e42 * b.e3   -> e324
	a.e42 * b.e4   -> -e2
	a.e42 * b.e123 -> -e134
	a.e42 * b.e134 -> e1
	a.e42 * b.e142 -> -e123
	a.e42 * b.e324 -> -e3

	a.e43 * b.e1   -> -e134
	a.e43 * b.e2   -> e324
	a.e43 * b.e3   -> -e4
	a.e43 * b.e4   -> e3
	a.e43 * b.e123 -> -e142
	a.e43 * b.e134 -> e123
	a.e43 * b.e142 -> e1
	a.e43 * b.e324 -> -e2

	a.e1234 * b.e1   -> -e324
	a.e1234 * b.e2   -> -e134
	a.e1234 * b.e3   -> e142
	a.e1234 * b.e4   -> e123
	a.e1234 * b.e123 -> -e4
	a.e1234 * b.e134 -> -e3
	a.e1234 * b.e142 -> -e2
	a.e1234 * b.e324 -> -e1

	Therefore
	e1 = a.e * b.e1 + a.e12 * b.e2   - a.e31 * b.e3   + a.e23 * b.e123 + a.e41 * b.e4   + a.e42 * b.e134 + a.e43 * b.e142 - a.e1234 * b.e324
	e2 = a.e * b.e2 - a.e12 * b.e1   + a.e31 * b.e123 + a.e23 * b.e3   + a.e41 * b.e134 - a.e42 * b.e4   - a.e43 * b.e324 - a.e1234 * b.e142
	e3 = a.e * b.e3 + a.e12 * b.e123 + a.e31 * b.e1   - a.e23 * b.e2   - a.e41 * b.e142 - a.e42 * b.e324 + a.e43 * b.e4   - a.e1234 * b.e134
	e4 = a.e * b.e4 - a.e12 * b.e142 - a.e31 * b.e134 - a.e23 * b.e324 - a.e41 * b.e1   + a.e42 * b.e2   - a.e43 * b.e3   - a.e1234 * b.e123

	*/


	float e1 = a.e1 * b.e - a.e2 * b.e12 + a.e3 * b.e31;
	float e2 = a.e2 * b.e + a.e1 * b.e12 - a.e3 * b.e23 + a.e4 * b.e42;
	float e3 = a.e3 * b.e - a.e1 * b.e31 + a.e2 * b.e23 - a.e4 * b.e43;
	float e4 = a.e4 * b.e + a.e1 * b.e41 - a.e4 * b.e41 + a.e3 * b.e43;

	float e123 =  a.e1 * b.e23   + a.e2 * b.e31   + a.e3 * b.e12   - a.e4 * b.e1234;
	float e142 =  a.e1 * b.e42   + a.e2 * b.e41   - a.e3 * b.e1234 - a.e4 * b.e12;
	float e134 = -a.e1 * b.e43   + a.e2 * b.e1234 + a.e3 * b.e41   + a.e4 * b.e31;
	float e324 =  a.e1 * b.e1234 + a.e2 * b.e43   + a.e3 * b.e42   + a.e4 * b.e23;

	// q = Px
	Vector4 q;
	q.x = e1;
	q.y = e2;
	q.z = e3;
	q.w = e4;

	e1 = a.e * b.e1 + a.e12 * b.e2   - a.e31 * b.e3   + a.e23 * b.e123 + a.e41 * b.e4   + a.e42 * b.e134 + a.e43 * b.e142 - a.e1234 * b.e324
	e2 = a.e * b.e2 - a.e12 * b.e1   + a.e31 * b.e123 + a.e23 * b.e3   + a.e41 * b.e134 - a.e42 * b.e4   - a.e43 * b.e324 - a.e1234 * b.e142
	e3 = a.e * b.e3 + a.e12 * b.e123 + a.e31 * b.e1   - a.e23 * b.e2   - a.e41 * b.e142 - a.e42 * b.e324 + a.e43 * b.e4   - a.e1234 * b.e134
	e4 = a.e * b.e4 - a.e12 * b.e142 - a.e31 * b.e134 - a.e23 * b.e324 - a.e41 * b.e1   + a.e42 * b.e2   - a.e43 * b.e3   - a.e1234 * b.e123

	// r = qP*
	Vector4 r;
	r.x = e1;
	r.y = e2;
	r.z = e3;
	r.w = e4;

	return r;
}
```