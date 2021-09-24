# 3D-Renderer-for-4D-Objects
Research and Development into Rendering 3D Cross sections of 4Dimensional Objects

# Ray Marcher\
Ray marching is described well in [this tutorial](https://www.youtube.com/watch?v=PGtv-dBi2wE&t=441s)
Research and Development into raymarching in Unity.
This portion of the project is following [this tutorial](https://www.youtube.com/watch?v=S8AWd66hoCo)

### Shapes
The mathematical definitions describing the shape distance functions of primitive shapes
 - cube
 - sphere
 - torus
 - cylinder
 - capsule

Following [this tutorial](https://www.youtube.com/watch?v=Ff0jJyyiVyw)

### Progress Report 21/09/2021
Left the project for a while and came back wanting to try a new angle. Real-time raymarching is viable and presumably a much more straight forward method of getting the cross sections of shapes and defining 4 dimensional objects.

I may still build a render engine from scratch, but it seems like a wise idea to get something working in a preexisting render engine, i.e unity, and creating something from scratch at a later date.

The purpose of this project will be to investigate different representations of 4D objects in 3D (in 2D) and therefore getting something working first is a priority.

### Progress Report 25/09/2021
Back with a 3D scene using ray marching containing a bunch of shapes. Also created a 4D sphere and cube.
Implemented 3D and 4D rotation.

Looking into SDF shapes described [here](https://iquilezles.org/www/articles/distfunctions/distfunctions.htm)

## 3DRenderer\
Attempting a 3D render engine from scratch in Java.
This portion of the project is following [this tutorial](https://www.youtube.com/watch?v=gnT6YC5Nf70&list=PLsRmsZm0xMNogPyRn6gNWq4OM5j22FkAU&index=1) 
and learning the basics of rendering 3D items from scratch in java.
[Link to their GitHub Repo](https://github.com/javatutorials101/Tutorials)

### Planning\
Dedicated to researching the how to view objects in a lower dimension. 
Firstly 2D cross sections of 3D objects and then 3D cross sections of 4D objects.

The coordinate system:
x = north and south
y = east and west
z = up and down
w = 4D forward and 4D backwards

### Progress Report 20/12/2020 
There are 5 main Classes so far, Vector3, Vector4, ThreeObject, FourObject and ShapeGenerator  
Vector3 and Vector4 (actually need to be renamed as they have no direction or magnitude, they are just points in 3D an 4D space respectively) hold the coordinates for the objects.  
Both ThreeObject and FourObject contain an array of Vector3 coordinates and Vector4 coordinates respectively, they also hold an array of faces. A face is an array of coordinates.
ShapeGenerator Contains methods on creating a unit 3cube and a unit4cube.

### Update 31/12/2020
Updated Shape Generator with methods to create any sized cube with any offset. The method takes a minor and major coordinate, an example of this would be -1 and 1 which would create
a unity cube spanning (-1,-1,-1) to (1,1,1). if you were then to apply a z offset of 1 the unit cube would now span from (-1,-1,0) to (1,1,2). Practically this means we can create a
cube of any size and then move it to a desired position. In future I could add a scale factor to create cuboids in a similar way before the offset is applied.
