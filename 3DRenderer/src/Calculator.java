public class Calculator {

    private static double DrawX=0, DrawY=0;

    // calculate the position x y and z to a 2d plane from our view point.
    public static double[] Calculate(double[] ViewFrom, double[] ViewTo, double x, double y, double z){
        setStuff(Screen.ViewFrom, Screen.ViewTo, x,y,z);
        return new double[] {DrawX, DrawY};
    }

    static void setStuff(double[] ViewFrom, double[] ViewTo, double x,double y,double z){
        Vector ViewVector = new Vector(ViewTo[0] - ViewFrom[0], ViewTo[1] - ViewFrom[1], ViewTo[2] - ViewFrom[2]);
        Vector DirectionVector = new Vector(1,1,1);
        Vector PlaneVector1 = ViewVector.CrossProduct(DirectionVector);
        Vector PlaneVector2 = ViewVector.CrossProduct(PlaneVector1);

        //from view point to the point we are trying to calculate
        Vector ViewToPoint = new Vector(x - ViewFrom[0], y - ViewFrom[1], z - ViewFrom[2]);

        //t value - how far along the vector befor eit collides along the plane
        double t = (ViewVector.x*ViewTo[0] + ViewVector.y*ViewTo[1] + ViewVector.z*ViewTo[2]
                    - (ViewVector.x*ViewFrom[0] + ViewVector.y*ViewFrom[1] + ViewVector.z*ViewFrom[2]))
                    / (ViewVector.x*ViewToPoint.x + ViewVector.y*ViewToPoint.y + ViewVector.z*ViewToPoint.z);

        //find the point of collision with the plane
        x = ViewFrom[0] + ViewToPoint.x *t;
        y = ViewFrom[1] + ViewToPoint.y *t;
        z = ViewFrom[2] + ViewToPoint.z *t;

        if (t>=0){ //if smaller than 0 it would be behind us
            DrawX = PlaneVector2.x*x + PlaneVector2.y*y + PlaneVector2.z*z;
            DrawY = PlaneVector1.x*x + PlaneVector1.y*y + PlaneVector1.z*z;
        }
    }
}
