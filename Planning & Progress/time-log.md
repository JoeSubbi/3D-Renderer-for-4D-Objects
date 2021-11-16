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

### 6 Oct 2021

 - *1 hour* Read [A visualization method of four-dimensional polytopes by oval display of parallel hyperplane slices](https://link.springer.com/article/10.1007%2Fs12650-015-0319-5)
   - very similar to my idea of a 4d cross section timeline and 4d layered cross sections.
 - *0.5 hours* Looked up shape name conventions.
 - *0.5 hours* organising *Zotero* with colour coding, to keep track of what I have read, need to read and favourites.

### 7 Oct 2021

 - *0.5 hours* Weekly meeting - How to improve finding research papers - what to look for, and advice.
   - take a look at psychology of understanding/teaching geometry.
 - Reread relevant chapters in *Things to make and do in the fourth dimension*.
   - may be good for explaining basic concepts?

### 8 Oct 2021

 - *2 hours* Read multiple articles by [Inigo Quilez](https://www.iquilezles.org/)
   - good for getting to grips with ray marching.
 - *0.5 hours* Read [The van Hiele Model of Geometric Thinking](https://www.mff.cuni.cz/veda/konference/wds/proc/pdf12/WDS12_112_m8_Vojkuvkova.pdf)
   - will be great dissertation material.
 - *1 hours* Read [Keyboard-based control of four-dimensional rotations](https://link.springer.com/article/10.1007/s12650-015-0313-y#ref-CR4)
   - not super helpful, was hoping for something more interesting - rotates around 4th dimension $\pi/16$ rad with key presses, not very precise.

## Week 4

### 11 Oct 2021

 - *1 hour* Writing C# script to control rotation of an object outside of the shader code.
 - *3 hours* Researching how to control  the shapes without gimbal lock.

### 12 Oct 2021

 - *2 hours* implemented 3D rotation by multiplying rotation matrices. axis don't mis-align, but gimbal lock is still a problem

### 13 Oct 2021

 - *2 hours* *attempted* implemented 4D rotation by multiplying matrices. It was very messy and does not work very well...

### 14 Oct 2021

 - *0.5 hours* weekly meeting - look into exp/log map rotation
   - http://15462.courses.cs.cmu.edu/fall2021content/exercises/Solutions06.pdf
 - *3 hours* *attempted* to implement a quaternion rotation system. wen't well until I tried it... going to need to work on that

### 15 Oct 2021

 - *2 hours* implemented quaternion rotation. Very stable in 3D. Could not extend to 4D.
 - *0.5 hours* Reading about [Marc ten Bosch's solution to 4D rotations](https://marctenbosch.com/quaternions/)
 - *1.5 hours* studying and digesting [Geometric Algebra Rotors](https://marctenbosch.com/news/2011/05/4d-rotations-and-the-4d-equivalent-of-quaternions/)
 - *2 hours* studying to learn about rotors - [Geometric Algebra](https://www.youtube.com/playlist?list=PLpzmRsG7u_gqaTo_vEseQ7U8KFvtiJY4K)

### 16 Oct 2021

 - *6 hours* continuing studies on geometric algebra [Geometric Algebra](https://www.youtube.com/playlist?list=PLpzmRsG7u_gqaTo_vEseQ7U8KFvtiJY4K)

### 17 Oct 2021

 - *2 hours* begin implementation of a rotor class
   - very slow process. I am struggling to translate mathematical concepts to code

## Week 5

### 18 Oct 2021

 - *1 hour* make some developments on the rotor class
   - I think I am heading in the right direction - each bivector - e.g $B_xy$ is assigned the projected area of the parallelogram defined by the wedge product of 2 vectors.
   - the rotor produced from the geometric product $ab = ab + a\wedge b$ can be built from the projected areas to define its orientation in space
   - still not sure how to produce the rotor
   - not sure how to rotate about that rotor

### 19 Oct 2021

 - *4 hours* Implement a quaternion based arc/grab ball to rotate shapes based on 3 circles
   - works well, but need to implement a more fluid experience. The direction of rotation reverses if the object rotates 180deg in another direction.
   - currently scrub left or right on screen to rotate whilst touching a circle with the mouse

### 20 Oct 2021

 - *3 hours* research how you determine confidence/understanding in a learning environment
   - definition of understanding
   - visual aid helps rather than just words - i should do demos (already planned)
   - 121 discussion is usually good, but maybe not feasible

### 21 Oct 2021

 - *3 hours* working on rotors

### 22 Oct 2021

 - *0.5 hours* weekly meeting
   - discussion of how to implement rotors - need to implement a sense of direction
 - *2 hours* further attempts at implementing rotors - the shape does change, but very very weirdly

## Week 6

### 25 Oct 2021

 - *1.5 hours* Creating a new unity project for final program
   - Create a 3D and 2D shader side by side
 - *2 hours* Create a separate 2D shader and overlay it on the UI

### 26 Oct 2021

 - *2 hours* Implement a script to control the cross section/object position
 - *1 hours* Creating a scalable UI
 - *2 hours* Create a 2 Layer transparency effect for the plane cross section

### 27 Oct 2021

 - *1 hour* tidy up the UI and add a UI slider to control the object position
 - *0.5 hours* add other shapes and control them a slider
 - *2 hours* refining the 3D to 2D app and finishing it

### 28 Oct 2021

 - *0.5 hours* weekly meeting
   - discussion of Rotors, potentially use exponential and log addition of matricies for rotation
   - linked to marc ten bosch Rotor3 implementation!
 - *1.5 hours* improvements to 2D to 3D demonstration

### 29 Oct 2021

 - *4 hours* implementing 3D rotor using swipe gestures - still maintains axis during rotation so not intuitive
   - for swipe rotation - each gesture will have to be a new rotor. I will have to create a new rotor each time, and add it to the rotor, without continuing rotation

### 30 Oct 2021

 - *1 hours* implement 4D rotor (besides rotor * rotor product, and rotation of a vector by a rotor)
 - *2 hours* attempting to derive rotor * rotor product

## Week 7

### 1 Nov 2021

 - *2 hours* trying to derive rotor * rotor product using the geometric product
   - 3D rotation works, but cannot prove it works in 4D until 4D rotation function works - which i have not derived yet.
 - *3 hours* building a 4D to 3D demo, where the 3D object rotates to mimic the XW YW and ZW rotations.

### 2 Nov 2021

 - *2.5 hours* implementing onion skin shader
   - not as intuitive as I hoped for
 - *1.5 hours* implementing timeline shader
   - x y z rotation - I can have the entire timeline rotate, or I can have each object rotate - not sure whats better

### 3 Nov 2021

 - *3 hours* developing a menu system
   - partially just to chill out, but also to learn more about UI and scene loading which will be important later for navigating the tests for users

### 4 Nov 2021

 - *3 hours* completing the menu system
   - not as refined as a final product but the menu navigation and carousel is working

### 5 Nov 2021

 - *1.5 hours* transferring common code from shaders to an include file `.cginc`
   - cannot transfer ray march functions unfortunately because GLSL does not use function pointers as it does not have a stack

### 7 Nov 2021

 - *1 hour* implementing rotors to the grab ball
   - Cannot transform rotors to unity `Transform.Rotate`, so have to rotate the actual arc ball with quaternions
 - *2 hours* converting swipe rotation to use the global axis for intuitive rotation
   - Realised I have to swap bxy and bxz or the rotation equation to use the local axes. This may have reprocusions for 4D
 - *1 hour* trying to have the grab ball rotation follow the mouse when tangent to the arc
   - stuggling to work properly. restricting the axes do not seem to help. May need to base it off the quaternion

## Week 8

### 8 Nov 2021

 - *2 hours* begin script plans for how to teach users
 - *2.5 hours* polishing grab ball to correctly use the tangent and to highlight the trajectory to move the mouse for users
 - *2 hours* create multi-rotation view of 4D (inspired by poly-vision), showing default view, xw, yw and zw. Used Cone and torus as example.
   - also solved why cone was being unpredictable, but yet to implement in the main build.

### 10 Nov 2021

 - *3 hours* Experiments with sdf polygon mesh to construct a pentachoron
   - very glitchy in unity - triangles do not work

### 11 Nov 2021

 - *3 hours* polish multi view representation and add it to the 4D shapes project

### 13 Nov 2021

 - *2.5 hours* testing and implementing a pose matching script&scene that will randomly orient an object and check if 2 objects are of the same orientation

## Week 9

### 15 Nov 2021

 - *4 hours* building a modular scene that will be used to conduct tests
   - when specific options are set the scene can load each representation and the associated UI elements
   - when specific options are set the scene can load the UI elements associated with each test
 - *1 hour* changing and improving the UI for "Shape Matching"
 - *3 hours* general improvements and polishing
   - ensuring the UI is scalable to different sized screens
   - ensure UI is clear and concise
   - have not uploaded images yet