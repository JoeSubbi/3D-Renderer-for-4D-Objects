```
same number of components
same API
efficient
avoid gimbal lock



rotations happen in 2D planes
not on an axis



to compute axis of rotation to rotate vector a to vector b - take cross product of the 2 vectors
cross product creates perpendicular vector

outter product - Bivector, B = a ^ b - like a plane

therefore we need 3 Bivectors for 3D - x^z, z^y, x^y



if to bivectors are parrallel, the "area of the plane" is 0 (the vectors outer product is 0) - (a+b) ^ (a+b) = 0
a^b = - b^a



don't have to be unit vectors.
double vector magnitude, doubles bivector area
a ^ b = (a_x x + a_y y) ^ (b_x x + b_y y)
expands to: B_xy = a_x b_y - b_x a_y



geometric product
ab

associative and commutative

ab = 0.5(ab+ba) + 0.5(ab-ba)
    a.b + a^b



Rotating and reflecting
reflecting:

v = v|| + vL
Ra(v) = v|| - vL

    = (v - (v.a)a) - ((v.a)a)
    = v-2(v.a)a
    
    replace dot product with geometric product
    
        = v-2(0.5(va+av))a
        = v - va^2 -ava
        = -ava seen before
        
2 reflections is a rotation!

Apply Ra(v), Apply Rb(Ra(v))
-b(-ava)b = ba v ab

we call ab = a.b + a^b a rotor


3D rotor
a + B_xy x^y + B_xz x^z + B_yz y^z
```