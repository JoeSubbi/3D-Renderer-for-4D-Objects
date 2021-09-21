# Ray Marching - Shape Distance Functions

(https://www.youtube.com/watch?v=Ff0jJyyiVyw)

```
\\distance between camera and center of an object
length(p)
```

## Sphere
```C#
d = length(p) - radius
```
![img](Shapes/sphere.PNG)

## Capsule
```C#
// project the line ab
// 0 on A, 1 on B
t = dot(ap,ab) / dot(ab,ab)
// anything above A or below B is less than 0 or greater than 1 respectively
t = clamp(t, 0, 1)
// closest point on line AB
// start at A and move t-steps along ab
c = A + t*ab
// the distance is the length from the camera to anywhere on 
// line ab - the radius of the capsule
d = length(p-c) - radius
```
![img](Shapes/capsule.PNG)


## Torus
```C#
// Get the horizontal distance from the point below the camera 
// to the origin of the torus. subtract the major radius
x = length(P_xz) - r_1
// distance y
y = P_y
// Get the distance to the point on the major radius using x and y
//subtract the minor radius
d = length(vec2(x, y)) - r_2
```

![img](Shapes/torus.PNG)


## Box
```C#
// Get the corner of the box by finding the distance from the camera to the center of the box 
// and subtract the hight, width and depth
// Cannot find just an edge
d = length(abs(p)-size)

// if dx, dy or dz is negative, ignore it
// if it is smaller than 0 it gets capped at 0
d = length(max(abs(p)-size, 0))
```

![img](Shapes/box.PNG)

## Cylinder
```C#
//Capsule but without the clamp

// project the line ab
// 0 on A, 1 on B
t = dot(ap,ab) / dot(ab,ab)
// anything above A or below B is less than 0 or greater than 1 respectively
t = clamp(t, 0, 1)
// closest point on line AB
// start at A and move t-steps along ab
c = A + t*ab
// the distance is the length from the camera to anywhere on 
// line ab - the radius of the capsule
d = length(p-c) - radius

// To view the side, similar to the box]
// t goes from 0 to 1
// makes cylinder 0 from everything either side of A or B
y = (abs)t-0.5)-0.5) * length(ab)
//if looking directly at the edge
//same as box
e = length(max(vec2(d,y), 0))
//interior distance
i = min(max(d,y),0)
```

![img](Shapes/cylinder.PNG)