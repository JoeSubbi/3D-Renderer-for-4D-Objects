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

## SEMESTER 1 Week 1

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
 - *2 hours* Researching existing SDFs for other shapes - namely an Octahedron and Tetrahedron.
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

### 16 Nov 2021

 - *2.5 hours* polishing modular scene
   - reduced w axis timeline to only show 2 cross sections forward and backwards rather than 3 - totally 5 rather than 7
   - have pose matching window be able to move anywhere on the screen, and is set to custom positions depending on the representation
   - improved scale of UI elements

### 17 Nov 2021

 - *2 hours* Add capsule to polished scenes, and 3D objects to `Shapes.cginc`

### 18 Nov 2021

 - *0.5 hours* weekly meeting

### 19 Nov 2021

 - *2 hours* writing up intro script

### 20 Nov 2021

 - *3 hours* re-organising plan for testing
   - each stage of the test
   - data to be collected
 - *2 hours* Creating some visualisations for the intro video for users
 - *2 hours* Trying to build a pentachoron

### 21 Nov 2021

 - *2 hours* Created a working pentachoron (hyper tetrahedron)

## Week 10

### 22 Nov 2021

 - *2 hours* playing with different "texturing"
   - currently just colouring faces
 - *3 hours* deriving rotor-rotor product
   - appears successful - interacts well with 3D rotation

### 23 Nov 2021

 - *2 hours* created more visualisations for the users
   - tesseract
 - *2 hours* derive 3D rotation expressions
   - correct - but some signs are wrong, presumably because bosch takes Z as up, but the multiplication table, and me take Y as up
 - *0.5 hours* improving face colouring for "texturing"

### 24 Nov 2021

 - *3 hours* finished deriving rotate function... it did not work
 - *1 hour* diagnose why it is not working, improved a bit but still not working as expected - I think something is wrong with the signs of each component of the equations
 - *0.5 hours* added another colour based texture - all positive red, all positive blue, overlayed over lighting

### 25 Nov 2021

 - *0.5 hours* weekly meeting
 - *1 hour* 4D rotor works on 3D!, 4D rotation still doesn't work

### 26 Nov 2021

 - *1.5 hours* re-derived 4D rotation - turns out I was doing P\*uP rather than PuP\*
   - Rotation now works in yz, xz, xy AND 2 w3 planes. but not the last one... Need to re-derive Rotor3 rotation correctly and see if I learn anything.

### 27 Nov 2021

 - *0.5 hours* considering how a grab ball should interact with XW, YW and ZW. Should it remain unmoving? or should I be controlling 2 3D grab balls to represent 3D and 4D

### 28 Nov 2021

 - *1.5 hours* re-deriving rotor3 product and rotor4 rotation - found some stuff I thought maybe I missed but seems less accurate than before

## Week 11

### 29 Nov 2021

 - *1.5 hours* working texture patterns
   - Best Textures:
     - 2. Normal based diffuse lighting
     - 6. RGBW Lit
     - 13. Blue & Red normal direction with pattern
     - 14. RGBW with pattern
   - Pentachoron is a bit funny, needs to have normal negated to show good results, shouldn't affect any other model whilst negative, just swipes direction of colours
 - *1 hour* further investigation into rotors
   - found difference between local vs global rotation within rotor product. defined rotor product using "/" operator for global rotation - swiping

### 01 Dec 2021

 - *1.5 hours* checking tri-planar rotation does not refer to planes of rotation - 4D texturing textures X Y Z and W not the associated planes.
 - *1.5 hours* more or less finished scripts
 - *2.5 hours* creating explanation animation for planar rotation. 
  
### 02 Dec 2021

 - *0.5 hours* Weekly meeting

### 03 Dec 2021

 - *2.5 hours* re-deriving rotor equations using Johns multiplication table
   - stably incorrect - I think I have had same behaviour before, but the equations seem nicer than my most recent attempts, even if it appears to be working less
 - *1.5 hours* implement toggle for shape options
   - user can select which shape, and system can read selected shape
 - *1 hour* retry objects on UI with a second camera in the scene

### 04 Dec 2021

 - *4 hours* tidy up the modular scene ALOT
   - cameras for UI image displays
   - redo all the UI positioning code
   - set UI rescaling to only be called when the screen size changes - much less taxing
   - create axis to show user what direction each of the 3 axes are pointing towards
 - *1 hour* implementing texturing in modular scene
 - *4 hours* implementing procedural file generation and json read/write
   - each new file has a new id and contains the json data for the tests

## Week 12

### 07 Dec 2021

 - *3 hours* Created a script that when triggered externally, will move the the next stage in the experiment.
   - loop through representations
     - loop through tests
   - a lot of planning for how it will interact with other components i.e the object controller

