
## *Investigation into the different representations of 4D shapes* 
#### *Joe Subbiani* 
#### *2377990S* 

## Proposal
### Motivation
<!--[Clearly motivate the purpose of your project; why someone would care about what you are doing]-->

4 Dimensional objects are something that cannot be created physically and cannot be visualised in their true state. By finding an intuitive way to represent hyper dimensional shapes, especially in a mathematical format, utilising ray marching, low file size, data visualisation.

### Aims
<!--[Clearly state what the project is intended to do. This should be something which is measurable; it should be possible to tell if you succeeded]-->

This project focuses on the development of a wide range of 4 Dimensional shapes and a variety of ways to display these shapes in a way humans can best interpret them. As such the usability of each representation will be experimentally validated in order to find the most intuitive and effective way to represent higher dimensional shapes.

## Progress
<!--[Briefly state your progress so far, as a bulleted list]-->

 - Experimented with a face based render engine in Java
 - Experimented with and developed a 3D ray march shader in Unity.
 - Use signed distance functions to define a series of shapes
   - Sphere
   - Box
   - Torus
   - Capsule
   - Cylinder
   - Octahedron
   - Tetrahedron
 - Use boolean intersections to find a ~2D cross section of these 3D shapes
 - Converted several of these shapes into 4 Dimensional "equivilants".
   - 4D hyper sphere
   - 4D hyper cube/box
   - 4D Torus
   - 4D Octahedron
   - 4D Tetrahedron
 - Render 3D cross sections of these 4D shapes (the camera is in the 3D "slice" of this 4D world).
 - Research transformations on the objects
   - Rotate the objects in 3D
 - Develop 4D rotation around the 3 axis of rotation.
 - Created a super basic demo/concept program I am considering for testing uses ability to manipulate 4D shapes. On thr right hand side is a randomly oriented 4D shape in all 6 degrees of rotation, on the left hand side is an un-rotated version of the same shape. The users goal is to orient the shape to match the right hand side using some rotation sliders. (There were issues)
 - Developed function to map separate materials to different object.
 - Created a reflective material

## Problems and risks
### Problems
<!--[What problems have you had so far, that have held up the project?]-->

 - Researching/deriving the signed distance function (SDF) for a cone that will translate well into 4D.
   - This is holding up the project a little because a cone can provide some very intuitive but slightly more complex concepts that I imagine an unexperienced user may find more intuitive compared to some other objects.
 - I originally intended to implement 2 variations of 4D representation.
   - Taking a 3D cross section - a "slice" of the 4D world.
   - Taking the "shadow" of 4D objects which would be inherently 3D by collapsing a dimension
   - It turns out the shadow is of course the same as the cross section at the center of the object
   - this has not held up the project, if anything it has shortened it a very small amount.
 - Having the faces of shapes coloured and having those colours stay "attached" to that face of the shape.
   - The coordinate system that colours 3D dimensional objects based on the direction of the normal of the face was not dependant on the objects, but their container.
   - Having these coordinates translated/oriented exactly how the objects are allows the faces to consistantly maintain their colouration.
 - The onion skin effect I intended to do is harder than I expected. 
   - I thought I could intersect the same object and move the cross section inline with all other cross sections afterwards. This is not the case, I will have to create another object, offset by the same amount and then take the cross section such that the cross section will be in the desired position. This should not hold up the project.

### Risks
<!--[What problems do you foresee in the future and how will you mitigate them?]-->

 - Developing intuitive rotation may be a problem. The order of rotation matters.
   - originally I was intending to just have a slider from $-\pi$ to $\pi$ (-180deg to 180deg), however if you adjust one slider and then another slider it can behave unexpectedly and unintuitively.
   - The new plan is to have 2D rotation mechanics where the user can slide up or down or left to right and that will change the pose of the object relative to the users perspective. This may create some challenges for the 3 degrees of 4D rotation.
   - This will be time consuming, but I will mitigate it by doing several basic test programs in 3D before trying anything in 4D.

## Plan
<!--[Time plan, in roughly weekly to monthly blocks, up until submission week]-->

Discuss project with John to get an idea of the direction I am heading in...

Implement and test onion skin interpretation of the 4th dimension.
Implement a 3D perspective that in real time mimics the 4D object.

Develop more intuitive rotation mechanics

Create intuitive rotation and UI for users to manipulate shapes with.

Digitally Draw up polished diagrams of each shape implemented:
 - deriving the SDF.
 - Explaining how they are projected into the 4th dimension.

Create an explanation video that will be shown to users of the system.