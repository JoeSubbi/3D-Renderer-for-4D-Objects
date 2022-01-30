import math
import random

class Vector4:
    
    def __init__(self, x, y, z, w):
        self.x = x
        self.y = y
        self.z = z
        self.w = w

    def magnitude(self):
        return math.sqrt(self.x**2 + self.y**2 + self.z**2 + self.w**2)

    def dot(a, b):
        return (a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w)

    def norm(self):
        mag = self.magnitude()
        self.x /= mag
        self.y /= mag
        self.z /= mag
        self.w /= mag

    def __str__(self):
        return str(self.x) + ", " + str(self.y) + ", " + str(self.z) + ", " + str(self.w) + ", "

class Bivector4:
    
    def __init__(self, bxy, bxz, byz, bxw, byw, bzw):
        self.bxy = bxy
        self.bxz = bxz
        self.byz = byz
        self.bxw = bxw
        self.byw = byw
        self.bzw = bzw

    @staticmethod
    def wedge(a, b):
        return Bivector4(
            a.x * b.y - a.y * a.x,
            a.x * b.z - a.z * b.x,
            a.y * b.z - a.z * b.y,
            a.x * b.w - a.w * b.x,
            a.y * b.w - a.w * b.y,
            a.z * b.w - a.w * b.z
        )