### 08 Dec 2021

 - *3 hours* improvements to how data is handled and stored - each sub test stores the results in a new dictionary

### 09 Dec 2021

 - *0.5 hours* Weekly Meeting
 - *2 hours* implementing new closed equation for rotors - THEY WORK (but are a bit glitchy)

### 10 Dec 2021

 - *2 hours* reviewing rotor implementation to work out why it is glitchy. cannot find why. seems like it should work fine...

## CHRISTMAS

### 17 Dec 2021

 - *2 hours* playing around with rotor to solve visual glitch - realised I was not normalising pseudo vector! IT WORKS - although sometimes shape scale up, but very rarely...
 - *4 hours* implementing swipe rotation in the modular scene
   - several issues arose
     - compiler could not replace rotation functions as there is so much content. had to reduce calls to rotate, but should be fine
     - if the objects were shifted based on UI elements, textures would be offset and move across object
   - I now have working swipe rotation

### 18 Dec 2021

 - *0.5 hours* converting generic manipulation script to use rotor, as the scenes to showcase each representation no longer work
   - Not that this will be used in the experiment
 - *1.5 hours* create static class to contain game state between scenes.
 - *1 hour* moved swipe rotation code to add the ability to enable and disable swipe and grab ball rotation
 - *1 hour* implemented and improved grab ball rotation - was way over complicated before...
 - *1 hours* improve local rotation
   - it was scaling weirdly, played around with it, found negating scalar and pseudo vector produces a much much more stable local rotation
 - *1 hour* make tetrahedron normals inside out to show pattern properly
 - *1 hour* creating and including icon to show center of rotation when using the swipe rotation along the Z axis

### 19 Dec 2021

 - *1 hour* Recording audio for opening tutorial
 - *4 hours* editing and screen recording content to build up the intro tutorial video

### 20 Dec 2021

 - *3 hours* realised local rotation still had issues and tried to solve them
   - played around with it. I understand the problem but cannot yet come up with a solution
   - may have to ditch the grab ball... Will keep trying though

### 21 Dec 2021

 - *0.5 hours* realised rotation between 3D-2D and 4D cross section for YZ and XZ rotation was different in tutorial video...
   - turns out if you base the rotation as a single rotation over `Time.time`, the order of rotation actually matters even with rotors  - so rather than a combined rotation it will do YZ THEN XZ.
   - by multiplying rotations over `Time.deltaTime`, the order of rotations does not matter
 - *1.5 hours* began plans to move TestController content to StateController - does not need to be checked every frame, much more robust, and can be checked between scenes.
 - *0.5 hours* rearranging UI controller into representation specific functions, with the plan to call them externally rather than every frame
 - *0.5 hours* Adding continuous rotation functionality to object controller
 - *1 hour* implementing test loading function in state controller
   - still need to call UIController functions which are not static

### 22 Dec 2021

 - *1 hour* experimenting with minimum angle between rotors
   - `2 * Acos( (p * q.Reverse()).a )` does not seem to work on normalised or un-normalised rotors...
 - *0.5 hours* moving the canvas scalar script into UI controller (simpler and more efficient)
 - *0.5 hours* adding initialisation of external non-static variables to static state controller class via another script.
   - this will probably have to be moved around at a later date when I start actually loading and unloading scenes
 - *2 hours* Wrote up intro briefing for experiment using old main menu background. Added button to tutorial video - need to add start/pause buttons
 - *0.5 hours* rearranged project structure as a lot of directories with only a couple of files was making the project cluttered
 - *2 hours* building up tutorial video scene to have working scrub timeline and pause and play.
   - You can now:
     - read the experiment briefing, which takes you to
     - tutorial video which you can pause and rewind, which takes you to
     - modular scene to begin tests (this will be changed for an explanation scene)

### 23 Dec 2021

 - *0.5 hours* test questionaire on paper - too many pages per participant
 - *2 hours* build survey scene where user can fill out answers between a test
 - *1 hours* add scripts for survey scene to read data

### 24 Dec 2021

 - *0.5 hour* Create small script to initialise StateController on start up
 - *1 hour* Moving and modifying save functionality to state controller

### 26 Dec 2021

 - *4 hours*
   - Added button to go from survey scene to modular scene and save data
   - Added button to go from modular scene to survey scene and save data
   - Save selected shape and rotation data
   - Solved issue with order of initialisation for StaticController
   - Made 4D-3D counterpart also randomly rotated with main object

### 01 Jan 2022

 - *3 hours* implementing progression from each test to each representation

