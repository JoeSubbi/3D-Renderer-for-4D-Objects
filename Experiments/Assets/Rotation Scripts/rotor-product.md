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

		/*
		ATTEMPT 1

		a.e * b.e     -> e
		a.e * b.e12   -> e12
		a.e * b.e31   -> e31
		a.e * b.e23   -> e23
		a.e * b.e41   -> e41
		a.e * b.e42   -> e42
		a.e * b.e43   -> e43
		a.e * b.e1234 -> e1234

		a.e12 * b.e     -> e12
		a.e12 * b.e12   -> -1 (-e)
		a.e12 * b.e31   -> e23
		a.e12 * b.e23   -> -e31
		a.e12 * b.e41   -> e42
		a.e12 * b.e42   -> e41
		a.e12 * b.e43   -> e1234
		a.e12 * b.e1234 -> e43

		a.e31 * b.e     -> e31
		a.e31 * b.e12   -> -e23
		a.e31 * b.e31   -> -1 (-e)
		a.e31 * b.e23   -> e12
		a.e31 * b.e41   -> -e43
		a.e31 * b.e42   -> -e1234
		a.e31 * b.e43   -> e41
		a.e31 * b.e1234 -> e42

		a.e23 * b.e     -> e23
		a.e23 * b.e12   -> e31
		a.e23 * b.e31   -> -e12
		a.e23 * b.e23   -> -1 (-e)
		a.e23 * b.e41   -> e1234
		a.e23 * b.e42   -> -e43
		a.e23 * b.e43   -> -e42
		a.e23 * b.e1234 -> e41

		a.e41 * b.e     -> e41
		a.e41 * b.e12   -> -e42
		a.e41 * b.e31   -> e43
		a.e41 * b.e23   -> e1234
		a.e41 * b.e41   -> -1 (-e)
		a.e41 * b.e42   -> e12
		a.e41 * b.e43   -> e31
		a.e41 * b.e1234 -> -e23

		a.e42 * b.e     -> e42
		a.e42 * b.e12   -> -e41
		a.e42 * b.e31   -> -e1234
		a.e42 * b.e23   -> e43
		a.e42 * b.e41   -> -e12
		a.e42 * b.e42   -> -1 (-e)
		a.e42 * b.e43   -> e23
		a.e42 * b.e1234 -> e31

		a.e43 * b.e     -> e43
		a.e43 * b.e12   -> e1234
		a.e43 * b.e31   -> -e41
		a.e43 * b.e23   -> e42
		a.e43 * b.e41   -> -e31
		a.e43 * b.e42   -> -e23
		a.e43 * b.e43   -> -1 (-e)
		a.e43 * b.e1234 -> -e12

		a.e1234 * b.e     -> e1234
		a.e1234 * b.e12   -> e43
		a.e1234 * b.e31   -> e42
		a.e1234 * b.e23   -> e41
		a.e1234 * b.e41   -> -e23
		a.e1234 * b.e42   -> e31
		a.e1234 * b.e43   -> -e12
		a.e1234 * b.e1234 -> 1 (e)
		*/

		float e     = a.e * b.e     - a.e12 * b.e12   - a.e31 * b.e31   - a.e23 * b.e23   - a.e41 * b.e41   - a.e42 * b.e42   - a.e43 * b.e43   + a.e1234 * b.e1234;
		float e12   = a.e * b.e12   + a.e12 * b.e     + a.e31 * b.e23   - a.e23 * b.e31   + a.e41 * b.e42   - a.e42 * b.e41   - a.e43 * b.e1234 - a.e1234 * b.e43; 
		float e31   = a.e * b.e31   - a.e12 * b.e23   + a.e31 * b.e     + a.e23 * b.e12   + a.e41 * b.e43   + a.e42 * b.e1234 - a.e43 * b.e41   + a.e1234 * b.e42;
		float e23   = a.e * b.e23   + a.e12 * b.e31   - a.e31 * b.e12   + a.e23 * b.e     - a.e41 * b.e1234 + a.e42 * b.e43   - a.e43 * b.e42   - a.e1234 * b.e41;
		float e41   = a.e * b.e41   + a.e12 * b.e42   + a.e31 * b.e43   + a.e23 * b.e1234 + a.e41 * b.e     - a.e42 * b.e12   - a.e43 * b.e31   + a.e1234 * b.e23;
		float e42   = a.e * b.e42   + a.e12 * b.e41   + a.e31 * b.e1234 - a.e23 * b.e43   - a.e41 * b.e12   + a.e42 * b.e     + a.e43 * b.e23   + a.e1234 * b.e31;
		float e43   = a.e * b.e43   + a.e12 * b.e1234 - a.e31 * b.e41   - a.e23 * b.e42   + a.e41 * b.e31   + a.e42 * b.e23   + a.e43 * b.e     + a.e1234 * b.e12;
		float e1234 = a.e * b.e1234 + a.e12 * b.e43   - a.e31 * b.e42   + a.e23 * b.e41   + a.e41 * b.e23   - a.e42 * b.e31   + a.e43 * b.e12   + a.e1234 * b.e;

		/*
		Rotor3 version
		
		rules: 
			e31 -> -e31

		ae * be   -> e
		ae * be12 -> e12
		ae * be31 -> e31
		ae * be23 -> e23
		
		ae12 * be   -> e12
		ae12 * be12 -> -e
		ae12 * be31 -> e23
		ae12 * be23 -> -e31
		
		ae31 * be   -> e31
		ae31 * be12 -> -e23
		ae31 * be31 -> -1
		ae31 * be23 -> e12
		
		ae23 * be   -> e23
		ae23 * be12 -> e31
		ae23 * be31 -> -e12
		ae23 * be23 -> -e

		Ordered
		e   = ae * be   - ae12 * be12 - ae31 * be31 - ae23 * be23
		e12 = ae12 * be + ae   * be12 - ae23 * be31 + ae31 * be23 
		e31 = ae31 * be + ae   * be31 - ae12 * be23 + ae23 * be12
		e23 = ae23 * be + ae   * be23 - ae31 * be12 + ae12 * be31 

		Signed
		e   = ae * be   - ae12 * be12 - ae31 * be31 - ae23 * be23
		e12 = ae12 * be + ae   * be12 + ae23 * be31 - ae31 * be23 
		e31 = ae31 * be + ae   * be31 - ae12 * be23 + ae23 * be12
		e23 = ae23 * be + ae   * be23 + ae31 * be12 - ae12 * be31 

		31 -> -31

		anything multiplied by ea keeps the same sign
		*/


		/*
		ATTEMPT 2
		
		rules: 
			e31 -> -e31
			e41 -> -e41
			e42 -> -e42
			e43 -> -e43
			
		ae * be     -> e
		ae * be12   -> e12
		ae * be31   -> e31		(-)
		ae * be23   -> e23
		ae * be41   -> e41		(-)
		ae * be42   -> e42		(-)
		ae * be43   -> e43		(-)
		ae * be1234 -> e1234

		ae12 * be     -> e12
		ae12 * be12   -> -e
		ae12 * be31   -> e23	(-)
		ae12 * be23   -> -e31
		ae12 * be41   -> e42	(-)
		ae12 * be42   -> e41	(-)
		ae12 * be43   -> e1234	(-)
		ae12 * be1234 -> e43

		ae31 * be     -> e31
		ae31 * be12   -> -e23	(-)
		ae31 * be31   -> -e		
		ae31 * be23   -> e12	(-)
		ae31 * be41   -> -e43	
		ae31 * be42   -> -e1234	
		ae31 * be43   -> e41	
		ae31 * be1234 -> e42	(-)

		ae23 * be     -> e23
		ae23 * be12   -> e31
		ae23 * be31   -> -e12	(-)
		ae23 * be23   -> -e
		ae23 * be41   -> e1234	(-)
		ae23 * be42   -> -e43	(-)
		ae23 * be43   -> -e42	(-)
		ae23 * be1234 -> e41

		ae41 * be     -> e41
		ae41 * be12   -> -e42	(-)
		ae41 * be31   -> e43
		ae41 * be23   -> e1234	(-)
		ae41 * be41   -> -e
		ae41 * be42   -> e12
		ae41 * be43   -> e31
		ae41 * be1234 -> -e23	(-)

		ae42 * be     -> e42
		ae42 * be12   -> -e41	(-)
		ae42 * be31   -> -e1234
		ae42 * be23   -> e43	(-)
		ae42 * be41   -> -e12
		ae42 * be42   -> -e
		ae42 * be43   -> e23
		ae42 * be1234 -> e31	(-)

		ae43 * be     -> e43
		ae43 * be12   -> e1234	(-)
		ae43 * be31   -> -e41
		ae43 * be23   -> e42	(-)
		ae43 * be41   -> -e31
		ae43 * be42   -> -e23
		ae43 * be43   -> -e
		ae43 * be1234 -> -e12	(-)

		ae1234 * be     -> e1234
		ae1234 * be12   -> e43
		ae1234 * be31   -> e42	(-)
		ae1234 * be23   -> e41
		ae1234 * be41   -> -e23	(-)
		ae1234 * be42   -> e31	(-)
		ae1234 * be43   -> -e12	(-)
		ae1234 * be1234 -> e
		*/
		
		//Unsigned
		float e     = ae * be	    - ae12 * be12   - ae31 * be31   - ae23 * be23   - ae41 * be41   - ae42 * be42   - ae43 * be43   + ae1234 * be1234;
		float e12   = ae * be12   + ae12 * be     + ae31 * be23   - ae23 * be31   + ae41 * be42   - ae42 * be41   - ae43 * be1234 - ae1234 * be43;
		float e31   = ae * be31   - ae12 * be23   + ae31 * be     + ae23 * be12   + ae41 * be43   + ae42 * be1234 - ae43 * be41   + ae1234 * be42;
		float e23   = ae * be23   + ae12 * be31   - ae31 * be12   + ae23 * be     - ae41 * be1234 + ae42 * be43   - ae43 * be42   - ae1234 * be41;
		float e41   = ae * be41   + ae12 * be42   + ae31 * be43   + ae23 * be1234 + ae41 * be     - ae42 * be12   - ae43 * be31   + ae1234 * be23;
		float e42   = ae * be42   + ae12 * be41   + ae31 * be1234 - ae23 * be43   - ae41 * be12   + ae42 * be     + ae43 * be23   + ae1234 * be31;
		float e43   = ae * be43   + ae12 * be1234 - ae31 * be41   - ae23 * be42   + ae41 * be31   + ae42 * be23   + ae43 * be     + ae1234 * be12;
		float e1234 = ae * be1234 + ae12 * be43   - ae31 * be42   + ae23 * be41   + ae41 * be23   - ae42 * be31   + ae43 * be12   + ae1234 * be;

		//Signed
		float e     =   ae * be     - ae12 * be12   - ae31 * be31   - ae23 * be23   - ae41 * be41   - ae42 * be42   - ae43 * be43   + ae1234 * be1234;
		float e12   =   ae * be12   + ae12 * be     - ae31 * be23   + ae23 * be31   + ae41 * be42   - ae42 * be41   + ae43 * be1234 + ae1234 * be43;
		float e31   = - ae * be31   - ae12 * be23   + ae31 * be     + ae23 * be12   + ae41 * be43   - ae42 * be1234 - ae43 * be41   - ae1234 * be42;
		float e23   =   ae * be23   - ae12 * be31   + ae31 * be12   + ae23 * be     - ae41 * be1234 + ae42 * be43   - ae43 * be42   + ae1234 * be41;
		float e41   = - ae * be41   - ae12 * be41   + ae31 * be43   + ae23 * be1234 + ae41 * be     + ae42 * be12   - ae43 * be31   + ae1234 * be23;
		float e42   = - ae * be42   - ae12 * be42   - ae31 * be1234 + ae23 * be43   + ae41 * be12   + ae42 * be     - ae43 * be23   - ae1234 * be31;
		float e43   = - ae * be43   - ae12 * be43   - ae31 * be41   + ae23 * be42   + ae41 * be31   - ae42 * be23   + ae43 * be     + ae1234 * be12;
		float e1234 =   ae * be1234 + ae12 * be1234 - ae31 * be42   - ae23 * be41   - ae41 * be23   - ae42 * be31   - ae43 * be12   + ae1234 * be;
	}
}
```

```cs
// Rotate a Vector with a Rotor
public Vector4 Rotate(Vector4 u)
{
	Rotor4 p = this;

	/*
	Rotor3 version ATTEMPT 2

	q = Px

	ae * be1 -> e1
	ae * be2 -> e2
	ae * be3 -> e3
	
	ae12 * be1 -> -e2
	ae12 * be2 -> e1
	ae12 * be3 -> e123

	ae31 * be1 -> e3	(-)
	ae31 * be2 -> e123	(-)
	ae31 * be3 -> -e1	(-)

	ae23 * be1 -> e123
	ae23 * be2 -> -e3
	ae23 * be3 -> e2

	e1 = ae * be1 + ae12 * be2 - ae31 * be3
	e2 = ae * be2 - ae12 * be1 + ae23 * be3
	e3 = ae * be3 + ae31 * be1 - ae23 * be2

	e123 = ae12 * be3 + ae31 * be2 + ae23 * be1

	SIGNED
	e1 = ae * be1 + ae12 * be2 + ae31 * be3
	e2 = ae * be2 - ae12 * be1 + ae23 * be3
	e3 = ae * be3 - ae31 * be1 - ae23 * be2

	e123 = ae23 * be1 - ae31 * be2 + ae12 * be3

	ae31 -> -ae31

	r = qP*

	ae1 * be   -> e1
	ae1 * be12 -> e2		(-)
	ae1 * be31 -> -e3
	ae1 * be23 -> e123		(-)

	ae2 * be   -> e2
	ae2 * be12 -> -e1		(-)
	ae2 * be31 -> e123
	ae2 * be23 -> e3		(-)
	
	ae3 * be   -> e3
	ae3 * be12 -> e123		(-)
	ae3 * be31 -> e1
	ae3 * be23 -> -e2		(-)
	
	ae123 * be   -> e123
	ae123 * be12 -> -e3		(-)
	ae123 * be31 -> -e2
	ae123 * be23 -> -e1		(-)

	e1 =  ae1 * be   - ae2 * be12 + ae3 * be31 - ae123 * be23
	e2 =  ae1 * be12 + ae2 * be   - ae3 * be23 - ae123 * be31
	e3 = -ae1 * be31 + ae2 * be23 + ae3 * be   - ae123 * be12

	SIGNED
	be12  -> -be12
	be23  -> -be23
	
	e1 =  ae1 * be   + ae2 * be12 + ae3 * be31 + ae123 * be23
	e2 = -ae1 * be12 + ae2 * be   + ae3 * be23 - ae123 * be31
	e3 = -ae1 * be31 - ae2 * be23 + ae3 * be   + ae123 * be12

	ORDERED
	e1 = ae1 * be + ae2   * be12 + ae3   * be31 + ae123 * be23
	e2 = ae2 * be - ae1   * be12 - ae123 * be31 + ae3   * be23 
	e3 = ae3 * be + ae123 * be12 - ae1   * be31 - ae2   * be23

	100% CORRECT

	*/

	/*
	Rotor4 Version

	q = Px

	rules: 
		e31 -> -e31
		e41 -> -e41
		e42 -> -e42
		e43 -> -e43

	ae * be1 -> e1
	ae * be2 -> e2
	ae * be3 -> e3
	ae * be4 -> e4
	
	ae12 * be1 -> -e2
	ae12 * be2 -> e1
	ae12 * be3 -> e123
	ae12 * be4 -> -e142
	
	ae31 * be1 -> e3	(-)
	ae31 * be2 -> e123	(-)
	ae31 * be3 -> -e1	(-)
	ae31 * be4 -> e134	(-)
	
	ae23 * be1 -> e123
	ae23 * be2 -> -e3
	ae23 * be3 -> e2
	ae23 * be4 -> e324
	
	ae41 * be1 -> -e4	(-)
	ae41 * be2 -> e142	(-)
	ae41 * be3 -> e134	(-)
	ae41 * be4 -> e1	(-)
	
	ae42 * be1 -> e142	(-)
	ae42 * be2 -> e4	(-)
	ae42 * be3 -> e324	(-)
	ae42 * be4 -> -e2	(-)
	
	ae43 * be1 -> -e134	(-)
	ae43 * be2 -> e324	(-)
	ae43 * be3 -> -e4	(-)
	ae43 * be4 -> e3	(-)
	
	ae1234 * be1 -> -e324
	ae1234 * be2 -> -e134
	ae1234 * be3 -> e142
	ae1234 * be4 -> e123

	e1 = ae * be1 + ae12 * be2 - ae31 * be3 + ae41 * be4
	e2 = ae * be2 - ae12 * be1 + ae23 * be3 - ae42 * be4
	e3 = ae * be3 + ae31 * be1 - ae23 * be2 + ae43 * be4 
	e4 = ae * be4 - ae41 * be1 + ae42 * be2 - ae43 * be3

	Maybe can go without the last column
	e123 =   ae12 * be3 + ae31 * be2 + ae23 * be1 + ae1234 * be4
	e142 = - ae12 * be4 + ae41 * be2 + ae42 * be1 + ae1234 * be3
	e134 =   ae31 * be4 + ae41 * be3 - ae43 * be1 - ae1234 * be2
	e324 =   ae23 * be4 + ae42 * be3 + ae43 * be2 - ae1234 * be1

	SIGNED
	e1 = ae * be1 + ae12 * be2 + ae31 * be3 - ae41 * be4
	e2 = ae * be2 - ae12 * be1 + ae23 * be3 + ae42 * be4
	e3 = ae * be3 - ae31 * be1 - ae23 * be2 - ae43 * be4
	e4 = ae * be4 + ae41 * be1 - ae42 * be2 + ae43 * be3

	Maybe can go without the last column
	e123 =   ae12 * be3 - ae31 * be2 + ae23 * be1 + ae1234 * be4
	e142 = - ae12 * be4 - ae41 * be2 - ae42 * be1 + ae1234 * be3
	e134 = - ae31 * be4 - ae41 * be3 + ae43 * be1 - ae1234 * be2
	e324 =   ae23 * be4 - ae42 * be3 - ae43 * be2 - ae1234 * be1

	r = qP*

	rules: 
		e12 -> -e12
		e23 -> -e23
		e1234 -> -e1234?

	ae1 * be     -> e1
	ae1 * be12   -> e2		(-)
	ae1 * be31   -> -e3
	ae1 * be23   -> e123	(-)
	ae1 * be41   -> e4
	ae1 * be42   -> e142
	ae1 * be43   -> -e134
	ae1 * be1234 -> e324	(-)?
	
	ae2 * be     -> e2
	ae2 * be12   -> -e1		(-)
	ae2 * be31   -> e123
	ae2 * be23   -> e3		(-)
	ae2 * be41   -> e142
	ae2 * be42   -> -e4
	ae2 * be43   -> e324
	ae2 * be1234 -> e134	(-)?
	
	ae3 * be     -> e3
	ae3 * be12   -> e123	(-)
	ae3 * be31   -> e1
	ae3 * be23   -> -e2		(-)
	ae3 * be41   -> e134
	ae3 * be42   -> e324
	ae3 * be43   -> e4
	ae3 * be1234 -> -e142	(-)?
	
	ae4 * be     -> e4
	ae4 * be12   -> -e142	(-)
	ae4 * be31   -> e134
	ae4 * be23   -> e324	(-)
	ae4 * be41   -> -e4
	ae4 * be42   -> e2
	ae4 * be43   -> -e3
	ae4 * be1234 -> -e123	(-)?

	ae123 * be     -> e123
	ae123 * be12   -> e3	(-)
	ae123 * be31   -> e2
	ae123 * be23   -> e1	(-)
	ae123 * be41   -> -e324
	ae123 * be42   -> e134
	ae123 * be43   -> e142
	ae123 * be1234 -> e4	(-)?
	
	ae142 * be     -> e142
	ae142 * be12   -> -e4	(-)
	ae142 * be31   -> e324
	ae142 * be23   -> -e134	(-)
	ae142 * be41   -> e2
	ae142 * be42   -> e1
	ae142 * be43   -> -e123
	ae142 * be1234 -> e3	(-)?
	
	ae134 * be     -> e134
	ae134 * be12   -> -e324	(-)
	ae134 * be31   -> -e4
	ae134 * be23   -> e142	(-)
	ae134 * be41   -> -e3
	ae134 * be42   -> e123
	ae134 * be43   -> e1
	ae134 * be1234 -> e2	(-)?
	
	ae324 * be     -> e324
	ae324 * be12   -> e134	(-)
	ae324 * be31   -> e142
	ae324 * be23   -> -e4	(-)
	ae324 * be41   -> -e123
	ae324 * be42   -> -e3
	ae324 * be43   -> -e2
	ae324 * be1234 -> e1	(-)?


	e1 =   ae1 * be   + ae2 * be12 + ae3 * be31                           - ae123 * be23   + ae142 * be42   + ae134 * be43   - ae324 * be1234
	e2 = - ae1 * be12 + ae2 * be   + ae3 * be23 + ae4 * be42              + ae123 * be31   + ae142 * be41   - ae134 * be1234 - ae324 * be43
	e3 = - ae1 * be31 - ae2 * be23 + ae3 * be   - ae4 * be43              - ae123 * be12   - ae142 * be1234 - ae134 * be41   - ae324 * be42
	e4 =   ae1 * be41 - ae2 * be42 + ae3 * be43 + ae4 * be   - ae4 * be41 - ae123 * be1234 + ae142 * be12   - ae134 * be31   + ae324 * be23

	*/

	e1 = ae * be1 + ae12 * be2 + ae31 * be3 - ae41 * be4;
	e2 = ae * be2 - ae12 * be1 + ae23 * be3 + ae42 * be4;
	e3 = ae * be3 - ae31 * be1 - ae23 * be2 - ae43 * be4;
	e4 = ae * be4 + ae41 * be1 - ae42 * be2 + ae43 * be3;

	float e123 =   ae12 * be3 - ae31 * be2 + ae23 * be1 + ae1234 * be4;
	float e142 = - ae12 * be4 - ae41 * be2 - ae42 * be1 + ae1234 * be3;
	float e134 = - ae31 * be4 - ae41 * be3 + ae43 * be1 - ae1234 * be2;
	float e324 =   ae23 * be4 - ae42 * be3 - ae43 * be2 - ae1234 * be1;

	// Including negative for be1234
	e1 =   ae1 * be   + ae2 * be12 + ae3 * be31                           - ae123 * be23   + ae142 * be42   + ae134 * be43   - ae324 * be1234;
	e2 = - ae1 * be12 + ae2 * be   + ae3 * be23 + ae4 * be42              + ae123 * be31   + ae142 * be41   - ae134 * be1234 - ae324 * be43;
	e3 = - ae1 * be31 - ae2 * be23 + ae3 * be   - ae4 * be43              - ae123 * be12   - ae142 * be1234 - ae134 * be41   - ae324 * be42;
	e4 =   ae1 * be41 - ae2 * be42 + ae3 * be43 + ae4 * be   - ae4 * be41 - ae123 * be1234 + ae142 * be12   - ae134 * be31   + ae324 * be23;

	// Excluding negative for be1234
	e1 =   ae1 * be   + ae2 * be12 + ae3 * be31                           - ae123 * be23   + ae142 * be42   + ae134 * be43   + ae324 * be1234;
	e2 = - ae1 * be12 + ae2 * be   + ae3 * be23 + ae4 * be42              + ae123 * be31   + ae142 * be41   + ae134 * be1234 - ae324 * be43;
	e3 = - ae1 * be31 - ae2 * be23 + ae3 * be   - ae4 * be43              - ae123 * be12   + ae142 * be1234 - ae134 * be41   - ae324 * be42;
	e4 =   ae1 * be41 - ae2 * be42 + ae3 * be43 + ae4 * be   - ae4 * be41 + ae123 * be1234 + ae142 * be12   - ae134 * be31   + ae324 * be23;
	


	/*
	DERIVING MULTI-VECTOR PRODUCTS USING JOHNS MULTIPLICATION TABLE

	rotor4-rotor4 product

	ae * be     -> 1 e
	ae * be12   -> e12
	ae * be13   -> e13
	ae * be14   -> e14
	ae * be23   -> e23
	ae * be24   -> e24
	ae * be34   -> e34
	ae * be1234 -> e123

	ae12 * be     -> e12
	ae12 * be12   -> -1 e
	ae12 * be13   -> -e23   (+)(-)
	ae12 * be14   -> -e24   (+)(-)
	ae12 * be23   -> e13    (+)
	ae12 * be24   -> e14       (-)
	ae12 * be34   -> e1234     (-)
	ae12 * be1234 -> -e34

	ae13 * be     -> e13
	ae13 * be12   -> e23    (+)(-)
	ae13 * be13   -> -1 e
	ae13 * be14   -> -e34
	ae13 * be23   -> -e12   (+)(-)
	ae13 * be24   -> -e1234
	ae13 * be34   -> e14
	ae13 * be1234 -> e24       (-)

	ae14 * be     -> e14
	ae14 * be12   -> e24    (+)(-)
	ae14 * be13   -> e34
	ae14 * be14   -> -1 e
	ae14 * be23   -> e1234     (-)
	ae14 * be24   -> -e12   (+)
	ae14 * be34   -> -e13   (+)
	ae14 * be1234 -> -e23      (-)
	
	ae23 * be     -> e23
	ae23 * be12   -> -e13   (+)
	ae23 * be13   -> e12    (+)(-)
	ae23 * be14   -> e1234     (-)
	ae23 * be23   -> -1 e
	ae23 * be24   -> -e34      (-)
	ae23 * be34   -> e24    (+)(-)
	ae23 * be1234 -> -e14   (+)
	
	ae24 * be     -> e24
	ae24 * be12   -> -e14      (-)
	ae24 * be13   -> -e1234
	ae24 * be14   -> e12    (+)
	ae24 * be23   -> e34       (-)
	ae24 * be24   -> -1 e
	ae24 * be34   -> -e23   (+)
	ae24 * be1234 -> e13       (-)
	
	ae34 * be     -> e34
	ae34 * be12   -> e1234     (-)
	ae34 * be13   -> -e14
	ae34 * be14   -> e13    (+)
	ae34 * be23   -> -e24   (+)(-)
	ae34 * be24   -> e23    (+)
	ae34 * be34   -> -1 e
	ae34 * be1234 -> -e12      (-)

	ae1234 * be     -> e1234
	ae1234 * be12   -> -e34  (+)
	ae1234 * be13   -> e24      (-)
	ae1234 * be14   -> -e23     (-)
	ae1234 * be23   -> -e14  (+)
	ae1234 * be24   -> e13      (-)
	ae1234 * be34   -> -e12     (-)
	ae1234 * be1234 -> 1 e

	// Unsigned
	e     = ae * be     - ae12 * be12   - ae13 * be13   - ae14 * be14   - ae23 * be23   - ae24 * be24   - ae34 * be34   + ae1234 * be1234
	e12   = ae * be12   + ae12 * be     - ae13 * be23   - ae14 * be24   + ae23 * be13   + ae24 * be14   - ae34 * be1234 - ae1234 * be34
	e13   = ae * be13   + ae12 * be23   + ae13 * be     - ae14 * be34   - ae23 * be12   + ae24 * be1234 + ae34 * be14   + ae1234 * be24
	e14   = ae * be14   + ae12 * be24   + ae13 * be34   + ae14 * be     - ae23 * be1234 - ae24 * be12   - ae34 * be13   - ae1234 * be23
	e23   = ae * be23   - ae12 * be13   + ae13 * be12   - ae14 * be1234 + ae23 * be     - ae24 * be34   + ae34 * be24   - ae1234 * be14
	e24   = ae * be24   - ae12 * be14   + ae13 * be1234 + ae14 * be12   + ae23 * be34   + ae24 * be     - ae34 * be23   + ae1234 * be13
	e34   = ae * be34   - ae12 * be1234 - ae13 * be14   + ae14 * be13   - ae23 * be24   + ae24 * be23   + ae34 * be     - ae1234 * be12
	e1234 = ae * be1234 + ae12 * be34   - ae13 * be24   + ae14 * be23   + ae23 * be14   - ae24 * be13   + ae34 * be12   + ae1234 * be

	*/

	// Unsigned
	float e     = ae * be     - ae12 * be12   - ae13 * be13   - ae14 * be14   - ae23 * be23   - ae24 * be24   - ae34 * be34   + ae1234 * be1234;
	float e12   = ae * be12   + ae12 * be     - ae13 * be23   - ae14 * be24   + ae23 * be13   + ae24 * be14   - ae34 * be1234 - ae1234 * be34;
	float e13   = ae * be13   + ae12 * be23   + ae13 * be     - ae14 * be34   - ae23 * be12   + ae24 * be1234 + ae34 * be14   + ae1234 * be24;
	float e14   = ae * be14   + ae12 * be24   + ae13 * be34   + ae14 * be     - ae23 * be1234 - ae24 * be12   - ae34 * be13   - ae1234 * be23;
	float e23   = ae * be23   - ae12 * be13   + ae13 * be12   - ae14 * be1234 + ae23 * be     - ae24 * be34   + ae34 * be24   - ae1234 * be14;
	float e24   = ae * be24   - ae12 * be14   + ae13 * be1234 + ae14 * be12   + ae23 * be34   + ae24 * be     - ae34 * be23   + ae1234 * be13;
	float e34   = ae * be34   - ae12 * be1234 - ae13 * be14   + ae14 * be13   - ae23 * be24   + ae24 * be23   + ae34 * be     - ae1234 * be12;
	float e1234 = ae * be1234 + ae12 * be34   - ae13 * be24   + ae14 * be23   + ae23 * be14   - ae24 * be13   + ae34 * be12   + ae1234 * be;

	/*

	q = Px

	(+) means opposite sign before but not now
	(!) different term
	(-) means could be treated as negative

	ae * be1 -> e1
	ae * be2 -> e2
	ae * be3 -> e3
	ae * be4 -> e4
	
	ae12 * be1 -> -e2
	ae12 * be2 -> e1
	ae12 * be3 -> e123
	ae12 * be4 -> e124  (+)

	ae13 * be1 -> -e3   (+) (-)
	ae13 * be2 -> -e123 (+) (-)
	ae13 * be3 -> e1	(+) (-)
	ae13 * be4 -> e134      (-)

	ae14 * be1 -> -e4       (-)
	ae14 * be2 -> -e124 (+) (-)
	ae14 * be3 -> -e134 (+) (-)
	ae14 * be4 -> e1        (-)

	ae23 * be1 -> e123
	ae23 * be2 -> -e3
	ae23 * be3 -> e2
	ae23 * be4 -> e234

	ae24 * be1 -> e124      (-)
	ae24 * be2 -> -e4   (+) (-)
	ae24 * be3 -> -e234 (+) (-)
	ae24 * be4 -> e2    (+) (-)
	
	ae34 * be1 -> e134  (+) (-)
	ae34 * be2 -> e234      (-)
	ae34 * be3 -> -e4       (-)
	ae34 * be4 -> e3        (-)

	ae1234 * be1 -> -e234
	ae1234 * be2 -> e134  (+)
	ae1234 * be3 -> -e124 (+)
	ae1234 * be4 -> e123

	// Unsigned
	e1 = ae * be1 + ae12 * be2 + ae13 * be3 + ae14 * be4
	e2 = ae * be2 - ae12 * be1 + ae23 * be3 + ae24 * be4
	e3 = ae * be3 - ae13 * be1 - ae23 * be2 + ae34 * be4
	e4 = ae * be4 - ae14 * be1 - ae24 * be2 - ae34 * be3

	e123 = ae12 * be3 - ae13 * be2 + ae23 * be1 + ae1234 * be4
	e124 = ae12 * be4 - ae14 * be2 + ae24 * be1 - ae1234 * be3
	e134 = ae13 * be4 - ae14 * be3 + ae34 * be1 + ae1234 * be2
	e234 = ae23 * be4 - ae24 * be3 + ae34 * be2 - ae1234 * be1

	// Signed
	e1 = ae * be1 + ae12 * be2 - ae13 * be3 - ae14 * be4
	e2 = ae * be2 - ae12 * be1 + ae23 * be3 - ae24 * be4
	e3 = ae * be3 + ae13 * be1 - ae23 * be2 - ae34 * be4
	e4 = ae * be4 + ae14 * be1 + ae24 * be2 + ae34 * be3

	e123 =  ae12 * be3 + ae13 * be2 + ae23 * be1 + ae1234 * be4
	e124 =  ae12 * be4 + ae14 * be2 - ae24 * be1 - ae1234 * be3
	e134 = -ae13 * be4 + ae14 * be3 - ae34 * be1 + ae1234 * be2
	e234 =  ae23 * be4 + ae24 * be3 - ae34 * be2 - ae1234 * be1

	r = qP*

	ae1 * be     -> e1
	ae1 * be12   -> e2       (-)
	ae1 * be13   -> e3    (+)
	ae1 * be14   -> e4
	ae1 * be23   -> e123     (-)
	ae1 * be24   -> e124
	ae1 * be34   -> e134  (+)
	ae1 * be1234 -> e234     (-)

	ae2 * be     -> e2
	ae2 * be12   -> -e1      (-)
	ae2 * be13   -> -e123 (+)
	ae2 * be14   -> -e124 (+)
	ae2 * be23   -> e3       (-)
	ae2 * be24   -> e4    (+)
	ae2 * be34   -> e234
	ae2 * be1234 -> -e134 (+)(-)

	ae3 * be     -> e3
	ae3 * be12   -> e123     (-)
	ae3 * be13   -> -e1   (+)
	ae3 * be14   -> -e134 (+)
	ae3 * be23   -> -e2      (-)
	ae3 * be24   -> -e234 (+)
	ae3 * be34   -> e4
	ae3 * be1234 -> e124  (+)(-)

	ae4 * be     -> e4
	ae4 * be12   -> e124  (+)(-)
	ae4 * be13   -> e134
	ae4 * be14   -> -e1   (!)
	ae4 * be23   -> e234     (-)
	ae4 * be24   -> -e2   (+)
	ae4 * be34   -> -e3
	ae4 * be1234 -> -e123    (-)

	ae123 * be     -> e123
	ae123 * be12   -> -e3   (+)(-)
	ae123 * be13   -> e2
	ae123 * be14   -> e234  (+)
	ae123 * be23   -> -e1   (+)(-)
	ae123 * be24   -> -e134 (+)
	ae123 * be34   -> e124
	ae123 * be1234 -> -e4   (+)(-)

	ae124 * be     -> e124
	ae124 * be12   -> -e4      (-)
	ae124 * be13   -> -e234 (+)
	ae124 * be14   -> e2
	ae124 * be23   -> e134  (+)(-)
	ae124 * be24   -> -e1
	ae124 * be34   -> -e123
	ae124 * be1234 -> e3       (-)

	ae134 * be     -> e134
	ae134 * be12   -> e234  (+)(-)
	ae134 * be13   -> -e4
	ae134 * be14   -> e3    (+)
	ae134 * be23   -> -e124    (-)
	ae134 * be24   -> e123
	ae134 * be34   -> -e1   (+)
	ae134 * be1234 -> -e2   (+)(-)

	ae234 * be     -> e234
	ae234 * be12   -> -e134 (+)(-)
	ae234 * be13   -> e124
	ae234 * be14   -> -e123
	ae234 * be23   -> -e4      (-)
	ae234 * be24   -> e3    (+)
	ae234 * be34   -> -e2
	ae234 * be1234 -> e1       (-)

	// Unsigned
	e1 = ae1 * be   - ae2 * be12 - ae3 * be13 - ae4 * be14 - ae123 * be23   - ae124 * be24   - ae134 * be34   + ae234 * be1234
	e2 = ae1 * be12 + ae2 * be   - ae3 * be23 - ae4 * be24 + ae123 * be13   + ae124 * be14   - ae134 * be1234 - ae234 * be34 
	e3 = ae1 * be13 + ae2 * be23 + ae3 * be   - ae4 * be34 - ae123 * be12   + ae124 * be1234 + ae134 * be14   + ae234 * be24
	e4 = ae1 * be14 + ae2 * be24 + ae3 * be34 + ae4 * be   - ae123 * be1234 - ae124 * be12   - ae134 * be13   - ae234 * be23

	// Signed
	e1 =   ae1 * be   + ae2 * be12 - ae3 * be13 - ae4 * be14 + ae123 * be23   - ae124 * be24   - ae134 * be34   - ae234 * be1234
	e2 = - ae1 * be12 + ae2 * be   - ae3 * be23 - ae4 * be24 + ae123 * be13   + ae124 * be14   + ae134 * be1234 - ae234 * be34
	e3 =   ae1 * be13 - ae2 * be23 + ae3 * be   - ae4 * be34 + ae123 * be12   - ae124 * be1234 + ae134 * be14   + ae234 * be24
	e4 =   ae1 * be14 + ae2 * be24 + ae3 * be34 + ae4 * be   + ae123 * be1234 + ae124 * be12   - ae134 * be13   + ae234 * be23
	*/

	// Unsigned
	// q = Px
	e1 = ae * be1 + ae12 * be2 + ae13 * be3 + ae14 * be4;
	e2 = ae * be2 - ae12 * be1 + ae23 * be3 + ae24 * be4;
	e3 = ae * be3 - ae13 * be1 - ae23 * be2 + ae34 * be4;
	e4 = ae * be4 - ae14 * be1 - ae24 * be2 - ae34 * be3;

	float e123 = ae12 * be3 - ae13 * be2 + ae23 * be1 + ae1234 * be4;
	float e124 = ae12 * be4 - ae14 * be2 + ae24 * be1 - ae1234 * be3;
	float e134 = ae13 * be4 - ae14 * be3 + ae34 * be1 + ae1234 * be2;
	float e234 = ae23 * be4 - ae24 * be3 + ae34 * be2 - ae1234 * be1;

	// r = qP*
	e1 = ae1 * be   - ae2 * be12 - ae3 * be13 - ae4 * be14 - ae123 * be23   - ae124 * be24   - ae134 * be34   + ae234 * be1234;
	e2 = ae1 * be12 + ae2 * be   - ae3 * be23 - ae4 * be24 + ae123 * be13   + ae124 * be14   - ae134 * be1234 - ae234 * be34;
	e3 = ae1 * be13 + ae2 * be23 + ae3 * be   - ae4 * be34 - ae123 * be12   + ae124 * be1234 + ae134 * be14   + ae234 * be24;
	e4 = ae1 * be14 + ae2 * be24 + ae3 * be34 + ae4 * be   - ae123 * be1234 - ae124 * be12   - ae134 * be13   - ae234 * be23;

	// Signed
	// q = Px
	e1 = ae * be1 + ae12 * be2 - ae13 * be3 - ae14 * be4;
	e2 = ae * be2 - ae12 * be1 + ae23 * be3 - ae24 * be4;
	e3 = ae * be3 + ae13 * be1 - ae23 * be2 - ae34 * be4;
	e4 = ae * be4 + ae14 * be1 + ae24 * be2 + ae34 * be3;

	float e123 =  ae12 * be3 + ae13 * be2 + ae23 * be1 + ae1234 * be4;
	float e124 =  ae12 * be4 + ae14 * be2 - ae24 * be1 - ae1234 * be3;
	float e134 = -ae13 * be4 + ae14 * be3 - ae34 * be1 + ae1234 * be2;
	float e234 =  ae23 * be4 + ae24 * be3 - ae34 * be2 - ae1234 * be1;

	// r = qP*
	e1 =   ae1 * be   + ae2 * be12 - ae3 * be13 - ae4 * be14 + ae123 * be23   - ae124 * be24   - ae134 * be34   - ae234 * be1234;
	e2 = - ae1 * be12 + ae2 * be   - ae3 * be23 - ae4 * be24 + ae123 * be13   + ae124 * be14   + ae134 * be1234 - ae234 * be34;
	e3 =   ae1 * be13 - ae2 * be23 + ae3 * be   - ae4 * be34 + ae123 * be12   - ae124 * be1234 + ae134 * be14   + ae234 * be24;
	e4 =   ae1 * be14 + ae2 * be24 + ae3 * be34 + ae4 * be   + ae123 * be1234 + ae124 * be12   - ae134 * be13   + ae234 * be23;

}
```