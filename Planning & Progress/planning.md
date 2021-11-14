- [Tasks](#tasks)
  - [Define the shape](#define-the-shape)
  - [Match the pose](#match-the-pose)
- [Representations](#representations)
  - [Onion Skin](#onion-skin)
  - [W Axis timeline](#w-axis-timeline)
  - [3D representation of the 4D object](#3d-representation-of-the-4d-object)
  - [4D Multi-View](#4d-multi-view)
- [Shapes](#shapes)

# Tasks

## Define the shape

Given a 4D shape in a random pose and allowing the user to play around with it for an allotted time, can the user correctly determine what the shape is?

## Match the pose

Given an Active shape on one side of the screen and a Target shape on the other side of the screen in a random pose, can the user manipulate the Active shape such that it matches the pose of the Target shape.

---

# Representations

## Onion Skin

A central object with a translucent blue and red 3D "slice" overlaying a slice in the +W and -W direction, directly in line with the central object.

## W Axis timeline

A central object adjacent to 1 or more +W and -W "slices" on the right and left of the center object respectively.

## 3D representation of the 4D object

Have a 3D shape that is similar to the displayed 4D object - i.e a hyper cube and a cube - that rotates along x, y, and z, with the wx, wy, and wz rotation respectively.

## 4D Multi-View

Show 4 different perspectives. The regular 3D cross section of the 4D shape, and the same shape rotated on xw, yw and zw planes.

---

# Shapes

 - [x] Cube - 4D Hyper Cube

 - [x] Sphere - 4D Hyper Sphere

 - [x] Tetrahedron - 4D Hyper Tetrahedron - need to analyse
 - [ ] Pentachron - Tetrahedron with a vertex extended into 4D

 - [x] Octahedron - 4D Hyper Octahedron - need to analyse

 - [x] Torus - 4D Hyper Torus
 - [ ] ~~Torus - Extruded Torus into the 4th Dimension~~ uninteresting

 - [ ] Cylinder - 3D Sphere extruded into the 4th Dimension
 - [ ] ~~Cylinder - Extruded Cylinder into the 4th Dimension~~ uninteresting

 - [x] Capsule - 4D Sphere extruded along any dimension
 - [ ] ~~Capsule - Extruded Capsule into the 4th Dimension~~ uninteresting

 - [x] Cone - 4D Hyper Cone - point extended into w
 - [x] Cone - 4D Hyper Cone - point extended into y