### 02 Jan 2022

 - *0.5 hours* implemented a better random rotation for rotation match
   - 60% chance of 3D rotation
   - 60% chance 1 of 4D rotation
   - 100% chance of a 2nd 4D rotation
 - *1.5 hours* trying to find issue where the object continuously grows...
   - xy and zw rotation together seems to cause the expansion, maybe I have copied something incorrectly - same behaviour has local rotation and part of the reason I needed to ditch it
 - *0.5 hours* implementing intro card when user is given a new representation
 - *1 hour* testing and improving
 - *1.5 hours* build project, fix issues with persistent data path + filename not combining properly

### 03 Jan 2022

 - *0.5 hours* implemented timer
   - if new representation it resets the timer when they finish reading the description of the representation

### 07 Jan 2022

 - *1 hour* creating graph scene

### 08 Jan 2022

 - *1 hour* improving graph scene to separate pose match
 - *0.5 hours* Weekly presentation

## SEMESTER 2 Week 13

### 10 Jan 2022

 - *0.5 hours* adding timer
 - *1 hour* running tests
 - *1 hour* polishing various components

### 13 Jan 2022

 - *0.5 hours* weekly meeting
 - *0.5 hours* working angle between rotors
 - *0.5 hours* set up pop-up cards for each type of test
 - *1.5 hours* pause timer when pop up card is visible and activate/disable cards explaining the test at the beginning of each test
 - *1 hour* add time limit for each test, and set a different number of iterations for each type of test
 - *1.5 hours* implement graph functionality based on JSON data

### 14 Jan 2022

 - *3 hours* experimenting with pattern texture on all elements of multi-view
   - very chaotic, but does provide extra information
 - *0.5 hours* creating new logo

### 16 Jan 2022

 - *1.5 hours* trying to find problem with rotors AND FIXING THEM
   - Rotors seem to 100% work now!
 - *0.5 hours* Added torus with spherical crossSection to 
 - *1 hour* Navigation from Survey to Graph scene when new representation
 - *2.5 hours* playing with patterns on multi-view and timeline (multi-view was actually wrong)

## Week 14

### 17 Jan 2022

 - *3 hours* playing around with patterns on multi-view and timeline trying to fit the patterns in the right place
   - super slow as compiling the complex shaders takes ages

### 18 Jan 2022

 - *2.5 hours* finally finished getting pattern on timeline and multi-view correctly
   - was using W separation for ages rather than X separation between objects
 - *1 hour* debugging why it was not saving correct file

### 19 Jan 2022

 - *3.5 hours* Implemented Practice Scene
 - *0.5 hours* Added Play Icon on play button for video
 - *2.5 hours* Fixing bug where practice was skipped
 - *1.5 hours* Running practice with Sophie and making improvements
   - [x] reword some of intro
   - [ ] not enough focus on object cross section in video
   - [x] space bar not explained in practice scene
   - [x] space bar removing selected option through automatic navigation 
   - [x] not enough time in practice (add 30 secs)
   - [x] hard to understand 4D Multi-view description - add example
   - [x] improve wording in rotation_match intro
   - [ ] explain within plane 2D rotation
   - [ ] better 2x 4D planes of rotation explanation
   - [x] reword pose_match survey
   - [x] disable movement in rotation_match

### 20 Jan 2022

 - *0.5 hours* Show graph at end, before thank you
 - *2 hours* implementing saving results via email as WebGL does not have a file system, it is temporary
 - *1 hour* testing out itch.io - video player doesn't work...
 - *0.5 hours* weekly meeting
 - *0.5 hours* polishing - adding copy to clipboard and improving intro scene
 - *1 hour* writing up rotor in python and creating data analysis jupyter notebook
 - *1 hours* writing up data analysis tool

### 21 Jan 2022

 - *1.5 hour* Adding data to be saved and creating a variant of the app without the tutorial video
 - *0.5 hours* setting up calender booking with *calendly*
 - *1 hour* testing WebGL without video
   - works great except... 
   - cannot copy to clipboard
   - cannot email
   - Overlapping UI Element in Graph scene - fixed
 - *1 hour* alternative WebGL specific emailing
   - still does not work with itch.io
 - *1.5 hours* researching other places to host game
   - new grounds - email works
 - *0.5 hours* transferring questionnaire from google docs to google forms and creating a checklist for when running the experiment
 - *0.5 hours* rewriting script for tutorial covering rotation

TODO:
 1. ~~Read and reword Johns Intro~~
 2. Better Tutorial Explanations of rotation
   - 2 planes of rotation at once
   - 2D vs 3D, 3D vs 4D rotations
 3. ~~Variant of Program without Tutorial~~
   - ~~Test if WebGL can Email me with eduroam~~
   - Test if NewGrounds can Email me with eduroam
   - Get working WebGL copy to clipboard
 4. Test entire system properly
 5. ~~Calender booking thing~~
 6. Advertise Experiment
 7. Start Dissertation
 8. Data Analysis