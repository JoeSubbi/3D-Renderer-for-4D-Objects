# Weekly Meeting-Minutes

- [Weekly Meeting-Minutes](#weekly-meeting-minutes)
- [Semester 1](#semester-1)
  - [07/10/2021](#07102021)
  - [14/10/2021](#14102021)
  - [22/10/2021](#22102021)
  - [04/11/2021](#04112021)
  - [18/11/2021](#18112021)
  - [25/11/2021](#25112021)
  - [02/12/2021](#02122021)
  - [09/12/2021](#09122021)
- [Semester 2](#semester-2)
  - [13/01/2022](#13012022)
  - [20/01/2022](#20012022)
  - [27/01/2022](#27012022)
  - [03/02/2022](#03022022)
  - [10/02/2022](#10022022)
  - [18/02/2022](#18022022)
  - [24/02/2022](#24022022)
  - [03/03/2022](#03032022)
  - [10/03/2022](#10032022)
  - [17/03/2022](#17032022)
  - [24/03/2022](#24032022)
  - [29/03/2022](#29032022)

# Semester 1
## 07/10/2021

Rudy Rucker - 4D book

Philosophy and psychological approach to higher dimensional understanding

Inigo Quilez - Creator of *Shader Toy*
https://www.iquilezles.org/index.html

try get most done by week 7 as assessed exercises will reduce time to work on project

## 14/10/2021

how do you prove that someone has understood the geometry?

look into exp/log map rotation - expandable to multiple dimensions

## 22/10/2021

Give Rotors sense of direction by multiplying with a vector

## 04/11/2021

How to decide whether to use certain representations
 - 10 different ideas - informal trial  
 - 3 winners and more in-depth study  
   - even if just asking some friends

User interaction
 - geodesic - representation of the surface by shortest path
 - project touch onto a sphere to rotate between pos1 and pos2 vectors

## 18/11/2021

Consider different textural visualisations

Within groups testing
 - have each user try multiple representations

Show user progress between each representation

## 25/11/2021

rotation is something to do preliminary tests with, if one is clearly better pick that

pick and choose what to experiment on. What is the question I am trying to answer

textures - can be simple or complex, as long as it is easy for a user to find a landmark

## 02/12/2021

john will take a look at rotors

probably set end of december as deadline for rotors

## 09/12/2021

discussion of plan for next term

more discussion about rotors

# Semester 2

## 13/01/2022

angle between rotors - just take dot product of random vectors rotated by the 2 rotors DUHHH...

experiments - try an online thing - webGL or screen share

discussion of road map - make as much time as possible so i don't loose time with AEs etc

length of test - 50minutes best for an hour slot - don't spend too much time in experiment

## 20/01/2022

don't email entire university

port is blocked on eduroam - use copy to clipboard as backup

have tutorial as youtube video if not working on itch.io

proper analysis with some analysis tools before opening experiment applications

## 27/01/2022

do not need proofs - just explain laws of geometric algebra and derivation using sympy
 - anti commute, add multivectors etc

include code/algorithm for ray marching

rule of thumb - write word for numbers if < 12 (except for abbreviations like 4D)

Summer directory has no use - its old

## 03/02/2022

try take multiple random vectors and take the average minimum angle between rotated vectors

## 10/02/2022

don't delve to deep into data due to not enough of it
 - mention stuff i would like to explore but couldn't

can send bits of dissertation whenever

## 18/02/2022

aims:
 - list research questions
 - discussion of potential implication of results

evaluation
 - discussion of implication of results
 - 0.5-1 page tie into aims

background
 - argue other peoples work
   - good and bad - limitations etc

motivation
 - being able to understand 4D
   - very high nD spaces - lots of corners short distance apart
   - some things just don't behave as intuition predicts
   - introduction without being to jarring to new people
 - interactive exploration - value of learning this way
 - bringing intuition of 3D world to 4D

to get high grade
 - **good argument throughout whole dissertation and in evaluation**
 - **good questions**
 - good presentation of results and their **implications**
 - implementation - credit for everything that I have done (they wont look at source code)
 - high quality diagrams
 - do not jump from high level to implementation

## 24/02/2022

Research Questions:
 - what representation is most effective for representing surface type of 4D object
 - what rep most effective for conveying rotational pose
 - increase in interaction increase understanding of 4D shapes
   - what is understanding
     - pose
     - shape
     - confidence and accuracy

how does interactive visualisation effect how people understand 4d geometry
 - benefits of interaction
 - drawbacks of interaction

Textures - representations couldn't take full advantage
 - looked into but requires more investigation

3D rotation
 - https://dl.acm.org/doi/pdf/10.1145/258549.258778
 - https://dl.acm.org/doi/pdf/10.1145/263407.263408
 - https://scholar.google.co.uk/scholar?cites=3287387963043952182&as_sdt=2005&sciodt=0,5&hl=en

Writing:
 - no personal opinion
   - hypothesis - longer use, more familiarity, more useful
 - def of net? sub-mesh is a weird term
 - don't use "of course"

**common criteria to apply to different papers**
 - discuss variation between object over space/time? object in animation
 - imagine table of ways it could be done, and reference where i fit compared to everyone else
 - e.g kageyama rotation with keys and kageyama ovular display

*Add labels to ray marching diagram*

## 03/03/2022

don't write in future tense
 - consistent passive past tense
  
unity
 - needed to build project
 - host online given pandemic

aims: discuss
 - build system for interactive rendering of 4D objects
 - evaluate effectiveness to visualise 4D spaces
 - different ways of rendering
 - logical ordering

stats
 - bootstrap
 - randomly take sample from it - 100 samples
   - repeat 1000s of times
   - distribution of statistics
 - distribution of statistic to see if correlation is relevant

diverging colour map - red-blue
 - correlation with signs

pose match - ease vs accuracy
 - looks to be evidence
 - strong unintuitive results - seek other correlations
 - small data - coincidence

timeline - unanswered
 - less time talking about less sure conclusions

## 10/03/2022

bold name of shapes - bring attention

equation for each sdf?

subsections for reps

design should discuss high level issues of interacting with 4D objects
 - how to represent
 - how to manipulate
   - build system to freely rotate 4D object via UI
   - high level overview

## 17/03/2022

describe json data
 - what it represents and what it stores
 - schema in appendix

sub headings
 - email, copy paste etc

implementation chapter:
 - explain the moving parts before everything else
   - diagram of different things involved
     - renderer
     - manipulation
     - experimental platform
 - cut some fluff out
   - 5.1 - compact radius of sphere
 - lambertian diffuse elimination
 - subjective judgments
   - controversial -> risky or exploratory
 - explanation of why I chose simple 4D geometry
   - simple things before hard things
   - lack of predefined objects - 4D machine parts
   - 4D fractals - complicated hard to explain

readme.md
 - build
 - open jupyter nb

## 24/03/2022

-

## 29/03/2022

conclusion:
 - could discuss improvements for rendering engine  
 - could discuss possibility for system to assist with good rotations rather than x and y key presses
 - generalisability to higher dimensions

need ethics forms

evaluation aims:
 - "more effective: measure through accuracy confidence and time"

pose match
 - graphs 
   - capped time limit - max time for task
 - min angle
   - assume radians, remove subscript
   - brackets around arccos

Title: To 3D instead of to the 3d

Wednesday 6th 12.30 - return book on topology