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