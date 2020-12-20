public class Calculator {

    private static double DrawX=0, DrawY=0;

    // calculate the position x y and z to a 2d plane from our view point.
    public static double[] Calculate(double[] ViewFrom, double[] ViewTo, double x, double y, double z){
        setStuff(Screen.ViewFrom, Screen.ViewTo, x,y,z);
        return new double[] {DrawX, DrawY};
    }

    static void setStuff(double[] ViewFrom, double[] ViewTo, double x,double y,double z){
        Vector ViewVector = new Vector(ViewTo[0] - ViewFrom[0], ViewTo[1] - ViewFrom[1], ViewTo[2] - ViewFrom[2]);
        /*
        Vector DirectionVector = new Vector(1,1,1);
        Vector PlaneVector1 = ViewVector.CrossProduct(DirectionVector);
        Vector PlaneVector2 = ViewVector.CrossProduct(PlaneVector1);
        */

        //decide how much to rotate to compensate for camera
        Vector RotationVector = getRotationVector(ViewFrom, ViewTo);
        //Weird Vector 1 and 2 in Tutorial. Names are guesses as to function
        Vector RotationView1 = ViewVector.CrossProduct(RotationVector);
        Vector RotationView2 = ViewVector.CrossProduct(RotationView1);


        //from view point to the point we are trying to calculate
        Vector ViewToPoint = new Vector(x - ViewFrom[0], y - ViewFrom[1], z - ViewFrom[2]);

        //t value - how far along the vector before it collides along the plane
        double t = (ViewVector.x*ViewTo[0] + ViewVector.y*ViewTo[1] + ViewVector.z*ViewTo[2]
                    - (ViewVector.x*ViewFrom[0] + ViewVector.y*ViewFrom[1] + ViewVector.z*ViewFrom[2]))
                    / (ViewVector.x*ViewToPoint.x + ViewVector.y*ViewToPoint.y + ViewVector.z*ViewToPoint.z);

        //find the point of collision with the plane
        x = ViewFrom[0] + ViewToPoint.x *t;
        y = ViewFrom[1] + ViewToPoint.y *t;
        z = ViewFrom[2] + ViewToPoint.z *t;

        if (t>=0){ //if smaller than 0 it would be behind us
            //DrawX = PlaneVector2.x*x + PlaneVector2.y*y + PlaneVector2.z*z;
            //DrawY = PlaneVector1.x*x + PlaneVector1.y*y + PlaneVector1.z*z;
            DrawX = RotationView2.x*x + RotationView2.y*y + RotationView2.z*z;
            DrawY = RotationView1.x*x + RotationView1.y*y + RotationView1.z*z;
        }
    }

    static Vector getRotationVector(double[] ViewFrom, double[] ViewTo){
        //distance from view point
        double dx = Math.abs(ViewFrom[0]-ViewTo[0]);
        double dy = Math.abs(ViewFrom[1]-ViewTo[1]);

        //local rotation
        double xRot=dy/(dx+dy); //pitch
        double yRot=dx/(dx+dy); //yaw

        //if behind you, rotation will be negative
        if(ViewFrom[1]>ViewTo[1]) xRot = -xRot;
        if(ViewFrom[0]>ViewTo[0]) yRot = -yRot;

        return new Vector(xRot, yRot, 0);
    }
}
