
## *Investigation into the different representations of 4D shapes* 
#### *Joe Subbiani* 
#### *2377990S* 

## Proposal
### Motivation
<!--[Clearly motivate the purpose of your project; why someone would care about what you are doing]-->

4 Dimensional objects are something that cannot be created physically and cannot be visualised in their true state. The motivation for the project is to further a persons understanding of high dimensional space and to test their understanding of geometry that cannot be wholely visualised.

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
 - have face colours "stick" to the faces
 - Developed function to map separate materials to different object.
 - Created a reflective material
 - Implemented rotation using matrix multiplication so axis don't mis-align - still has gimbal lock problem
 - Implemented rotation in 3D using quaternions - cannot be extended to 4D

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
 - Implementing the rotation has been much harder than expected
   - I have implemented euler matrices/matrix multiplication in order to prevent axis from skewing unexpectedly - had gimbal lock
   - I implemented 3D quaternion but it cannot be extended to 4D
   - *Attempting* to implement a geometric algebra "rotor". they have held up the project only a little so far. I was hoping to be experimenting on user interaction, however I am still playing around with the implementation of 4D rotation.

### Risks
<!--[What problems do you foresee in the future and how will you mitigate them?]-->

 - Developing intuitive rotation may be a problem. The order of rotation matters.
   - originally I was intending to just have a slider from $-\pi$ to $\pi$ (-180deg to 180deg), however if you adjust one slider and then another slider it can behave unexpectedly and unintuitively.
   - ~~The new plan is to have 2D rotation mechanics where the user can slide up or down or left to right and that will change the pose of the object relative to the users perspective. This may create some challenges for the 3 degrees of 4D rotation.~~
   - I intend to implement 2 forms of user interaction based rotation
     1. screen based gestures to push and slide the object in an expected way
     2. an arc/grab ball to slide single axis constrained disks for very controlled rotation
   - This will be time consuming, but I will mitigate it by doing several basic test programs in 3D before trying anything in 4D.

 - Developing stable rotation. Matrix multiplication has gimbal lock. Quaternions don't extend to 4D
   - Attempting to implement Rotors but is proving hard to translate the abstract maths to code
   - I will attempt to mitigate the impact on my schedule by completing other tasks at the same time, so I am not making no progress on the project. 

## Plan
<!--[Time plan, in roughly weekly to monthly blocks, up until submission week]-->

### Semester 1

 - [x] Take 2 weeks (Weeks 3 and 4) to research papers focused in the fields of geometrical representation and interaction.

 - [ ] ~~Week 5: attempt to implement an intuitive rotation mechanic using click and drag.~~
 - [ ] ~~Week 6: attempt to implement an intuitive rotation mechanic using an arc/grab ball.~~
 - [x] Week 4: implement basic click and drag rotation mechanic
 - [x] Week 5: implement basic arc/grab ball rotation mechanic
 - [ ] ~~Week 6: Refine the rotation mechanics - keep attempting to implement Rotors.~~

Week 6:
 - [x] Create UI to manipulate 3D shapes with 2D cross sections
 - [x] Keep attempting to implement Rotors.

Week 7:  
 - [ ] Implement and test "onion skin" and cross section "timeline" interpretation of the 4th dimension.  
 - [ ] Implement a 3D perspective that in real time mimics the 4D object.

Week 8:
 - [ ] Create intuitive UI for users to manipulate shapes with.

Week 9:  
 - [ ] Create an "Identify the shape" mode. 
 - [ ] Add more shapes - 4D cylinder, ~~cone, capsule~~ pentachoron.

Week 10:  
 - [ ] Plan and script a walk through for users to play with shapes and attempt to identify them.  
 - [ ] Set up a new mode for shape matching.

Week 11:  
 - [ ] Tutorial videos that explain why shapes behave they do, and traits to identify what the shape is.  
 - [ ] Polish the program to be a "final product".

Week 12:
 - [ ] Finishing polishing "final product"

### Stuff I hope to make consistent progress on:

Digitally Draw up polished diagrams of each shape implemented:
 - deriving the SDF.
 - Explaining how they are projected into the 4th dimension.

Working on intuitive rotation mechanics

Developing Demo programs to prepare for "final product"

quality of life polish

### Christmas

Start planning for user experiments

### Semester 2

 - Create some automated data analysis based on plan for user interaction
 - Run user tests
 - Write up results
 - Write dissertation