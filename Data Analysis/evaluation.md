# Evaluation of Data

- [Evaluation of Data](#evaluation-of-data)
- [Shape Matching](#shape-matching)
  - [Change with Iteration](#change-with-iteration)
  - [Performance per Representation](#performance-per-representation)
  - [Correlations](#correlations)
- [Rotation Matching](#rotation-matching)
  - [Change with Iteration](#change-with-iteration-1)
  - [Performance per Representation](#performance-per-representation-1)
  - [Correlations](#correlations-1)
- [Pose Matching](#pose-matching)
  - [Change with Iteration](#change-with-iteration-2)
  - [Performance per Representation](#performance-per-representation-2)
  - [Correlations](#correlations-2)
- [User opinions of representations](#user-opinions-of-representations)
- [Summary](#summary)
  - [Representations](#representations)
  - [Iteration](#iteration)
  - [Correlations](#correlations-3)

# Shape Matching

 - large number of unanswered in 1st iteration, similar to timeline - may have skewed data a little with timeline

## Change with Iteration

correctness
 - much less like to not answer over time
 - more correct over time

time
 - first 2 attempts is jarring with short time limit
 - faster responses over time

## Performance per Representation

correctness
 - less information is more likely to be correct
 - similar amount of incorrect answers for each rep

time
 - longer to answer with more information on screen
   - highest (and also most complex) is multi-view
 - control is fastest - least info on screen

## Correlations

between numerical features
 - confidence and understanding increase with correctness
 - correctness decreases with an increase in time taken
   - running out of time
 - if the initial rotation is further away from a 90 degree interval, the participant interacts with the object more often

between shapes
 - general less correct with cone - potentially given it will often look like a sphere
 - a user interacts with a sphere far more than any other object for longer
 - time taken to submit answer increases for pentachoron
   - talking to participants - they often were not sure if it could be cube
 - less time taken with a torus and pentachoron

between representations
 - correctness decreased with timeline - unanswered items from above
 - confidence at highest with control - less info
 - bigger focus on rotating the object with multi-view
 - bigger focus on w axis with 4D to 3D - *weird*

# Rotation Matching

## Change with Iteration

absolute correct
 - first iteration is worse, but not a clear trend of improvement

jaccard index
 - on average does increase over time

time taken
 - time taken decreases over time very clearly

## Performance per Representation

absolute correct
 - least correct with multi-view
   - people seeing patterns in how shapes behave misleading them
 - best with 4D-3D abstraction
   - does basically give you the answer for 4D
 - timeline better than control and multi-view
   - surprising

jaccard index
 - multi view is worse on average (mean)
 - control and timeline same median
 - timeline and 4D-3D more often correct
 - 4D-3D is best

time taken
 - more information, longer it takes
 - 4D-3D generally best

## Correlations

between numerical features
 - slight correlation between increase of confidence and correctness
 - longer time to answer results in lower confidence and understanding
 - less correct with a higher number of rotations at one time 
 - generally no correlation between correctness and iteration

between shapes
 - correctness and confidence highest with capsule
 - correctness and confidence lowest with pentachoron
   - rotations appear much more complex
 - time taken to answer highest with pentachoron
 - time taken lowest with capsule

between representations
 - correctness and confidence highest with 4D-3D
 - time taken reduced if less stuff on screen

# Pose Matching

ALOT of outliers

## Change with Iteration

accuracy
 - very slight improvements on average over time
 - generally worst accuracy in first iteration

time
 - first two iterations - very common to use entire time
 - much more likely for earlier submission over time
 - faster with each iteration on average

## Performance per Representation

generally quite hard to draw conclusions - not any immediate conclusions

accuracy
 - similar ranges and averages
 - 4D-3D had lower minimum with fewer outliers

time
 - more information meant higher time taken
 - 4D-3D least information thats not control best times

## Correlations

between numerical features
 - accuracy increased as time taken increased
 - accuracy increased as users manipulated the shape more
 - accuracy increased if rotation was further from 90degrees
 - accuracy increased as ease decreased - *weird*
 - easiness decreased if user manipulated shape more
 - easiness decreased if it took longer
 - time taken was greater if the initial rotation was closer to 90degrees
 - had to rotate shape more if rotation was closer to 90degrees

between shapes
 - accuracy worst with box
 - accuracy best with torus
 - accuracy surprisingly good with pentachoron
   - least easy
 - torus found to be easiest
 - time taken longest with box
 - less time taken with cone torus and capsule
 - most interaction with pentachoron and box
 - least interaction with capsule and cone

between representations
 - timeline had greatest accuracy
 - multi-view second greatest accuracy
   - found to be least easy
 - 4D-3D generally worst - surprising
   - found to be easiest
 - time taken increased with more info on screen
 - interaction increased with more info on screen
 - 4D-3D used w slider a lot more than anything else - *weird*

# User opinions of representations

Timeline
 - positives
   - simple presentation of geometry
   - indication of what will occur before interacting with slider
   - saved time identifying shapes like the sphere
 - negatives
   - hard to understand in small time frame
   - did lag in browser
   - no helpful for rotation

Multi-rotational view
 - positives
   - helpful for understanding which planes of rotation ocurred
   - interesting in way of showcasing 4D
 - negatives
   - too much information
   - can lag a bit in browser
   - new users can misinterpret rotations - i.e 3D rotation will look like 4D from other perspectives and visa versa

4D-3D abstraction
 - positives
   - easy to work with
   - helps with rotation a lot
   - easier to differentiate multiple rotations
 - negatives
   - people often didn't pay attention to it except for animated rotation matching

# Summary

## Representations

New users don't have enough experience to take advantage of extensions
 - more information that is on the screen, more likely to identify the correct shape
 - less information the less time is taken
 - confidence increased with less information on screen
 
specifics of representations
 - 3D-4D is nice introduction to differentiate 3D and 4D rotations to new users but is kind of cheat-y
 - Timeline and Multi-view tended to perform worse
   - sometimes lag-y in browser
   - mostly too much info for new users given a limited time

not much benefit when trying to directly manipulate the shape
 - focus on the main object, ignore extensions

## Iteration

correctness would generally increase over time
 - less so for pose matching

answers would be submitted faster over time
 - no notable increase in confidence over time

## Correlations

between numerical features
 - interactions with the objects increased accuracy/correctness
 - spending more time increased accuracy
   - time limits too short - trade off argument
   - except when running out of time - shape match
 - confidence / understanding and correctness were well correlated
   - not just blind guessing
 - less correct if more happening on screen - increased complexity
   - expected
     - rotation match
   - representations

between shapes
 - general
   - increased time to answer - pentachoron
   - less confidence / harder - pentachoron
   - box is hard to manipulate
   - circular shapes behave more as expected to new users
 - shape match
   - less correct with cone
     - can often look like sphere or capsule
   - a lot more interaction with sphere
     - verify it is actually a sphere
   - less time to answer - torus
 - rotation match
   - correctness highest with capsule
   - correctness lowest with pentachoron
   - less time to answer - capsule and cone
 - pose match
   - worst accuracy - box
   - more accurate - torus
   - longer time - box
   - most interaction with pentachoron and box