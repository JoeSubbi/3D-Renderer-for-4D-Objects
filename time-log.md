# Timelog

* Investigation into the different representations of 4D shapes
* Joe Subbiani
* 2377990s
* John Williamson

## Guidance

* This file contains the time log for your project. It will be submitted along with your final dissertation.
* **YOU MUST KEEP THIS UP TO DATE AND UNDER VERSION CONTROL.**
* This timelog should be filled out honestly, regularly (daily) and accurately. It is for *your* benefit.
* Follow the structure provided, grouping time by weeks.  Quantise time to the half hour.

## Week 1

### 21 Sep 2021

 - *1 hour* Create a unity project and do some research into ray marching.
 - *2 hours* Research into the derivation of signed distance functions (SDFs) for a series of shapes; a sphere, box, capsule, cylinder and torus.

### 22 Sep 2021

 - *1 hour* Researching transformations that can be applied to SDF shapes. Includes translation, rotation, boolean operations.
 - *1 hour* Researching diffuse lighting for the ray marched scene.

### 23 Sep 2021

 - *0.5 hours* Create a 3D scene containing all the shapes I had researched.
 - *0.5 hours* Use boolean intersection to find the 2D cross section of the 3D scene.
 - *0.5 hours* Implementing diffuse lighting into the 3D scene.

### 24 Sep 2021

 - *1 hour* Created a working 4D sphere and a slider to travel along the W axis.
 - *0.5 hours* Created a working 4D box.
 - *0.5 hours* Implementing 2D rotation.
 - *0.5 hours Successful experiment in implementing 4D rotation.
 - *2 hours* Researching and attempting to implement a 3D cone that would translate well to 4D. - Failure.

### 25 Sep 2021

 - *1 hour* Researching and attempting to implement a 3D cone that would translate well to 4D. - Failure.
 -  - *2 hours* Researching existing SDFs for other shapes - namely an Octahedron and Tetrahedron.
 - *2 hours* Implementing a 4D octahedron and tetrahedron. - had to make some small tweaks but they work.

### 26 Sep 2021

 - *1 hour* Implementing a 4D torus.
 - *2 hour* Setting up a scene to create a shape pose matching demo - it is really hard using the diffuse light. Would be better having each face coloured.
 - *3 hours* The shapes already used face normals and a coordinate system to colour each face, but the colour was dependant on the container, not the ray marched shapes orientation. Time was spent having the coordinate system rotate with the objects rotation. because of this I need a separate container to maintain 2 objects, one in the desired pose, and 1 for the user to manipulate.

## Week 2

### 27 Sep 2021

 - *2 hours* Experimenting with a 3D "Shadow" of a 4D object, where the W axis is collapsed. Realised this is the same as taking the cross section at the center of the object.

### 28 Sep 2021

 - *1 hour* writing up time log.
 - *1 hour* writing up status report.

### 29 Sep 2021

 - *1 hour* created a new version of the 3D > 2D renderer using materials.
 - *0.5 hours* tried an onion skin effect, but will have to try a different way.
 - *2 hour* implemented materials for different objects.

### 30 Sep 2021

 - *0.5 hours* Weekly meeting - direction to head in.

### 31 Sep 2021

 - *3 hour* collecting research papers to look through to understand the field better
   - read [Arcball Rotation Control](https://research.cs.wisc.edu/graphics/Courses/559-f2001/Examples/Gl3D/arcball-gems.pdf)
     - potential for 'commutative' rotation?
   - read [Rotating 3D shapes - Google Classroom](https://www.khanacademy.org/computing/computer-programming/programming-games-visualizations/programming-3d-shapes/a/rotating-3d-shapes)
     - Potential a simple and intuitive rotation solution


## Week 3

### 4 Oct 2021

- *1 hour* Using Zotero to compile research papers.
   - read [Polyvision: 4D Space Manipulation through Multiple Projections](https://www.math.kyoto-u.ac.jp/~kaji/papers/SIGGRAPH2019_Polyvision_final.pdf).
     - Motivation to have side by side slices of 4D space to see how the object will change.
 - *1 hour* Making a plan for tasks, representations and the shapes I intend to create for the user experiments.

### 5 Oct 2021
 - *2 hour*
   - read [3D Scene manipulation with 2D Devices and Constraints](https://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.26.5800&rep=rep1&type=pdf)
     - Less relevant than I thought it would be.
     - Using real world constraints such as gravity and object intersection to manipulate objects in a 3D world with utilising all 6 degrees of freedom.
   - read [4D Rendering: Projection & Cross Section](https://luicat.github.io/2018/05/23/4D-rendering.html)
     - Uses verticies, rather than ray marching (similar to Marc Ten Bosch).
       - Seems easier to construct complex shapes.
       - Harder to get a cross section.
       - Cannot do curves well.
 - *1 hour* expanding the sources of relevant papers to find new material.