class Rotor4:

    def __init__(self):
        # Scalar
        self.a = 1
        # Bivector
        self.bxy = 0
        self.bxz = 0
        self.byz = 0
        self.bxw = 0
        self.byw = 0
        self.bzw = 0
        # Pseudo Scalar
        self.bxyzw = 0

    def constructor(self, a, bxy, bxz, byz, bxw, byw, bzw, bxyzw):
        self.a = a
        self.bxy = bxy
        self.bxz = bxz
        self.byz = byz
        self.bxw = bxw
        self.byw = byw
        self.bzw = bzw
        self.bxyzw = bxyzw

    def bv_constructor(self, bv, angle):
        sina = math.sin(angle/2)
        self.a = math.cos(angle/2)
        self.bxy = -sina * bv.bxy
        self.bxz = -sina * bv.bxz
        self.byz = -sina * bv.byz
        self.bxw = -sina * bv.bxw
        self.byw = -sina * bv.byw
        self.bzw = -sina * bv.bzw
        self.bxyzw = 0

    @staticmethod
    def __mul__(a, b):
        e     = -a.bxw * b.bxw   - a.bxy * b.bxy   - a.bxz * b.bxz   - a.byw * b.byw   - a.byz * b.byz   - a.bzw * b.bzw   + a.bxyzw * b.bxyzw + a.a * b.a;
        exy   = -a.bxw * b.byw   + a.bxy * b.a     - a.bxz * b.byz   + a.byw * b.bxw   + a.byz * b.bxz   - a.bzw * b.bxyzw - a.bxyzw * b.bzw   + a.a * b.bxy;
        exz   = -a.bxw * b.bzw   + a.bxy * b.byz   + a.bxz * b.a     + a.byw * b.bxyzw - a.byz * b.bxy   + a.bzw * b.bxw   + a.bxyzw * b.byw   + a.a * b.bxz;
        exw   =  a.bxw * b.a     + a.bxy * b.byw   + a.bxz * b.bzw   - a.byw * b.bxy   - a.byz * b.bxyzw - a.bzw * b.bxz   - a.bxyzw * b.byz   + a.a * b.bxw;
        eyz   = -a.bxw * b.bxyzw - a.bxy * b.bxz   + a.bxz * b.bxy   - a.byw * b.bzw   + a.byz * b.a     + a.bzw * b.byw   - a.bxyzw * b.bxw   + a.a * b.byz;
        eyw   =  a.bxw * b.bxy   - a.bxy * b.bxw   + a.bxz * b.bxyzw + a.byw * b.a     + a.byz * b.bzw   - a.bzw * b.byz   + a.bxyzw * b.bxz   + a.a * b.byw;
        ezw   =  a.bxw * b.bxz   - a.bxy * b.bxyzw - a.bxz * b.bxw   + a.byw * b.byz   - a.byz * b.byw   + a.bzw * b.a     - a.bxyzw * b.bxy   + a.a * b.bzw;
        exyzw =  a.bxw * b.byz   + a.bxy * b.bzw   - a.bxz * b.byw   - a.byw * b.bxz   + a.byz * b.bxw   + a.bzw * b.bxy   + a.bxyzw * b.a     + a.a * b.bxyzw;

        return Rotor4(e, exy, exz, eyz, exw, eyw, ezw, exyzw)

    def rotate(self, a):
        s = self.a
        bxy = self.bxy
        bxz = self.bxz
        byz = self.byz
        bxw = self.bxw
        byw = self.byw
        bzw = self.byw
        bxyzw = self.bxyzw

        r = Vector4(
            2 * a.w * bxw * s
            + 2 * a.w * bxy * byw
            + 2 * a.w * bxz * bzw
            + 2 * a.w * byz * bxyzw
            - a.x * bxw * bxw
            - a.x * bxy * bxy
            - a.x * bxz * bxz
            + a.x * byw * byw
            + a.x * byz * byz
            + a.x * bzw * bzw
            - a.x * bxyzw * bxyzw
            + a.x * s * s
            - 2 * a.y * bxw * byw
            + 2 * a.y * bxy * s
            - 2 * a.y * bxz * byz
            + 2 * a.y * bzw * bxyzw
            - 2 * a.z * bxw * bzw
            + 2 * a.z * bxy * byz
            + 2 * a.z * bxz * s
            - 2 * a.z * byw * bxyzw,

            - 2 * a.w * bxw * bxy
            - 2 * a.w * bxz * bxyzw
            + 2 * a.w * byw * s
            + 2 * a.w * byz * bzw
            - 2 * a.x * bxw * byw
            - 2 * a.x * bxy * s
            - 2 * a.x * bxz * byz
            - 2 * a.x * bzw * bxyzw
            + a.y * bxw * bxw
            - a.y * bxy * bxy
            + a.y * bxz * bxz
            - a.y * byw * byw
            - a.y * byz * byz
            + a.y * bzw * bzw
            - a.y * bxyzw * bxyzw
            + a.y * s * s
            + 2 * a.z * bxw * bxyzw
            - 2 * a.z * bxy * bxz
            - 2 * a.z * byw * bzw
            + 2 * a.z * byz * s,

            - 2 * a.w * bxw * bxz
            + 2 * a.w * bxy * bxyzw
            - 2 * a.w * byw * byz
            + 2 * a.w * bzw * s
            - 2 * a.x * bxw * bzw
            + 2 * a.x * bxy * byz
            - 2 * a.x * bxz * s
            + 2 * a.x * byw * bxyzw
            - 2 * a.y * bxw * bxyzw
            - 2 * a.y * bxy * bxz
            - 2 * a.y * byw * bzw
            - 2 * a.y * byz * s
            + a.z * bxw * bxw
            + a.z * bxy * bxy
            - a.z * bxz * bxz
            + a.z * byw * byw
            - a.z * byz * byz
            - a.z * bzw * bzw
            - a.z * bxyzw * bxyzw
            + a.z * s * s,

            - a.w * bxw * bxw
            + a.w * bxy * bxy
            + a.w * bxz * bxz
            - a.w * byw * byw
            + a.w * byz * byz
            - a.w * bzw * bzw
            - a.w * bxyzw * bxyzw
            + a.w * s * s
            - 2 * a.x * bxw * s
            + 2 * a.x * bxy * byw
            + 2 * a.x * bxz * bzw
            - 2 * a.x * byz * bxyzw
            - 2 * a.y * bxw * bxy
            + 2 * a.y * bxz * bxyzw
            - 2 * a.y * byw * s
            + 2 * a.y * byz * bzw
            - 2 * a.z * bxw * bxz
            - 2 * a.z * bxy * bxyzw
            - 2 * a.z * byw * byz
            - 2 * a.z * bzw * s
        )
        return r

    def rotorRotate(self, r):
        return self * r * self.reverse()

    def reverse(self):
        return Rotor4(self.a,   -self.bxy, -self.bxz, -self.byz,
                     -self.bxw, -self.byw, -self.bzw,  self.bxyzw)

    def lengthSquared(self):
        return (self.a**2   + self.bxy**2 + self.bxz**2 + self.byz**2 +
              + self.bxw**2 + self.byw**2 + self.bzw**2 + self.bxyzw**2)

    def length(self):
        return math.sqrt(self.lengthSquared)

    def normalise(self):
        l = self.length();
        self.a /= l;
        self.bxy /= l;
        self.bxz /= l;
        self.byz /= l;
        self.bxw /= l;
        self.byw /= l;
        self.bzw /= l;
        self.bxyzw /= l;

    def normal(self):
        r = self
        r.normalise()
        return r

    @staticmethod
    def GeoProd(a, b):
        bv = Bivector4.wedge(a, b)
        return Rotor4.bv_constructor(Vector4.dot(a, b), 
            bv.bxy, bv.bxz, bv.byz, bv.bxw, bv.byw, bv.bzw, 0)

    @staticmethod
    def difference(a, b):
        #randomVec = Vector4(random.uniform(0, 10.0), random.uniform(0, 10.0),
        #                    random.uniform(0, 10.0), random.uniform(0, 10.0))
        randomVec = Vector4(1,1,1,1)
        randomVec.norm()
        randomVecA = a.rotate(randomVec)
        randomVecB = b.rotate(randomVec)

        t = (Vector4.dot(randomVecA, randomVecB)) / (randomVecA.magnitude() * randomVecB.magnitude())
        if t > 1: t = 1
        elif t < -1: t = -1
        return math.acos( t )